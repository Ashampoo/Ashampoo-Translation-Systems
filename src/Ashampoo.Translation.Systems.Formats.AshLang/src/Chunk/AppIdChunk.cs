/*
Application Id:

[ChunkString][ChunkString]
[Name       ][Version    ]
*/

namespace Ashampoo.Translation.Systems.Formats.AshLang.Chunk;

/// <summary>
/// Chunk for Application Id
/// </summary>
public class AppIdChunk : IChunk
{
    /// <summary>
    /// Id of the chunk
    /// </summary>
    public const string Id = "apid";
    string IChunk.Id => Id;
    /// <summary>
    /// Name of the application
    /// </summary>
    public string Name { get; set; } = "";
    /// <summary>
    /// Version of the application
    /// </summary>
    public string Version { get; set; } = "";

    /// <inheritdoc />
    public bool IsEmpty => string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(Version);

    /// <inheritdoc />
    public void Read(BinaryReader reader)
    {
        Name = ChunkString.Read(reader);
        Version = ChunkString.Read(reader);
    }

    /// <inheritdoc />
    public void Write(BinaryWriter writer)
    {
        ChunkString.Write(writer, Name);
        ChunkString.Write(writer, Version);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        var version = string.IsNullOrEmpty(Version) ? "" : $" v{Version}";
        return $"{Id}: {Name}{version}";
    }
}