using System.Globalization;
using System.Text;
using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.IO;
using Ashampoo.Translation.Systems.Formats.Abstractions.Models;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using CommunityToolkit.Diagnostics;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

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
        var record = line.GetRecord<CsvRecordFormat>();
        var translationString = new DefaultTranslationString(record.Translation, Header.TargetLanguage,
            record.Comments.Split('|').ToList());

        var unit = new DefaultTranslationUnit(record.Id)
        {
            Translations =
            {
                translationString
            }
        };

        if (Header.SourceLanguage is null) return Task.FromResult<ITranslationUnit>(unit);
        var sourceTranslationString =
            new DefaultTranslationString(record.Original, (Language)Header.SourceLanguage,
                record.Comments.Split('|').ToList());
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
        csvWriter.WriteHeader<CsvRecordFormat>();
        await csvWriter.NextRecordAsync();
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
        var record = new CsvRecordFormat
        {
            Id = unit.Id,
            Original = sourceTranslation?.Value ?? string.Empty,
            Translation = translation.Value,
            Comments = string.Join("|", translation.Comments)
        };
        writer.WriteRecord(record);
        await writer.NextRecordAsync();
    }

    private async Task WriteHeader(TextWriter writer)
    {
        await CsvFormatHeader.WriteHeader(writer);
        await writer.WriteLineAsync();
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

internal record CsvRecordFormat
{
    [Name("id")]
    public string Id { get; init; } = string.Empty;

    [Name("original")]
    public string Original { get; init; } = string.Empty;

    [Name("translation")]
    public string Translation { get; init; } = string.Empty;

    [Name("comments")]
    public string Comments { get; init; } = string.Empty;
}