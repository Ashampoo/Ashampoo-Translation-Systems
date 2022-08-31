using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using Ashampoo.Translation.Systems.Formats.AshLang.Chunk;

namespace Ashampoo.Translation.Systems.Formats.AshLang;

/// <summary>
/// Implementation of the <see cref="ITranslationString"/> interface, representing a source translation string for the AshLang format.
/// </summary>
public class SourceTranslationString : ITranslationString
{
    private readonly TranslationChunk.Translation translation;

    private readonly string language;

    /// <summary>
    /// Initializes a new instance of the <see cref="SourceTranslationString"/> class.
    /// </summary>
    /// <param name="language">
    /// The language of the translation string.
    /// </param>
    /// <param name="translation">
    /// The <see cref="TranslationChunk.Translation"/>
    /// </param>
    public SourceTranslationString(string language, TranslationChunk.Translation translation)
    {
        this.language = language;
        this.translation = translation;
    }

    /// <inheritdoc />
    public string Value
    {
        get => translation.Fallback;
        set
        {
            // Do nothing - Sources in AshLang are readonly.
        }
    }

    /// <inheritdoc />
    public string Id => translation.Id;

    /// <inheritdoc />
    public string? Comment
    {
        get => translation.Comment;
        set => translation.Comment = value ?? "";
    }

    /// <inheritdoc />
    public bool IsEmpty => string.IsNullOrWhiteSpace(Value);

    /// <inheritdoc />
    public string Language
    {
        get => language;
        set
        {
            //Do nothing, source language is always en-us
        }
    }
}