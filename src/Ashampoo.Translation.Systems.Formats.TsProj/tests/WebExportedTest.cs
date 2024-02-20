using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using Ashampoo.Translation.Systems.TestBase;
using Xunit;

namespace Ashampoo.Translation.Systems.Formats.TsProj.Tests;

public class WebExportedTest : FormatTestBase<TsProjFormat>
{
    [Fact]
    public void TranslationTest()
    {
        var format = CreateAndReadFromFile("HU-DE_web_15_10_2021-12_59_44.tsproj");

        const string id = "textids.content.T_1904_SUBMIT_80";
        const string value = "Auf Windows 11 Systemvoraussetzungen testen und Installation ermöglichen";

        Assert.Equal(value, format.TranslationUnits.GetTranslationUnit(id).Translations.GetTranslation(format.Header.SourceLanguage!).Value);
        Assert.Equal(string.Empty, format.TranslationUnits.GetTranslationUnit(id).Translations.GetTranslation(format.Header.TargetLanguage).Value);
    }
}