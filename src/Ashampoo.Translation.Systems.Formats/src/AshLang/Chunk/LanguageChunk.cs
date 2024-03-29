/*
Language:

[ChunkString][ChunkString  ][ChunkString ]
[LanguageId ][Language Name][Country Name]
*/

using Ashampoo.Translation.Systems.Formats.Abstractions.Models;

namespace Ashampoo.Translation.Systems.Formats.AshLang.Chunk;

/// <summary>
/// Represents a language chunk.
/// </summary>
public class LanguageChunk : IChunk
{
    /// <summary>
    /// The id of the chunk.
    /// </summary>
    public const string Id = "lang";
    string IChunk.Id => Id;

    /// <summary>
    /// The language id.
    /// </summary>
    public Language LanguageId { get; set; } = Abstractions.Models.Language.Empty;
    /// <summary>
    /// The language name.
    /// </summary>
    public string Language { get; set; } = "";
    /// <summary>
    /// The country name.
    /// </summary>
    public string Country { get; set; } = "";

    /// <inheritdoc />
    public bool IsEmpty => string.IsNullOrEmpty(LanguageId.ToString()) && string.IsNullOrEmpty(Language) &&
                           string.IsNullOrEmpty(Country);

    /// <inheritdoc />
    public void Read(BinaryReader reader)
    {
        LanguageId = new Language(ChunkString.Read(reader));
        Language = ChunkString.Read(reader);
        Country = ChunkString.Read(reader);
    }

    /// <inheritdoc />
    public void Write(BinaryWriter writer)
    {
        ChunkString.Write(writer, LanguageId.ToString());
        ChunkString.Write(writer, Language);
        ChunkString.Write(writer, Country);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{Id}: {LanguageId} - {Language} ({Country})";
    }
}