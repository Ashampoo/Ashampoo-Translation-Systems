using System;
using System.IO;
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
        var format = CreateAndReadFromFile("translation_de.po",
            new FormatReadOptions { SourceLanguage = new Language("en-US") });

        var poHeader = format.Header as POHeader;
        poHeader.Should().NotBeNull();

        format.TranslationUnits.Count.Should().Be(69);
        format.Header.TargetLanguage.Should().Be(new Language("de"));
        poHeader?.Author.Should().Be("FULL NAME <EMAIL@ADDRESS>");

        const string id =
            "{\\\"cxt\\\": \\\"collector_disqualification\\\", \\\"id\\\": 254239623, \\\"checksum\\\": 2373663968}||Thank you for completing our survey!";

        format.TranslationUnits.GetTranslationUnit(id).Translations.GetTranslation(new Language("de")).Value.Should()
            .Be("Vielen Dank, dass Sie die Umfrage abgeschlossen haben!");
    }

    [Fact]
    public void CreateBuilderWithDisabledPipeSplittingTest()
    {
        var format = CreateAndReadFromFile("translation_de.po",
            new FormatReadOptions { SourceLanguage = new Language("en-US") });


        var poHeader = format.Header as POHeader;
        poHeader.Should().NotBeNull();

        var poBuilder = new POFormatBuilder();
        foreach (var unit in format.TranslationUnits)
        {
            foreach (var translation in unit.Translations)
            {
                poBuilder.Add(unit.Id, translation.Value);
            }
        }
        poBuilder.SetTargetLanguage(format.Header.TargetLanguage);
        var newFormat = poBuilder.Build(new PoBuilderOptions { SplitContextAndId = false });
        var memoryStream = new MemoryStream();
        newFormat.Write(memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin);
        var streamReader = new StreamReader(memoryStream);
        var result = streamReader.ReadToEnd();
        result.Should().NotContain("msgctxt ");
    }

    [Fact]
    public void ReadWithoutMsgCtxtText()
    {
        var format = CreateAndReadFromFile("test.po",
            new FormatReadOptions { SourceLanguage = new Language("en-US") });

        var poHeader = format.Header as POHeader;
        poHeader.Should().NotBeNull();

        format.TranslationUnits.Count.Should().Be(2);
        format.Header.TargetLanguage.Should().Be(new Language("de-DE"));

        const string id = "testid2";

        format.TranslationUnits.GetTranslationUnit(id).Translations.GetTranslation(new Language("de-DE")).Value.Should()
            .Be("deutscher testid2 Text");
    }
    
    [Fact]
    public async Task ReadAndWriteWithoutMsgCtxtText()
    {
        var format = await CreateAndReadFromFileAsync("test.po",
            new FormatReadOptions { SourceLanguage = new Language("en-US") });

        var outStream = new MemoryStream();
        await format.WriteAsync(outStream);
        await outStream.FlushAsync();
        outStream.Seek(0, SeekOrigin.Begin);

        var reader = new StreamReader(outStream);
        var result = await reader.ReadToEndAsync();
        result.Should().NotBeEmpty();
        result.Should().NotContain("msgctxt ");
    }

    [Fact]
    public async Task ReadAndWriteAsync()
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

    [Fact]
    public void ReadAndWrite()
    {
        var format = CreateAndReadFromFile("normalized_translation_de.po",
            new FormatReadOptions { TargetLanguage = new Language("de") });

        var temp = Path.GetTempPath();
        var outStream = new FileStream($"{temp}normalized_translation_de.po", FileMode.Create, FileAccess.Write,
            FileShare.ReadWrite);
        format.Write(outStream);
        outStream.Flush();
        outStream.Close();

        var ms = new MemoryStream();
        format.Write(ms);
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
    public void WriteFormatLeavesStreamOpen()
    {
        var format = CreateAndReadFromFile("translation_de.po",
            new FormatReadOptions { SourceLanguage = new Language("en-US") });

        var memoryStream = new MemoryStream();
        format.Write(memoryStream);
        memoryStream.CanRead.Should().BeTrue();
        memoryStream.CanWrite.Should().BeTrue();
    }

    [Fact]
    public async Task WriteFormatLeavesStreamOpenAsync()
    {
        var format = await CreateAndReadFromFileAsync("translation_de.po",
            new FormatReadOptions { SourceLanguage = new Language("en-US") });

        var memoryStream = new MemoryStream();
        await format.WriteAsync(memoryStream);
        memoryStream.CanRead.Should().BeTrue();
        memoryStream.CanWrite.Should().BeTrue();
    }
}