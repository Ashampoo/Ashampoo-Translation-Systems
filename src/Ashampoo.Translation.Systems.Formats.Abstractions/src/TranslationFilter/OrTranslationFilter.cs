using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

namespace Ashampoo.Translation.Systems.Formats.Abstractions.TranslationFilter;

/// <summary>
/// Implementation of the <see cref="ITranslationFilter"/> interface, representing an OR-combination of other filters.
/// </summary>
public class OrTranslationFilter : ITranslationFilter
{
    private readonly List<ITranslationFilter> filters = new();

    public OrTranslationFilter(params ITranslationFilter[] filters)
    {
        this.filters.AddRange(filters);
    }

    public bool IsValid(ITranslationUnit translationUnit)
    {
        return filters.Any(filter => filter.IsValid(translationUnit));
    }

    public override string ToString()
    {
        return "OR";
    }
}