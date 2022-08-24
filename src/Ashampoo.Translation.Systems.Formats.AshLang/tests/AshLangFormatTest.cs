using System;
using System.IO;
using System.Threading.Tasks;
using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using Ashampoo.Translation.Systems.Formats.Abstractions.TranslationFilter;
using Ashampoo.Translation.Systems.TestBase;
using Xunit;

namespace Ashampoo.Translation.Systems.Formats.AshLang.Tests;

public class AshLangFormatTest : FormatTestBase<AshLangFormat>
{
    private readonly IFormatFactory formatFactory;

    public AshLangFormatTest(IFormatFactory formatFactory)
    {
        this.formatFactory = formatFactory;
    }

    [Fact]
    public void NewFormat()
    {
        var format = CreateFormat();

        Assert.Empty(format);

        // source sets are 'en-US' per default.
        Assert.Equal("en-US", format.Header.SourceLanguage);

        format.Header.TargetLanguage = "es-ES";
        Assert.Equal("es-ES", format.Header.TargetLanguage);

        // set Language null must throw.
        var thrownException = Assert.Throws<ArgumentNullException>(() => format.Header.TargetLanguage = null!);
        Assert.Equal("Value cannot be null. (Parameter 'TargetLanguage')", thrownException.Message);

        // author is nullable and can be changed.
        //Assert.Null(format.Header.Author);
        //format.Header.Author = "Ashampoo";
        //Assert.Equal("Ashampoo", format.Header.Author);
        //format.Header.Author = null;
        //Assert.Null(format.Header.Author);
    }

    [Fact]
    public void ImportSuccessTest()
    {
        var format = CreateAndReadFromFile("normalized_peru-de-DE.ashLang");

        const string id = "peru.CFileSystemManager.MoveFolderFailed.CreateFolder";
        const string value = "Import Test";
        var imported = format.ImportMockTranslationWithUnits("de-DE", id);

        var translationUnit = format[id];
        Assert.NotNull(translationUnit);
        Assert.Equal(value, (translationUnit["de-DE"] as ITranslationString)?.Value);
        Assert.Equal(1, imported.Count);
    }

    [Fact]
    public void NoMatchImportTest()
    {
        var format = CreateAndReadFromFile("normalized_peru-de-DE.ashLang");

        const string id = "Not matching Import-Id";
        var imported = format.ImportMockTranslationWithUnits("de-DE", id);

        Assert.Equal(0, imported.Count);
    }

    [Fact]
    public void ImportEqualTranslationTest()
    {
        var format = CreateAndReadFromFile("normalized_peru-de-DE.ashLang");

        const string id = "peru.CFileSystemManager.MoveFolderFailed.CreateFolder";
        const string value =
            "Fehler beim Verschieben von Ordner '%SRC%' nach '%DEST%''. Konnte Ordner nicht erstellen.";
        var imported = format.ImportMockTranslationWithUnits("de-DE", id, value);

        Assert.Equal(0, imported.Count);
    }

    [Fact]
    public void ReadFromFile()
    {
        var format = CreateAndReadFromFile("normalized_peru-de-DE.ashLang");

        Assert.Equal(67, format.Count);
        Assert.Equal("en-US", format.Header.SourceLanguage);
        //Assert.Null(format.Header.Author);
        Assert.Equal("de-DE", format.Header.TargetLanguage);
        //Assert.Equal("Ashampoo", format.Header.Author);

        const string id = "peru.CSystem.MoveFileFailed";

        var translationUnit = format[id];
        Assert.NotNull(translationUnit);
        Assert.Equal(id, translationUnit.Id);
        Assert.Equal("Error moving file '%SRC%' to '%DEST%'.", (translationUnit["en-US"] as ITranslationString)?.Value);
        Assert.Equal("Fehler beim Verschieben der Datei '%SRC%' nach '%DEST%'.",
            (translationUnit["de-DE"] as ITranslationString)?.Value);
    }

    [Fact]
    public void ReadAndWrite()
    {
        var format = CreateAndReadFromFile("normalized_peru-de-DE.ashLang");

        var ms = new MemoryStream();
        format.Write(ms);
        ms.Seek(0, SeekOrigin.Begin);

        // var fs = createFileInStream("normalized_peru-de-DE.ashLang");

        //FIXME: compare formats like in the other tests, and not the streams!
        //ms.MustBeEqualTo(fs);
    }

    [Fact]
    public async Task ConvertTest()
    {
        var mockFormat =
            MockFormatWithTranslationUnits.CreateMockFormatWithTranslationUnits("en-US", "Convert ID", "Convert Test");

        var assignOptions = new AssignOptions
        {
            SourceLanguage = "en-US",
            TargetLanguage = "de-DE",
            Filter = new DefaultTranslationFilter()
        };

        var ashLang = await mockFormat.ConvertToAsync<AshLangFormat>(formatFactory, assignOptions);

        Assert.Equal("en-US", ashLang.Header.SourceLanguage);
        Assert.Equal("de-DE", ashLang.Header.TargetLanguage);

        Assert.Single(ashLang);
        Assert.Equal(2, ashLang["Convert ID"]?.Count);
        Assert.Equal("Convert Test", (ashLang["Convert ID"]?["en-US"] as ITranslationString)?.Value);
        Assert.Null(ashLang["Convert Test"]?.TryGet("de-DE"));
    }

    [Fact]
    public void FormatBuilderTest()
    {
        var builder =
            (IFormatBuilderWithSourceAndTarget)formatFactory.GetFormatProvider(typeof(AshLangFormat))
                .GetFormatBuilder();

        builder.SetTargetLanguage("de-DE");
        builder.Add("Test ID 1", "Test Source 1", "Test Ziel 1");
        builder.Add("Test ID 2", "Test Source 2", string.Empty);
        builder.Add("Test ID 3", string.Empty, "Test Ziel 2");

        var ashLang = builder.Build();

        Assert.Equal("en-US", ashLang.Header.SourceLanguage);
        Assert.Equal("de-DE", ashLang.Header.TargetLanguage);

        Assert.Equal(3, ashLang.Count);
        Assert.Equal("Test Source 1", (ashLang["Test ID 1"]?["en-US"] as ITranslationString)?.Value);
        Assert.Equal("Test Ziel 1", (ashLang["Test ID 1"]?["de-DE"] as ITranslationString)?.Value);

        Assert.Equal("Test Source 2", (ashLang["Test ID 2"]?["en-US"] as ITranslationString)?.Value);
        Assert.Equal(string.Empty, (ashLang["Test ID 2"]?.TryGet("de-DE") as ITranslationString)?.Value);

        Assert.Equal(string.Empty, (ashLang["Test ID 3"]?.TryGet("en-US") as ITranslationString)?.Value);
        Assert.Equal("Test Ziel 2", (ashLang["Test ID 3"]?["de-DE"] as ITranslationString)?.Value);


        builder = (IFormatBuilderWithSourceAndTarget)formatFactory.GetFormatProvider(typeof(AshLangFormat))
            .GetFormatBuilder();
        builder.SetSourceLanguage("de-DE");

        Assert.Throws<ArgumentNullException>(() => builder.Build());

        builder.SetTargetLanguage("de-DE");

        var format = builder.Build();

        Assert.Equal("en-US", format.Header.SourceLanguage);
        Assert.Equal("de-DE", format.Header.TargetLanguage);
    }
}