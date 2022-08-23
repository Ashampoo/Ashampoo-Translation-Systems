using System.Collections.Generic;
using System.IO;
using Ashampoo.Translations.Formats.Abstractions;
using Ashampoo.Translations.Formats.Abstractions.Translation;
using Xunit;

namespace Ashampoo.Translations.TestBase;

public static class Helper
{
    public static string GetPathToFilename(params string[] path)
    {
        var append = Path.Combine(path);
        return Path.Combine(Directory.GetCurrentDirectory(), append);
    }

    public static void MustBeEqualTo(this Stream lhStream, Stream rhStream)
    {
        Assert.Equal(lhStream.Length, rhStream.Length);

        var lhByte = -1;
        var rhByte = -1;
        while ((lhByte = lhStream.ReadByte()) != -1 && (rhByte = rhStream.ReadByte()) != -1)
        {
            Assert.Equal(lhByte, rhByte);
        }
    }

    public static IList<ITranslation> ImportMockTranslationWithUnits(this IFormat format, string language, string id,
        string value = "Import Test")
    {
        var mockFormat =
            MockFormatWithTranslationUnits.CreateMockFormatWithTranslationUnits(language: language, id: id,
                value: value);
        return format.ImportFrom(mockFormat);
    }
}