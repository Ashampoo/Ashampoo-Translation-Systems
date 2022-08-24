namespace Ashampoo.Translation.Systems.Formats.Abstractions.TranslationFilterParser;

/// <summary>
/// This class is used to parse a filter string and return a list of tokens.
/// </summary>
public class Lexer : IDisposable
{
    private readonly List<Token> tokens = new();
    private int position;
    private readonly TextReader reader;

    /// <summary>
    /// Initializes a new instance of the <see cref="Lexer"/> class.
    /// </summary>
    /// <param name="input">
    /// The input string to tokenize.
    /// </param>
    public Lexer(string input)
    {
        reader = new StringReader(input);
    }
    
    /// <summary>
    /// Tokenizes the input string.
    /// </summary>
    /// <returns>A list of tokens.</returns>
    public List<Token> Tokenize()
    {
        ParseToken();
        return tokens;
    }

    //private bool hasMove();
    //private char nextChar(); -> position++
    // char peakChar('');
    
    /// <summary>
    /// Parses all the tokens in the input string.
    /// </summary>
    /// <exception cref="Exception"></exception>
    private void ParseToken()
    {
        while (HasMore())
        {
            var c = PeekChar();
            if (char.IsWhiteSpace(c))
            {
                NextChar();
                continue;
            }

            if (char.IsLetter(c))
                ParseName();
            else if (c == '"')
                ParseString();
            else if (c == '(')
            {
                tokens.Add(new Token { Type = TokenType.OpenBracket, Value = c.ToString(), Position = position });
                NextChar();
            }
            else if (c == ')')
            {
                tokens.Add(new Token { Type = TokenType.CloseBracket, Value = c.ToString(), Position = position });
                NextChar();
            }
            else if (c == ',')
            {
                tokens.Add(new Token { Type = TokenType.Comma, Value = c.ToString(), Position = position });
                NextChar();
            }
            else
                throw new Exception($"Unexpected character '{c}'");
        }
    }
    
    /// <summary>
    /// Parses a name token.
    /// </summary>
    private void ParseName()
    {
        StringBuilder builder = new();
        var start = position;
        char c;
        do
        {
            c = NextChar();
            builder.Append(c);
            c = PeekChar();
        } while (char.IsLetterOrDigit(c) || c == '.'); // read until we hit a dot or digit

        if (builder.ToString().ToLower().Equals("and")) // create AND token
        {
            tokens.Add(new Token { Type = TokenType.And, Value = "AND", Position = start });
            return;
        }

        if (builder.ToString().ToLower().Equals("or")) // create OR token
        {
            tokens.Add(new Token { Type = TokenType.Or, Value = "OR", Position = start });
            return;
        }

        tokens.Add(new Token { Type = TokenType.Name, Value = builder.ToString(), Position = start }); // create name token
    }
    
    /// <summary>
    /// Parses a string token.
    /// </summary>
    private void ParseString()
    {
        StringBuilder builder = new();
        var start = position;
        var c = NextChar(); //skip leading '"'

        while ((c = PeekChar()) != '"') // read until we hit a '"'
        {
            builder.Append(c);
            NextChar();
        }

        NextChar(); //skip trailing '"'

        tokens.Add(new Token { Type = TokenType.String, Value = builder.ToString(), Position = start }); // create string token
    }

    private bool HasMore()
    {
        return reader.Peek() != -1;
    }

    private char NextChar()
    {
        position++;
        return (char)reader.Read();
    }

    private char PeekChar()
    {
        return (char)reader.Peek();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        reader.Dispose();
        GC.SuppressFinalize(this);
    }
    
    /// <summary>
    /// Destructor.
    /// </summary>
    ~Lexer()
    {
        Dispose();
    }
}