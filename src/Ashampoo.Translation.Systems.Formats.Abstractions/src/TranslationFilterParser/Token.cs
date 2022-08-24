namespace Ashampoo.Translation.Systems.Formats.Abstractions.TranslationFilterParser;

/// <summary>
/// Enum representing the type of a <see cref="Token"/>.
/// </summary>
public enum TokenType
{
    /// <summary>
    /// The token represents the name of a filter.
    /// </summary>
    Name,
    /// <summary>
    /// The token represents a string literal.
    /// </summary>
    String,
    /// <summary>
    /// The token represents an And-Filter
    /// </summary>
    And,
    /// <summary>
    /// The token represents an Or-Filter
    /// </summary>
    Or,
    /// <summary>
    /// The token represents an open bracket.
    /// </summary>
    OpenBracket,
    /// <summary>
    /// The token represents a close bracket.
    /// </summary>
    CloseBracket,
    /// <summary>
    /// The token represents a comma.
    /// </summary>
    Comma
}

/// <summary>
/// Represents a token in a filter expression.
/// </summary>
public class Token
{
    /// <summary>
    /// The type of the token.
    /// </summary>
    public TokenType Type { get; init; }
    /// <summary>
    /// The value of the token.
    /// </summary>
    public string Value { get; init; } = string.Empty;
    /// <summary>
    /// The position of the token in the filter expression.
    /// </summary>
    public int Position { get; init; }

    /// <inheritdoc />
    public override string ToString()
    {
        return @$"{Type} = ""{Value}""";
    }
}