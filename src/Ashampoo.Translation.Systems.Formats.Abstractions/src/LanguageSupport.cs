namespace Ashampoo.Translation.Systems.Formats.Abstractions;

/// <summary>
/// Enum to differentiate between different translation format types.
/// </summary>
public enum LanguageSupport
{
    /// <summary>
    /// Indicates that the format only has support for one language.
    /// </summary>
    OnlyTarget,
    /// <summary>
    /// Indicates that the format has support for two languages.
    /// </summary>
    SourceAndTarget,
    /// <summary>
    /// Indicates that the format has support for multiple languages.
    /// </summary>
    Multiple
}