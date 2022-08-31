/*
AshLang Format Version:

[Uint32  ]
[Version ]
*/

namespace Ashampoo.Translation.Systems.Formats.AshLang.Chunk;

/// <summary>
/// Version of the AshLang format.
/// </summary>
public enum AshLangVersion : uint
{
    /// <summary>
    /// Represents an unknown version.
    /// </summary>
    AshLangVersionUnknown = 0,
    /// <summary>
    /// Represents version 1 of the AshLang format.
    /// </summary>
    AshLangFormatV1 = 1,
    /// <summary>
    /// Represents version 2 of the AshLang format.
    /// </summary>
    AshLangFormatV2 = 2
}

/// <summary>
/// Represents the Version chunk of the AshLang format.
/// </summary>
public class VersionChunk : IChunk
{
    /// <summary>
    /// The id of the chunk.
    /// </summary>
    public const string Id = "vers";
    string IChunk.Id => Id;
    /// <summary>
    /// The version of the AshLang format.
    /// </summary>
    public AshLangVersion Version { get; private set; } = AshLangVersion.AshLangFormatV2;

    // is never empty.
    /// <inheritdoc />
    public bool IsEmpty => false;

    /// <inheritdoc />
    public void Read(BinaryReader reader)
    {
        var version = reader.ReadUInt32();
        if (Enum.IsDefined(typeof(AshLangVersion), version)) Version = (AshLangVersion)version;
        else Version = AshLangVersion.AshLangVersionUnknown;
    }

    /// <inheritdoc />
    public void Write(BinaryWriter writer)
    {
        writer.Write(Convert.ToUInt32(Version));
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{Id}: {Version}";
    }
}