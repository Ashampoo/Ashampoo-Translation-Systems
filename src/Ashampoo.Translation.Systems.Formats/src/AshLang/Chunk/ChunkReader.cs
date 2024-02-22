using Ashampoo.Translation.Systems.Formats.AshLang.IO;

namespace Ashampoo.Translation.Systems.Formats.AshLang.Chunk;

/// <summary>
/// A reader to read chunks from a stream.
/// </summary>
public class ChunkReader
{
    private BinaryReader? reader;
    private readonly IDictionary<string, IChunk> chunks = new Dictionary<string, IChunk>();

    /// <summary>
    /// Initializes a new instance of the ChunkReader class.
    /// </summary>
    /// <param name="stream"></param>
    /// <exception cref="FormatException"></exception>
    public ChunkReader(Stream stream)
    {
        if (stream.Length < 8) throw new FormatException("Stream size cannot be less than 8 bytes.");

        reader = new BinaryReader(stream);
        var header = reader.ReadUTF8String(8);
        if (header != "URESFILE")
            throw new FormatException("Stream is not an AshLang format, header 'URESFILE' was not found.");

        // We read over the size because we work with chunk ids anyway.
        var size = reader.ReadUInt64();
    }

    /// <summary>
    /// Returns an array of chunks.
    /// </summary>
    public IChunk[] Chunks
    {
        get
        {
            Flush();
            return chunks.Values.ToArray();
        }
    }

    /// <summary>
    /// If there are still chunks to be read, the remaining ones are read in with this.
    /// </summary>
    public void Flush()
    {
        // Return early.
        if (!HasChunksToRead()) return;

        // Read the rest of the chunks.
        while (ReadNextChunk() is { } chunk)
        {
            chunks.Add(chunk.Id, chunk);
        }
    }

    /// <summary>
    /// Internal helper function to read chunks.
    /// </summary>
    /// <param name="id">Id of the Chunk.</param>
    /// <returns>Instance of a chunk if the given id was found, null otherwise.</returns>
    private IChunk? TryGet(string id)
    {
        // Chunk already read and in the dictionary?
        if (chunks.TryGetValue(id, out var chunk)) return chunk;

        // Return early.
        if (!HasChunksToRead()) return null;

        // Read chunks till we find the given id.
        while ((chunk = ReadNextChunk()) is not null)
        {
            chunks.Add(chunk.Id, chunk);
            if (String.Equals(chunk.Id, id, StringComparison.CurrentCultureIgnoreCase)) return chunk;
        }

        // Nothing found.
        return null;
    }

    /// <summary>
    /// Returns the found chunk.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="id"></param>
    /// <returns>Instance of a chunk if the given id was found, null otherwise.</returns>
    public T? TryGetOrNull<T>(string id) where T : IChunk
    {
        var chunk = TryGet(id);
        if (chunk is T chunkT) return chunkT;
        return default(T);
    }

    /// <summary>
    /// Returns the found Chunk or a default created of the same id.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="id"></param>
    /// <returns>Instance of a Chunk if the given id was found, a new default created of the same id otherwise.</returns>
    public T TryGetOrDefault<T>(string id) where T : IChunk, new()
    {
        var chunk = TryGet(id);
        if (chunk is T tChunk) return tChunk;

        chunk = new T();
        chunks.Add(chunk.Id, chunk);
        return (T)chunk;
    }

    /// <summary>
    /// Determines of there are still chunks to read.
    /// </summary>
    /// <returns>true if there are still chunks to read, false otherwise.</returns>
    private bool HasChunksToRead() => reader is not null;

    /// <summary>
    /// Read the next chunk of the stream and returns an instance of it.
    /// </summary>
    /// <returns>Instance of a chunk, null if there are no more chunks to read.</returns>
    /// <exception cref="NullReferenceException"></exception>
    private IChunk? ReadNextChunk()
    {
        if (reader is null) throw new NullReferenceException(nameof(reader));

        // Close the stream and return null.
        if (reader.IsEndOfStream())
        {
            reader.Close();
            reader = null;
            return null;
        }

        // read the next chunk.
        var chunkId = reader.ReadUTF8String(4);
        var chunkSize = reader.ReadUInt64();
        return ReadChunk(reader, chunkId, chunkSize);
    }

    /// <summary>
    /// Helper function to create the correct instance of a chunk based on the given id.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="size"></param>
    /// <returns>Instance of a chunk based on the given id.</returns>
    private IChunk CreateChunk(string id, ulong size)
    {
        if (id == AppIdChunk.Id) return new AppIdChunk();
        if (id == CommentChunk.Id) return new CommentChunk();
        if (id == LanguageChunk.Id) return new LanguageChunk();
        if (id == VersionChunk.Id) return new VersionChunk();
        if (id == XDataChunk.Id) return new XDataChunk();
        if (id == TranslationChunk.Id) return new TranslationChunk();

        return new UnknownChunk(id, size);
    }

    /// <summary>
    /// Reads the chunk with given id and size at the current position of the stream.
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="id"></param>
    /// <param name="size"></param>
    /// <returns>Instance of a chunk.</returns>
    private IChunk ReadChunk(BinaryReader stream, string id, ulong size)
    {
        var chunk = CreateChunk(id, size);
        chunk.Read(stream);
        return chunk;
    }
}