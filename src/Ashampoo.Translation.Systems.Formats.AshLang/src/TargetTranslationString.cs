using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using Ashampoo.Translation.Systems.Formats.AshLang.Chunk;

namespace Ashampoo.Translation.Systems.Formats.AshLang;

/// <summary>
/// Implementation of the <see cref="ITranslationString"/> interface, representing a target translation string for the AshLang format.
/// </summary>
public class TargetTranslationString : ITranslationString
{
    private readonly TranslationChunk.Translation translation;

    /// <summary>
    /// Initializes a new instance of the <see cref="TargetTranslationString"/> class.
    /// </summary>
    /// <param name="language">
    /// The language of the translation string.
    /// </param>
    /// <param name="translation">
    /// The <see cref="TranslationChunk.Translation"/> of the translation string.
    /// </param>
    public TargetTranslationString(string language, TranslationChunk.Translation translation)
    {
        Language = language;
        this.translation = translation;
    }

    /// <inheritdoc />
    public string Value
    {
        get => translation.Value;
        set => translation.Value = value;
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
    public string Language { get; set; }
}