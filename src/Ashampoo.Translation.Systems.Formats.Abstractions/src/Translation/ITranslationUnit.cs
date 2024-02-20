using System.Diagnostics.CodeAnalysis;

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
    
    ICollection<ITranslation> Translations { get; }
}


public static class TranslationCollectionExtensions
{
    public static bool TryGetTranslation(this ICollection<ITranslation> translations, string language,
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

    public static ITranslation GetTranslation(this ICollection<ITranslation> translations, string language) =>
        translations.First(t => t.Language == language);

    public static void AddOrUpdateTranslation(this ICollection<ITranslation> translations, string language, ITranslation value)
    {
        var existingTranslation = translations.FirstOrDefault(t => t.Language == language);

        if (existingTranslation is not null)
        {
            translations.Remove(existingTranslation);
        }

        translations.Add(value);
    }
}