namespace Ashampoo.Translation.Systems.Formats.Abstractions.TranslationFilterParser.Exceptions;

/// <summary>
/// Exception that is thrown when a filter string is invalid.
/// </summary>
public class ParserException : Exception
{
    /// <inheritdoc />
    public ParserException(string message) : base(message)
    {
    }
}

/// <inheritdoc />
public class UnexpectedTokenException : ParserException
{
    /// <summary>
    /// The token that was unexpected.
    /// </summary>
    public Token Token { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnexpectedTokenException"/> class.
    /// </summary>
    /// <param name="token">
    /// The token that was unexpected.
    /// </param>
    public UnexpectedTokenException(Token token) : base(
        $"Unexpected token: '{token.Type}' at position: '{token.Position}'.")
    {
        Token = token;
    }
}

/// <inheritdoc />
public class ExpectedTokenException : ParserException
{
    /// <summary>
    /// The token that was expected.
    /// </summary>
    public TokenType Type { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExpectedTokenException"/> class.
    /// </summary>
    /// <param name="type">
    /// The token type that was expected.
    /// </param>
    public ExpectedTokenException(TokenType type) : base($"Expected token: '{type}'.")
    {
        Type = type;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExpectedTokenException"/> class.
    /// </summary>
    /// <param name="type">
    /// The token type that was expected.
    /// </param>
    /// <param name="position">
    /// The position where the token was expected.
    /// </param>
    public ExpectedTokenException(TokenType type, int position) : base(
        $"Expected token: '{type}' at position: '{position}'.")
    {
        Type = type;
    }
}