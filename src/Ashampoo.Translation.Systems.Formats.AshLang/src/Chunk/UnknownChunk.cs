namespace Ashampoo.Translation.Systems.Formats.AshLang.Chunk;

public class UnknownChunk : IChunk
{
    private readonly byte[] buffer;

    public UnknownChunk(string id, ulong size)
    {
        Id = id;
        buffer = new byte[size];
    }

    public string Id { get; }

    // Is never empty.
    public bool IsEmpty => false;

    public void Read(BinaryReader reader)
    {
        reader.Read(buffer, 0, buffer.Length);
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(buffer);
    }
}