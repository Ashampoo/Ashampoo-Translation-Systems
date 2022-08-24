/*
Comment:

[ChunkString]
[Comment    ]
*/

namespace Ashampoo.Translation.Systems.Formats.AshLang.Chunk;

public class CommentChunk : IChunk
{
    public const string Id = "cmnt";
    string IChunk.Id => Id;

    public string Comment { get; set; } = "";
    public bool IsEmpty => string.IsNullOrEmpty(Comment);

    public void Read(BinaryReader reader)
    {
        Comment = ChunkString.Read(reader);
    }

    public void Write(BinaryWriter writer)
    {
        ChunkString.Write(writer, Comment);
    }

    public override string ToString()
    {
        return $"{Id}: {Comment}";
    }
}