using Ashampoo.Translations.Formats.Abstractions.Translation;
using Ashampoo.Translations.Formats.AshLang.Chunk;

namespace Ashampoo.Translations.Formats.AshLang;

public class SourceTranslationString : ITranslationString
{
    private readonly TranslationChunk.Translation translation;

    private readonly string language;

    public SourceTranslationString(string language, TranslationChunk.Translation translation)
    {
        this.language = language;
        this.translation = translation;
    }

    public string Value
    {
        get => translation.Fallback;
        set
        {
            // Do nothing - Sources in AshLang are readonly.
        }
    }

    public string Id => translation.Id;

    public string? Comment
    {
        get => translation.Comment;
        set => translation.Comment = value ?? "";
    }

    public bool IsEmpty => string.IsNullOrWhiteSpace(Value);

    public string Language
    {
        get => language;
        set
        {
            //Do nothing, source language is always en-us
        }
    }
}