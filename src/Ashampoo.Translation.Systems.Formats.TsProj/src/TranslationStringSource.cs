using Ashampoo.Translation.Systems.Formats.Abstractions.Models;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

namespace Ashampoo.Translation.Systems.Formats.TsProj;

/// <summary>
/// Special implementation of the <see cref="ITranslation"/> interface, for source string for the <see cref="TsProjFormat"/> .
/// </summary>
public class TranslationStringSource : ITranslation
{
    private readonly Element.Translation _translationElement;

    /// <inheritdoc />
    public string Value
    {
        get => _translationElement.Source ?? string.Empty;
        set => _translationElement.Source = value;
    }
    
    /// <inheritdoc />
    public string? Comment
    {
        get => _translationElement.Comment;
        set => _translationElement.Comment = value;
    }

    /// <inheritdoc />
    public Language Language { get; set; } = Language.Empty;

    /// <summary>
    /// Implementation of <see cref="ITranslation"/> interface
    /// </summary>
    /// <param name="translationElement">
    /// The <see cref="Element.Translation"/> element to be used as a source for the <see cref="TranslationStringSource"/>.
    /// </param>
    public TranslationStringSource(Element.Translation translationElement)
    {
        _translationElement = translationElement;
    }
}