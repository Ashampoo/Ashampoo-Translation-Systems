using System.Text;

namespace Ashampoo.Translations.Formats.AshLang.IO;

public static class BinaryReaderExtensions
{
    private static readonly Encoding Utf8Encoding = new UTF8Encoding();

    public static bool IsEndOfStream(this BinaryReader stream) =>
        stream.BaseStream.Position >= stream.BaseStream.Length;

    // ReSharper disable once InconsistentNaming
    public static string ReadUTF8String(this BinaryReader stream, uint count)
    {
        var buffer = new byte[count];
        stream.Read(buffer, 0, Convert.ToInt32(count));
        return Utf8Encoding.GetString(buffer);
    }
}