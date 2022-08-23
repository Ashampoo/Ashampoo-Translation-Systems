using Ashampoo.Translations.Formats.Abstractions.Translation;

namespace Ashampoo.Translations.Formats.Abstractions.TranslationFilter;

/// <summary>
/// An implementation of the <see cref="ITranslationFilter"/> interface that si valid for every <see cref="ITranslationUnit"/> .
/// </summary>
public class DefaultTranslationFilter : ITranslationFilter
{
    public string? Language { get; init; }

    public bool IsValid(ITranslationUnit translationUnit) => true;
}