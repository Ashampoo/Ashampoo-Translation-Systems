﻿using System.Text;
using System.Text.RegularExpressions;
using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.IO;
using Ashampoo.Translation.Systems.Formats.Abstractions.Models;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using CommunityToolkit.Diagnostics;

namespace Ashampoo.Translation.Systems.Formats.PO;

/// <summary>
/// Implementation of the PO (Portable Object) format.
/// </summary>
public class POFormat : IFormat
{
    private static Regex _reComment = new("#(?<type>[|:,. ]{0,1})(?<content>.*)");

    /// <inheritdoc />
    public IFormatHeader Header { get; init; } = new POHeader();

    /// <inheritdoc />
    public LanguageSupport LanguageSupport => LanguageSupport.OnlyTarget;

    /// <inheritdoc />
    public ICollection<ITranslationUnit> TranslationUnits { get; } = new List<ITranslationUnit>();

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

        Guard.IsNotNullOrWhiteSpace(Header.TargetLanguage.Value, nameof(Header.TargetLanguage)); // check if target language is set

        await ReadMessagesAsTranslationsAsync(lineReader);
    }

    private async Task<bool> ConfigureOptionsAsync(FormatReadOptions? options)
    {
        if (!Header.TargetLanguage.IsNullOrWhitespace()) return true;
        if (string.IsNullOrWhiteSpace(options?.TargetLanguage.Value))
        {
            if (options?.FormatOptionsCallback is null)
                throw new InvalidOperationException("Callback for Format options required.");

            FormatStringOption targetLanguageOption = new("Target language", true);
            FormatOptions formatOptions = new()
            {
                Options =
                [
                    targetLanguageOption
                ]
            };

            await options.FormatOptionsCallback.Invoke(formatOptions); // invoke callback
            if (formatOptions.IsCanceled) return false;

            Header.TargetLanguage = Language.Parse(targetLanguageOption.Value);
        }

        else
        {
            Header.TargetLanguage = (Language)options.TargetLanguage!;
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
            Header.AdditionalHeaders.Add(key, value); // add key:value to header
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
            TranslationUnits.Add(translationUnit);
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

        var language = omitTargetLanguage ? Language.Empty : Header.TargetLanguage;
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
        using var writer = new StreamWriter(stream, Encoding.UTF8, leaveOpen: true);

        // write header.
        if (Header is not POHeader poHeader) throw new Exception($"Unexpected format header {Header.GetType()}");
        poHeader.Write(writer);

        // write messages.
        foreach (var unit in TranslationUnits)
        {
            if (unit is not TranslationUnit poTranslationUnit)
                throw new Exception($"Unexpected translation unit: {unit.GetType()}");
            poTranslationUnit.Write(writer);
            writer.WriteLine();
        }

        writer.Flush();
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
        await using var writer = new StreamWriter(stream, Encoding.UTF8, leaveOpen: true);

        // write header.
        if (Header is not POHeader poHeader) throw new Exception($"Unexpected format header {Header.GetType()}");
        await poHeader.WriteAsync(writer);

        // write messages.
        foreach (var unit in TranslationUnits)
        {
            if (unit is not TranslationUnit poTranslationUnit)
                throw new Exception($"Unexpected translation unit: {unit.GetType()}");
            await poTranslationUnit.WriteAsync(writer);
            await writer.WriteLineAsync();
        }

        await writer.FlushAsync();
    }
}