using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ashampoo.Translations.Formats.Abstractions;
using Ashampoo.Translations.Formats.Abstractions.Translation;
using Ashampoo.Translations.Formats.Abstractions.TranslationFilter;
using Ashampoo.Translations.TestBase;
using Xunit;

namespace Ashampoo.Translations.Formats.ResX.Tests;

public class FormatTest : FormatTestBase<ResXFormat>
{
    private readonly IFormatFactory formatFactory;

    public FormatTest(IFormatFactory formatFactory)
    {
        this.formatFactory = formatFactory;
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
    public async Task ReadFromFile()
    {
        var format =
            await CreateAndReadFromFileAsync("Res.en.resx", new FormatReadOptions { TargetLanguage = "en-US" });
        Assert.Equal(117, format.Count);
        Assert.Equal("en-US", format.Header.TargetLanguage);
        Assert.Equal("Remove Added Items", (format["Text_RemoveAddedItems"]?["en-US"] as ITranslationString)?.Value);
    }

    [Fact]
    public async Task ReadAndWrite()
    {
        var format =
            await CreateAndReadFromFileAsync("Res.en.resx", new FormatReadOptions { TargetLanguage = "en-US" });

        await using var ms = new MemoryStream();
        await format.WriteAsync(ms);
        ms.Seek(0, SeekOrigin.Begin);
    }

    [Fact]
    public void ImportSuccessTest()
    {
        IFormat format = CreateAndReadFromFile("Res.en.resx", new FormatReadOptions { TargetLanguage = "en-US" });

        const string id = "Button_RemoveAll";
        const string value = "Import Test";

        var importedWithUnits = format.ImportMockTranslationWithUnits(language: "en-US", id: id, value: value);
        Assert.NotNull(importedWithUnits);
        Assert.Single(importedWithUnits);
        Assert.Equal("Import Test", (format[id]?["en-US"] as ITranslationString)?.Value);
    }

    [Fact]
    public void NoMatchImportTest()
    {
        IFormat format = CreateAndReadFromFile("Res.en.resx", new FormatReadOptions { TargetLanguage = "en-US" });

        const string id = "Not matching Import-Id";
        const string value = "Import Test";
        var imported = format.ImportMockTranslationWithUnits(language: "en-US", id: id, value: value);

        Assert.Empty(imported);
    }

    [Fact]
    public void ImportEqualTranslationTest()
    {
        IFormat format = CreateAndReadFromFile("Res.en.resx", new FormatReadOptions { TargetLanguage = "en-US" });

        const string id = "Button_RemoveAll";
        const string value = "Remove All";
        var imported = format.ImportMockTranslationWithUnits(language: "en-US", id: id, value: value);

        Assert.Empty(imported);
    }

    [Fact]
    public async Task SimpleAssign()
    {
        var mock = MockFormatWithTranslationUnits.CreateMockFormatWithTranslationUnits("en-US", "ID", "Hello World");
        var converted = await mock.ConvertToAsync<ResXFormat>(formatFactory,
            new AssignOptions { TargetLanguage = "en-US", Filter = new DefaultTranslationFilter() });

        Assert.NotNull(converted);
        Assert.Single(converted);
        Assert.Single(converted["ID"] ?? Enumerable.Empty<ITranslation>());
        Assert.Equal("Hello World", (converted["ID"]?["en-US"] as ITranslationString)?.Value);
    }

    [Fact]
    public async Task ComplexAssign()
    {
        const string id = "peru.CSystem.CreateUniqueFileFailed";
        var mockFormat = new MockFormatWithTranslationUnits
        {
            { "en-US", id, "Error creating unique file name." },
            { "de-DE", id, "Fehler beim Erzeugen eines eindeutigen Dateinamens" }
        };

        var optionsEn = new AssignOptions { TargetLanguage = "en-US", Filter = new DefaultTranslationFilter() };
        var optionsDe = new AssignOptions { TargetLanguage = "de-DE", Filter = new DefaultTranslationFilter() };

        var convertedEnUs = await mockFormat.ConvertToAsync<ResXFormat>(formatFactory, optionsEn);
        var convertedDeDe = await mockFormat.ConvertToAsync<ResXFormat>(formatFactory, optionsDe);

        Assert.NotNull(convertedEnUs);
        Assert.NotNull(convertedDeDe);


        Assert.Equal("en-US", convertedEnUs.Header.TargetLanguage);
        Assert.Equal("de-DE", convertedDeDe.Header.TargetLanguage);

        Assert.Equal("Error creating unique file name.", (convertedEnUs[id]?["en-US"] as ITranslationString)?.Value);
        Assert.Equal("Fehler beim Erzeugen eines eindeutigen Dateinamens",
            (convertedDeDe[id]?["de-DE"] as ITranslationString)?.Value);
    }
}