namespace Ashampoo.Translations.Formats.Abstractions.TranslationFilterParser;

public enum TokenType
{
    Name,
    String,
    And,
    Or,
    OpenBracket,
    CloseBracket,
    Comma
}

public class Token
{
    public TokenType Type { get; init; }
    public string Value { get; init; } = string.Empty;
    public int Position { get; init; }

    public override string ToString()
    {
        return @$"{Type} = ""{Value}""";
    }
}