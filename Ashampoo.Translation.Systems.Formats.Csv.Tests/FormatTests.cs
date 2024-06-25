using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Models;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using Ashampoo.Translation.Systems.Formats.CSV;
using Ashampoo.Translation.Systems.TestBase;
using FluentAssertions;

namespace Ashampoo.Translation.Systems.Formats.Csv.Tests;

public class FormatTests : FormatTestBase<CsvFormat>
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
        var format =
            CreateAndReadFromFile("testfile_en.csv",
                new FormatReadOptions
                    { TargetLanguage = new Language("de-DE"), SourceLanguage = new Language("en-US") });
        const string id = "HOME.HOME";

        format.TranslationUnits.Count.Should().Be(2);

        var foundById = format.TranslationUnits.GetTranslationUnit(id);
        foundById.Should().NotBeNull();
        var translationString = foundById.Translations.GetTranslation(new Language("de-DE"));
        translationString.Should().NotBeNull();
        translationString.Value.Should().Be("Haus, Zuhause");
        translationString.Comments.Should().AllBe(string.Empty);
        var sourceTranslation = foundById.Translations.GetTranslation(new Language("en-US"));
        sourceTranslation.Should().NotBeNull();
        sourceTranslation.Value.Should().Be("Home");
    }

    [Fact]
    public void WriteFormatLeavesStreamOpen()
    {
        IFormat format = CreateAndReadFromFile("testfile_en.csv",
            new FormatReadOptions { TargetLanguage = new Language("de-DE"), SourceLanguage = new Language("en-US") });

        var memoryStream = new MemoryStream();
        format.Write(memoryStream);
        memoryStream.CanRead.Should().BeTrue();
        memoryStream.CanWrite.Should().BeTrue();
    }

    [Fact]
    public async Task WriteFormatLeavesStreamOpenAsync()
    {
        var format = await CreateAndReadFromFileAsync("testfile_en.csv",
            new FormatReadOptions { TargetLanguage = new Language("de-DE"), SourceLanguage = new Language("en-US") });

        var memoryStream = new MemoryStream();
        await format.WriteAsync(memoryStream);
        memoryStream.CanRead.Should().BeTrue();
        memoryStream.CanWrite.Should().BeTrue();
    }

    [Fact]
    public async Task ReadFormatWithoutOptions()
    {
        Func<Task> action = () => CreateAndReadFromFileAsync("testfile_en.csv",
            new FormatReadOptions());
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task ReadFormatWithOptions()
    {
        var format = await CreateAndReadFromFileAsync("testfile_en.csv",
            new FormatReadOptions { TargetLanguage = new Language("nl-NL"), SourceLanguage = new Language("de-DE")});
        format.Header.SourceLanguage?.Value.Should().Be("de-DE");
        format.Header.TargetLanguage.Value.Should().Be("nl-NL");
    }

    [Fact]
    public async Task ReadFormatWithoutOptionsAndHeader()
    {
        Func<Task> action = () => CreateAndReadFromFileAsync("testfile_en_noHeader.csv",
            new FormatReadOptions());
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task ReadFormatWithoutOptionsAndEmptyHeader()
    {
        Func<Task> action = () => CreateAndReadFromFileAsync("testfile_en_noHeader.csv",
            new FormatReadOptions());
        await action.Should().ThrowAsync<ArgumentNullException>();
    }
}