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
    public void NewFormat()
    {
        var format = CreateFormat();

        format.Should().NotBeNull();
        format.TranslationUnits.Should().BeEmpty();
        format.Header.SourceLanguage.Should().BeNull();
        format.Header.TargetLanguage.Should().BeEmpty();
        format.LanguageSupport.Should().Be(LanguageSupport.SourceAndTarget);

        format = new GengoFormat
            { Header = new DefaultFormatHeader() { SourceLanguage = "en-US", TargetLanguage = "de-DE" } };
        format.Should().NotBeNull();
        format.TranslationUnits.Should().BeEmpty();
        format.Header.SourceLanguage.Should().Be("en-US");
        format.Header.TargetLanguage.Should().Be("de-DE");
    }


    [Fact]
    public void ReadFromFile()
    {
        var format = CreateAndReadFromFile("normalized-excel-test.xlsx",
            new FormatReadOptions { SourceLanguage = "de-DE", TargetLanguage = "en-US" });

        format.TranslationUnits.Count.Should().Be(4);

        const string id = "MESSAGES.MESSAGE_BETAVERSION_EXPIRED";
        var foundById = format.TranslationUnits.GetTranslationUnit(id);
        var translationString = foundById.Translations.GetTranslation("en-US");

        const string target =
            "Unfortunately, the beta version of the software has expired. Please install the final version of this software.%CRLFYou can download it from ‘www.ashampoo.com’.";
        foundById.Should().NotBeNull();
        foundById.Translations.Count.Should().Be(2);
        translationString.Value.Should().Be(target);
    }

    [Fact]
    public void ReadFromFileWithoutTarget()
    {
        IFormat format = CreateAndReadFromFile("empty-target-excel-test.xlsx",
            new FormatReadOptions { SourceLanguage = "de-DE", TargetLanguage = "en-US" });

        format.Header.TargetLanguage.Should().Be("en-US");
        format.Header.SourceLanguage.Should().Be("de-DE");

        format.TranslationUnits.Count.Should().Be(4);
        foreach (var unit in format.TranslationUnits)
        {
            unit.Translations.Count.Should().Be(2);
        }

        const string id = "MESSAGES.MESSAGE_BETAVERSION_EXPIRED";
        const string value =
            "Diese Betaversion der Software ist leider abgelaufen. Bitte installieren Sie die finale Version dieser Software.%CRLFSie können diese z.B. von \"www.ashampoo.com\" herunterladen.";
        var foundById = format.TranslationUnits.GetTranslationUnit(id);
        foundById.Should().NotBeNull();
        foundById.Translations.TryGetTranslation("de-DE", out var translation).Should().BeTrue();
        translation?.Value.Should().Be(value);
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

        format.TranslationUnits.Should().BeEmpty();
    }

    [Fact]
    public async Task EmptyCellsTest()
    {
        var format = await CreateAndReadFromFileAsync("empty-cells-excel-test.xlsx",
            new FormatReadOptions { SourceLanguage = "en-US", TargetLanguage = "de-DE" });
        format.TranslationUnits.Should().BeEmpty();
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

        options.IsCancelled.Should().BeTrue();
        format.TranslationUnits.Should().BeEmpty();
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

        exception.Message.Should().Be("Callback for Format options required.");
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

        format.Header.SourceLanguage.Should().Be("en-US");
        format.Header.TargetLanguage.Should().Be("de-DE");
        format.TranslationUnits.Should().ContainSingle();
        format.TranslationUnits.GetTranslationUnit("ID Test").Should().NotBeNull();
        format.TranslationUnits.GetTranslationUnit("ID Test").Translations.GetTranslation("en-US").Value.Should()
            .Be("Test source");
        format.TranslationUnits.GetTranslationUnit("ID Test").Translations.GetTranslation("de-DE").Value.Should()
            .Be("Test target");
    }
}