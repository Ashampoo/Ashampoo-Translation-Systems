using System.IO;
using Ashampoo.Translations.Formats.AshLang.Chunk;
using Ashampoo.Translations.TestBase;
using Xunit;

namespace Ashampoo.Translations.Formats.AshLang.Tests;

public class ChunkReaderTest : FormatTestBase<AshLangFormat>
{
    private Stream GetNewStream()
    {
        var filename = GetAbsoluteFileName("normalized_peru-de-DE.ashLang");
        return new FileStream(filename, FileMode.Open, FileAccess.Read);
    }

    [Fact]
    public void LanguageChunk()
    {
        var chunkReader = new ChunkReader(GetNewStream());
        var chunk = chunkReader.TryGetOrNull<LanguageChunk>(Chunk.LanguageChunk.Id);
        Assert.Equal("de-DE", chunk?.LanguageId);
        Assert.Equal("Deutsch", chunk?.Language);
        Assert.Equal("Deutschland", chunk?.Country);
    }

    [Fact]
    public void CommentChunk()
    {
        var chunkReader = new ChunkReader(GetNewStream());
        var chunk = chunkReader.TryGetOrNull<CommentChunk>(Chunk.CommentChunk.Id);

        Assert.Null(chunk);
    }

    [Fact]
    public void AppIdChunk()
    {
        var chunkReader = new ChunkReader(GetNewStream());
        var chunk = chunkReader.TryGetOrNull<AppIdChunk>(Chunk.AppIdChunk.Id);

        Assert.Equal("peru", chunk?.Name);
        Assert.Equal("1.0.0.204", chunk?.Version);
    }

    [Fact]
    public void VersionChunk()
    {
        var chunkReader = new ChunkReader(GetNewStream());
        var chunk = chunkReader.TryGetOrNull<VersionChunk>(Chunk.VersionChunk.Id);

        Assert.Equal(AshLangVersion.AshLangFormatV2, chunk?.Version);
    }

    [Fact]
    public void XDataChunk()
    {
        var chunkReader = new ChunkReader(GetNewStream());
        var chunk = chunkReader.TryGetOrNull<XDataChunk>(Chunk.XDataChunk.Id);

        Assert.Equal(4, chunk?.Count);
        if (chunk is not null && chunk.ContainsKey("ASH_LANG_AUTHOR"))
            Assert.Equal("Ashampoo", chunk["ASH_LANG_AUTHOR"]);
        if (chunk is not null && chunk.ContainsKey("ASH_LANG_CREATION_TOOL"))
            Assert.Equal("Ashampoo Translation Studio Advanced", chunk["ASH_LANG_CREATION_TOOL"]);
        if (chunk is not null && chunk.ContainsKey("ASH_LANG_CREATION_TOOL_VERSION"))
            Assert.Equal("1.8.20.1", chunk["ASH_LANG_CREATION_TOOL_VERSION"]);
        if (chunk is not null && chunk.ContainsKey("ASH_LANG_MAIL"))
            Assert.Equal("translations@ashampoo.com", chunk["ASH_LANG_MAIL"]);
    }

    [Fact]
    public void TranslationChunk()
    {
        var chunkReader = new ChunkReader(GetNewStream());
        var chunk = chunkReader.TryGetOrNull<TranslationChunk>(Chunk.TranslationChunk.Id);

        Assert.Equal(67, chunk?.Translations.Count);
        var translation = chunk?.Translations[3];
        Assert.Equal("", translation?.Comment);
        Assert.Equal("Music files", translation?.Fallback);
        Assert.Equal((uint)0, translation?.Flags);
        Assert.Equal("peru.filesystem.smart.Music", translation?.Id);
        Assert.Equal("peru.filesystem.smart.Music", translation?.Key);
        Assert.Equal("Musikdateien", translation?.Value);
    }
}