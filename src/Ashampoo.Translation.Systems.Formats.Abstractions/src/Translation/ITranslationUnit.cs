using System.Diagnostics.CodeAnalysis;
using Ashampoo.Translation.Systems.Formats.Abstractions.Models;

namespace Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

/// <summary>
/// Interface for a container that contains <see cref="ITranslation"/>.
/// </summary>
public interface ITranslationUnit
{
    /// <summary>
    /// The id of the translation unit.
    /// </summary>
    string Id { get; }
    
    /// <summary>
    /// Gets the collection of translations associated with the translation unit.
    /// </summary>
    ICollection<ITranslation> Translations { get; }
}

/// <summary>
/// Provides extension methods for collections of translations.
/// </summary>
public static class TranslationCollectionExtensions
{
    /// <summary>
    /// Tries to get a translation from the collection by its language.
    /// </summary>
    /// <param name="translations">The collection of translations.</param>
    /// <param name="language">The language of the translation to get.</param>
    /// <param name="translation">When this method returns, contains the translation with the specified language, if found; otherwise, null.</param>
    /// <returns>true if a translation with the specified language is found; otherwise, false.</returns>
    public static bool TryGetTranslation(this ICollection<ITranslation> translations, Language language,
        [NotNullWhen(true)] out ITranslation? translation)
    {
        var foundTranslation = translations.FirstOrDefault(t => t.Language == language);
        if (foundTranslation is null)
        {
            translation = null;
            return false;
        }

        translation = foundTranslation;
        return true;
    }

    /// <summary>
    /// Gets a translation from the collection by its language.
    /// </summary>
    /// <param name="translations">The collection of translations.</param>
    /// <param name="language">The language of the translation to get.</param>
    /// <returns>The translation with the specified language.</returns>
    /// <exception cref="InvalidOperationException">No translation with the specified language is found.</exception>
    public static ITranslation GetTranslation(this ICollection<ITranslation> translations, Language language) =>
        translations.First(t => t.Language == language);

    /// <summary>
    /// Adds a new translation to the collection or updates an existing one.
    /// </summary>
    /// <param name="translations">The collection of translations.</param>
    /// <param name="language">The language of the translation to add or update.</param>
    /// <param name="value">The new translation.</param>
    public static void AddOrUpdateTranslation(this ICollection<ITranslation> translations, Language language, ITranslation value)
    {
        var existingTranslation = translations.FirstOrDefault(t => t.Language == language);

        if (existingTranslation is not null)
        {
            translations.Remove(existingTranslation);
        }

        translations.Add(value);
    }
}