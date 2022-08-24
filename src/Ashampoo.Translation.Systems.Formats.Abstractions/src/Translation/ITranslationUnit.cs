namespace Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

/// <summary>
/// Interface for a container that contains <see cref="ITranslation"/>.
/// </summary>
public interface ITranslationUnit : IEnumerable<ITranslation>
{
    /// <summary>
    /// The id of the translation unit.
    /// </summary>
    string Id { get; }
    /// <summary>
    /// The count of translations in the translation unit.
    /// </summary>
    int Count { get; }

    /// <summary>
    /// Gets the <see cref="ITranslation"/> with the specified language.
    /// </summary>
    /// <param name="language">
    /// The language of the translation.
    /// </param>
    ITranslation this[string language] { get; set; }
    /// <summary>
    /// Try to get the <see cref="ITranslation"/> with the specified language.
    /// </summary>
    /// <param name="language">
    /// The language of the translation.
    /// </param>
    /// <returns>
    /// The <see cref="ITranslation"/> with the specified language.
    /// </returns>
    ITranslation? TryGet(string language);
}

/// <summary>
/// Interface for a container that contains <see cref="ITranslationUnit"/>.
/// </summary>
public interface ITranslationUnits : IEnumerable<ITranslationUnit>
{
    /// <summary>
    /// The count of translation units.
    /// </summary>
    int Count { get; }
    /// <summary>
    /// Gets the <see cref="ITranslationUnit"/> with the specified id.
    /// </summary>
    /// <param name="id">
    /// The id of the <see cref="ITranslationUnit"/>.
    /// </param>
    ITranslationUnit? this[string id] { get; set; }
}