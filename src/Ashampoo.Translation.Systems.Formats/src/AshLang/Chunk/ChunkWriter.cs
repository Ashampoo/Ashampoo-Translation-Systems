using Ashampoo.Translation.Systems.Formats.AshLang.IO;

namespace Ashampoo.Translation.Systems.Formats.AshLang.Chunk;

/// <summary>
/// A writer to write AshLang chunks.
/// </summary>
public class ChunkWriter
{
    private readonly BinaryWriter _writer;
    private readonly IChunk[] _chunks;

    /// <summary>
    /// Initializes a new instance of the ChunkWriter class.
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="chunks"></param>
    public ChunkWriter(Stream stream, IChunk[] chunks)
    {
        _chunks = chunks;
        _writer = new BinaryWriter(stream);
    }

    /// <summary>
    /// Writes the complete AshLang format.
    /// </summary>
    public void Write()
    {
        // Header
        _writer.WriteUTF8String("URESFILE");

        // Write all chunks.
        WriteDataAndUpdateSize(_writer, () =>
        {
            foreach (var chunk in _chunks)
            {
                // TODO: Should this be optional?
                if (chunk.IsEmpty) continue;
                WriteChunk(chunk);
            }
        });

        _writer.Flush();
    }

    /// <summary>
    /// Writes one chunk.
    /// </summary>
    /// <param name="chunk"></param>
    private void WriteChunk(IChunk chunk)
    {
        _writer.WriteUTF8String(chunk.Id);

        WriteDataAndUpdateSize(_writer, () => { chunk.Write(_writer); });
    }

    /// <summary>
    /// Helper function to write data and update the size afterwards.
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="writeCallback"></param>
    public static void WriteDataAndUpdateSize(BinaryWriter writer, Action writeCallback)
    {
        // Remember the position in stream where we start.
        var startSizePosition = writer.BaseStream.Position;

        // Write 8 bytes for the size of the data (will be written later).
        writer.Write(Convert.ToUInt64(0));

        // Remember the position where the data starts.
        var startDataPosition = writer.BaseStream.Position;

        // Invoke callback to write the real data (e.g. a lambda).
        writeCallback.Invoke();

        // Remember last position in stream.
        var lastPosition = writer.BaseStream.Position;

        // Calculate the size of the data.
        var dataSize = lastPosition - startDataPosition;

        // Jump back to the beginning.
        writer.BaseStream.Seek(startSizePosition, SeekOrigin.Begin);

        // Write the size of the data (the 8 bytes).
        writer.Write(Convert.ToUInt64(dataSize));

        // Jump to the end of the stream.
        writer.BaseStream.Seek(lastPosition, SeekOrigin.Begin);
    }
}