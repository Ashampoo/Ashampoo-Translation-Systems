using System.Text;
using System.Text.RegularExpressions;
using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.IO;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using CommunityToolkit.Diagnostics;
using IFormatProvider = Ashampoo.Translation.Systems.Formats.Abstractions.IFormatProvider;

namespace Ashampoo.Translation.Systems.Formats.PO;

/// <summary>
/// Implementation of the PO (Portable Object) format.
/// </summary>
public class POFormat : AbstractTranslationUnits, IFormat
{
    private static Regex reComment = new(@"#(?<type>[|:,. ]{0,1})(?<content>.*)");

    /// <inheritdoc />
    public IFormatHeader Header { get; init; } = new POHeader();

    /// <inheritdoc />
    public LanguageSupport LanguageSupport => LanguageSupport.OnlyTarget;

    /// <inheritdoc />
    public async Task ReadAsync(Stream stream, FormatReadOptions? options = null)
    {
        // TODO: dispose stream readers?
        var streamReader = new StreamReader(stream);
        var lineReader = new LineReader(streamReader);

        await ReadFirstMessageAsHeaderAsync(lineReader); // read header
        var configureSuccess = await ConfigureOptionsAsync(options); // configure options 
        if (!configureSuccess)
        {
            options!.IsCancelled = true;
            return;
        }

        Guard.IsNotNullOrWhiteSpace(Header.TargetLanguage, nameof(Header.TargetLanguage)); // check if target language is set

        await ReadMessagesAsTranslationsAsync(lineReader);
    }

    private async Task<bool> ConfigureOptionsAsync(FormatReadOptions? options)
    {
        if (!string.IsNullOrWhiteSpace(Header.TargetLanguage)) return true;
        if (string.IsNullOrWhiteSpace(options?.TargetLanguage))
        {
            if (options?.FormatOptionsCallback is null)
                throw new InvalidOperationException("Callback for Format options required.");

            FormatStringOption targetLanguageOption = new("Target language", true);
            FormatOptions formatOptions = new()
            {
                Options = new FormatOption[]
                {
                    targetLanguageOption
                }
            };

            await options.FormatOptionsCallback.Invoke(formatOptions); // invoke callback
            if (formatOptions.IsCanceled) return false;

            Header.TargetLanguage = targetLanguageOption.Value;
        }

        else
        {
            Header.TargetLanguage = options.TargetLanguage;
        }

        return true;
    }

    private async Task ReadFirstMessageAsHeaderAsync(LineReader lineReader)
    {
        await lineReader.SkipEmptyLinesAsync(); 

        var message = await ReadMessageAsync(lineReader, true); // read header as first message
        if (message is not MessageString { MsgId: "" } messageString)
            throw new UnsupportedFormatException(this, "Header is missing.");

        var lines = messageString.MsgStr.Split('\n'); // split header into lines
        foreach (var line in lines)
        {
            var tuple = line.Split(':'); // split line into key and value
            if (tuple.Length != 2) continue; // skip if not key:value
            var key = tuple[0].Trim();
            var value = tuple[1].Trim();
            Header.Add(key, value); // add key:value to header
        }
    }

    private async Task ReadMessagesAsTranslationsAsync(LineReader lineReader)
    {
        while (await lineReader.HasMoreLinesAsync())
        {
            await lineReader.SkipEmptyLinesAsync();
            
            var message = await ReadMessageAsync(lineReader);
            if (message is not MessageString messageString)
                throw new Exception($"Unexpected message {message.GetType()} .");
            
            if (string.IsNullOrWhiteSpace(messageString.Id)) continue; // skip if id is empty

            var translationUnit = new TranslationUnit(messageString.Id) // Create translation unit
            {
                Translations =
                {
                    messageString
                }
            };
            Add(translationUnit);
        }
    }

    private async Task<Message> ReadMessageAsync(LineReader lineReader, bool omitTargetLanguage = false)
    {
        IList<string> comments = new List<string>();
        var msgId = "";
        var msgStr = "";
        var msgCtxt = "";

        string? line;
        while (!string.IsNullOrWhiteSpace(line = await lineReader.PeekLineAsync()))
        {
            // TODO: interpret comments as flags, options and so on.
            if (line.StartsWith("#")) comments = await ReadCommentsAsync(lineReader);
            else if (line.StartsWith(Message.TypeMsgId)) msgId = await ReadMsgIdAsync(lineReader);
            else if (line.StartsWith(Message.TypeMsgStr)) msgStr = await ReadMsgStrAsync(lineReader);
            else if (line.StartsWith(Message.TypeMsgCtxt)) msgCtxt = await ReadMsgCtxtAsync(lineReader);
            else
                throw new UnsupportedFormatException(this,
                    $"Unsupported line '{line}' at line number {lineReader.LineNumber}.");
        }

        var comment = comments.Count > 0 ? string.Join("", comments) : null;

        var language = omitTargetLanguage ? "" : Header.TargetLanguage;
        return new MessageString(id: msgId, value: msgStr, language: language, comment: comment, msgCtxt: msgCtxt);
    }

    private async Task<string> ReadMsgCtxtAsync(LineReader lineReader)
    {
        return await ReadMsgXAsync(lineReader, Message.TypeMsgCtxt);
    }

    private async Task<string> ReadMsgStrAsync(LineReader lineReader)
    {
        return await ReadMsgXAsync(lineReader, Message.TypeMsgStr);
    }

    private async Task<string> ReadMsgIdAsync(LineReader lineReader)
    {
        return await ReadMsgXAsync(lineReader, Message.TypeMsgId);
    }

    private async Task<string> ReadMsgXAsync(LineReader lineReader, string msgType)
    {
        var stringBuilder = new StringBuilder();

        string? line;
        while (!string.IsNullOrWhiteSpace(line = await lineReader.PeekLineAsync()))
        {
            if (!line.StartsWith(msgType) && !line.StartsWith("\"")) break; // break if not msgType or multi-line string

            line = await lineReader.ReadLineAsync() ?? string.Empty; // read line
            line = Regex.Replace(line, $"^{msgType}", "");

            //Don't use trim(char), trim(char) removes all leading and trailing characters
            if (line.StartsWith("\"")) line = line[1..];
            if (line.EndsWith("\"")) line = line[..^1];

            stringBuilder.Append(line);
        }

        return stringBuilder.ToString().Replace("\\n", "\n");
    }

    private async Task<IList<string>> ReadCommentsAsync(LineReader lineReader)
    {
        var comments = new List<string>();

        while ((await lineReader.PeekLineAsync() ?? "").StartsWith("#"))
        {
            if (await lineReader.ReadLineAsync() is { } line) comments.Add(line);
        }

        return comments;
    }

    /// <inheritdoc />
    public void Write(Stream stream)
    {
        WriteAsync(stream).Wait();
    }

    /// <summary>
    /// Asynchronously writes the format to the given stream.
    /// </summary>
    /// <param name="stream">
    /// The stream to write to.
    /// </param>
    /// <exception cref="Exception">
    /// Thrown if an error occurs.
    /// </exception>
    public async Task WriteAsync(Stream stream)
    {
        var writer = new StreamWriter(stream, Encoding.UTF8);

        // write header.
        if (Header is not POHeader poHeader) throw new Exception($"Unexpected format header {Header.GetType()}");
        await poHeader.WriteAsync(writer);

        // write messages.
        foreach (var unit in this)
        {
            if (unit is not TranslationUnit poTranslationUnit)
                throw new Exception($"Unexpected translation unit: {unit.GetType()}");
            await poTranslationUnit.WriteAsync(writer);
            await writer.WriteLineAsync();
        }

        await writer.FlushAsync();
    }

    /// <inheritdoc />
    public Func<FormatProviderBuilder, IFormatProvider> BuildFormatProvider()
    {
        return builder => builder.SetId("po")
            .SetSupportedFileExtensions(new[] { ".po" })
            .SetFormatType<POFormat>()
            .SetFormatBuilder<POFormatBuilder>()
            .Create();
    }
}