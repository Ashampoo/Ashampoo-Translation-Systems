using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using Ashampoo.Translation.Systems.Formats.AshLang.Chunk;

namespace Ashampoo.Translation.Systems.Formats.AshLang;

/// <summary>
/// Provides a format for the AshLang format.
/// </summary>
public class AshLangFormat : IFormat
{
    /// <summary>
    /// The chunks of the ashlang file.
    /// </summary>
    public IChunk[] Chunks { get; private set; }

    /// <inheritdoc />
    public LanguageSupport LanguageSupport => LanguageSupport.SourceAndTarget;

    /// <inheritdoc />
    public ICollection<ITranslationUnit> TranslationUnits { get; } = new List<ITranslationUnit>();

    /// <inheritdoc />
    public IFormatHeader Header { get; private set; }


    /// <summary>
    /// Initializes a new instance of the <see cref="AshLangFormat"/> class.
    /// </summary>
    public AshLangFormat()
    {
        var ashLangFormatHeader = new AshLangFormatHeader();
        Header = ashLangFormatHeader;

        // some default Chunks on the way we need.
        var appIdChunk = new AppIdChunk();
        var versionChunk = new VersionChunk();
        var translationChunk = new TranslationChunk();
        Chunks =
        [
            ashLangFormatHeader.LanguageChunk,
            appIdChunk,
            ashLangFormatHeader.XDataChunk,
            versionChunk,
            translationChunk
        ];
    }

    /// <inheritdoc />
    public void Write(Stream stream)
    {
        var chunkWriter = new ChunkWriter(stream, Chunks);
        chunkWriter.Write();
    }
    
    /// <inheritdoc />
    public Task WriteAsync(Stream stream)
    {
        Write(stream);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task ReadAsync(Stream stream, FormatReadOptions? options = null)
    {
        try
        {
            var reader = new ChunkReader(stream);

            var language = reader.TryGetOrDefault<LanguageChunk>(LanguageChunk.Id);
            var xdata = reader.TryGetOrDefault<XDataChunk>(XDataChunk.Id);

            Header = new AshLangFormatHeader(language, xdata);

            var translations = reader.TryGetOrDefault<TranslationChunk>(TranslationChunk.Id);

            var sourceLanguage = Header.SourceLanguage ??
                                 throw new NullReferenceException("SourceLanguage can not be null.");
            foreach (var translation in translations.Translations)
            {
                var translationUnit = new DefaultTranslationUnit(translation.Id)
                {
                    Translations =
                    {
                        new SourceTranslationString(sourceLanguage, translation),
                        new TargetTranslationString(Header.TargetLanguage, translation)
                    }
                };
                TranslationUnits.Add(translationUnit);
            }

            Chunks = reader.Chunks;
        }
        catch (FormatException ex)
        {
            throw new UnsupportedFormatException(this, ex.Message, ex);
        }

        return Task.CompletedTask;
    }
}