using System.Text;
using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Models;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using CommunityToolkit.Diagnostics;

namespace Ashampoo.Translation.Systems.Formats.CSV;

internal sealed class CsvFormat : IFormat
{
    public IFormatHeader Header { get; } = new DefaultFormatHeader();
    public LanguageSupport LanguageSupport => LanguageSupport.SourceAndTarget;
    public ICollection<ITranslationUnit> TranslationUnits { get; } = [];
    private char Delimiter { get; set; } = ';';

    public async Task ReadAsync(Stream stream, FormatReadOptions? options = null)
    {
        if (!await ConfigureOptionsAsync(options))
        {
            options!.IsCancelled = true;
            return;
        }

        Guard.IsNullOrWhiteSpace(Header.TargetLanguage.Value);
        Guard.IsNullOrWhiteSpace(Header.SourceLanguage?.Value);
    }

    public async Task WriteAsync(Stream stream)
    {
        await using StreamWriter writer = new(stream, leaveOpen: true, encoding: Encoding.UTF8);
        await WriteHeader(writer);
        foreach (var translationUnit in TranslationUnits)
        {
            foreach (var translation in translationUnit.Translations)
            {
                var sourceTranslation =
                    translationUnit.Translations.FirstOrDefault(t => t.Language == Header.SourceLanguage);
                await writer.WriteLineAsync(
                    $"{translationUnit.Id}{Delimiter}{sourceTranslation?.Value}{Delimiter}{translation.Value}" +
                    $"{Delimiter}{string.Join(". ", translation.Comments)}");
            }
        }

        await writer.FlushAsync();
    }

    private async Task WriteHeader(StreamWriter writer)
    {
        await writer.WriteLineAsync($"Target Language: {Header.TargetLanguage.Value}");
        await writer.WriteLineAsync($"Source Language: {Header.SourceLanguage?.Value}");
        await writer.WriteLineAsync($"Delimiter: {Delimiter}");
        await writer.WriteLineAsync();
        await writer.WriteLineAsync($"id{Delimiter}original{Delimiter}translation{Delimiter}comments");
    }

    // TODO: Can this be made Protected in a abstract class to avoid duplicate code?
    private async Task<bool> ConfigureOptionsAsync(FormatReadOptions? options)
    {
        if (string.IsNullOrWhiteSpace(options?.TargetLanguage.Value))
        {
            Guard.IsNotNull(options?.FormatOptionsCallback);

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
            Header.TargetLanguage = options.TargetLanguage;
        }

        if (Header.AdditionalHeaders.TryGetValue("delimiter", out var delimiter))
        {
            Delimiter = delimiter.ToCharArray().First();
        }

        return true;
    }
}