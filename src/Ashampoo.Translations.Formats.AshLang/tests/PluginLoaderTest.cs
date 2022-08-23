using Ashampoo.Translations.Formats.Abstractions;
using Xunit;

namespace Ashampoo.Translations.Formats.AshLang.Tests;

public class PluginLoaderTest
{
    private readonly IFormatFactory formatFactory;

    public PluginLoaderTest(IFormatFactory formatFactory)
    {
        this.formatFactory = formatFactory;
    }
    
    [Fact]
    public void LoadTest()
    {
        Assert.NotEmpty(formatFactory.GetFormatProviders());
    }
}