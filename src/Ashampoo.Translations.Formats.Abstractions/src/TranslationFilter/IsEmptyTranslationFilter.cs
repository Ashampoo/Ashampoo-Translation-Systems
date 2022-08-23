using Ashampoo.Translations.Formats.Abstractions.Translation;

namespace Ashampoo.Translations.Formats.Abstractions.TranslationFilter;

/// <summary>
/// An implementation of <see cref="ITranslationFilter"/> that filters indicates whether a translation is empty for the specified language.
/// </summary>
public class IsEmptyTranslationFilter : ITranslationFilter
{
    public string? Language { get; init; }

    public IsEmptyTranslationFilter(string? language = null)
    {
        Language = language;
    }

    public bool IsValid(ITranslationUnit translationUnit)
    {
        if (Language is not null) return translationUnit.TryGet(Language)?.IsEmpty ?? true;

        return translationUnit.Any(translation => translation.IsEmpty);
    }
}