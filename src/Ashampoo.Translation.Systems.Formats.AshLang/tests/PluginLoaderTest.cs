using Ashampoo.Translation.Systems.Formats.Abstractions;
using FluentAssertions;
using Xunit;

namespace Ashampoo.Translation.Systems.Formats.AshLang.Tests;

public class PluginLoaderTest
{
    private readonly IFormatFactory _formatFactory;

    public PluginLoaderTest(IFormatFactory formatFactory)
    {
        _formatFactory = formatFactory;
    }
    
    [Fact]
    public void LoadTest()
    {
        _formatFactory.GetFormatProviders().Should().NotBeEmpty();
    }
}