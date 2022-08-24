using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

namespace Ashampoo.Translation.Systems.Formats.Abstractions.TranslationFilter;

/// <summary>
/// Interface for translation filters.
/// </summary>
public interface ITranslationFilter
{
    /// <summary>
    /// Check if the <see cref="ITranslationUnit"/> is valid under the filter.
    /// </summary>
    /// <param name="translationUnit">The <see cref="ITranslationUnit"/> that is to be validated.</param>
    /// <returns>A <see langword="bool"/> indicating whether the <see cref="ITranslationUnit"/> is valid under the filter.</returns>
    bool IsValid(ITranslationUnit translationUnit);

    /// <summary>
    /// The optional language for the filter, not all filters require a language.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the filter does not support a language.
    /// </exception>
    string? Language
    {
        get => throw new InvalidOperationException($"{GetType()} has no language support.");
        init => throw new InvalidOperationException($"{GetType()} has no language support.");
    }
}