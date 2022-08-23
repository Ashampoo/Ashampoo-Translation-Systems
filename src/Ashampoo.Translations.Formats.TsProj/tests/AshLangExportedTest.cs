using Ashampoo.Translations.Formats.Abstractions;
using Ashampoo.Translations.Formats.Abstractions.Translation;
using Ashampoo.Translations.TestBase;
using Xunit;

namespace Ashampoo.Translations.Formats.TsProj.Tests
{
    public class AshLangExportedTest : FormatTestBase<TsProjFormat>
    {
        [Fact]
        public void TranslationTest()
        {
            IFormat format = CreateAndReadFromFile("normalized_export_ashlang-de-DE.tsproj");

            const string id = "peru.CFileNotFoundError.GeneralDesc";

            Assert.Equal("The file was not found.",
                (format[id]?[format.Header.SourceLanguage ?? ""] as ITranslationString)?.Value);
            Assert.Equal("Die Datei wurde nicht gefunden.",
                (format[id]?[format.Header.TargetLanguage] as ITranslationString)?.Value);
        }
    }
}