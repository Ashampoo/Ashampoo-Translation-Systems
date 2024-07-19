using FluentAssertions;

namespace Ashampoo.Translation.Systems.Formats.Abstractions.Tests;

public class LanguageParserTests
{
    [Theory]
    [InlineData("App.en-US.resx", "en-US")]
    [InlineData("App.en.resx", "en")]
    [InlineData("App_en.resx", "en")]
    [InlineData("App-en.resx", "en")]
    [InlineData("App_en-US.resx", "en-US")]
    [InlineData("App-en-US.resx", "en-US")]
    [InlineData("en-US.resx", "en-US")]
    [InlineData("en.resx", "en")]
    public void ShouldParseLanguageFromFileName(string fileName, string expectedLanguage)
    {
        LanguageParser.TryParseLanguageId(fileName).Should().NotBeNullOrWhiteSpace().And.Be(expectedLanguage);
    }
}