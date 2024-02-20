using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using Ashampoo.Translation.Systems.TestBase;
using Xunit;

namespace Ashampoo.Translation.Systems.Formats.ResX.Tests;

public class FormatTest : FormatTestBase<ResXFormat>
{
    private readonly IFormatFactory _formatFactory;

    public FormatTest(IFormatFactory formatFactory)
    {
        _formatFactory = formatFactory;
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
    public async Task ReadFromFile()
    {
        var format =
            await CreateAndReadFromFileAsync("Res.en.resx", new FormatReadOptions { TargetLanguage = "en-US" });
        Assert.Equal(117, format.Count);
        Assert.Equal("en-US", format.Header.TargetLanguage);
        Assert.Equal("Remove Added Items", format["Text_RemoveAddedItems"]?.Translations.GetTranslation("en-US")?.Value);
    }

    [Fact]
    public async Task ReadAndWrite()
    {
        var format =
            await CreateAndReadFromFileAsync("Res.en.resx", new FormatReadOptions { TargetLanguage = "en-US" });

        await using var ms = new MemoryStream();
        await format.WriteAsync(ms);
        ms.Seek(0, SeekOrigin.Begin);
    }

    [Fact]
    public void ImportSuccessTest()
    {
        IFormat format = CreateAndReadFromFile("Res.en.resx", new FormatReadOptions { TargetLanguage = "en-US" });

        const string id = "Button_RemoveAll";
        const string value = "Import Test";

        var importedWithUnits = format.ImportMockTranslationWithUnits(language: "en-US", id: id, value: value);
        Assert.NotNull(importedWithUnits);
        Assert.Single(importedWithUnits);
        Assert.Equal("Import Test", format[id]?.Translations.GetTranslation("en-US")?.Value);
    }

    [Fact]
    public void NoMatchImportTest()
    {
        IFormat format = CreateAndReadFromFile("Res.en.resx", new FormatReadOptions { TargetLanguage = "en-US" });

        const string id = "Not matching Import-Id";
        const string value = "Import Test";
        var imported = format.ImportMockTranslationWithUnits(language: "en-US", id: id, value: value);

        Assert.Empty(imported);
    }

    [Fact]
    public void ImportEqualTranslationTest()
    {
        IFormat format = CreateAndReadFromFile("Res.en.resx", new FormatReadOptions { TargetLanguage = "en-US" });

        const string id = "Button_RemoveAll";
        const string value = "Remove All";
        var imported = format.ImportMockTranslationWithUnits(language: "en-US", id: id, value: value);

        Assert.Empty(imported);
    }
}