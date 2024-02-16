using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using Ashampoo.Translation.Systems.Formats.Abstractions.TranslationFilter;
using Ashampoo.Translation.Systems.TestBase;
using FluentAssertions;

namespace Ashampoo.Translation.Systems.Formats.JavaProperties.Test;

public class FormatTest : FormatTestBase<JavaPropertiesFormat>
{
    private readonly IFormatFactory _formatFactory;

    public FormatTest(IFormatFactory formatFactory)
    {
        _formatFactory = formatFactory;
    }

    [Fact]
    public void IsAssignableFrom()
    {
        IFormat format = CreateFormat();
        format.Should().BeAssignableTo(typeof(ITranslationUnits));
    }

    [Fact]
    public void NewFormat()
    {
        IFormat format = CreateFormat();

        format.Should().NotBeNull().And.BeEmpty();
        format.Header.SourceLanguage.Should().BeNull();
        format.Header.TargetLanguage.Should().BeEmpty();
    }

    [Fact]
    public void ReadFromFile()
    {
        IFormat format =
            CreateAndReadFromFile("messages_de.properties", new FormatReadOptions() { TargetLanguage = "de-DE" });
        const string id = "aboutTheApp";

        foreach (var unit in format)
        {
            unit.Should().ContainSingle();
        }

        format.Count.Should().Be(186);

        var foundById = format[id];
        foundById.Should().NotBeNull();
        var translationString = foundById!["de-DE"] as AbstractTranslationString;
        translationString.Should().NotBeNull();
        translationString!.Value.Should().Be("Über Photos");
        translationString.Comment.Should().BeNull();
        translationString.Id.Should().Be(id);
    }

    [Fact]
    public void ImportSuccessTest()
    {
        IFormat format =
            CreateAndReadFromFile("messages.properties", new FormatReadOptions() { TargetLanguage = "en-US" });

        const string id = "albums";
        const string value = "Import Test";

        var importedWithUnits = format.ImportMockTranslationWithUnits("en-US", id);
        importedWithUnits.Should().NotBeNull().And.ContainSingle();
        (format[id]?["en-US"] as ITranslationString)?.Value.Should().Be(value);
    }

    [Fact]
    public void NoMatchImportTest()
    {
        IFormat format =
            CreateAndReadFromFile("messages.properties", new FormatReadOptions() { TargetLanguage = "en-US" });
        
        const string id = "Not a matching Id";

        var imported = format.ImportMockTranslationWithUnits("en-US", id);

        imported.Should().BeEmpty();
    }

    [Fact]
    public void ImportEqualTranslationTest()
    {
        IFormat format =
            CreateAndReadFromFile("messages.properties", new FormatReadOptions() { TargetLanguage = "en-US" });
        
        const string id = "albums";
        const string value = "Albums";
        
        var imported = format.ImportMockTranslationWithUnits("en-US", id, value);

        imported.Should().BeEmpty();
    }

    [Fact]
    public async Task ConvertTest()
    {
        var mockFormat =
            MockFormatWithTranslationUnits.CreateMockFormatWithTranslationUnits("en-US", "Convert ID", "Convert Test");
        var assignOptions = new AssignOptions { TargetLanguage = "en-US", Filter = new DefaultTranslationFilter() };
        var javaProperties = await mockFormat.ConvertToAsync<JavaPropertiesFormat>(_formatFactory, assignOptions);

        javaProperties.Should().NotBeNull().And.ContainSingle();
        javaProperties.First().Id.Should().Be("Convert ID");
        (javaProperties["Convert ID"]?["en-US"] as ITranslationString)?.Value.Should().Be("Convert Test");
    }
}