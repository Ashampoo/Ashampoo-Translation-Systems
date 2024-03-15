using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Models;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using Ashampoo.Translation.Systems.TestBase;
using FluentAssertions;

namespace Ashampoo.Translation.Systems.Formats.QT.Tests;

public class FormatTest : FormatTestBase<QtFormat>
{
    [Fact]
    public void NewFormat()
    {
        IFormat format = CreateFormat();

        format.Should().NotBeNull();
        format.TranslationUnits.Should().BeEmpty();
        format.Header.SourceLanguage.Should().BeNull();
        format.Header.TargetLanguage.Value.Should().BeEmpty();
    }
    
    [Fact]
    public void ReadFromFileTest()
    {
        IFormat format =
            CreateAndReadFromFile("translation_de.ts", new FormatReadOptions() { TargetLanguage = new Language("de-DE") });
        const string id = "Lizenz Aktivieren/Deaktivieren";

        format.Header.TargetLanguage.Should().Be(new Language("de-DE"));
        format.TranslationUnits.Should().NotBeEmpty();
        format.TranslationUnits.Count.Should().Be(11636);
        foreach (var unit in format.TranslationUnits)
        {
            unit.Translations.Should().ContainSingle();
        }
        
        var foundById = format.TranslationUnits.GetTranslationUnit(id);
        foundById.Should().NotBeNull();
        var translationString = foundById.Translations.GetTranslation(new Language("de-DE"));
        translationString.Should().NotBeNull();
        translationString.Value.Should().Be("Lizenz Aktivieren/Deaktivieren");
        translationString.Comments.Should().BeEmpty();
        translationString.Should().BeOfType<QtTranslationString>();
        var translation = translationString as QtTranslationString;
        translation?.Type.Should().Be(QtTranslationType.Finished);
    }
    
    [Fact]
    public void WriteFormatLeavesStreamOpen()
    {
        var format = CreateAndReadFromFile("translation_de.ts",
            new FormatReadOptions { TargetLanguage = new Language("de-DE") }) as IFormat;

        var memoryStream = new MemoryStream();
        format.Write(memoryStream);
        memoryStream.CanRead.Should().BeTrue();
        memoryStream.CanWrite.Should().BeTrue();
    }
    
    [Fact]
    public async Task WriteFormatLeavesStreamOpenAsync()
    {
        var format = await CreateAndReadFromFileAsync("translation_de.ts",
            new FormatReadOptions { TargetLanguage = new Language("de-DE") });
        
        var memoryStream = new MemoryStream();
        await format.WriteAsync(memoryStream);
        memoryStream.CanRead.Should().BeTrue();
        memoryStream.CanWrite.Should().BeTrue();
        memoryStream.Length.Should().BeGreaterThan(0);
    }
}