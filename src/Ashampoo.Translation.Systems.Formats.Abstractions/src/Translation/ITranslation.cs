namespace Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

/// <summary>
/// Interface for translations
/// </summary>
public interface ITranslation
{
    /// <summary>
    /// The value of the translation.
    /// </summary>
    string Value { get; set; }

    /// <summary>
    /// Optional comment for this translation.
    /// </summary>
    string? Comment { get; set; }

    /// <summary>
    /// The language of the translation.
    /// </summary>
    string Language { get; set; }
}