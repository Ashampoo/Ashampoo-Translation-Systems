using Ashampoo.Translation.Systems.Formats.Abstractions;
using Xunit;

namespace Ashampoo.Translation.Systems.Formats.AshLang.Tests;

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