using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Models;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using Ashampoo.Translation.Systems.TestBase;
using FluentAssertions;

namespace Ashampoo.Translation.Systems.Formats.QT.Tests;

public class FormatTest : FormatTestBase<QTFormat>
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
        const string id = "Verbleibende Testzeit:";

        format.Header.TargetLanguage.Should().Be(new Language("de-DE"));
        format.TranslationUnits.Should().NotBeEmpty();
        foreach (var unit in format.TranslationUnits)
        {
            unit.Translations.Should().ContainSingle();
        }
        
        var foundById = format.TranslationUnits.GetTranslationUnit(id);
        foundById.Should().NotBeNull();
        var translationString = foundById.Translations.GetTranslation(new Language("de-DE"));
        translationString.Should().NotBeNull();
        translationString.Value.Should().Be("Verbleibende Testzeit:");
        translationString.Comments.Should().BeEmpty();
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
    }
}