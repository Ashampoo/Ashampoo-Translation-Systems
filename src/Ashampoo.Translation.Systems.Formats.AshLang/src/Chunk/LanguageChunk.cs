/*
Language:

[ChunkString][ChunkString  ][ChunkString ]
[LanguageId ][Language Name][Country Name]
*/

namespace Ashampoo.Translation.Systems.Formats.AshLang.Chunk;

public class LanguageChunk : IChunk
{
    public const string Id = "lang";
    string IChunk.Id => Id;
    public string LanguageId { get; set; } = "";
    public string Language { get; set; } = "";
    public string Country { get; set; } = "";

    public bool IsEmpty => string.IsNullOrEmpty(LanguageId) && string.IsNullOrEmpty(Language) &&
                           string.IsNullOrEmpty(Country);

    public void Read(BinaryReader reader)
    {
        LanguageId = ChunkString.Read(reader);
        Language = ChunkString.Read(reader);
        Country = ChunkString.Read(reader);
    }

    public void Write(BinaryWriter writer)
    {
        ChunkString.Write(writer, LanguageId);
        ChunkString.Write(writer, Language);
        ChunkString.Write(writer, Country);
    }

    public override string ToString()
    {
        return $"{Id}: {LanguageId} - {Language} ({Country})";
    }
}