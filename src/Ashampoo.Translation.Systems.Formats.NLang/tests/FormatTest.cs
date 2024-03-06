using System.IO;
using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Models;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using Ashampoo.Translation.Systems.TestBase;
using FluentAssertions;
using Xunit;

namespace Ashampoo.Translation.Systems.Formats.NLang.Tests;

public class FormatTest : FormatTestBase<NLangFormat>
{
    [Fact]
    public void NewFormat()
    {
        var format = CreateFormat();
        
        format.Should().NotBeNull();
        format.TranslationUnits.Should().BeEmpty();
        format.Header.SourceLanguage.Should().BeNull();
        format.Header.TargetLanguage.Value.Should().BeEmpty();
    }

    [Fact]
    public void ReadFromFile()
    {
        IFormat format = CreateAndReadFromFile("de-de.nlang3", new FormatReadOptions { TargetLanguage = new Language("de-DE") });

        foreach (var translationUnit in format.TranslationUnits)
        {
            translationUnit.Translations.Should().ContainSingle();
        }

        format.TranslationUnits.Count.Should().Be(3223);

        const string id = "Form_BOOT.MenuItem_TASK_Delete";

        var foundById = format.TranslationUnits.GetTranslationUnit(id);
        foundById.Should().NotBeNull();
        var translationString = foundById.Translations.GetTranslation(new Language("de-DE"));
        translationString.Should().NotBeNull();
        translationString!.Value.Should().Be("Löschen");
        translationString.Comments.Should().BeEmpty();
    }

    [Fact]
    public void ReadAndWrite()
    {
        IFormat format = CreateAndReadFromFile("de-de.nlang3", new FormatReadOptions { TargetLanguage = new Language("de-DE") });

        var ms = new MemoryStream();
        format.Write(ms);
        ms.Seek(0, SeekOrigin.Begin);

        // var fs = createFileInStream("de-de.nlang3");

        //FIXME: compare formats like in the other tests, and not the streams!
        //fs.MustBeEqualTo(ms);
        
    }
}