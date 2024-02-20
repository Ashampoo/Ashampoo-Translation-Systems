using System.IO;
using System.Linq;
using Ashampoo.Translation.Systems.Formats.AshLang.Chunk;
using Ashampoo.Translation.Systems.TestBase;
using FluentAssertions;
using Xunit;

namespace Ashampoo.Translation.Systems.Formats.AshLang.Tests;

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

        chunk.Should().NotBeNull();
        chunk!.LanguageId.Should().Be("de-DE");
        chunk.Language.Should().Be("Deutsch");
        chunk.Country.Should().Be("Deutschland");
    }

    [Fact]
    public void CommentChunk()
    {
        var chunkReader = new ChunkReader(GetNewStream());
        var chunk = chunkReader.TryGetOrNull<CommentChunk>(Chunk.CommentChunk.Id);

        chunk.Should().BeNull();
    }

    [Fact]
    public void AppIdChunk()
    {
        var chunkReader = new ChunkReader(GetNewStream());
        var chunk = chunkReader.TryGetOrNull<AppIdChunk>(Chunk.AppIdChunk.Id);

        chunk.Should().NotBeNull();
        chunk!.Name.Should().Be("peru");
        chunk.Version.Should().Be("1.0.0.204");
    }

    [Fact]
    public void VersionChunk()
    {
        var chunkReader = new ChunkReader(GetNewStream());
        var chunk = chunkReader.TryGetOrNull<VersionChunk>(Chunk.VersionChunk.Id);

        chunk.Should().NotBeNull();
        chunk!.Version.Should().Be(AshLangVersion.AshLangFormatV2);
    }

    [Fact]
    public void XDataChunk()
    {
        var chunkReader = new ChunkReader(GetNewStream());
        var chunk = chunkReader.TryGetOrNull<XDataChunk>(Chunk.XDataChunk.Id);

        chunk.Should().NotBeNull();
        chunk!.Count.Should().Be(4);
        chunk["ASH_LANG_AUTHOR"].Should().Be("Ashampoo");
        chunk["ASH_LANG_CREATION_TOOL"].Should().Be("Ashampoo Translation Studio Advanced");
        chunk["ASH_LANG_CREATION_TOOL_VERSION"].Should().Be("1.8.20.1");
        chunk["ASH_LANG_MAIL"].Should().Be("translations@ashampoo.com");
    }

    [Fact]
    public void TranslationChunk()
    {
        var chunkReader = new ChunkReader(GetNewStream());
        var chunk = chunkReader.TryGetOrNull<TranslationChunk>(Chunk.TranslationChunk.Id);

        chunk.Should().NotBeNull();
        chunk!.Translations.Count.Should().Be(67);
        
        var translation = chunk.Translations[3];
        translation.Should().NotBeNull();
        translation.Comment.Should().BeEmpty();
        translation.Fallback.Should().Be("Music files");
        translation.Flags.Should().Be(0);
        translation.Id.Should().Be("peru.filesystem.smart.Music");
        translation.Key.Should().Be("peru.filesystem.smart.Music");
        translation.Value.Should().Be("Musikdateien");
    }
}