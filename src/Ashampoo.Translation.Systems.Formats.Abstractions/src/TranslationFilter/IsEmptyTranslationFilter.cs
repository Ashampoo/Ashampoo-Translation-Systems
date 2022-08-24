using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

namespace Ashampoo.Translation.Systems.Formats.Abstractions.TranslationFilter;

/// <summary>
/// An implementation of <see cref="ITranslationFilter"/> that filters indicates
/// whether a translation is empty for the specified language.
/// </summary>
public class IsEmptyTranslationFilter : ITranslationFilter
{
    /// <inheritdoc />
    public string? Language { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="IsEmptyTranslationFilter"/> class, with the specified language.
    /// If no language is specified, the filter will match all languages.
    /// </summary>
    /// <param name="language"></param>
    public IsEmptyTranslationFilter(string? language = null)
    {
        Language = language;
    }

    
    /// <summary>
    /// Determines whether the specified translation is empty for the specified language, or for all languages.
    /// </summary>
    /// <param name="translationUnit">
    /// The translation unit to test.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the translation is empty for the specified language, or for all languages;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public bool IsValid(ITranslationUnit translationUnit)
    {
        if (Language is not null) return translationUnit.TryGet(Language)?.IsEmpty ?? true;

        return translationUnit.Any(translation => translation.IsEmpty);
    }
}