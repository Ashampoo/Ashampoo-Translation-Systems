using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Models;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using Ashampoo.Translation.Systems.TestBase;
using FluentAssertions;
using Xunit;

namespace Ashampoo.Translation.Systems.Formats.PO.Tests;

public class FormatTest : FormatTestBase<POFormat>
{
    [Fact]
    public void NewFormat()
    {
        IFormat format = CreateFormat();

        format.TranslationUnits.Count.Should().Be(0);
    }

    [Fact]
    public void ReadFromFile()
    {
        var format = CreateAndReadFromFile("translation_de.po", new FormatReadOptions { SourceLanguage = new Language("en-US") });

        var poHeader = format.Header as POHeader;
        poHeader.Should().NotBeNull();

        format.TranslationUnits.Count.Should().Be(69);
        format.Header.TargetLanguage.Should().Be(new Language("de"));
        poHeader?.Author.Should().Be("FULL NAME <EMAIL@ADDRESS>");

        const string id =
            "{\\\"cxt\\\": \\\"collector_disqualification\\\", \\\"id\\\": 254239623, \\\"checksum\\\": 2373663968}/Thank you for completing our survey!";

        format.TranslationUnits.GetTranslationUnit(id).Translations.GetTranslation(new Language("de")).Value.Should()
            .Be("Vielen Dank, dass Sie die Umfrage abgeschlossen haben!");
    }

    [Fact]
    public async Task ReadAndWrite()
    {
        var format = await CreateAndReadFromFileAsync("normalized_translation_de.po",
            new FormatReadOptions { TargetLanguage = new Language("de") });

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