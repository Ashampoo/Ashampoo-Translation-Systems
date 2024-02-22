/*
Comment:

[ChunkString]
[Comment    ]
*/

namespace Ashampoo.Translation.Systems.Formats.AshLang.Chunk;

/// <summary>
/// Represents a comment chunk.
/// </summary>
public class CommentChunk : IChunk
{
    /// <summary>
    /// Id of the chunk.
    /// </summary>
    public const string Id = "cmnt";
    string IChunk.Id => Id;

    /// <summary>
    /// The comment.
    /// </summary>
    public string Comment { get; set; } = "";

    /// <inheritdoc />
    public bool IsEmpty => string.IsNullOrEmpty(Comment);

    /// <inheritdoc />
    public void Read(BinaryReader reader)
    {
        Comment = ChunkString.Read(reader);
    }

    /// <inheritdoc />
    public void Write(BinaryWriter writer)
    {
        ChunkString.Write(writer, Comment);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{Id}: {Comment}";
    }
}