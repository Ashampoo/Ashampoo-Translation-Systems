using Ashampoo.Translations.Formats.Abstractions.TranslationFilter;
using Ashampoo.Translations.TestBase;
using Xunit;

namespace Ashampoo.Translations.Formats.Abstractions.Tests;

public class MatchValueTranslationFilterTest
{
    [Fact]
    public void StartsWith()
    {
        var unit = MockFormatWithTranslationUnits.CreateMockFormatWithTranslationUnits("en-US", "hello.id",
            "Hello World");
        Assert.NotNull(unit["hello.id"]);
        Assert.True(MatchValueTranslationFilter.StartsWith("Hello").IsValid(unit["hello.id"]!));
        Assert.False(MatchValueTranslationFilter.StartsWith("World").IsValid(unit["hello.id"]!));

        Assert.True(MatchValueTranslationFilter.StartsWith("Hello", "en-US").IsValid(unit["hello.id"]!));
        Assert.False(MatchValueTranslationFilter.StartsWith("World", "en-US").IsValid(unit["hello.id"]!));

        Assert.False(MatchValueTranslationFilter.StartsWith("Hello", "de-DE").IsValid(unit["hello.id"]!));
        Assert.False(MatchValueTranslationFilter.StartsWith("World", "de-DE").IsValid(unit["hello.id"]!));
    }

    [Fact]
    public void EndsWith()
    {
        var unit = MockFormatWithTranslationUnits.CreateMockFormatWithTranslationUnits("en-US", "hello.id",
            "Hello World");
        Assert.NotNull(unit["hello.id"]);
        Assert.True(MatchValueTranslationFilter.EndsWith("World").IsValid(unit["hello.id"]!));
        Assert.False(MatchValueTranslationFilter.EndsWith("Hello").IsValid(unit["hello.id"]!));

        Assert.True(MatchValueTranslationFilter.EndsWith("World", "en-US").IsValid(unit["hello.id"]!));
        Assert.False(MatchValueTranslationFilter.EndsWith("Hello", "en-US").IsValid(unit["hello.id"]!));

        Assert.False(MatchValueTranslationFilter.EndsWith("World", "de-DE").IsValid(unit["hello.id"]!));
        Assert.False(MatchValueTranslationFilter.EndsWith("Hello", "de-DE").IsValid(unit["hello.id"]!));
    }

    [Fact]
    public void Contains()
    {
        var unit = MockFormatWithTranslationUnits.CreateMockFormatWithTranslationUnits("en-US", "hello.id",
            "Hello World");
        Assert.NotNull(unit["hello.id"]);
        Assert.True(MatchValueTranslationFilter.Contains("lo Wo").IsValid(unit["hello.id"]!));
        Assert.False(MatchValueTranslationFilter.Contains("lolo").IsValid(unit["hello.id"]!));

        Assert.True(MatchValueTranslationFilter.Contains("lo Wo", "en-US").IsValid(unit["hello.id"]!));
        Assert.False(MatchValueTranslationFilter.Contains("lolo", "en-US").IsValid(unit["hello.id"]!));

        Assert.False(MatchValueTranslationFilter.Contains("lo Wo", "de-DE").IsValid(unit["hello.id"]!));
        Assert.False(MatchValueTranslationFilter.Contains("lolo", "de-DE").IsValid(unit["hello.id"]!));
    }
}