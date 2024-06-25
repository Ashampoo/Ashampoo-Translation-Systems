using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Models;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using Ashampoo.Translation.Systems.TestBase;
using FluentAssertions;

namespace Ashampoo.Translation.Systems.Formats.JavaProperties.Test;

public class FormatTest : FormatTestBase<JavaPropertiesFormat>
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
    public void ReadFromFile()
    {
        IFormat format =
            CreateAndReadFromFile("messages_de.properties",
                new FormatReadOptions { TargetLanguage = new Language("de-DE") });
        const string id = "aboutTheApp";

        foreach (var unit in format.TranslationUnits)
        {
            unit.Translations.Should().ContainSingle();
        }

        format.TranslationUnits.Count.Should().Be(186);

        var foundById = format.TranslationUnits.GetTranslationUnit(id);
        foundById.Should().NotBeNull();
        var translationString = foundById.Translations.GetTranslation(new Language("de-DE"));
        translationString.Should().NotBeNull();
        translationString.Value.Should().Be("Über Photos");
        translationString.Comments.Should().BeEmpty();
    }

    [Fact]
    public void WriteFormatLeavesStreamOpen()
    {
        var format = CreateAndReadFromFile("messages_de.properties",
            new FormatReadOptions { TargetLanguage = new Language("de-DE") });
        
        var memoryStream = new MemoryStream();
        format.Write(memoryStream);
        memoryStream.CanRead.Should().BeTrue();
        memoryStream.CanWrite.Should().BeTrue();
    }
    
    [Fact]
    public async Task WriteFormatLeavesStreamOpenAsync()
    {
        var format = await CreateAndReadFromFileAsync("messages_de.properties",
            new FormatReadOptions { TargetLanguage = new Language("de-DE") });
        
        var memoryStream = new MemoryStream();
        await format.WriteAsync(memoryStream);
        memoryStream.CanRead.Should().BeTrue();
        memoryStream.CanWrite.Should().BeTrue();
    }
}