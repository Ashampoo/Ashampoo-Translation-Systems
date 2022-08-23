using Ashampoo.Translations.Formats.Abstractions.Translation;

namespace Ashampoo.Translations.Formats.TsProj;

/// <summary>
/// Special implementation of the <see cref="ITranslationString"/> interface for the TsProj format.
/// </summary>
public class TranslationStringTarget : ITranslationString
{
    private readonly Element.Translation translationElement;

    public string Value
    {
        get => translationElement.Value;
        set => translationElement.Value = value;
    }

    public string Id => translationElement.Id;

    public string? Comment
    {
        get => translationElement.Comment;
        set => translationElement.Comment = value;
    }

    public string Language { get; set; } = string.Empty;

    /// <summary>
    /// Implementation of ITranslationString interface
    /// </summary>
    /// <param name="translationElement">
    /// /// The <see cref="Element.Translation"/> element to be used as a source for the <see cref="TranslationStringTarget"/>.
    /// </param>
    public TranslationStringTarget(Element.Translation translationElement)
    {
        this.translationElement = translationElement;
    }

    public bool IsEmpty => string.IsNullOrWhiteSpace(Value);
}