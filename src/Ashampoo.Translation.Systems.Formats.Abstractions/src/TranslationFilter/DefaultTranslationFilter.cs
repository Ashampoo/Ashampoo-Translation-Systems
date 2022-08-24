using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

namespace Ashampoo.Translation.Systems.Formats.Abstractions.TranslationFilter;

/// <summary>
/// An implementation of the <see cref="ITranslationFilter"/> interface that is valid for every <see cref="ITranslationUnit"/> .
/// </summary>
public class DefaultTranslationFilter : ITranslationFilter
{
    /// <inheritdoc />
    public string? Language { get; init; }

    
    /// <summary>
    /// Check if the <see cref="ITranslationUnit"/> is valid for the filter.
    /// </summary>
    /// <param name="translationUnit">
    /// The <see cref="ITranslationUnit"/> to check.
    /// </param>
    /// <returns>
    /// <see langword="true"/>, this filter is valid for every <see cref="ITranslationUnit"/>.
    /// </returns>
    public bool IsValid(ITranslationUnit translationUnit) => true;
}