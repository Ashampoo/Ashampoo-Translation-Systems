using Ashampoo.Translations.Formats.Abstractions.TranslationFilterParser;
using Xunit;

namespace Ashampoo.Translations.Formats.Abstractions.Tests
{
    public class LexerTest
    {
        [Fact]
        public void TokenizeX()
        {
            Lexer lexer = new(@"(aadasd");
            var tokens = lexer.Tokenize();
        }

        [Fact]
        public void Tokenize()
        {
            Lexer lexer = new(@"StartsWith(""Hello World"")");
            var tokens = lexer.Tokenize();
        }

        [Fact]
        public void Tokenize13()
        {
            Lexer lexer = new(@"(StartsWith(""Hello World""))");
            var tokens = lexer.Tokenize();
        }

        [Fact]
        public void Tokenize2()
        {
            Lexer lexer = new(@"StartsWith(""Hello World"") AND EndsWith(""sfsdfdsf"")");
            var tokens = lexer.Tokenize();
        }

        [Fact]
        public void TokenizeWithLanguage1()
        {
            Lexer lexer = new(@"StartsWith(""Hello World"", ""en-US"")");
            var tokens = lexer.Tokenize();
        }

        [Fact]
        public void TokenizeWithLanguage2()
        {
            Lexer lexer = new(@"StartsWith(""Hello World"") AND EndsWith(""sfsdfdsf"", ""en-US"")");
            var tokens = lexer.Tokenize();
        }
    }
}