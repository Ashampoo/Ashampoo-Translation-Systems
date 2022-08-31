using Ashampoo.Translation.Systems.Formats.AshLang.IO;

/*
TranslationData:

[uint ][ChunkString][ChunkString][ChunkString][ChunkString]
[Flags][Key        ][Value      ][Fallback   ][Comment    ]

Translations:
[4Bytes][UInt64   ][UInt32           ][TranslationData]*
['data'][Data Size][Translation Count][Translation    ]*
*/

namespace Ashampoo.Translation.Systems.Formats.AshLang.Chunk;

/// <summary>
/// Represents a translation chunk.
/// </summary>
public class TranslationChunk : IChunk
{
    /// <summary>
    /// Represents a translation in an AshLang file.
    /// </summary>
    public class Translation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Translation"/> class.
        /// </summary>
        /// <param name="flags">
        /// The flags of the translation.
        /// </param>
        /// <param name="key">
        /// The key of the translation.
        /// </param>
        /// <param name="value">
        /// The value of the translation.
        /// </param>
        /// <param name="fallback">
        /// The fallback of the translation.
        /// </param>
        /// <param name="comment">
        /// The comment of the translation.
        /// </param>
        public Translation(uint flags, string key, string value, string fallback, string comment)
        {
            Flags = flags;
            Key = key;
            Id = key;
            Value = value;
            Fallback = fallback;
            Comment = comment;
        }

        /// <summary>
        /// Gets the flags of the translation.
        /// </summary>
        public uint Flags { get; }
        /// <summary>
        /// Gets the key of the translation.
        /// </summary>
        public string Key { get; }
        /// <summary>
        /// Gets the id of the translation.
        /// </summary>
        public string Fallback { get; }
        /// <summary>
        /// Gets or sets the value of the translation.
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// Gets or sets the fallback of the translation.
        /// </summary>
        public string Value { get; set; }
        
        /// <summary>
        /// Gets or sets the Id of the translation.
        /// </summary>
        public string Id { get; set; }
    }

    string IChunk.Id => Id;
    /// <summary>
    /// The id of the chunk.
    /// </summary>
    public const string Id = "STMP";
    /// <summary>
    /// The translations of the chunk.
    /// </summary>
    public readonly IList<Translation> Translations = new List<Translation>();

    /// <inheritdoc />
    public bool IsEmpty => Translations.Count == 0;

    /// <inheritdoc />
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

    /// <inheritdoc />
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