using System.Text;
using System.Text.RegularExpressions;
using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.IO;
using Ashampoo.Translation.Systems.Formats.Abstractions.Models;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using CommunityToolkit.Diagnostics;
using IFormatProvider = Ashampoo.Translation.Systems.Formats.Abstractions.IFormatProvider;

namespace Ashampoo.Translation.Systems.Formats.NLang;

/// <summary>
/// Implementation of <see cref="IFormat"/> interface for the NLang format.
/// </summary>
public class NLangFormat : IFormat
{
    private static readonly Regex ReMsg = new("(?<key>.*?)=(?<value>.*)");

    /// <inheritdoc />
    public IFormatHeader Header { get; init; } = new DefaultFormatHeader();

    /// <inheritdoc />
    public LanguageSupport LanguageSupport => LanguageSupport.OnlyTarget;

    /// <inheritdoc />
    public ICollection<ITranslationUnit> TranslationUnits { get; } = new List<ITranslationUnit>();

    /// <inheritdoc />
    public void Read(Stream stream, FormatReadOptions? options = null)
    {
        ReadAsync(stream, options).Wait();
    }

    /// <inheritdoc />
    public async Task ReadAsync(Stream stream, FormatReadOptions? options = null)
    {
        var configureSuccess = await ConfigureOptionsAsync(options); // Configure options
        if (!configureSuccess)
        {
            options!.IsCancelled = true;
            return;
        }

        Guard.IsNotNullOrWhiteSpace(Header.TargetLanguage.ToString(),
            nameof(Header.TargetLanguage)); // Target language is required

        // TODO: Dispose of streams and readers?
        var streamReader = new StreamReader(stream);
        var lineReader = new LineReader(streamReader);

        await ReadTranslations(lineReader);
    }

    private async Task<bool> ConfigureOptionsAsync(FormatReadOptions? options)
    {
        if (string.IsNullOrWhiteSpace(options?.TargetLanguage.ToString()))
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

            await options.FormatOptionsCallback.Invoke(formatOptions); // Invoke callback
            if (formatOptions.IsCanceled) return false;

            Header.TargetLanguage = Language.Parse(targetLanguageOption.Value);
        }
        else
        {
            Header.TargetLanguage = (Language)options.TargetLanguage!;
        }

        return true;
    }

    private async Task ReadTranslations(LineReader lineReader)
    {
        await lineReader.SkipEmptyLinesAsync();
        while (await lineReader.HasMoreLinesAsync())
        {
            TranslationUnits.Add(await ReadTranslation(lineReader));
            await lineReader.SkipEmptyLinesAsync();
        }
    }

    //TODO: add comment support
    private async Task<ITranslationUnit> ReadTranslation(LineReader lineReader)
    {
        var line = await lineReader.ReadLineAsync() ?? string.Empty;

        var match = ReMsg.Match(line);
        if (!match.Success)
            throw new UnsupportedFormatException(this,
                $"Unsupported line '{line}' at line number {lineReader.LineNumber}.");

        var id = match.Groups["key"].Value;
        var value = match.Groups["value"].Value;
        value = value.Replace("%CRLF", "\n");
        
        var translation =  new TranslationString // Create translation string
        (
            id,
            value,
            Header.TargetLanguage
        );
        return new TranslationUnit(id)
        {
            Translations =
            {
                translation
            }
        };
    }

    /// <inheritdoc />
    public void Write(Stream stream)
    {
        WriteAsync(stream).Wait();
    }

    /// <summary>
    /// Asynchronously writes the format to the specified stream.
    /// </summary>
    /// <param name="stream">
    /// The stream to write to.
    /// </param>
    /// <exception cref="Exception">
    /// Thrown if the format is invalid.
    /// </exception>
    public async Task WriteAsync(Stream stream)
    {
        // NLang is UTF16 LE
        var writer = new StreamWriter(stream, Encoding.Unicode);

        foreach (var translationUnit in TranslationUnits)
        {
            if (translationUnit is not TranslationUnit nLangTranslationUnit)
                throw new Exception($"Unexpected translation unit: {translationUnit.GetType()}");
            await nLangTranslationUnit.WriteAsync(writer);
        }

        await writer.FlushAsync();
    }

    /// <inheritdoc />
    public Func<FormatProviderBuilder, IFormatProvider> BuildFormatProvider()
    {
        return builder => builder.SetId("nlang")
            .SetSupportedFileExtensions([".nlang3"])
            .SetFormatType<NLangFormat>()
            .SetFormatBuilder<NLangFormatBuilder>()
            .Create();
    }
}