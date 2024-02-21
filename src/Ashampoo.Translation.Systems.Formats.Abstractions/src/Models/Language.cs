using StronglyTypedIds;

namespace Ashampoo.Translation.Systems.Formats.Abstractions.Models;

/// <summary>
/// 
/// </summary>
[StronglyTypedId(Template.String)]
public readonly partial struct Language;

public static class LanguageExtensions
{
    public static bool IsNullOrWhitespace(this Language? language)
    {
        return string.IsNullOrWhiteSpace(language?.Value);
    }
    
    public static bool IsNullOrWhitespace(this Language language)
    {
        return string.IsNullOrWhiteSpace(language.Value);
    }
}