using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using Ashampoo.Translation.Systems.Formats.Abstractions.TranslationFilter;
using Ashampoo.Translation.Systems.TestBase;
using Xunit;

namespace Ashampoo.Translation.Systems.Formats.Json.Tests;

public class FormatTest : FormatTestBase<JsonFormat>
{
    private readonly IFormatFactory formatFactory;

    public FormatTest(IFormatFactory formatFactory)
    {
        this.formatFactory = formatFactory;
    }

    [Fact]
    public void IsAssignableFrom()
    {
        IFormat format = CreateFormat();

        // provides translation units
        Assert.IsAssignableFrom<ITranslationUnits>(format);
    }

    [Fact]
    public void NewFormat()
    {
        var format = CreateFormat();

        Assert.NotNull(format);
        Assert.Empty(format);
        Assert.Null(format.Header.SourceLanguage);
        Assert.Equal(string.Empty, format.Header.TargetLanguage);
        Assert.Equal(FormatLanguageCount.OnlyTarget, format.LanguageCount);
    }


    [Fact]
    public void ReadFromFile()
    {
        IFormat format = CreateAndReadFromFile("en-us.json", new FormatReadOptions { TargetLanguage = "en-US" });

        Assert.Equal(301, format.Count);
        Assert.Equal("en-US", format.Header.TargetLanguage);
        Assert.Null(format.Header.SourceLanguage);
        Assert.Equal("Save", (format["settings/save"]?["en-US"] as ITranslationString)?.Value);
    }

    [Fact]
    public void ReadAndWrite()
    {
        IFormat format = CreateAndReadFromFile("de-DE.json", new FormatReadOptions { TargetLanguage = "de-DE" });

        var ms = new MemoryStream();
        format.Write(ms);
        ms.Seek(0, SeekOrigin.Begin);

        // var fs = createFileInStream("de-DE.json");

        //FIXME: compare formats like in the other tests, and not the streams!
        //fs.MustBeEqualTo(ms);
    }

    [Theory]
    [InlineData("json-value-kind-false-test.json")]
    [InlineData("json-value-kind-true-test.json")]
    [InlineData("json-value-kind-null-test.json")]
    [InlineData("json-value-kind-number-test.json")]
    public async Task InvalidValueKindsTest(string filename)
    {
        var options = new FormatReadOptions { TargetLanguage = "de-DE" };
        var exception =
            await Assert.ThrowsAsync<JsonException>(async () => await CreateAndReadFromFileAsync(filename, options));
        Assert.Equal("Array element must be either an object, array or a string.", exception.Message);
    }

    [Fact]
    public async Task ReadAndWriteWithReorder()
    {
        var format = await CreateAndReadFromFileAsync("de-DE.json", new FormatReadOptions { TargetLanguage = "de-DE" });

        var units = format.OrderBy(u => u.Id);
        format = new JsonFormat { Header = format.Header };
        foreach (var unit in units)
        {
            format.Add(unit);
        }

        var ms = new MemoryStream();
        await format.WriteAsync(ms);
        ms.Seek(0, SeekOrigin.Begin);
    }

    [Fact]
    public async Task SimpleAssign()
    {
        var mock =
            MockFormatWithTranslationUnits.CreateMockFormatWithTranslationUnits("en-US", "ID", "Hello World");
        var converted = await mock.ConvertToAsync<JsonFormat>(formatFactory,
            new AssignOptions { TargetLanguage = "en-US", Filter = new DefaultTranslationFilter() });

        Assert.Single(converted);
        Assert.Single(converted["ID"] ?? Enumerable.Empty<ITranslation>());
        Assert.Equal("Hello World", (converted["ID"]?["en-US"] as ITranslationString)?.Value);
    }

    [Fact]
    public async Task ComplexAssign()
    {
        const string id = "peru.CSystem.CreateUniqueFileFailed";
        var mockFormat = new MockFormatWithTranslationUnits
        {
            { "en-US", id, "Error creating unique file name." },
            { "de-DE", id, "Fehler beim Erzeugen eines eindeutigen Dateinamens" }
        };

        var optionsEn = new AssignOptions { TargetLanguage = "en-US", Filter = new DefaultTranslationFilter() };
        var optionsDe = new AssignOptions { TargetLanguage = "de-DE", Filter = new DefaultTranslationFilter() };

        var convertedEnUs = await mockFormat.ConvertToAsync<JsonFormat>(formatFactory, optionsEn);
        var convertedDeDe = await mockFormat.ConvertToAsync<JsonFormat>(formatFactory, optionsDe);


        Assert.Equal("en-US", convertedEnUs.Header.TargetLanguage);
        Assert.Equal("de-DE", convertedDeDe.Header.TargetLanguage);

        Assert.Equal("Error creating unique file name.",
            (convertedEnUs[id]?["en-US"] as ITranslationString)?.Value);
        Assert.Equal("Fehler beim Erzeugen eines eindeutigen Dateinamens",
            (convertedDeDe[id]?["de-DE"] as ITranslationString)?.Value);
    }

    [Fact]
    public void ImportSuccessTest()
    {
        IFormat format = CreateAndReadFromFile("en-us.json", new FormatReadOptions { TargetLanguage = "en-US" });

        const string id = "settings/save";
        const string value = "Import Test";

        var importedWithUnits = format.ImportMockTranslationWithUnits(language: "en-US", id: id, value: value);
        Assert.NotNull(importedWithUnits);
        Assert.Single(importedWithUnits);
        Assert.Equal("Import Test", (format[id]?["en-US"] as ITranslationString)?.Value);
    }

    [Fact]
    public void NoMatchImportTest()
    {
        IFormat format = CreateAndReadFromFile("en-us.json", new FormatReadOptions { TargetLanguage = "en-US" });

        const string id = "Not matching Import-Id";
        const string value = "Import Test";
        var imported = format.ImportMockTranslationWithUnits(language: "en-US", id: id, value: value);

        Assert.Empty(imported);
    }

    [Fact]
    public void ImportEqualTranslationTest()
    {
        IFormat format = CreateAndReadFromFile("en-us.json", new FormatReadOptions { TargetLanguage = "en-US" });

        const string id = "settings/save";
        const string value = "Save";
        var imported = format.ImportMockTranslationWithUnits(language: "en-US", id: id, value: value);

        Assert.Empty(imported);
    }

    [Fact]
    public async Task OptionsCallbackCancelledTest()
    {
        Task OptionsCallback(FormatOptions options)
        {
            options.IsCanceled = true;
            return Task.CompletedTask;
        }

        var format = await CreateAndReadFromFileAsync("en-us.json",
            new FormatReadOptions { FormatOptionsCallback = OptionsCallback });
        Assert.Null(format.Header.SourceLanguage);
        Assert.Equal(string.Empty, format.Header.TargetLanguage);
        Assert.Empty(format);
    }

    [Fact]
    public async Task NoOptionsCallbackTest()
    {
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await CreateAndReadFromFileAsync("en-us.json", new FormatReadOptions()));
        Assert.Equal("Value cannot be null. (Parameter 'FormatOptionsCallback')", exception.Message);
        Assert.Equal("FormatOptionsCallback", exception.ParamName);
    }

    [Fact]
    public async Task OptionsCallbackTest()
    {
        Task OptionsCallback(FormatOptions options)
        {
            (options.Options[0] as FormatStringOption)!.Value = "de-DE";
            return Task.CompletedTask;
        }

        var options = new FormatReadOptions { FormatOptionsCallback = OptionsCallback };
        var format = await CreateAndReadFromFileAsync("de-DE.json", options);

        Assert.Equal("de-DE", format.Header.TargetLanguage);
        Assert.Null(format.Header.SourceLanguage);
        Assert.Equal(189, format.Count);
    }
}