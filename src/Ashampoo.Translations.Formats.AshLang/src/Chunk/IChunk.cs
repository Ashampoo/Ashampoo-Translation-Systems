namespace Ashampoo.Translations.Formats.AshLang.Chunk;

/// <summary>
/// Interface of a chunk.
/// </summary>
public interface IChunk
{
    /// <summary>
    /// Unique id of a chunk.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Determines if a chunk is empty and should be written.
    /// </summary>
    bool IsEmpty { get; }

    /// <summary>
    /// Reads the chunk from the given stream.
    /// </summary>
    /// <param name="reader"></param>
    void Read(BinaryReader reader);

    /// <summary>
    /// Writes the chunk to the given stream.
    /// </summary>
    /// <param name="writer"></param>
    void Write(BinaryWriter writer);
}