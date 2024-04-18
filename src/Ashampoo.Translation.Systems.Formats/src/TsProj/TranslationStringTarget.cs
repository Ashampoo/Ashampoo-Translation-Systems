using Ashampoo.Translation.Systems.Formats.Abstractions.Models;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

namespace Ashampoo.Translation.Systems.Formats.TsProj;

/// <summary>
/// Special implementation of the <see cref="ITranslation"/> interface for the TsProj format.
/// </summary>
public class TranslationStringTarget : ITranslation
{
    private readonly Element.Translation _translationElement;

    /// <inheritdoc />
    public string Value
    {
        get => _translationElement.Value;
        set => _translationElement.Value = value;
    }

    /// <inheritdoc />
    public IList<string> Comments { get; set; }

    /// <inheritdoc />
    public Language Language { get; set; } = Language.Empty;

    /// <summary>
    /// Implementation of <see cref="ITranslation"/> interface
    /// </summary>
    /// <param name="translationElement">
    /// /// The <see cref="Element.Translation"/> element to be used as a source for the <see cref="TranslationStringTarget"/>.
    /// </param>
    public TranslationStringTarget(Element.Translation translationElement)
    {
        _translationElement = translationElement;
        Comments = [_translationElement.Comment];
    }
}