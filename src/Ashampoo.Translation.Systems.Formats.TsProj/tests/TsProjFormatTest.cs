using System;
using System.IO;
using System.Threading.Tasks;
using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using Ashampoo.Translation.Systems.Formats.Abstractions.TranslationFilter;
using Ashampoo.Translation.Systems.TestBase;
using Xunit;

namespace Ashampoo.Translation.Systems.Formats.TsProj.Tests;

public class TsProjFormatTest : FormatTestBase<TsProjFormat>
{
    private readonly IFormatFactory formatFactory;

    public TsProjFormatTest(IFormatFactory formatFactory)
    {
        this.formatFactory = formatFactory;
    }

    [Fact]
    public void IsAssignableFrom()
    {
        IFormat format = CreateFormat();

        //provides translation units
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
    }

    [Fact]
    public void ReadFromFile()
    {
        IFormat format = CreateAndReadFromFile("normalized_export_ashlang-de-DE.tsproj");

        Assert.Equal(67, format.Count);
        Assert.Equal("en-US", format.Header.SourceLanguage);
        Assert.Equal("de-DE", format.Header.TargetLanguage);
        //TODO: add author to tsproj

        const string id = "peru.CFileNotFoundError.Desc";
        Assert.Equal("The file '%FILE%' was not found.", format[id]?.Translations.GetTranslation("en-US")?.Value);
        Assert.Equal("Die Datei '%FILE%' wurde nicht gefunden.",
            format[id]?.Translations.GetTranslation("de-DE")?.Value);
    }

    [Fact]
    public void ImportSuccessTest()
    {
        IFormat format = CreateAndReadFromFile("normalized_export_ashlang-de-DE.tsproj");


        const string id = "peru.gui.CImageConversionError.Desc";
        const string valueSource = "Import Test Source";
        const string valueTarget = "Import Test Target";
        var importedWithUnits =
            format.ImportMockTranslationWithUnits(language: "en-US", id: id, value: valueSource);

        Assert.NotNull(importedWithUnits);
        Assert.Single(importedWithUnits);
        Assert.Equal("Import Test Source", format[id]?.Translations.GetTranslation("en-US")?.Value);

        importedWithUnits = format.ImportMockTranslationWithUnits(language: "de-DE", id: id, value: valueTarget);

        Assert.NotNull(importedWithUnits);
        Assert.Single(importedWithUnits);
        Assert.Equal("Import Test Target", format[id]?.Translations.GetTranslation("de-DE")?.Value);
    }

    [Fact]
    public void NoMatchImportTest()
    {
        IFormat format = CreateAndReadFromFile("normalized_export_ashlang-de-DE.tsproj");

        const string id = "Not matching Import-Id";
        const string value = "Import Test";

        var imported = format.ImportMockTranslationWithUnits(language: "en-US", id: id, value: value);
        Assert.Empty(imported);

        imported = format.ImportMockTranslationWithUnits(language: "de-DE", id: id, value: value);
        Assert.Empty(imported);
    }

    [Fact]
    public void ImportEqualTranslationTest()
    {
        var format = CreateAndReadFromFile("normalized_export_ashlang-de-DE.tsproj");

        const string id = "peru.gui.CImageConversionError.Desc";
        const string value = "Fehler beim konvertieren der Bilddaten (%DETAILS%).";
        var imported = format.ImportMockTranslationWithUnits(language: "de-DE", id: id, value: value);

        Assert.Empty(imported);
    }


    [Fact]
    public async Task ReadAndWriteAshLangExportedFile()
    {
        var format = await CreateAndReadFromFileAsync("normalized_export_ashlang-de-DE.tsproj");

        var ms = new MemoryStream();
        await format.WriteAsync(ms);
        ms.Seek(0, SeekOrigin.Begin);

        await using var fs = CreateFileInStream("normalized_export_ashlang-de-DE.tsproj");

        //FIXME: compare formats like in the other tests, and not the streams!
        //ms.MustBeEqualTo(fs);
    }

    [Fact]
    public async Task ReadAndWriteNLangExportedFile()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await CreateAndReadFromFileAsync("normalized_export_nlang-de-DE.tsproj"));
    }

    [Fact]
    public async Task ConvertTest()
    {
        var mockFormat =
            MockFormatWithTranslationUnits.CreateMockFormatWithTranslationUnits(language: "de-DE",
                id: "Convert test", value: "Hallo Welt");
        var options = new AssignOptions
            { SourceLanguage = "de-DE", TargetLanguage = "en-US", Filter = new DefaultTranslationFilter() };
        var convertedFormat = await mockFormat.ConvertToAsync<TsProjFormat>(formatFactory, options);

        Assert.NotNull(convertedFormat);
        Assert.Single(convertedFormat);
        Assert.Equal("Hallo Welt", convertedFormat["Convert test"]?.Translations.GetTranslation("de-DE")?.Value);
    }

    [Fact]
    public async Task AssignWithSimpleFilter()
    {
        var mockFormat =
            MockFormatWithTranslationUnits.CreateMockFormatWithTranslationUnits(language: "de-DE",
                id: "Convert test", value: "Hallo Welt");
        var options = new AssignOptions
            { Filter = new IsEmptyTranslationFilter(), SourceLanguage = "de-DE", TargetLanguage = "en-US" };
        var assignedFormat = await mockFormat.ConvertToAsync<TsProjFormat>(formatFactory, options);

        Assert.NotNull(assignedFormat);
        Assert.Empty(assignedFormat);
    }
}