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
        format.Header.TargetLanguage.ToString().Should().BeEmpty();
    }

    [Fact]
    public async Task ReadFromFile()
    {
        var format =
            await CreateAndReadFromFileAsync("Res.en.resx", new FormatReadOptions { TargetLanguage = new Language("en-US") });

        format.TranslationUnits.Count.Should().Be(117);
        format.Header.TargetLanguage.Should().Be(new Language("en-US"));
        format.TranslationUnits.GetTranslationUnit("Text_RemoveAddedItems").Translations.GetTranslation(new Language("en-US"))
            .Value.Should().Be("Remove Added Items");
    }

    [Fact]
    public async Task ReadAndWrite()
    {
        var format =
            await CreateAndReadFromFileAsync("Res.en.resx", new FormatReadOptions { TargetLanguage = new Language("en-US") });

        await using var ms = new MemoryStream();
        await format.WriteAsync(ms);
        ms.Seek(0, SeekOrigin.Begin);
    }
}