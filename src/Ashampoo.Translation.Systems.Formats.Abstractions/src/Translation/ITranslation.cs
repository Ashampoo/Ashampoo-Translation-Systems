namespace Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

/// <summary>
/// Interface for translations
/// </summary>
public interface ITranslation
{
    /// <summary>
    /// Unique id of the translation.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Optional comment for this translation.
    /// </summary>
    string? Comment { get; set; }

    /// <summary>
    /// Indicates whether this translation is empty.
    /// </summary>
    bool IsEmpty { get; }

    /// <summary>
    /// The language of the translation.
    /// </summary>
    string Language { get; set; }
}