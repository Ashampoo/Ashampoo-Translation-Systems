using System.Text.RegularExpressions;
using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using Microsoft.Toolkit.Diagnostics;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using IFormatProvider = Ashampoo.Translation.Systems.Formats.Abstractions.IFormatProvider;


namespace Ashampoo.Translation.Systems.Formats.Gengo;

/// <summary>
/// Implementation of the <see cref="IFormat"/> interface for the Gengo translation format.
/// </summary>
public class GengoFormat : AbstractTranslationUnits, IFormat
{
    /// <inheritdoc />
    public IFormatHeader Header { get; init; } = new DefaultFormatHeader();

    /// <inheritdoc />
    public LanguageSupport LanguageSupport => LanguageSupport.SourceAndTarget;

    /// <inheritdoc />
    public ICollection<ITranslationUnit> TranslationUnits { get; } = new List<ITranslationUnit>();

    private static readonly Regex
        RegexMarker =
            new(@"^\[{3}(?<id>.*)\]{3}$",
                RegexOptions.Singleline); // Regex to get the id with square brackets around it.

    private static readonly Regex
        RegexWithoutMarker = new(@"^(?<id>.*)$"); // Regex to get the id without square brackets around it.

    /// <inheritdoc />
    public async Task ReadAsync(Stream stream, FormatReadOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(options); // Options have to be provided

        var configureSuccess =
            await ConfigureOptionsAsync(options); // Configure the options if they are not already configured
        if (!configureSuccess)
        {
            options.IsCancelled = true;
            return;
        }

        var workbook = WorkbookFactory.Create(stream); // Create the workbook from the stream
        var sheet = workbook.GetSheetAt(0); // Get the first sheet

        Guard.IsNotNullOrWhiteSpace(Header.TargetLanguage,
            nameof(Header.TargetLanguage)); // The target language has to be set
        ReadTranslations(sheet); // Read the translations from the sheet
    }

    private async Task<bool> ConfigureOptionsAsync(FormatReadOptions options)
    {
        var setTargetLanguage =
            string.IsNullOrWhiteSpace(options.TargetLanguage); // Check if the target language needs to be set
        var setSourceLanguage =
            string.IsNullOrWhiteSpace(options.SourceLanguage); // Check if the source language needs to be set
        if (setTargetLanguage || setSourceLanguage)
        {
            if (options.FormatOptionsCallback is null)
                throw new InvalidOperationException("Callback for Format options required.");

            FormatStringOption sourceLanguageOption = new("Source language");
            FormatStringOption targetLanguageOption = new("Target language", true);

            List<FormatOption> optionList = new();
            if (setSourceLanguage) optionList.Add(sourceLanguageOption);
            if (setTargetLanguage) optionList.Add(targetLanguageOption);


            FormatOptions formatOptions = new()
            {
                Options = optionList.ToArray()
            };

            await options.FormatOptionsCallback.Invoke(formatOptions); // Invoke the callback to get the options
            if (formatOptions.IsCanceled) return false; // If the options were cancelled, return false


            Header.SourceLanguage =
                setSourceLanguage
                    ? sourceLanguageOption.Value
                    : options.SourceLanguage; // Set the source language if it was not set
            Header.TargetLanguage =
                setTargetLanguage
                    ? targetLanguageOption.Value
                    : options.TargetLanguage!; // Set the target language if it was not set
        }
        else
        {
            Header.SourceLanguage = options.SourceLanguage;
            Header.TargetLanguage = options.TargetLanguage!;
        }

        return true;
    }

    private void ReadTranslations(ISheet sheet)
    {
        for (var i = 1; i <= sheet.LastRowNum; i++) // loop through every row
        {
            var row = sheet.GetRow(i);
            if (row is null) continue; // Sometimes null rows are returned, skip them

            var translations = CreateTranslations(row); // Create the translations from the row
            if (translations is null) continue;

            var translationUnit = new DefaultTranslationUnit(translations.Item3) // Create the translation unit
            {
                Translations =
                {
                    translations.Item1,
                    translations.Item2
                }
            };
            TranslationUnits.Add(translationUnit); // Add the translation unit to the hash set of translation units
        }
    }

    private Tuple<ITranslation, ITranslation, string>? CreateTranslations(IRow row)
    {
        var idCell = row.TryGetCell(0); // Get the first cell
        if (idCell is null) return null; // If the cell is null, return null
        if (idCell.Address.Column != 0) return null; // If the first cell is not in the first column, skip the row
        var id = idCell.StringCellValue ?? string.Empty; // Get the id from the cell
        if (string.IsNullOrWhiteSpace(id)) return null; // If the id is empty, skip the row

        id = RemoveMarker(id); // Remove the marker from the id
        if (string.IsNullOrWhiteSpace(id)) return null; // If the id is empty, skip the row

        var sourceCell = row.TryGetCell(1); // Get the second cell
        if (sourceCell is null) return null; // If the cell is null, return null
        if (sourceCell.Address.Column != 1) return null; // If the second cell is not in the second column, skip the row
        var source = sourceCell.StringCellValue ?? string.Empty; // Get the source from the cell
        if (string.IsNullOrWhiteSpace(source)) return null; // If the source is empty, skip the row

        var target =
            row.TryGetStringCellValue(2) ??
            string.Empty; // Get the target from the third cell or an empty string if it is null

        var sourceTranslation = new DefaultTranslationString
        (
            id,
            source,
            Header.SourceLanguage ?? throw new ArgumentNullException(nameof(Header.SourceLanguage))
        );

        var targetTranslation = new DefaultTranslationString
        (
            id,
            target,
            Header.TargetLanguage ?? throw new ArgumentNullException(nameof(Header.TargetLanguage))
        );

        return new Tuple<ITranslation, ITranslation, string>(sourceTranslation, targetTranslation, id);
    }

    private string RemoveMarker(string str)
    {
        var match = RegexMarker.Match(str);
        if (!match.Success && !(match = RegexWithoutMarker.Match(str)).Success)
            throw new UnsupportedFormatException(this, "Incompatible ID-Format");

        return match.Groups["id"].Value;
    }

    /// <inheritdoc />
    public void Write(Stream stream)
    {
        XSSFWorkbook workbook = new(); // Create a new workbook
        var sheet = workbook.CreateSheet("Sheet 1"); // Create a new sheet

        CreateHeaderRow(sheet); // Create the header row
        var rowCount = 1; // The row count

        foreach (var translationUnit in TranslationUnits) // Loop through all translation units
        {
            var row = sheet.CreateRow(rowCount); // Create a new row

            for (var i = 0; i < 3; i++) // Create three cells
            {
                row.CreateCell(i);
            }

            row.Cells[0].SetCellValue($"[[[{translationUnit.Id}]]]"); // Set the id with square brackets around it
            row.Cells[1].SetCellValue(translationUnit.Translations.GetTranslation(Header.SourceLanguage!).Value);
            row.Cells[2].SetCellValue(translationUnit.Translations.GetTranslation(Header.TargetLanguage).Value);

            rowCount++;
        }

        AutosizeColumns(sheet, 0, 3); // Autosize the columns


        workbook.Write(stream,
            true); // Write the workbook to the stream, and leave the stream open TODO: Is this correct?
    }

    private void CreateHeaderRow(ISheet sheet)
    {
        var row = sheet.CreateRow(0); // Create a new row

        for (var j = 0; j < 3; j++) // Create three cells
        {
            row.CreateCell(j);
        }

        row.Cells[0].SetCellValue("[[[ID]]]"); // Set the header for the di column
        row.Cells[1].SetCellValue("source"); // Set the header for the source column
        row.Cells[2].SetCellValue("target"); // Set the header for the target column
    }

    private void AutosizeColumns(ISheet sheet, int start, int count)
    {
        for (var i = start; i < start + count; i++)
        {
            sheet.AutoSizeColumn(i);
        }
    }

    /// <inheritdoc />
    public Func<FormatProviderBuilder, IFormatProvider> BuildFormatProvider()
    {
        return builder => builder.SetId("gengo")
            .SetSupportedFileExtensions(new[] { ".xlsx", ".xls" })
            .SetFormatType<GengoFormat>()
            .SetFormatBuilder<GengoFormatBuilder>()
            .Create();
    }
}