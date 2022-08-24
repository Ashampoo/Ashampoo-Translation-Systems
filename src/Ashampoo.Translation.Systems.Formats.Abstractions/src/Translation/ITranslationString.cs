namespace Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

/// <summary>
/// Interface for translations that only have a single value.
/// </summary>
public interface ITranslationString : ITranslation
{
    /// <summary>
    /// The value of the translation.
    /// </summary>
    string Value { get; set; }
}