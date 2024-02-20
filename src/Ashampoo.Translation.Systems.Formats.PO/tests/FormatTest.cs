using System.IO;
using System.Threading.Tasks;
using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using Ashampoo.Translation.Systems.TestBase;
using Xunit;

namespace Ashampoo.Translation.Systems.Formats.PO.Tests;

public class FormatTest : FormatTestBase<POFormat>
{
    private readonly IFormatFactory _formatFactory;

    public FormatTest(IFormatFactory formatFactory)
    {
        _formatFactory = formatFactory;
    }

    [Fact]
    public void NewFormat()
    {
        IFormat format = CreateFormat();

        Assert.Equal(0, format.TranslationUnits.Count);
    }

    [Fact]
    public void ReadFromFile()
    {
        var format = CreateAndReadFromFile("translation_de.po", new FormatReadOptions { SourceLanguage = "en-US" });

        var poHeader = format.Header as POHeader;
        Assert.NotNull(poHeader);

        Assert.Equal(69, format.TranslationUnits.Count);
        Assert.Equal("de", format.Header.TargetLanguage);
        Assert.Equal("FULL NAME <EMAIL@ADDRESS>", poHeader.Author);

        const string id =
            "{\\\"cxt\\\": \\\"collector_disqualification\\\", \\\"id\\\": 254239623, \\\"checksum\\\": 2373663968}/Thank you for completing our survey!";
        Assert.Equal("Vielen Dank, dass Sie die Umfrage abgeschlossen haben!",
            format.TranslationUnits.GetTranslationUnit(id).Translations.GetTranslation("de").Value);
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
}