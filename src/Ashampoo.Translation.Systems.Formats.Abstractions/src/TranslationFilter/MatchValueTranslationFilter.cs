using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

namespace Ashampoo.Translation.Systems.Formats.Abstractions.TranslationFilter;

/// <summary>
/// An implementation of the <see cref="ITranslationFilter"/> interface, that checks, if the value of an <see cref="ITranslation"/> matches a specified value.
/// The Filter can be created to match "Contains", "StartsWith" or "EndsWith" for the id.
/// </summary>
public class MatchValueTranslationFilter : ITranslationFilter
{
    private readonly Regex regex;

    /// <inheritdoc />
    public string? Language { get; init; }

    private MatchValueTranslationFilter(Regex regex, string? language = null)
    {
        this.regex = regex;
        Language = language;
    }
    
    /// <summary>
    /// Creates a new <see cref="MatchIdTranslationFilter"/> that checks, if the value of the <see cref="ITranslation"/> for the given language contains the input.
    /// </summary>
    /// <param name="input">The string to match against the value.</param>
    /// <param name="language">The language of the translation that will be checked.</param>
    /// <returns>A new instance of <see cref="MatchValueTranslationFilter"/> .</returns>
    public static MatchValueTranslationFilter Contains(string input, string? language = null)
    {
        return new MatchValueTranslationFilter(new Regex(input), language);
    }
    
    /// <summary>
    /// Creates a new <see cref="MatchIdTranslationFilter"/> that checks, if the value of the <see cref="ITranslation"/> for the given language starts with the input.
    /// </summary>
    /// <param name="input">The string to match against the value.</param>
    /// <param name="language">The language of the translation that will be checked.</param>
    /// <returns>A new instance of <see cref="MatchValueTranslationFilter"/> .</returns>
    public static MatchValueTranslationFilter StartsWith(string input, string? language = null)
    {
        return new MatchValueTranslationFilter(new Regex($"^{input}"), language);
    }
    
    /// <summary>
    /// Creates a new <see cref="MatchIdTranslationFilter"/> that checks, if the value of the <see cref="ITranslation"/> for the given language ends with the input.
    /// </summary>
    /// <param name="input">The string to match against the value.</param>
    /// <param name="language">The language of the translation that will be checked.</param>
    /// <returns>A new instance of <see cref="MatchValueTranslationFilter"/> .</returns>
    public static MatchValueTranslationFilter EndsWith(string input, string? language = null)
    {
        return new MatchValueTranslationFilter(new Regex($"{input}$"), language);
    }

    /// <summary>
    /// Checks, if the value of the <see cref="ITranslation"/> for the given language matches the input.
    /// </summary>
    /// <param name="translationUnit">
    /// The <see cref="ITranslationUnit"/> containing the <see cref="ITranslation"/> that will be checked.
    /// </param>
    /// <returns>
    /// True, if the value of the <see cref="ITranslation"/> for the given language matches the input.
    /// </returns>
    public bool IsValid(ITranslationUnit translationUnit)
    {
        if (Language is not null)
        {
            if (translationUnit.Translations.TryGetTranslation(Language, out var translation))
            {
                var translationString = translation;
                return regex.IsMatch(translationString.Value);
            }

            return false;
        }

        foreach (var translation in translationUnit.Translations)
        {
            if (regex.IsMatch(translation.Value))
                return true;
        }

        return false;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"Value = {regex}";
    }
}