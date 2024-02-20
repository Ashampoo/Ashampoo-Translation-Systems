using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using Ashampoo.Translation.Systems.TestBase;
using FluentAssertions;
using Xunit;

namespace Ashampoo.Translation.Systems.Formats.TsProj.Tests
{
    public class AshLangExportedTest : FormatTestBase<TsProjFormat>
    {
        [Fact]
        public void TranslationTest()
        {
            IFormat format = CreateAndReadFromFile("normalized_export_ashlang-de-DE.tsproj");

            const string id = "peru.CFileNotFoundError.GeneralDesc";

            format.TranslationUnits.GetTranslationUnit(id).Translations
                .GetTranslation(format.Header.SourceLanguage ?? "").Value.Should().Be("The file was not found.");
            format.TranslationUnits.GetTranslationUnit(id).Translations.GetTranslation(format.Header.TargetLanguage)
                .Value.Should().Be("Die Datei wurde nicht gefunden.");
        }
    }
}