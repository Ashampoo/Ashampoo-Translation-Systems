using StronglyTypedIds;

namespace Ashampoo.Translation.Systems.Formats.Abstractions.Models;

/// <summary>
/// Represents a strongly typed identifier for a language.
/// </summary>
[StronglyTypedId(Template.String)]
public readonly partial struct Language;

/// <summary>
/// Provides extension methods for the Language struct.
/// </summary>
public static class LanguageExtensions
{
    /// <summary>
    /// Checks if the Language instance is null or its value is whitespace.
    /// </summary>
    /// <param name="language">The Language instance to check.</param>
    /// <returns>True if the Language instance is null or its value is whitespace, otherwise false.</returns>
    public static bool IsNullOrWhitespace(this Language? language)
    {
        return string.IsNullOrWhiteSpace(language?.Value);
    }
    
    /// <summary>
    /// Checks if the value of the Language instance is whitespace.
    /// </summary>
    /// <param name="language">The Language instance to check.</param>
    /// <returns>True if the value of the Language instance is whitespace, otherwise false.</returns>
    public static bool IsNullOrWhitespace(this Language language)
    {
        return string.IsNullOrWhiteSpace(language.Value);
    }
}