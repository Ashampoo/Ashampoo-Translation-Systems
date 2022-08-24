using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

namespace Ashampoo.Translation.Systems.Formats.Abstractions.TranslationFilter;

/// <summary>
/// An implementation of the <see cref="ITranslationFilter"/> interface, that checks, if the id of the <see cref="ITranslationFilter"/> matches a specified value.
/// The Filter can be created to match "Contains", "StartsWith" or "EndsWith" for the id.
/// </summary>
public class MatchIdTranslationFilter : ITranslationFilter
{
    private readonly Regex regex;

    private MatchIdTranslationFilter(Regex regex)
    {
        this.regex = regex;
    }
    
    /// <summary>
    /// Creates a new <see cref="MatchIdTranslationFilter"/> that checks, if the id of an <see cref="ITranslationUnit"/> contains the input.
    /// </summary>
    /// <param name="input">The string to match against the id.</param>
    /// <returns> A new instance of <see cref="MatchIdTranslationFilter"/> .</returns>
    public static MatchIdTranslationFilter Contains(string input)
    {
        return new MatchIdTranslationFilter(new Regex(input));
    }
    
    /// <summary>
    /// Creates a new <see cref="MatchIdTranslationFilter"/> that checks, if the id of an <see cref="ITranslationUnit"/> starts with the input.
    /// </summary>
    /// <param name="input">The string to match against the id.</param>
    /// <returns> A new instance of <see cref="MatchIdTranslationFilter"/> .</returns>
    public static MatchIdTranslationFilter StartsWith(string input)
    {
        return new MatchIdTranslationFilter(new Regex($"^{input}"));
    }
    
    /// <summary>
    /// Creates a new <see cref="MatchIdTranslationFilter"/> that checks, if the id of an <see cref="ITranslationUnit"/> ends with the input.
    /// </summary>
    /// <param name="input">The string to match against the id.</param>
    /// <returns> A new instance of <see cref="MatchIdTranslationFilter"/> .</returns>
    public static MatchIdTranslationFilter EndsWith(string input)
    {
        return new MatchIdTranslationFilter(new Regex($"{input}$"));
    }

    /// <summary>
    /// Checks, if the id of an <see cref="ITranslationUnit"/> matches the input.
    /// </summary>
    /// <param name="translationUnit">
    /// The <see cref="ITranslationUnit"/> to check against the input.
    /// </param>
    /// <returns>
    /// True, if the id of the <see cref="ITranslationUnit"/> matches the input.
    /// </returns>
    public bool IsValid(ITranslationUnit translationUnit)
    {
        return regex.IsMatch(translationUnit.Id);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"Id = {regex}";
    }
}