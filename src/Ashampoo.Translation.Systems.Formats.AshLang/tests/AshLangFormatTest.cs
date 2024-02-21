using System;
using System.IO;
using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Models;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using Ashampoo.Translation.Systems.TestBase;
using FluentAssertions;
using Xunit;

namespace Ashampoo.Translation.Systems.Formats.AshLang.Tests;

public class AshLangFormatTest : FormatTestBase<AshLangFormat>
{

    [Fact]
    public void NewFormat()
    {
        var format = CreateFormat();

        format.TranslationUnits.Should().BeEmpty();

        // source sets are 'en-US' per default.
        format.Header.SourceLanguage.Should().Be(new Language("en-US"));
        
        format.Header.TargetLanguage = new Language("es-ES");
        format.Header.TargetLanguage.Should().Be(new Language("es-ES"));
        
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

        format.TranslationUnits.Count.Should().Be(67);
        format.Header.SourceLanguage.Should().Be(new Language("en-US"));
        format.Header.TargetLanguage.Should().Be(new Language("de-DE"));

        const string id = "peru.CSystem.MoveFileFailed";

        var translationUnit = format.TranslationUnits.GetTranslationUnit(id);
        translationUnit.Should().NotBeNull();
        translationUnit.Id.Should().Be(id);
        translationUnit.Translations.GetTranslation(new Language("en-US")).Value.Should()
            .Be("Error moving file '%SRC%' to '%DEST%'.");
        translationUnit.Translations.GetTranslation(new Language("de-DE")).Value.Should()
            .Be("Fehler beim Verschieben der Datei '%SRC%' nach '%DEST%'.");
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
        var builder = new AshLangFormatBuilder();

        builder.SetTargetLanguage(new Language("de-DE"));
        builder.Add("Test ID 1", "Test Source 1", "Test Ziel 1");
        builder.Add("Test ID 2", "Test Source 2", string.Empty);
        builder.Add("Test ID 3", string.Empty, "Test Ziel 2");

        var ashLang = builder.Build();

        ashLang.Header.SourceLanguage.Should().Be(new Language("en-US"));
        ashLang.Header.TargetLanguage.Should().Be(new Language("de-DE"));
        ashLang.TranslationUnits.Count.Should().Be(3);
        ashLang.TranslationUnits.GetTranslationUnit("Test ID 1").Translations.GetTranslation(new Language("en-US")).Value.Should()
            .Be("Test Source 1");
        ashLang.TranslationUnits.GetTranslationUnit("Test ID 1").Translations.GetTranslation(new Language("de-DE")).Value.Should()
            .Be("Test Ziel 1");
        ashLang.TranslationUnits.GetTranslationUnit("Test ID 2").Translations.GetTranslation(new Language("en-US")).Value.Should()
            .Be("Test Source 2");
        ashLang.TranslationUnits.GetTranslationUnit("Test ID 2").Translations.GetTranslation(new Language("de-DE")).Value.Should()
            .BeEmpty();
        ashLang.TranslationUnits.GetTranslationUnit("Test ID 3").Translations.GetTranslation(new Language("en-US")).Value.Should()
            .BeEmpty();
        ashLang.TranslationUnits.GetTranslationUnit("Test ID 3").Translations.GetTranslation(new Language("de-DE")).Value.Should()
            .Be("Test Ziel 2");

        builder = new AshLangFormatBuilder();
        builder.SetSourceLanguage(new Language("de-DE"));

        builder.Invoking(x => x.Build()).Should().Throw<ArgumentException>();
        builder.SetTargetLanguage(new Language("de-DE"));
        
        var format = builder.Build();

        format.Header.SourceLanguage.Should().Be(new Language("en-US"));
        format.Header.TargetLanguage.Should().Be(new Language("de-DE"));
    }
}