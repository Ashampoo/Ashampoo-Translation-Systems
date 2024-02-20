using System.Diagnostics.CodeAnalysis;

namespace Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

/// <summary>
/// Abstract base class for the <see cref="ITranslationUnit"/> interface.
/// </summary>
public abstract class AbstractTranslationUnit : ITranslationUnit
{
    /// <summary>
    /// Base constructor for the <see cref="AbstractTranslationUnit"/> class.
    /// </summary>
    /// <param name="id">
    /// The id of the translation unit.
    /// </param>
    protected AbstractTranslationUnit(string id)
    {
        Id = id;
    }

    /// <inheritdoc />
    public string Id { get; init; }

    /// <inheritdoc />
    public HashSet<ITranslation> Translations { get; } = new();
}

public static class HashSetExtension
{
    public static bool TryGetTranslation(this HashSet<ITranslation> translations, string language,
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

    public static ITranslation? GetTranslation(this HashSet<ITranslation> translations, string language) =>
        translations.FirstOrDefault(t => t.Language == language);

    public static void AddOrUpdateTranslation(this HashSet<ITranslation> translations, string language, ITranslation value)
    {
        if (translations.Add(value)) return;
        translations.RemoveWhere(x => x.Language == language);
        if (!translations.Add(value))
            throw new InvalidOperationException(
                "AbstractTranslationUnit: not able to add translation.");
    }
}