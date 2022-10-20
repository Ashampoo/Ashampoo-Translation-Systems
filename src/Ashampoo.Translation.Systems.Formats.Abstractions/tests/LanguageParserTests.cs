using Xunit;

namespace Ashampoo.Translation.Systems.Formats.Abstractions.Tests;

public class LanguageParserTests
{
    [Fact]
    public void ParseLanguageCountryTest()
    {
        const string filePath = "Snap14-en-us.json";
        var language = LanguageParser.TryParseLanguageId(filePath);
        Assert.Equal("en-US", language);
    }

    [Fact]
    public void ParseLanguageScriptTagCountryTest()
    {
        const string filePath = "Snap14-zh-Hant-TW.json";
        var language = LanguageParser.TryParseLanguageId(filePath);
        Assert.Equal("zh-Hant-TW", language);
    }
    

    [Fact]
    public void ValidLanguageCodeTest()
    {
        const string languageCodeChinese = "zh-Hant-TW";
        const string languageCodeEnglish = "en-US";
        var chineseIsValid = LanguageParser.IsValidLanguageCode(languageCodeChinese);
        var englishIsValid = LanguageParser.IsValidLanguageCode(languageCodeEnglish);
        Assert.True(chineseIsValid);
        Assert.True(englishIsValid);
    }
}