using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

namespace Ashampoo.Translation.Systems.Formats.Abstractions.TranslationFilter;

/// <summary>
/// Implementation of the <see cref="ITranslationFilter"/> interface, representing an OR-combination of other filters.
/// </summary>
public class OrTranslationFilter : ITranslationFilter
{
    private readonly List<ITranslationFilter> filters = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="OrTranslationFilter"/> class, with the given filters.
    /// </summary>
    /// <param name="filters"></param>
    public OrTranslationFilter(params ITranslationFilter[] filters)
    {
        this.filters.AddRange(filters);
    }

    /// <summary>
    /// Determines whether any of the filters in this filter matches the given <see cref="ITranslationUnit"/>.
    /// </summary>
    /// <param name="translationUnit">
    /// The <see cref="ITranslationUnit"/> to check.
    /// </param>
    /// <returns>
    /// <see langword="true" /> if any of the filters in this filter matches the given <see cref="ITranslationUnit"/>;
    /// otherwise, <see langword="false" />.
    /// </returns>
    public bool IsValid(ITranslationUnit translationUnit)
    {
        return filters.Any(filter => filter.IsValid(translationUnit));
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return "OR";
    }
}