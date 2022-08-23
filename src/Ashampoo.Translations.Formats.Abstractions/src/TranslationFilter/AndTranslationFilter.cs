using Ashampoo.Translations.Formats.Abstractions.Translation;

namespace Ashampoo.Translations.Formats.Abstractions.TranslationFilter;

/// <summary>
/// Implementation of <see cref="ITranslationFilter"/>, representing an And-combination of filters.
/// </summary>
public class AndTranslationFilter : ITranslationFilter
{
    private readonly List<ITranslationFilter> filters = new();

    public AndTranslationFilter(params ITranslationFilter[] filters)
    {
        this.filters.AddRange(filters);
    }

    public bool IsValid(ITranslationUnit translationUnit)
    {
        return filters.All(filter => filter.IsValid(translationUnit));
    }

    public override string ToString()
    {
        return "AND";
    }
}