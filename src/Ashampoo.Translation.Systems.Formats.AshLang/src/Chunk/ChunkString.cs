/*
ChunkString:

[Uint32                              ][2 Byte...  ]
[Count of 2 bytes (utf-16) characters][String Data]
*/

using System.Text;

namespace Ashampoo.Translation.Systems.Formats.AshLang.Chunk;

/// <summary>
/// Helper class to read and write ChunkStrings.
/// </summary>
public static class ChunkString
{
    private static readonly Encoding Utf16Encoding = new UnicodeEncoding();

    /// <summary>
    /// Reads a string from stream.
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    /// <exception cref="FormatException"></exception>
    public static string Read(BinaryReader reader)
    {
        /*
        The size is the number of 2 bytes characters. Therefore
        we have to multiply the number by 2 to read in the corresponding bytes.
        */
        var size = reader.ReadUInt32() * 2;
        var buffer = new byte[size];
        if (reader.Read(buffer, 0, buffer.Length) != buffer.Length)
            throw new FormatException($"Could not read ChunkString of {buffer.Length} bytes.");
        var result = Utf16Encoding.GetString(buffer);

        // remove null-termination.
        return result.Trim('\0').Replace("\r\n", "\n");
    }

    /// <summary>
    /// Writes a string to the stream.
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="input"></param>
    public static void Write(BinaryWriter writer, string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            // Write string size of '0'.
            writer.Write(Convert.ToInt32(0));
        }
        else
        {
            // Add null-termination.
            var buffer = Utf16Encoding.GetBytes(input + '\0');

            /*
            The size is the number of 2 bytes characters. Therefore
            we have to divide the number by 2 to write the corresponding bytes.
            */
            writer.Write(Convert.ToInt32(buffer.Length / 2));
            writer.Write(buffer, 0, buffer.Length);
        }
    }
}