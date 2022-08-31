using System.Text;

namespace Ashampoo.Translation.Systems.Formats.AshLang.IO;

/// <summary>
/// Provides extension methods for the <see cref="BinaryReader"/> class.
/// </summary>
public static class BinaryReaderExtensions
{
    private static readonly Encoding Utf8Encoding = new UTF8Encoding();

    /// <summary>
    /// Indicates whether the current stream position is at the end of the stream.
    /// </summary>
    /// <param name="stream">
    /// The stream to check.
    /// </param>
    /// <returns>
    /// <see langword="true" /> if the current stream position is at the end of the stream; otherwise, <see langword="false" />.
    /// </returns>
    public static bool IsEndOfStream(this BinaryReader stream) =>
        stream.BaseStream.Position >= stream.BaseStream.Length;

    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// Reads the next characters from the stream as a UTF-8 encoded string.
    /// </summary>
    /// <param name="stream">
    /// The stream to read from.
    /// </param>
    /// <param name="count">
    /// The number of characters to read.
    /// </param>
    /// <returns>
    /// The read string.
    /// </returns>
    public static string ReadUTF8String(this BinaryReader stream, uint count)
    {
        var buffer = new byte[count];
        stream.Read(buffer, 0, Convert.ToInt32(count));
        return Utf8Encoding.GetString(buffer);
    }
}