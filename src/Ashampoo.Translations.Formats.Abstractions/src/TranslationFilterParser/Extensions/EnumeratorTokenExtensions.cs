using Ashampoo.Translations.Formats.Abstractions.TranslationFilterParser.Exceptions;

namespace Ashampoo.Translations.Formats.Abstractions.TranslationFilterParser.Extensions;

public static class EnumeratorTokenExtensions
{
    /// <summary>
    /// Ensures, that the next <see cref="Token"/> is of the specified type.
    /// </summary>
    /// <param name="token">An <see cref="IEnumerable{Token}"/> containing tokens.</param>
    /// <param name="type">The required type of the next token.</param>
    /// <exception cref="ExpectedTokenException">There was no text token in the <see cref="IEnumerable{T}"/> .</exception>
    /// <exception cref="UnexpectedTokenException">The next token was not of the specified type.</exception>
    public static void Required(this IEnumerator<Token> token, TokenType type)
    {
        if (!token.MoveNext()) throw new ExpectedTokenException(type);
        if (token.Current.Type != type) throw new UnexpectedTokenException(token.Current);
    }

    /// <summary>
    /// Checks, if the current token in the <see cref="IEnumerable{T}"/> is of the specified type.
    /// </summary>
    /// <param name="token"></param>
    /// <param name="type"></param>
    /// <returns><see langword="true"/> if the current token is of the specified type, otherwise <see langword="false"/> .</returns>
    public static bool IsType(this IEnumerator<Token> token, TokenType type)
    {
        return token.Current.Type == type;
    }
    
    /// <summary>
    /// Get the value of the current token.
    /// </summary>
    /// <param name="token">The <see cref="IEnumerable{T}"/> containing the tokens.</param>
    /// <returns></returns>
    public static string GetValue(this IEnumerator<Token> token)
    {
        return token.Current.Value;
    }
    
    /// <summary>
    /// Compares the value of the current token with the specified value.
    /// </summary>
    /// <param name="token">
    /// The <see cref="IEnumerable{T}"/> containing the tokens.
    /// </param>
    /// <param name="value">
    /// The value to compare with.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the value of the current token is equal to the specified value, otherwise <see langword="false"/> .
    /// </returns>
    public static bool IsValue(this IEnumerator<Token> token, string value)
    {
        return string.Equals(token.Current.Value, value, StringComparison.CurrentCultureIgnoreCase);
    }
}