using Ashampoo.Translation.Systems.Formats.Abstractions.Models;

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
    IList<string> Comments { get; set; }

    /// <summary>
    /// The language of the translation.
    /// </summary>
    Language Language { get; set; }
}