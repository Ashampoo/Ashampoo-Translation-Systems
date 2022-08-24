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
    AshLangVersionUnknown = 0,
    AshLangFormatV1 = 1,
    AshLangFormatV2 = 2
}

public class VersionChunk : IChunk
{
    public const string Id = "vers";
    string IChunk.Id => Id;
    public AshLangVersion Version { get; private set; } = AshLangVersion.AshLangFormatV2;

    // is never empty.
    public bool IsEmpty => false;

    public void Read(BinaryReader reader)
    {
        var version = reader.ReadUInt32();
        if (Enum.IsDefined(typeof(AshLangVersion), version)) Version = (AshLangVersion)version;
        else Version = AshLangVersion.AshLangVersionUnknown;
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(Convert.ToUInt32(Version));
    }

    public override string ToString()
    {
        return $"{Id}: {Version}";
    }
}