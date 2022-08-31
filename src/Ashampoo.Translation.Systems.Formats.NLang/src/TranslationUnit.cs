using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

namespace Ashampoo.Translation.Systems.Formats.NLang;

/// <summary>
/// Implements the <see cref="ITranslationUnit"/> interface for the NLang format.
/// </summary>
public class TranslationUnit : AbstractTranslationUnit
{
    /// <inheritdoc />
    public TranslationUnit(string id) : base(id)
    {
    }

    /// <summary>
    /// Asynchronously writes the translation unit to the specified <see cref="TextWriter"/>.
    /// </summary>
    /// <param name="writer">
    /// The <see cref="TextWriter"/> to write to.
    /// </param>
    public async Task WriteAsync(TextWriter writer)
    {
        foreach (var translation in this)
        {
            if (translation is TranslationString translationString) await translationString.WriteAsync(writer);
        }
    }
}