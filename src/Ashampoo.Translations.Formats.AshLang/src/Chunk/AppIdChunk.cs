/*
Application Id:

[ChunkString][ChunkString]
[Name       ][Version    ]
*/

namespace Ashampoo.Translations.Formats.AshLang.Chunk;

public class AppIdChunk : IChunk
{
    public const string Id = "apid";
    string IChunk.Id => Id;
    public string Name { get; set; } = "";
    public string Version { get; set; } = "";
    public bool IsEmpty => string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(Version);

    public void Read(BinaryReader reader)
    {
        Name = ChunkString.Read(reader);
        Version = ChunkString.Read(reader);
    }

    public void Write(BinaryWriter writer)
    {
        ChunkString.Write(writer, Name);
        ChunkString.Write(writer, Version);
    }

    public override string ToString()
    {
        var version = string.IsNullOrEmpty(Version) ? "" : $" v{Version}";
        return $"{Id}: {Name}{version}";
    }
}