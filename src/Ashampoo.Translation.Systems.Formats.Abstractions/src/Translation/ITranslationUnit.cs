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
    
    HashSet<ITranslation> Translations { get; }
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