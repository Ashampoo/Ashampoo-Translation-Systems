using Ashampoo.Translation.Systems.Formats.Abstractions.Models;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

namespace Ashampoo.Translation.Systems.Formats.TsProj;

/// <summary>
/// Special implementation of the <see cref="ITranslation"/> interface, for source string for the <see cref="TsProjFormat"/> .
/// </summary>
public class TranslationStringSource : ITranslation
{
    /// <inheritdoc />
    public string Value { get; set; }

    /// <inheritdoc />
    public IList<string> Comments { get; set; }

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
        Comments = string.IsNullOrWhiteSpace(translationElement.Comment) ? [] : [translationElement.Comment];
        Value = translationElement.Source ?? string.Empty;
    }
}