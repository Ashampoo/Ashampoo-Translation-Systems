using System.Text;
using System.Text.RegularExpressions;
using Ashampoo.Translations.Formats.Abstractions;
using Ashampoo.Translations.Formats.Abstractions.IO;
using Ashampoo.Translations.Formats.Abstractions.Translation;
using CommunityToolkit.Diagnostics;
using IFormatProvider = Ashampoo.Translations.Formats.Abstractions.IFormatProvider;

namespace Ashampoo.Translations.Formats.NLang;

/// <summary>
/// Implementation of <see cref="IFormat"/> interface for the NLang format.
/// </summary>
public class NLangFormat : AbstractTranslationUnits, IFormat
{
    private static readonly Regex ReMsg = new(@"(?<key>.*?)=(?<value>.*)");
    public IFormatHeader Header { get; init; } = new DefaultFormatHeader();

    public FormatLanguageCount LanguageCount => FormatLanguageCount.OnlyTarget;

    public void Read(Stream stream, FormatReadOptions? options = null)
    {
        ReadAsync(stream, options).Wait();
    }

    public async Task ReadAsync(Stream stream, FormatReadOptions? options = null)
    {
        var configureSuccess = await ConfigureOptionsAsync(options); // Configure options
        if (!configureSuccess)
        {
            options!.IsCancelled = true;
            return;
        }

        Guard.IsNotNullOrWhiteSpace(Header.TargetLanguage,
            nameof(Header.TargetLanguage)); // Target language is required

        // TODO: Dispose of streams and readers?
        var streamReader = new StreamReader(stream);
        var lineReader = new LineReader(streamReader);

        await ReadTranslations(lineReader);
    }

    private async Task<bool> ConfigureOptionsAsync(FormatReadOptions? options)
    {
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

            await options.FormatOptionsCallback.Invoke(formatOptions); // Invoke callback
            if (formatOptions.IsCanceled) return false;

            Header.TargetLanguage = targetLanguageOption.Value;
        }
        else
        {
            Header.TargetLanguage = options.TargetLanguage;
        }

        return true;
    }

    private async Task ReadTranslations(LineReader lineReader)
    {
        while (await lineReader.HasMoreLinesAsync())
        {
            var translation = await ReadTranslation(lineReader); // Read translation
            TranslationUnit translationUnit = new(id: translation.Id) // Create translation unit
            {
                [translation.Language] = translation
            };
            Add(translationUnit);
        }
    }

    //TODO: add comment support
    private async Task<ITranslation> ReadTranslation(LineReader lineReader)
    {
        await lineReader.SkipEmptyLinesAsync();
        var line = await lineReader.ReadLineAsync() ?? string.Empty;

        var match = ReMsg.Match(line);
        if (!match.Success)
            throw new UnsupportedFormatException(this,
                $"Unsupported line '{line}' at line number {lineReader.LineNumber}.");

        var key = match.Groups["key"].Value;
        var value = match.Groups["value"].Value;
        value = value.Replace("%CRLF", "\n");
        return new TranslationString // Create translation string
        (
            key,
            value,
            Header.TargetLanguage
        );
    }

    public void Write(Stream stream)
    {
        WriteAsync(stream).Wait();
    }

    public async Task WriteAsync(Stream stream)
    {
        // NLang is UTF16 LE
        var writer = new StreamWriter(stream, Encoding.Unicode);

        foreach (var translationUnit in this)
        {
            if (translationUnit is not TranslationUnit nLangTranslationUnit)
                throw new Exception($"Unexpected translation unit: {translationUnit.GetType()}");
            await nLangTranslationUnit.WriteAsync(writer);
        }

        await writer.FlushAsync();
    }
    
    public Func<FormatProviderBuilder, IFormatProvider> BuildFormatProvider()
    {
        return builder => builder.SetId("nlang")
            .SetSupportedFileExtensions(new[] { ".nlang3" })
            .SetFormatType<NLangFormat>()
            .SetFormatBuilder<NLangFormatBuilder>()
            .Create();
    }
}