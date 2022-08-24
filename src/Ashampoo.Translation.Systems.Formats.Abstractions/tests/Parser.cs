using System;
using Ashampoo.Translation.Systems.Formats.Abstractions.TranslationFilter;
using Ashampoo.Translation.Systems.Formats.Abstractions.TranslationFilterParser;
using Ashampoo.Translation.Systems.Formats.Abstractions.TranslationFilterParser.Exceptions;
using Xunit;

namespace Ashampoo.Translation.Systems.Formats.Abstractions.Tests;

public class ParserTest
{
    [Fact]
    public void ParseStartsWith()
    {
        var parser = new Parser(@"StartsWith(""Hello"")");
        var filter = parser.Filter;
    }

    [Fact]
    public void ParseIsEmpty()
    {
        var parser = new Parser(@"isEmpty()");
        var filter = parser.Filter;
    }

    [Fact]
    public void ParseAnd()
    {
        var parser = new Parser(@"StartsWith(""Hello"") AND EndsWith(""World"")");
        var filter = parser.Filter;
    }

    [Fact]
    public void ParseMultipleAnd()
    {
        var parser = new Parser(@"StartsWith(""Hello"") AND EndsWith(""World"") AND StartsWith(""Ashampoo"")");
        var filter = parser.Filter;
    }

    [Fact]
    public void ParseOr()
    {
        var parser = new Parser(@"StartsWith(""Hello"") OR EndsWith(""World"")");
        var filter = parser.Filter;
    }

    [Fact]
    public void ParseMultipleOr()
    {
        var parser = new Parser(@"StartsWith(""Hello"") OR EndsWith(""World"") OR StartsWith(""Ashampoo"")");
        var filter = parser.Filter;
    }

    [Fact]
    public void ParseAndOr()
    {
        var parser = new Parser(@"StartsWith(""Hello"") AND EndsWith(""World"") OR StartsWith(""Ashampoo"")");
        var filter = parser.Filter;
    }

    [Fact]
    public void ParseSimpleBracket()
    {
        var parser = new Parser(@"(StartsWith(""Hello""))");
        var filter = parser.Filter;
    }

    [Fact]
    public void ParseComplexBracket()
    {
        var parser = new Parser(@"StartsWith(""Hello"") AND ( EndsWith(""World"") OR EndsWith(""Universe"") )  ");
        var filter = parser.Filter;
    }

    [Fact]
    public void ParseComplexBracket2()
    {
        var parser =
            new Parser(
                @"(StartsWith(""Hello"") OR StartsWith(""Hi"")) AND ( EndsWith(""World"") OR EndsWith(""Universe"") )  ");
        var filter = parser.Filter;
    }

    [Fact]
    public void ParseComplexBracket3()
    {
        var parser =
            new Parser(
                @"(StartsWith(""Hello"") OR StartsWith(""Hi"")) AND (Contains(""Ever"") OR Contains(""Never"")) AND ( EndsWith(""World"") OR EndsWith(""Universe"") )  ");
        var filter = parser.Filter;
    }

    [Fact]
    public void ParseInvalidBracketExpression()
    {
        Exception thrownException = Assert.Throws<UnexpectedTokenException>(() => new Parser(@"()"));
        Assert.Equal("Unexpected token: 'CloseBracket' at position: '1'.", thrownException.Message);

        thrownException = Assert.Throws<ParserException>(() => new Parser(@"("));
        Assert.Equal("Expected name or open bracket.", thrownException.Message);

        thrownException = Assert.Throws<UnexpectedTokenException>(() => new Parser(@")"));
        Assert.Equal("Unexpected token: 'CloseBracket' at position: '0'.", thrownException.Message);

        thrownException = Assert.Throws<ExpectedTokenException>(() => new Parser(@"(StartsWith(""Hello"")"));
        Assert.Equal("Expected token: 'CloseBracket' at position: '19'.", thrownException.Message);

        thrownException = Assert.Throws<UnexpectedTokenException>(() => new Parser(@"StartsWith(""Hello""))"));
        Assert.Equal("Unexpected token: 'CloseBracket' at position: '19'.", thrownException.Message);
    }

    [Fact]
    public void ParseWithLanguage1()
    {
        var parser = new Parser(@"StartsWith(""Hello"", ""en-US"")");
        var filter = parser.Filter;
        Assert.IsAssignableFrom<MatchValueTranslationFilter>(filter);
        Assert.Equal("en-US", filter.Language);

        parser = new Parser(@"EndsWith(""Hello"", ""en-US"")");
        filter = parser.Filter;
        Assert.IsAssignableFrom<MatchValueTranslationFilter>(filter);
        Assert.Equal("en-US", filter.Language);

        parser = new Parser(@"Contains(""Hello"", ""en-US"")");
        filter = parser.Filter;
        Assert.IsAssignableFrom<MatchValueTranslationFilter>(filter);
        Assert.Equal("en-US", filter.Language);

        parser = new Parser(@"IsEmpty(""en-US"")");
        filter = parser.Filter;
        Assert.IsAssignableFrom<IsEmptyTranslationFilter>(filter);
        Assert.Equal("en-US", filter.Language);
    }

    [Fact]
    public void ParseWWithLanguage2()
    {
        Assert.Throws<UnexpectedTokenException>(() => new Parser(@"Id.StartsWith(""Hello"", ""en-US"")"));
        Assert.Throws<UnexpectedTokenException>(() => new Parser(@"Id.EndsWith(""Hello"", ""en-US"")"));
        Assert.Throws<UnexpectedTokenException>(() => new Parser(@"Id.Contains(""Hello"", ""en-US"")"));
        Assert.Throws<UnexpectedTokenException>(() => new Parser(@"IsEmpty(""Hello"", ""en-US"")"));
    }
}