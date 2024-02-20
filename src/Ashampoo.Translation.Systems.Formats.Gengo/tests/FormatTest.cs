using System;
using System.IO;
using System.Threading.Tasks;
using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using Ashampoo.Translation.Systems.TestBase;
using FluentAssertions;
using NPOI.XSSF.UserModel;
using Xunit;

namespace Ashampoo.Translation.Systems.Formats.Gengo.Tests;

public class FormatTest : FormatTestBase<GengoFormat>
{
    private readonly IFormatFactory _formatFactory;

    public FormatTest(IFormatFactory formatFactory)
    {
        _formatFactory = formatFactory;
    }

    private static XSSFWorkbook CreateFileWithHeaderRow()
    {
        var workbook = new XSSFWorkbook();
        var sheet = workbook.CreateSheet("Sheet 1");

        var row = sheet.CreateRow(0);
        for (var j = 0; j < 3; j++)
        {
            row.CreateCell(j);
        }

        row.Cells[0].SetCellValue("[[[ID]]]");
        row.Cells[1].SetCellValue("source");
        row.Cells[2].SetCellValue("target");

        return workbook;
    }

    [Fact]
    public void IsAssignableFrom()
    {
        IFormat format = CreateFormat();

        Assert.IsAssignableFrom<ITranslationUnits>(format);
    }

    [Fact]
    public void NewFormat()
    {
        var format = CreateFormat();

        Assert.NotNull(format);
        Assert.Empty(format);
        Assert.Null(format.Header.SourceLanguage);
        Assert.Equal(string.Empty, format.Header.TargetLanguage);
        Assert.Equal(LanguageSupport.SourceAndTarget, format.LanguageSupport);

        format = new GengoFormat { Header = new DefaultFormatHeader() { SourceLanguage = "en-US", TargetLanguage = "de-DE" } };
        Assert.NotNull(format);
        Assert.Empty(format);
        Assert.Equal("en-US", format.Header.SourceLanguage);
        Assert.Equal("de-DE", format.Header.TargetLanguage);
    }


    [Fact]
    public void ReadFromFile()
    {
        var format = CreateAndReadFromFile("normalized-excel-test.xlsx",
            new FormatReadOptions { SourceLanguage = "de-DE", TargetLanguage = "en-US" });

        Assert.Equal(4, format.Count);

        const string id = "MESSAGES.MESSAGE_BETAVERSION_EXPIRED";
        var foundById = format[id];
        var translationString = foundById?.Translations.GetTranslation("en-US");

        const string target =
            @"Unfortunately, the beta version of the software has expired. Please install the final version of this software.%CRLFYou can download it from ‘www.ashampoo.com’.";
        Assert.NotNull(foundById);
        Assert.Equal(2, foundById.Translations.Count);
        Assert.Equal(target, translationString?.Value);
    }

    [Fact]
    public void ReadFromFileWithoutTarget()
    {
        IFormat format = CreateAndReadFromFile("empty-target-excel-test.xlsx",
            new FormatReadOptions { SourceLanguage = "de-DE", TargetLanguage = "en-US" });

        Assert.Equal("en-US", format.Header.TargetLanguage);
        Assert.Equal("de-DE", format.Header.SourceLanguage);

        Assert.Equal(4, format.Count);
        foreach (var unit in format)
        {
            Assert.Equal(2, unit.Translations.Count);
        }

        const string id = "MESSAGES.MESSAGE_BETAVERSION_EXPIRED";
        const string value =
            "Diese Betaversion der Software ist leider abgelaufen. Bitte installieren Sie die finale Version dieser Software.%CRLFSie können diese z.B. von \"www.ashampoo.com\" herunterladen.";
        var foundById = format[id];
        foundById.Should().NotBeNull();
        foundById!.Translations.TryGetTranslation("de-DE", out var translation).Should().BeTrue();
        Assert.Equal(value, translation!.Value);
    }

    [Fact]
    public void ReadAndWrite()
    {
        IFormat format = CreateAndReadFromFile("normalized-excel-test.xlsx",
            new FormatReadOptions { SourceLanguage = "de-DE", TargetLanguage = "en-US" });

        var temp = Path.GetTempPath();
        using var outStream = new FileStream($"{temp}temp-normalized-excel-test.xlsx", FileMode.Create,
            FileAccess.Write, FileShare.ReadWrite);
        format.Write(outStream);
        outStream.Close();

        //TODO: check if files are equal
        // files not identical on binary level
        File.Delete($"{temp}temp-normalized-excel-test.xlsx");
    }

    [Fact]
    public void ReadWithoutTargetAndWrite()
    {
        IFormat format = CreateAndReadFromFile("empty-target-excel-test.xlsx",
            new FormatReadOptions { SourceLanguage = "de-DE", TargetLanguage = "en-US" });
        var temp = Path.GetTempPath();
        var outStream = new FileStream($"{temp}temp-empty-target-excel-test.xlsx", FileMode.Create,
            FileAccess.Write, FileShare.ReadWrite);
        format.Write(outStream);
        outStream.Close();
        //TODO: check if files are equal
        // files not identical on binary level
        File.Delete($"{temp}temp-empty-target-excel-test.xlsx");
    }

    [Fact]
    public void ImportSuccessTest()
    {
        IFormat format = CreateAndReadFromFile("normalized-excel-test.xlsx",
            new FormatReadOptions { SourceLanguage = "de-DE", TargetLanguage = "en-US" });

        const string id = "MESSAGES.MESSAGE_TRANSLATOR_NAME";
        const string valueSource = "Import Test Source";
        const string valueTarget = "Import Test Target";
        var importedWithUnits =
            format.ImportMockTranslationWithUnits(language: "de-DE", id: id, value: valueSource);

        Assert.NotNull(importedWithUnits);
        Assert.Single(importedWithUnits);
        Assert.Equal("Import Test Source", format[id]?.Translations.GetTranslation("de-DE")?.Value);

        importedWithUnits = format.ImportMockTranslationWithUnits(language: "en-US", id: id, value: valueTarget);
        Assert.NotNull(importedWithUnits);
        Assert.Single(importedWithUnits);
        Assert.Equal("Import Test Target", format[id]?.Translations.GetTranslation("en-US")?.Value);
    }

    [Fact]
    public void NoMatchImportTest()
    {
        IFormat format = CreateAndReadFromFile("normalized-excel-test.xlsx",
            new FormatReadOptions { SourceLanguage = "de-DE", TargetLanguage = "en-US" });

        const string id = "Not matching Import-Id";
        const string value = "Import Test";
        var imported = format.ImportMockTranslationWithUnits(language: "de-DE", id: id, value: value);
        Assert.Empty(imported);
    }

    [Fact]
    public void ImportEqualTranslationTest()
    {
        IFormat format = CreateAndReadFromFile("normalized-excel-test.xlsx",
            new FormatReadOptions { SourceLanguage = "de-DE", TargetLanguage = "en-US" });

        const string id = "MESSAGES.MESSAGE_TRANSLATOR_NAME";
        const string value = "Ashampoo Development GmbH & Co. KG";
        var imported = format.ImportMockTranslationWithUnits(language: "de-DE", id: id, value: value);
        Assert.Empty(imported);
    }

    [Fact]
    public async Task IncompatibleExcelFileTest()
    {
        var workbook = CreateFileWithHeaderRow();
        var sheet = workbook.GetSheetAt(0);
        var row = sheet.CreateRow(1);
        row.CreateCell(0);
        row.Cells[0].SetCellValue(string.Empty);

        sheet.CreateRow(3);

        var ms = new MemoryStream();
        workbook.Write(ms, true);
        ms.Seek(0, SeekOrigin.Begin);

        var format = CreateFormat();
        var options = new FormatReadOptions { SourceLanguage = "en-US", TargetLanguage = "de-DE" };
        await format.ReadAsync(ms, options);
    }

    [Fact]
    public async Task EmptySourceTest()
    {
        var workbook = CreateFileWithHeaderRow();
        var sheet = workbook.GetSheetAt(0);
        var row = sheet.CreateRow(1);
        row.CreateCell(0);
        row.CreateCell(1);

        row.Cells[0].SetCellValue("[[[ID Test]]]");
        row.Cells[1].SetCellValue(string.Empty);

        var ms = new MemoryStream();
        workbook.Write(ms, true);
        ms.Seek(0, SeekOrigin.Begin);

        var format = CreateFormat();
        var options = new FormatReadOptions { SourceLanguage = "en-US", TargetLanguage = "de-DE" };
        await format.ReadAsync(ms, options);

        Assert.Empty(format);
    }

    [Fact]
    public async Task EmptyCellsTest()
    {
        var format = await CreateAndReadFromFileAsync("empty-cells-excel-test.xlsx",
            new FormatReadOptions { SourceLanguage = "en-US", TargetLanguage = "de-DE" });
        Assert.Empty(format);
    }

    [Fact]
    public async Task OptionsCallbackCancelledTest()
    {
        Task OptionsCallback(FormatOptions options)
        {
            options.IsCanceled = true;
            return Task.CompletedTask;
        }

        var workbook = CreateFileWithHeaderRow();
        var sheet = workbook.GetSheetAt(0);
        var row = sheet.CreateRow(1);
        row.CreateCell(0);
        row.CreateCell(1);
        row.CreateCell(2);

        row.Cells[0].SetCellValue("[[[ID Test]]]");
        row.Cells[1].SetCellValue("Test source");
        row.Cells[2].SetCellValue("Test target");

        var ms = new MemoryStream();
        workbook.Write(ms, true);
        ms.Seek(0, SeekOrigin.Begin);

        var format = CreateFormat();
        var options = new FormatReadOptions { FormatOptionsCallback = OptionsCallback };
        await format.ReadAsync(ms, options);

        Assert.True(options.IsCancelled);
        Assert.Empty(format);
    }

    [Fact]
    public async Task NoOptionsCallbackTest()
    {
        var workbook = CreateFileWithHeaderRow();

        var ms = new MemoryStream();
        workbook.Write(ms, true);
        ms.Seek(0, SeekOrigin.Begin);

        var format = CreateFormat();
        var options = new FormatReadOptions();
        var exception =
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await format.ReadAsync(ms, options));

        Assert.Equal("Callback for Format options required.", exception.Message);
    }

    [Fact]
    public async Task OptionsCallbackTest()
    {
        Task OptionsCallback(FormatOptions options)
        {
            (options.Options[0] as FormatStringOption)!.Value = "en-US";
            (options.Options[1] as FormatStringOption)!.Value = "de-DE";
            return Task.CompletedTask;
        }

        var workbook = CreateFileWithHeaderRow();
        var sheet = workbook.GetSheetAt(0);
        var row = sheet.CreateRow(1);
        row.CreateCell(0);
        row.CreateCell(1);
        row.CreateCell(2);

        row.Cells[0].SetCellValue("[[[ID Test]]]");
        row.Cells[1].SetCellValue("Test source");
        row.Cells[2].SetCellValue("Test target");

        var ms = new MemoryStream();
        workbook.Write(ms, true);
        ms.Seek(0, SeekOrigin.Begin);

        var format = CreateFormat();
        var options = new FormatReadOptions { FormatOptionsCallback = OptionsCallback };
        await format.ReadAsync(ms, options);

        Assert.Equal("en-US", format.Header.SourceLanguage);
        Assert.Equal("de-DE", format.Header.TargetLanguage);
        Assert.Single(format);
        Assert.NotNull(format["ID Test"]);
        Assert.Equal("Test source", format["ID Test"]?.Translations.GetTranslation("en-US")?.Value);
        Assert.Equal("Test target", format["ID Test"]?.Translations.GetTranslation("de-DE")?.Value);
    }
}