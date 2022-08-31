/*
X-Data:

[Uint32][[ChunkString][ChunkString]]*
[Count ][[Key        ][Value]      ]*
*/

namespace Ashampoo.Translation.Systems.Formats.AshLang.Chunk;

/// <summary>
/// Represents a chunk of data.
/// </summary>
public class XDataChunk : Dictionary<string, string>, IChunk
{
    /// <summary>
    /// Default constructor.
    /// </summary>
    public XDataChunk()
    {
    }

    /// <summary>
    /// Copy constructor.
    /// </summary>
    /// <param name="dataChunk"></param>
    public XDataChunk(XDataChunk dataChunk) : base(dataChunk)
    {
    }

    /// <summary>
    /// The id of the chunk.
    /// </summary>
    public const string Id = "xdid";
    string IChunk.Id => Id;

    /// <inheritdoc />
    public bool IsEmpty => Count == 0;

    /// <inheritdoc />
    public void Read(BinaryReader reader)
    {
        var count = reader.ReadUInt32();
        for (var i = 0; i < count; ++i)
        {
            var key = ChunkString.Read(reader);
            var value = ChunkString.Read(reader);
            Add(key, value);
        }
    }

    /// <inheritdoc />
    public void Write(BinaryWriter writer)
    {
        writer.Write(Convert.ToUInt32(Count));
        foreach (var pair in this)
        {
            ChunkString.Write(writer, pair.Key);
            ChunkString.Write(writer, pair.Value);
        }
    }

    /// <inheritdoc />
    public override string ToString()
    {
        var res = string.Join(",", this.Select(p => $"{p.Key}={p.Value}"));
        return $"{Id}: {{{res}}}";
    }
}