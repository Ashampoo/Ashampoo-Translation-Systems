/*
X-Data:

[Uint32][[ChunkString][ChunkString]]*
[Count ][[Key        ][Value]      ]*
*/

namespace Ashampoo.Translations.Formats.AshLang.Chunk;

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

    public const string Id = "xdid";
    string IChunk.Id => Id;
    public bool IsEmpty => Count == 0;

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

    public void Write(BinaryWriter writer)
    {
        writer.Write(Convert.ToUInt32(Count));
        foreach (var pair in this)
        {
            ChunkString.Write(writer, pair.Key);
            ChunkString.Write(writer, pair.Value);
        }
    }

    public override string ToString()
    {
        var res = string.Join(",", this.Select(p => $"{p.Key}={p.Value}"));
        return $"{Id}: {{{res}}}";
    }
}