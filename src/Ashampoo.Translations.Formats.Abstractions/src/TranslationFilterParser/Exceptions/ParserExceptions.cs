namespace Ashampoo.Translations.Formats.Abstractions.TranslationFilterParser.Exceptions;

public class ParserException : Exception
{
    public ParserException(string message) : base(message)
    {
    }
}

public class UnexpectedTokenException : ParserException
{
    public Token Token { get; init; }

    public UnexpectedTokenException(Token token) : base(
        $"Unexpected token: '{token.Type}' at position: '{token.Position}'.")
    {
        Token = token;
    }
}

public class ExpectedTokenException : ParserException
{
    public TokenType Type { get; init; }

    public ExpectedTokenException(TokenType type) : base($"Expected token: '{type}'.")
    {
        Type = type;
    }

    public ExpectedTokenException(TokenType type, int position) : base(
        $"Expected token: '{type}' at position: '{position}'.")
    {
        Type = type;
    }
}