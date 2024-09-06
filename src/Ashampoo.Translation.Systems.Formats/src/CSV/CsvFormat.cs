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
    public IFormatHeader Header { get; }

    /// <inheritdoc />
    public LanguageSupport LanguageSupport => LanguageSupport.SourceAndTarget;

    /// <inheritdoc />
    public ICollection<ITranslationUnit> TranslationUnits { get; } = [];

    private CsvFormatHeader CsvFormatHeader { get; }

    private char Delimiter => CsvFormatHeader.Delimiter;

    private const char CommentDelimiter = '|';

    public CsvFormat()
    {
        var csvFormatHeader = new CsvFormatHeader();
        Header = csvFormatHeader;
        CsvFormatHeader = csvFormatHeader;
    }

    internal CsvFormat(CsvFormatHeader header)
    {
        Header = header;
        CsvFormatHeader = header;
    }

    /// <inheritdoc />
    public async Task ReadAsync(Stream stream, FormatReadOptions? options = null)
    {
        Guard.IsNotNull(options);
        using StreamReader reader = new(stream);
        await GetHeaderInformation(reader, options);

        if (!await ConfigureOptionsAsync(options))
        {
            options.IsCancelled = true;
            return;
        }

        Guard.IsNotNullOrWhiteSpace(Header.TargetLanguage.Value);
        Guard.IsNotNullOrWhiteSpace(Header.SourceLanguage?.Value);

        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            TrimOptions = TrimOptions.None,
            Delimiter = Delimiter.ToString(),
            Mode = CsvMode.RFC4180
        });
        await ReadCsv(csv);
    }

    private async Task GetHeaderInformation(StreamReader reader, FormatReadOptions options)
    {
        var lineReader = new LineReader(reader);
        await lineReader.SkipEmptyLinesAsync();
        while (await lineReader.HasMoreLinesAsync())
        {
            var line = await lineReader.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(line) || !line.StartsWith('#')) return;
            var headerLine = line.Split(':');
            switch (headerLine[0].Trim())
            {
                case "#Delimiter":
                    if (char.IsWhiteSpace(Delimiter) && !string.IsNullOrWhiteSpace(headerLine[1]))
                    {
                        CsvFormatHeader.Delimiter = headerLine[1].Trim().ToCharArray().First();
                    }

                    break;
                case "#Source Language":
                    if (string.IsNullOrWhiteSpace(options.SourceLanguage?.Value) && !string.IsNullOrWhiteSpace(headerLine[1]))
                    {
                        Header.SourceLanguage = new Language(headerLine[1].Trim());
                    }

                    break;
                case "#Target Language":
                    if (string.IsNullOrWhiteSpace(options.TargetLanguage.Value) && !string.IsNullOrWhiteSpace(headerLine[1]))
                    {
                        Header.TargetLanguage = new Language(headerLine[1].Trim());
                    }

                    break;
            }
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

        if (string.IsNullOrWhiteSpace(record.Original)) return Task.FromResult<ITranslationUnit>(unit);
        var sourceTranslationString =
            new DefaultTranslationString(record.Original, (Language)Header.SourceLanguage!,
                record.Comments.Split('|').ToList());
        unit.Translations.Add(sourceTranslationString);

        return Task.FromResult<ITranslationUnit>(unit);
    }

    /// <inheritdoc />
    public async Task WriteAsync(Stream stream)
    {
        Guard.IsNotNullOrWhiteSpace(Delimiter.ToString());
        await using StreamWriter writer = new(stream, leaveOpen: true, encoding: Encoding.UTF8);
        await WriteHeader(writer);
        var csvWriter = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = Delimiter.ToString(),
            DetectDelimiter = false,
            TrimOptions = TrimOptions.None,
            Mode = CsvMode.RFC4180
        });
        csvWriter.WriteHeader<CsvRecordFormat>();
        await csvWriter.NextRecordAsync();
        foreach (var translationUnit in TranslationUnits)
        {
            await WriteRow(translationUnit, csvWriter);
        }

        await csvWriter.FlushAsync();
        await writer.FlushAsync();
    }

    private async Task WriteRow(ITranslationUnit unit, CsvWriter writer)
    {
        var sourceTranslation =
            unit.Translations.FirstOrDefault(t => t.Language == Header.SourceLanguage);
        var translation = unit.Translations.FirstOrDefault(t =>
            t.Language.Value == Header.TargetLanguage.Value);
        var record = new CsvRecordFormat
        {
            Id = unit.Id,
            Original = sourceTranslation?.Value ?? string.Empty,
            Translation = translation?.Value ?? string.Empty,
            Comments = string.Join(CommentDelimiter, translation?.Comments ?? [])
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
    private async Task<bool> ConfigureOptionsAsync(FormatReadOptions options)
    {
        var setTargetLanguage =
            options.TargetLanguage.IsNullOrWhitespace() && Header.TargetLanguage.IsNullOrWhitespace();
        var setSourceLanguage =
            options.SourceLanguage.IsNullOrWhitespace() && Header.SourceLanguage.IsNullOrWhitespace();
        if (setTargetLanguage || setSourceLanguage || char.IsWhiteSpace(Delimiter))
        {
            Guard.IsNotNull(options.FormatOptionsCallback);

            FormatStringOption targetLanguageOption = new("Target language", true);
            FormatStringOption sourceLanguageOption = new("Source language", true);
            FormatCharacterOption delimiterOption = new("Delimiter", true);

            List<FormatOption> optionList = [];
            if (setTargetLanguage) optionList.Add(targetLanguageOption);
            if (setSourceLanguage) optionList.Add(sourceLanguageOption);
            if (char.IsWhiteSpace(Delimiter)) optionList.Add(delimiterOption);

            FormatOptions formatOptions = new()
            {
                Options = optionList.ToArray()
            };

            await options.FormatOptionsCallback.Invoke(formatOptions); // Invoke callback
            if (formatOptions.IsCanceled) return false;

            Header.SourceLanguage =
                setSourceLanguage
                    ? Language.Parse(sourceLanguageOption.Value)
                    : Header.SourceLanguage;
            Header.TargetLanguage =
                setTargetLanguage
                    ? Language.Parse(targetLanguageOption.Value)
                    : Header.TargetLanguage;
            CsvFormatHeader.Delimiter = char.IsWhiteSpace(Delimiter) ? delimiterOption.Value : Delimiter;
        }
        else
        {
            Header.TargetLanguage = Header.TargetLanguage.IsNullOrWhitespace()
                ? options.TargetLanguage
                : Header.TargetLanguage;
            Header.SourceLanguage = Header.SourceLanguage.IsNullOrWhitespace()
                ? options.SourceLanguage
                : Header.SourceLanguage;
        }

        return true;
    }
}

file record CsvRecordFormat
{
    [Name("id")] public string Id { get; init; } = string.Empty;

    [Name("original")] public string Original { get; init; } = string.Empty;

    [Name("translation")] public string Translation { get; init; } = string.Empty;

    [Name("comments")] public string Comments { get; init; } = string.Empty;
}