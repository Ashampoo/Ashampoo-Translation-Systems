using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using Ashampoo.Translation.Systems.Formats.Abstractions.TranslationFilter;
using Ashampoo.Translation.Systems.TestBase;
using Xunit;

namespace Ashampoo.Translation.Systems.Formats.NLang.Tests;

public class FormatTest : FormatTestBase<NLangFormat>
{
    private readonly IFormatFactory formatFactory;

    public FormatTest(IFormatFactory formatFactory)
    {
        this.formatFactory = formatFactory;
    }

    [Fact]
    public void IsAssignableFrom()
    {
        IFormat format = CreateFormat();
        // provides translation units
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
        IFormat format = CreateAndReadFromFile("de-de.nlang3", new FormatReadOptions { TargetLanguage = "de-DE" });


        foreach (var translationUnit in format)
        {
            Assert.Single(translationUnit);
        }


        Assert.Equal(3223, format.Count);

        const string id = "Form_BOOT.MenuItem_TASK_Delete";

        var foundById = format[id];
        var translationString = foundById?["de-DE"] as TranslationString;
        Assert.NotNull(foundById);
        Assert.NotNull(translationString);
        Assert.Equal("Löschen", translationString.Value);
        Assert.Null(translationString?.Comment);
        Assert.Equal(id, translationString?.Id);
    }

    [Fact]
    public void ReadAndWrite()
    {
        IFormat format = CreateAndReadFromFile("de-de.nlang3", new FormatReadOptions { TargetLanguage = "de-DE" });

        var ms = new MemoryStream();
        format.Write(ms);
        ms.Seek(0, SeekOrigin.Begin);

        // var fs = createFileInStream("de-de.nlang3");

        //FIXME: compare formats like in the other tests, and not the streams!
        //fs.MustBeEqualTo(ms);
    }

    [Fact]
    public void ImportSuccessTest()
    {
        IFormat format = CreateAndReadFromFile("de-de.nlang3", new FormatReadOptions { TargetLanguage = "de-DE" });

        const string id = "MESSAGES.MESSAGE_TRANSLATOR_NAME";
        const string value = "Import Test";

        var importedWithUnits = format.ImportMockTranslationWithUnits(language: "de-DE", id: id, value: value);
        Assert.NotNull(importedWithUnits);
        Assert.Single(importedWithUnits);
        Assert.Equal("Import Test", (format[id]?["de-DE"] as ITranslationString)?.Value);
    }

    [Fact]
    public void NoMatchImportTest()
    {
        IFormat format = CreateAndReadFromFile("de-de.nlang3", new FormatReadOptions { TargetLanguage = "de-DE" });

        const string id = "Not matching Import-Id";
        const string value = "Import Test";
        var imported = format.ImportMockTranslationWithUnits(language: "de-DE", id: id, value: value);

        Assert.Empty(imported);
    }

    [Fact]
    public void ImportEqualTranslationTest()
    {
        IFormat format = CreateAndReadFromFile("de-de.nlang3", new FormatReadOptions { TargetLanguage = "de-DE" });

        const string id = "MESSAGES.MESSAGE_TRANSLATOR_NAME";
        const string value = "Ashampoo Development GmbH & Co. KG";
        var imported = format.ImportMockTranslationWithUnits(language: "de-DE", id: id, value: value);

        Assert.Empty(imported);
    }

    [Fact]
    public async Task ConvertTest()
    {
        var mockFormat =
            MockFormatWithTranslationUnits.CreateMockFormatWithTranslationUnits("en-US", "Convert ID", "Convert Test");
        var assignOptions = new AssignOptions { TargetLanguage = "en-US", Filter = new DefaultTranslationFilter() };
        var nlang = await mockFormat.ConvertToAsync<NLangFormat>(formatFactory, assignOptions);

        Assert.NotNull(nlang);
        Assert.Single(nlang);
        Assert.Equal("Convert ID", nlang.First().Id);
        Assert.Equal("Convert Test", (nlang["Convert ID"]?["en-US"] as ITranslationString)?.Value);
    }
}