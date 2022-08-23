using System.Text;

namespace Ashampoo.Translations.Formats.AshLang.IO;

public static class BinaryWriterExtensions
{
    private static readonly Encoding Utf8Encoding = new UTF8Encoding();

    // ReSharper disable once InconsistentNaming
    public static void WriteUTF8String(this BinaryWriter stream, string input)
    {
        var buffer = Utf8Encoding.GetBytes(input);
        stream.Write(buffer, 0, buffer.Length);
    }
}