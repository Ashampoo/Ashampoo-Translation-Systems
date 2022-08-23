using Ashampoo.Translations.Formats.AshLang.IO;

/*
TranslationData:

[uint ][ChunkString][ChunkString][ChunkString][ChunkString]
[Flags][Key        ][Value      ][Fallback   ][Comment    ]

Translations:
[4Bytes][UInt64   ][UInt32           ][TranslationData]*
['data'][Data Size][Translation Count][Translation    ]*
*/

namespace Ashampoo.Translations.Formats.AshLang.Chunk;

public class TranslationChunk : IChunk
{
    public class Translation
    {
        public Translation(uint flags, string key, string value, string fallback, string comment)
        {
            Flags = flags;
            Key = key;
            Id = key;
            Value = value;
            Fallback = fallback;
            Comment = comment;
        }

        public uint Flags { get; }
        public string Key { get; }
        public string Fallback { get; }
        public string Comment { get; set; }
        public string Value { get; set; }
        public string Id { get; set; }
    }

    string IChunk.Id => Id;
    public const string Id = "STMP";
    public readonly IList<Translation> Translations = new List<Translation>();
    public bool IsEmpty => Translations.Count == 0;

    public void Read(BinaryReader reader)
    {
        var dataId = reader.ReadUTF8String(4);
        var dataSize = reader.ReadUInt64();
        var count = reader.ReadUInt32();
        for (var i = 0; i < count; ++i)
        {
            var flags = reader.ReadUInt32();
            var key = ChunkString.Read(reader);
            var value = ChunkString.Read(reader);
            var fallback = ChunkString.Read(reader);
            var comment = ChunkString.Read(reader);

            Translations.Add(new Translation(flags, key, value, fallback, comment));
        }
    }

    public void Write(BinaryWriter writer)
    {
        /*
        This header is unknown to me. It just exists and
        is undocumented in the old C++ sources. But we
        have to read and write it.
        */
        writer.WriteUTF8String("data");

        ChunkWriter.WriteDataAndUpdateSize(writer, () =>
        {
            // Write translation count.
            writer.Write(Convert.ToUInt32(Translations.Count));
            foreach (var translation in Translations)
            {
                writer.Write(Convert.ToUInt32(translation.Flags));
                ChunkString.Write(writer, translation.Key);
                ChunkString.Write(writer, translation.Value);
                ChunkString.Write(writer, translation.Fallback);
                ChunkString.Write(writer, translation.Comment);
            }
        });
    }
}