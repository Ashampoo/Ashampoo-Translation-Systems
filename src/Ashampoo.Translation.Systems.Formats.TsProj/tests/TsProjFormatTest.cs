using System;
using System.IO;
using System.Threading.Tasks;
using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Models;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using Ashampoo.Translation.Systems.TestBase;
using FluentAssertions;
using Xunit;

namespace Ashampoo.Translation.Systems.Formats.TsProj.Tests;

public class TsProjFormatTest : FormatTestBase<TsProjFormat>
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
        IFormat format = CreateAndReadFromFile("normalized_export_ashlang-de-DE.tsproj");

        format.TranslationUnits.Count.Should().Be(67);
        format.Header.SourceLanguage.Should().Be(new Language("en-US"));
        format.Header.TargetLanguage.Should().Be(new Language("de-DE"));
        //TODO: add author to tsproj

        const string id = "peru.CFileNotFoundError.Desc";
        format.TranslationUnits.GetTranslationUnit(id).Translations.GetTranslation(new Language("en-US")).Value.Should()
            .Be("The file '%FILE%' was not found.");
        format.TranslationUnits.GetTranslationUnit(id).Translations.GetTranslation(new Language("de-DE")).Value.Should()
            .Be("Die Datei '%FILE%' wurde nicht gefunden.");
    }


    [Fact]
    public async Task ReadAndWriteAshLangExportedFile()
    {
        var format = await CreateAndReadFromFileAsync("normalized_export_ashlang-de-DE.tsproj");

        var ms = new MemoryStream();
        await format.WriteAsync(ms);
        ms.Seek(0, SeekOrigin.Begin);

        await using var fs = CreateFileInStream("normalized_export_ashlang-de-DE.tsproj");

        //FIXME: compare formats like in the other tests, and not the streams!
        //ms.MustBeEqualTo(fs);
    }

    [Fact]
    public async Task ReadAndWriteNLangExportedFile()
    {
        await CreateAndReadFromFileAsync("normalized_export_nlang-de-DE.tsproj").Invoking(x => x).Should()
            .ThrowAsync<InvalidOperationException>();
    }
}