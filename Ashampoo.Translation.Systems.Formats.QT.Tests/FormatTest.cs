using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Models;
using Ashampoo.Translation.Systems.TestBase;

namespace Ashampoo.Translation.Systems.Formats.QT.Tests;

public class FormatTest : FormatTestBase<QTFormat>
{
    [Fact]
    public void ReadFromFileTest()
    {
        IFormat format =
            CreateAndReadFromFile("translation_de.ts", new FormatReadOptions() { TargetLanguage = new Language("de-DE") });
    }
}