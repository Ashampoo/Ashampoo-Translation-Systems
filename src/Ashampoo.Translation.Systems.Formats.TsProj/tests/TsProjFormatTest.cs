using System;
using System.IO;
using System.Threading.Tasks;
using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using Ashampoo.Translation.Systems.TestBase;
using Xunit;

namespace Ashampoo.Translation.Systems.Formats.TsProj.Tests;

public class TsProjFormatTest : FormatTestBase<TsProjFormat>
{
    [Fact]
    public void NewFormat()
    {
        var format = CreateFormat();

        Assert.NotNull(format);
        Assert.Empty(format.TranslationUnits);
        Assert.Null(format.Header.SourceLanguage);
        Assert.Equal(string.Empty, format.Header.TargetLanguage);
    }

    [Fact]
    public void ReadFromFile()
    {
        IFormat format = CreateAndReadFromFile("normalized_export_ashlang-de-DE.tsproj");

        Assert.Equal(67, format.TranslationUnits.Count);
        Assert.Equal("en-US", format.Header.SourceLanguage);
        Assert.Equal("de-DE", format.Header.TargetLanguage);
        //TODO: add author to tsproj

        const string id = "peru.CFileNotFoundError.Desc";
        Assert.Equal("The file '%FILE%' was not found.", format.TranslationUnits.GetTranslationUnit(id).Translations.GetTranslation("en-US").Value);
        Assert.Equal("Die Datei '%FILE%' wurde nicht gefunden.",
            format.TranslationUnits.GetTranslationUnit(id).Translations.GetTranslation("de-DE").Value);
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
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await CreateAndReadFromFileAsync("normalized_export_nlang-de-DE.tsproj"));
    }
}