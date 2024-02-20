using System;
using System.IO;
using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using Ashampoo.Translation.Systems.TestBase;
using Xunit;

namespace Ashampoo.Translation.Systems.Formats.AshLang.Tests;

public class AshLangFormatTest : FormatTestBase<AshLangFormat>
{
    private readonly IFormatFactory _formatFactory;

    public AshLangFormatTest(IFormatFactory formatFactory)
    {
        _formatFactory = formatFactory;
    }

    [Fact]
    public void NewFormat()
    {
        var format = CreateFormat();

        Assert.Empty(format.TranslationUnits);

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
    public void ReadFromFile()
    {
        var format = CreateAndReadFromFile("normalized_peru-de-DE.ashLang");

        Assert.Equal(67, format.TranslationUnits.Count);
        Assert.Equal("en-US", format.Header.SourceLanguage);
        //Assert.Null(format.Header.Author);
        Assert.Equal("de-DE", format.Header.TargetLanguage);
        //Assert.Equal("Ashampoo", format.Header.Author);

        const string id = "peru.CSystem.MoveFileFailed";

        var translationUnit = format.TranslationUnits.GetTranslationUnit(id);
        Assert.NotNull(translationUnit);
        Assert.Equal(id, translationUnit.Id);
        Assert.Equal("Error moving file '%SRC%' to '%DEST%'.",
            translationUnit.Translations.GetTranslation("en-US").Value);
        Assert.Equal("Fehler beim Verschieben der Datei '%SRC%' nach '%DEST%'.",
            translationUnit.Translations.GetTranslation("de-DE").Value);
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
    public void FormatBuilderTest()
    {
        var builder =
            (IFormatBuilderWithSourceAndTarget)_formatFactory.GetFormatProvider(typeof(AshLangFormat))
                .GetFormatBuilder();

        builder.SetTargetLanguage("de-DE");
        builder.Add("Test ID 1", "Test Source 1", "Test Ziel 1");
        builder.Add("Test ID 2", "Test Source 2", string.Empty);
        builder.Add("Test ID 3", string.Empty, "Test Ziel 2");

        var ashLang = builder.Build();

        Assert.Equal("en-US", ashLang.Header.SourceLanguage);
        Assert.Equal("de-DE", ashLang.Header.TargetLanguage);

        Assert.Equal(3, ashLang.TranslationUnits.Count);
        Assert.Equal("Test Source 1", ashLang.TranslationUnits.GetTranslationUnit("Test ID 1").Translations.GetTranslation("en-US").Value);
        Assert.Equal("Test Ziel 1", ashLang.TranslationUnits.GetTranslationUnit("Test ID 1").Translations.GetTranslation("de-DE").Value);

        Assert.Equal("Test Source 2", ashLang.TranslationUnits.GetTranslationUnit("Test ID 2").Translations.GetTranslation("en-US").Value);
        Assert.Equal(string.Empty, ashLang.TranslationUnits.GetTranslationUnit("Test ID 2").Translations.GetTranslation("de-DE").Value);

        Assert.Equal(string.Empty, ashLang.TranslationUnits.GetTranslationUnit("Test ID 3").Translations.GetTranslation("en-US").Value);
        Assert.Equal("Test Ziel 2", ashLang.TranslationUnits.GetTranslationUnit("Test ID 3").Translations.GetTranslation("de-DE").Value);


        builder = (IFormatBuilderWithSourceAndTarget)_formatFactory.GetFormatProvider(typeof(AshLangFormat))
            .GetFormatBuilder();
        builder.SetSourceLanguage("de-DE");

        Assert.Throws<ArgumentNullException>(() => builder.Build());

        builder.SetTargetLanguage("de-DE");

        var format = builder.Build();

        Assert.Equal("en-US", format.Header.SourceLanguage);
        Assert.Equal("de-DE", format.Header.TargetLanguage);
    }
}