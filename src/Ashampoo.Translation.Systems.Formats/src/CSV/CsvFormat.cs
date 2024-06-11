using System.Globalization;
using System.Text;
using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.IO;
using Ashampoo.Translation.Systems.Formats.Abstractions.Models;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using CommunityToolkit.Diagnostics;
using CsvHelper;
using CsvHelper.Configuration;

namespace Ashampoo.Translation.Systems.Formats.CSV;

/// <summary>
/// Represents a Csv files format.
/// </summary>
public sealed class CsvFormat : IFormat
{
    /// <inheritdoc />
    public IFormatHeader Header { get; } = new CsvFormatHeader();

    /// <inheritdoc />
    public LanguageSupport LanguageSupport => LanguageSupport.SourceAndTarget;

    /// <inheritdoc />
    public ICollection<ITranslationUnit> TranslationUnits { get; } = [];

    private CsvFormatHeader CsvFormatHeader =>
        Header as CsvFormatHeader ?? throw new InvalidOperationException("Wrong header format");

    private string Delimiter => CsvFormatHeader.Delimiter.ToString();

    /// <inheritdoc />
    public async Task ReadAsync(Stream stream, FormatReadOptions? options = null)
    {
        if (!await ConfigureOptionsAsync(options))
        {
            options!.IsCancelled = true;
            return;
        }

        Guard.IsNotNullOrWhiteSpace(Header.TargetLanguage.Value);
        Guard.IsNotNullOrWhiteSpace(Header.SourceLanguage?.Value);

        using StreamReader reader = new(stream);
        await GetHeaderInformation(reader);

        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            TrimOptions = TrimOptions.Trim,
            DetectDelimiter = string.IsNullOrWhiteSpace(Delimiter),
            Delimiter = string.IsNullOrWhiteSpace(Delimiter) ? string.Empty : Delimiter
        });
        await ReadCsv(csv);
    }

    private async Task GetHeaderInformation(StreamReader reader)
    {
        var lineReader = new LineReader(reader);
        await lineReader.SkipEmptyLinesAsync();
        while (await lineReader.HasMoreLinesAsync())
        {
            var line = await lineReader.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(line) || !line.StartsWith('#')) return;
            var headerLine = line.Split(':');
            CsvFormatHeader.Delimiter = headerLine[0].Trim() switch
            {
                "#Delimiter" => headerLine[1].Trim().ToCharArray().First(),
                _ => CsvFormatHeader.Delimiter
            };
        }
    }

    private async Task ReadCsv(CsvReader reader)
    {
        await reader.ReadAsync();
        reader.ReadHeader();
        while (await reader.ReadAsync())
        {
            TranslationUnits.Add(await ReadLine(reader));
        }
    }

    private Task<ITranslationUnit> ReadLine(CsvReader line)
    {
        var id = line.GetField<string>(0);
        var sourceTranslation = line.GetField<string>(1);
        var translation = line.GetField<string>(2);
        var comment = line.GetField<string>(3);

        var translationString = new DefaultTranslationString(translation, Header.TargetLanguage, [comment]);

        var unit = new DefaultTranslationUnit(id)
        {
            Translations =
            {
                translationString
            }
        };

        if (Header.SourceLanguage is null) return Task.FromResult<ITranslationUnit>(unit);
        var sourceTranslationString =
            new DefaultTranslationString(sourceTranslation, (Language)Header.SourceLanguage, [comment]);
        unit.Translations.Add(sourceTranslationString);

        return Task.FromResult<ITranslationUnit>(unit);
    }

    /// <inheritdoc />
    public async Task WriteAsync(Stream stream)
    {
        ApplyDelimiter();
        await using StreamWriter writer = new(stream, leaveOpen: true, encoding: Encoding.UTF8);
        await WriteHeader(writer);
        var csvWriter = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = Delimiter,
            DetectDelimiter = false,
            TrimOptions = TrimOptions.Trim,
        });
        foreach (var translationUnit in TranslationUnits)
        {
            foreach (var translation in translationUnit.Translations.Where(t =>
                         t.Language.Value == Header.TargetLanguage.Value))
            {
                await WriteRow(translationUnit, translation, csvWriter);
            }
        }

        await csvWriter.FlushAsync();
        await writer.FlushAsync();
    }

    private async Task WriteRow(ITranslationUnit unit, ITranslation translation, CsvWriter writer)
    {
        var sourceTranslation =
            unit.Translations.FirstOrDefault(t => t.Language == Header.SourceLanguage);
        writer.WriteRecord(new
        {
            id = unit.Id, original = sourceTranslation?.Value, translation = translation.Value,
            comments = string.Join(", ", translation.Comments)
        });
        await writer.NextRecordAsync();
    }

    private async Task WriteHeader(TextWriter writer)
    {
        await CsvFormatHeader.WriteHeader(writer);
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

        Header.SourceLanguage = options.SourceLanguage;
        return true;
    }

    private void ApplyDelimiter()
    {
        if (!Header.AdditionalHeaders.TryGetValue("delimiter", out var delimiter)) return;
        var actualDelimiter = delimiter.ToCharArray().FirstOrDefault();
        if (actualDelimiter is ',' or ';' or '|')
        {
            CsvFormatHeader.Delimiter = actualDelimiter;
        }
    }
}