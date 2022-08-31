namespace Ashampoo.Translation.Systems.Formats.AshLang.Chunk;

/// <summary>
/// Represents an unknown chunk.
/// </summary>
public class UnknownChunk : IChunk
{
    private readonly byte[] buffer;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnknownChunk"/> class.
    /// </summary>
    /// <param name="id">
    /// The chunk ID.
    /// </param>
    /// <param name="size">
    /// The chunk size.
    /// </param>
    public UnknownChunk(string id, ulong size)
    {
        Id = id;
        buffer = new byte[size];
    }

    /// <inheritdoc />
    public string Id { get; }


    /// <inheritdoc />
    public bool IsEmpty => false;


    /// <inheritdoc />
    public void Read(BinaryReader reader)
    {
        reader.Read(buffer, 0, buffer.Length);
    }

    /// <inheritdoc />
    public void Write(BinaryWriter writer)
    {
        writer.Write(buffer);
    }
}