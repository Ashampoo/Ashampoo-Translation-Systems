using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Models;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using Ashampoo.Translation.Systems.TestBase;
using FluentAssertions;
using Xunit;

namespace Ashampoo.Translation.Systems.Formats.ResX.Tests;

public class FormatTest : FormatTestBase<ResXFormat>
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
    public async Task ReadFromFile()
    {
        var format =
            await CreateAndReadFromFileAsync("Res.en.resx",
                new FormatReadOptions { TargetLanguage = new Language("en-US") });

        format.TranslationUnits.Count.Should().Be(117);
        format.Header.TargetLanguage.Should().Be(new Language("en-US"));
        format.TranslationUnits.GetTranslationUnit("Text_RemoveAddedItems").Translations
            .GetTranslation(new Language("en-US"))
            .Value.Should().Be("Remove Added Items");
    }

    [Fact]
    public async Task ReadAndWriteAsync()
    {
        var format =
            await CreateAndReadFromFileAsync("Res.en.resx",
                new FormatReadOptions { TargetLanguage = new Language("en-US") });

        await using var ms = new MemoryStream();
        await format.WriteAsync(ms);
        ms.Seek(0, SeekOrigin.Begin);
    }
    
    [Fact]
    public void ReadAndWrite()
    {
        var format =
            CreateAndReadFromFile("Res.en.resx",
                new FormatReadOptions { TargetLanguage = new Language("en-US") });

        using var ms = new MemoryStream();
        format.Write(ms);
        ms.Seek(0, SeekOrigin.Begin);
    }

    [Fact]
    public void WriteFormatLeavesStreamOpen()
    {
        var format =
            CreateAndReadFromFile("Res.en.resx", new FormatReadOptions { TargetLanguage = new Language("en-US") });

        var memoryStream = new MemoryStream();
        format.Write(memoryStream);
        memoryStream.CanRead.Should().BeTrue();
        memoryStream.CanWrite.Should().BeTrue();
    }

    [Fact]
    public async Task WriteFormatLeavesStreamOpenAsync()
    {
        var format =
            await CreateAndReadFromFileAsync("Res.en.resx",
                new FormatReadOptions { TargetLanguage = new Language("en-US") });

        var memoryStream = new MemoryStream();
        await format.WriteAsync(memoryStream);
        memoryStream.CanRead.Should().BeTrue();
        memoryStream.CanWrite.Should().BeTrue();
    }
}