using System.Text;

namespace Ashampoo.Translation.Systems.Formats.AshLang.IO;

/// <summary>
/// Provides extension methods for the <see cref="BinaryWriter"/> class.
/// </summary>
public static class BinaryWriterExtensions
{
    private static readonly Encoding Utf8Encoding = new UTF8Encoding();

    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// Writes a string to the underlying stream using UTF-8 encoding.
    /// </summary>
    /// <param name="stream">
    /// The stream to write to.
    /// </param>
    /// <param name="input">
    /// The string to write.
    /// </param>
    public static void WriteUTF8String(this BinaryWriter stream, string input)
    {
        var buffer = Utf8Encoding.GetBytes(input);
        stream.Write(buffer, 0, buffer.Length);
    }
}