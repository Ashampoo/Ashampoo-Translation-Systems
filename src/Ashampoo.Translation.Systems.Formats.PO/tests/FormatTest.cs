using System.IO;
using System.Threading.Tasks;
using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using Ashampoo.Translation.Systems.Formats.Abstractions.TranslationFilter;
using Ashampoo.Translation.Systems.TestBase;
using Xunit;

namespace Ashampoo.Translation.Systems.Formats.PO.Tests;

public class FormatTest : FormatTestBase<POFormat>
{
    private readonly IFormatFactory formatFactory;

    public FormatTest(IFormatFactory formatFactory)
    {
        this.formatFactory = formatFactory;
    }

    [Fact]
    public void ImportSuccessTest()
    {
        IFormat format = CreateAndReadFromFile("normalized_translation_de.po",
            new FormatReadOptions { TargetLanguage = "de" });

        const string id =
            "{\\\"cxt\\\": \\\"question_heading\\\", \\\"id\\\": 418828805, \\\"checksum\\\": 1234361483}/What do you think about Ashampoo? What changes would you like to see?";
        const string value = "Import Test";
        var imported = format.ImportMockTranslationWithUnits(language: "de", id: id, value: value);

        Assert.Equal(1, imported.Count);
        Assert.Equal(value, format[id]?.Translations.GetTranslation("de")?.Value);
    }

    [Fact]
    public void NoMatchImportTest()
    {
        IFormat format = CreateAndReadFromFile("normalized_translation_de.po",
            new FormatReadOptions { TargetLanguage = "de" });

        const string id = "Not matching Import-Id";
        const string value = "Import Test";
        var imported = format.ImportMockTranslationWithUnits(language: "de", id: id, value: value);

        Assert.Equal(0, imported.Count);
    }

    [Fact]
    public void ImportEqualTranslationTest()
    {
        IFormat format = CreateAndReadFromFile("normalized_translation_de.po",
            new FormatReadOptions { TargetLanguage = "de" });

        const string id =
            "What do you think about Ashampoo? What changes would you like to see?{\\\"cxt\\\": \\\"question_heading\\\", \\\"id\\\": 418828805, \\\"checksum\\\": 1234361483}";
        const string value = "Was denken Sie über die Firma Ashampoo? Worüber würden Sie sich freuen?";
        var imported = format.ImportMockTranslationWithUnits(language: "de", id: id, value: value);

        Assert.Equal(0, imported.Count);
    }


    [Fact]
    public void NewFormat()
    {
        IFormat format = CreateFormat();

        Assert.Equal(0, format.Count);
    }

    [Fact]
    public void ReadFromFile()
    {
        var format = CreateAndReadFromFile("translation_de.po", new FormatReadOptions { SourceLanguage = "en-US" });

        var poHeader = format.Header as POHeader;
        Assert.NotNull(poHeader);

        Assert.Equal(69, format.Count);
        Assert.Equal("de", format.Header.TargetLanguage);
        Assert.Equal("FULL NAME <EMAIL@ADDRESS>", poHeader.Author);

        const string id =
            "{\\\"cxt\\\": \\\"collector_disqualification\\\", \\\"id\\\": 254239623, \\\"checksum\\\": 2373663968}/Thank you for completing our survey!";
        Assert.Equal("Vielen Dank, dass Sie die Umfrage abgeschlossen haben!",
            format[id]?.Translations.GetTranslation("de")?.Value);
    }

    [Fact]
    public async Task ReadAndWrite()
    {
        var format = await CreateAndReadFromFileAsync("normalized_translation_de.po",
            new FormatReadOptions { TargetLanguage = "de" });

        var temp = Path.GetTempPath();
        var outStream = new FileStream($"{temp}normalized_translation_de.po", FileMode.Create, FileAccess.Write,
            FileShare.ReadWrite);
        await format.WriteAsync(outStream);
        outStream.Flush();
        outStream.Close();

        var ms = new MemoryStream();
        await format.WriteAsync(ms);
        ms.Seek(0, SeekOrigin.Begin);

        // var reader = new StreamReader(ms);
        // var text = await reader.ReadToEndAsync();
        //
        // var fs = createFileInStream("normalized_translation_de.po");

        //FIXME: compare formats like in the other tests, and not the streams!
        //fs.MustBeEqualTo(ms);
        File.Delete($"{temp}normalized_translation_de.po");
    }

    [Fact]
    public async Task ConvertTest()
    {
        var mockFormat =
            MockFormatWithTranslationUnits.CreateMockFormatWithTranslationUnits(language: "de", id: "Convert Test",
                value: "Hallo Welt");
        var options = new AssignOptions { TargetLanguage = "de", Filter = new DefaultTranslationFilter() };
        var convertedFormat = await mockFormat.ConvertToAsync<POFormat>(formatFactory, options);

        Assert.NotNull(convertedFormat);

        Assert.Equal("Hallo Welt", convertedFormat["Convert Test"]?.Translations.GetTranslation("de")?.Value);
    }

    [Fact]
    public async Task AssignWithSimpleFilter()
    {
        var mockFormat =
            MockFormatWithTranslationUnits.CreateMockFormatWithTranslationUnits(language: "de",
                id: "Assign Filter test", value: "Hallo Welt");

        var options = new AssignOptions { Filter = new IsEmptyTranslationFilter(), TargetLanguage = "de" };
        var assignedFormat = await mockFormat.ConvertToAsync<POFormat>(formatFactory, options);

        Assert.Empty(assignedFormat);

        var mockFormatWithEmptyValue =
            MockFormatWithTranslationUnits.CreateMockFormatWithTranslationUnits(language: "de",
                id: "Assign Filter test", value: "");
        assignedFormat = await mockFormatWithEmptyValue.ConvertToAsync<POFormat>(formatFactory, options);

        Assert.Single(assignedFormat);
    }
}