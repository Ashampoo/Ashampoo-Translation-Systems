using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

namespace Ashampoo.Translation.Systems.Formats.TsProj;

/// <summary>
/// Special implementation of the <see cref="ITranslationString"/> interface, for source string for the <see cref="TsProjFormat"/> .
/// </summary>
public class TranslationStringSource : ITranslation
{
    private readonly Element.Translation translationElement;

    /// <inheritdoc />
    public string Value
    {
        get => translationElement.Source ?? string.Empty;
        set => translationElement.Source = value;
    }

    /// <inheritdoc />
    public string Id => translationElement.Id;

    /// <inheritdoc />
    public string? Comment
    {
        get => translationElement.Comment;
        set => translationElement.Comment = value;
    }

    /// <inheritdoc />
    public string Language { get; set; } = string.Empty;

    /// <summary>
    /// Implementation of ITranslationString interface
    /// </summary>
    /// <param name="translationElement">
    /// The <see cref="Element.Translation"/> element to be used as a source for the <see cref="TranslationStringSource"/>.
    /// </param>
    public TranslationStringSource(Element.Translation translationElement)
    {
        this.translationElement = translationElement;
    }

    /// <inheritdoc />
    public bool IsEmpty => string.IsNullOrWhiteSpace(Value);
}