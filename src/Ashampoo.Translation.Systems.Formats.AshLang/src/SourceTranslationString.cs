using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using Ashampoo.Translation.Systems.Formats.AshLang.Chunk;

namespace Ashampoo.Translation.Systems.Formats.AshLang;

/// <summary>
/// Implementation of the <see cref="ITranslation"/> interface, representing a source translation string for the AshLang format.
/// </summary>
public class SourceTranslationString : ITranslation
{
    private readonly TranslationChunk.Translation _translation;

    private readonly string _language;

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
        _language = language;
        _translation = translation;
    }

    /// <inheritdoc />
    public string Value
    {
        get => _translation.Fallback;
        set
        {
            // Do nothing - Sources in AshLang are readonly.
        }
    }

    /// <inheritdoc />
    public string? Comment
    {
        get => _translation.Comment;
        set => _translation.Comment = value ?? "";
    }

    /// <inheritdoc />
    public bool IsEmpty => string.IsNullOrWhiteSpace(Value);

    /// <inheritdoc />
    public string Language
    {
        get => _language;
        set
        {
            //Do nothing, source language is always en-us
        }
    }
}