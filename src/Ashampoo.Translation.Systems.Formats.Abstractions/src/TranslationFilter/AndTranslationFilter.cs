using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

namespace Ashampoo.Translation.Systems.Formats.Abstractions.TranslationFilter;

/// <summary>
/// Implementation of <see cref="ITranslationFilter"/>, representing an And-combination of filters.
/// </summary>
public class AndTranslationFilter : ITranslationFilter
{
    private readonly List<ITranslationFilter> filters = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="AndTranslationFilter"/> class, with the given filters.
    /// </summary>
    /// <param name="filters"></param>
    public AndTranslationFilter(params ITranslationFilter[] filters)
    {
        this.filters.AddRange(filters);
    }

    /// <summary>
    /// Determine whether the given <see cref="ITranslationUnit"/> is accepted by every filter in the list.
    /// </summary>
    /// <param name="translationUnit">
    /// The <see cref="ITranslationUnit"/> to test.
    /// </param>
    /// <returns>
    /// True if the <see cref="ITranslationUnit"/> is accepted by every filter in the list, false otherwise.
    /// </returns>
    public bool IsValid(ITranslationUnit translationUnit)
    {
        return filters.All(filter => filter.IsValid(translationUnit));
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return "AND";
    }
}