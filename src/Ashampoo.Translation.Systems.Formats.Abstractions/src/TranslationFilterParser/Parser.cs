using Ashampoo.Translation.Systems.Formats.Abstractions.TranslationFilter;
using Ashampoo.Translation.Systems.Formats.Abstractions.TranslationFilterParser.Exceptions;
using Ashampoo.Translation.Systems.Formats.Abstractions.TranslationFilterParser.Extensions;

namespace Ashampoo.Translation.Systems.Formats.Abstractions.TranslationFilterParser;

/// <summary>
/// Parses a filter string into a filter object.
/// </summary>
public class Parser
{
    /// <summary>
    /// The filter that was parsed from the input string.
    /// </summary>
    public ITranslationFilter Filter { get; private set; }
    
    /// <summary>
    /// Create a new parser.
    /// </summary>
    /// <param name="input">The filter string</param>
    /// <exception cref="Exception">
    /// Thrown if the input string is invalid.
    /// </exception>
    public Parser(string input)
    {
        var tokens = new Lexer(input).Tokenize();
        Filter = ParseExpressions(tokens.GetEnumerator(), 0) ?? throw new Exception(); //TODO: throw correct exception
    }

    private enum Match
    {
        Value,
        Id
    }
    
    /// <summary>
    /// Parse the tokens into a filter object.
    /// </summary>
    /// <param name="token">
    /// The tokens to parse.
    /// </param>
    /// <param name="bracketIndex">
    /// The index of the current bracket.
    /// </param>
    /// <returns>
    /// The filter object.
    /// </returns>
    /// <exception cref="ParserException">
    /// Thrown if the tokens are invalid.
    /// </exception>
    /// <exception cref="UnexpectedTokenException">
    /// Thrown if the filter string is invalid.
    /// </exception>
    /// <exception cref="ExpectedTokenException">
    /// Thrown if the filter string is invalid.
    /// </exception>
    private ITranslationFilter ParseExpressions(IEnumerator<Token> token, int bracketIndex)
    {
        if (!token.MoveNext())
            throw new ParserException("Expected name or open bracket.");

        ITranslationFilter ParseNameOrBracket()
        {
            if (token.IsType(TokenType.Name))
                return ParseName(token);

            if (token.IsType(TokenType.OpenBracket))
                return ParseExpressions(token, bracketIndex + 1);

            throw new UnexpectedTokenException(token.Current);
        }

        var left = ParseNameOrBracket();

        var currentPosition = token.Current.Position;
        if (!token.MoveNext())
        {
            if (bracketIndex > 0)
                throw new ExpectedTokenException(TokenType.CloseBracket, currentPosition);
            return left;
        }

        if (token.IsType(TokenType.And))
        {
            var right = ParseExpressions(token, bracketIndex);
            return new AndTranslationFilter(left, right);
        }

        if (token.IsType(TokenType.Or))
        {
            var right = ParseExpressions(token, bracketIndex);
            return new OrTranslationFilter(left, right);
        }

        if (token.IsType(TokenType.CloseBracket))
        {
            if (bracketIndex > 0)
                return left;

            throw new UnexpectedTokenException(token.Current);
        }

        throw new UnexpectedTokenException(token.Current);
    }
    
    /// <summary>
    /// Parse a name token.
    /// </summary>
    /// <param name="token">
    /// The tokens to parse.
    /// </param>
    /// <returns></returns>
    /// <exception cref="UnexpectedTokenException"></exception>
    private ITranslationFilter ParseName(IEnumerator<Token> token)
    {
        if (token.IsValue("StartsWith") || token.IsValue("Value.StartsWith"))
            return ParseStartsWith(token, Match.Value);
        if (token.IsValue("EndsWith") || token.IsValue("Value.EndsWith")) return ParseEndsWith(token, Match.Value);
        if (token.IsValue("Contains") || token.IsValue("Value.Contains")) return ParseContains(token, Match.Value);
        if (token.IsValue("Id.StartsWith")) return ParseStartsWith(token, Match.Id);
        if (token.IsValue("Id.EndsWith")) return ParseEndsWith(token, Match.Id);
        if (token.IsValue("Id.Contains")) return ParseContains(token, Match.Id);
        if (token.IsValue("IsEmpty")) return ParseIsEmpty(token);
        throw new UnexpectedTokenException(token.Current);
    }
    
    /// <summary>
    /// Parse a name token, that corresponds to a contains filter.
    /// </summary>
    /// <param name="token"></param>
    /// <param name="match"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    /// <exception cref="UnexpectedTokenException"></exception>
    private ITranslationFilter ParseContains(IEnumerator<Token> token, Match match)
    {
        // Input string should look like this:
        // (Id. || Value.)Contains("{value}" || Contains("{value}", "{language}") when match = Match.Value
        
        token.Required(TokenType.OpenBracket);  
        token.Required(TokenType.String);

        var value = token.GetValue();

        string? language = null;

        try
        {
            token.Required(TokenType.CloseBracket);
        }
        catch (Exception e)
        {
            //TODO: implement try catch correctly
            if (e is not UnexpectedTokenException) throw new Exception(e.Message);

            if (token.Current.Type != TokenType.Comma || match != Match.Value)
                throw new UnexpectedTokenException(token.Current);

            token.Required(TokenType.String);
            language = token.GetValue();
            token.Required(TokenType.CloseBracket);
        }

        ITranslationFilter filter = match switch
        {
            Match.Value => MatchValueTranslationFilter.Contains(value, language),
            Match.Id => MatchIdTranslationFilter.Contains(value),
            _ => throw new Exception($"Unexpected match type: {match}")
        };

        return filter;
    }
    
    /// <summary>
    /// Parse a name token, that corresponds to a starts with filter.
    /// </summary>
    /// <param name="token"></param>
    /// <param name="match"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    /// <exception cref="UnexpectedTokenException"></exception>
    private ITranslationFilter ParseStartsWith(IEnumerator<Token> token, Match match)
    {
        // Input string should look like this:
        // (Id. || Value.)Contains("{value}" || Contains("{value}", "{language}") when match = Match.Value
        
        token.Required(TokenType.OpenBracket);
        token.Required(TokenType.String);

        var value = token.GetValue();
        string? language = null;

        try
        {
            token.Required(TokenType.CloseBracket);
        }
        catch (Exception e)
        {
            if (e is not UnexpectedTokenException) throw new Exception(e.Message);

            if (token.Current.Type != TokenType.Comma || match != Match.Value)
                throw new UnexpectedTokenException(token.Current);

            token.Required(TokenType.String);
            language = token.GetValue();
            token.Required(TokenType.CloseBracket);
        }

        ITranslationFilter filter = match switch
        {
            Match.Value => MatchValueTranslationFilter.StartsWith(value, language),
            Match.Id => MatchIdTranslationFilter.StartsWith(value),
            _ => throw new Exception($"Unexpected match type: {match}")
        };

        return filter;
    }
    
    /// <summary>
    /// Parse a name token, that corresponds to an ends with filter.
    /// </summary>
    /// <param name="token"></param>
    /// <param name="match"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    /// <exception cref="UnexpectedTokenException"></exception>
    private ITranslationFilter ParseEndsWith(IEnumerator<Token> token, Match match)
    {
        // Input string should look like this:
        // (Id. || Value.)Contains("{value}" || Contains("{value}", "{language}") when match = Match.Value
        
        token.Required(TokenType.OpenBracket);
        token.Required(TokenType.String);

        var value = token.GetValue();

        string? language = null;

        try
        {
            token.Required(TokenType.CloseBracket);
        }
        catch (Exception e)
        {
            if (e is not UnexpectedTokenException) throw new Exception(e.Message);

            if (token.Current.Type != TokenType.Comma || match != Match.Value)
                throw new UnexpectedTokenException(token.Current);

            token.Required(TokenType.String);
            language = token.GetValue();
            token.Required(TokenType.CloseBracket);
        }

        ITranslationFilter filter = match switch
        {
            Match.Value => MatchValueTranslationFilter.EndsWith(value, language),
            Match.Id => MatchIdTranslationFilter.EndsWith(value),
            _ => throw new Exception($"Unexpected match type: {match}")
        };

        return filter;
    }
    
    /// <summary>
    /// Parse a name token, that corresponds to an is empty filter.
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    /// <exception cref="UnexpectedTokenException"></exception>
    private ITranslationFilter ParseIsEmpty(IEnumerator<Token> token)
    {
        // Input string should look like this:
        // isEmpty()
        token.Required(TokenType.OpenBracket);

        string? language = null;

        try
        {
            token.Required(TokenType.CloseBracket);
        }
        catch (Exception e)
        {
            if (e is not UnexpectedTokenException) throw new Exception(e.Message);
            if (token.Current.Type != TokenType.String) throw new UnexpectedTokenException(token.Current);

            language = token.GetValue();
            token.Required(TokenType.CloseBracket);
        }

        return new IsEmptyTranslationFilter(language);
    }
}