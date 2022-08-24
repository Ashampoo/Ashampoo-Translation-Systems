using System.Collections.Generic;
using System.IO;
using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using Xunit;

namespace Ashampoo.Translation.Systems.TestBase;

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