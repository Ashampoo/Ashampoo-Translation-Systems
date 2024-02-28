using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.AshLang;
using Ashampoo.Translation.Systems.Formats.Gengo;
using Ashampoo.Translation.Systems.Formats.JavaProperties;
using Ashampoo.Translation.Systems.Formats.Json;
using Ashampoo.Translation.Systems.Formats.NLang;
using Ashampoo.Translation.Systems.Formats.PO;
using Ashampoo.Translation.Systems.Formats.ResX;
using Ashampoo.Translation.Systems.Formats.TsProj;
using FluentAssertions;

namespace Ashampoo.Translation.Systems.Formats.Tests;

public sealed class DefaultFormatFactoryTests
{
    public static IEnumerable<object[]> GetTestData()
    {
        yield return [new AshLangFormat(), typeof(AshLangFormatProvider)];
        yield return [new GengoFormat(), typeof(GengoFormatProvider)];
        yield return [new JavaPropertiesFormat(), typeof(JavaPropertiesFormatProvider)];
        yield return [new JsonFormat(), typeof(JsonFormatProvider)];
        yield return [new NLangFormat(), typeof(NLangFormatProvider)];
        yield return [new POFormat(), typeof(POFormatProvider)];
        yield return [new ResXFormat(), typeof(ResXFormatProvider)];
        yield return [new TsProjFormat(), typeof(TsProjFormatProvider)];
    } 
        
        
    [Theory]
    [MemberData(nameof(GetTestData))]
    public void GetFormatProvider_FromFormat(IFormat format, Type providerType)
    {
        IEnumerable<IFormatProvider<IFormat>> providers =
        [
            new AshLangFormatProvider(), new GengoFormatProvider(), new JavaPropertiesFormatProvider(),
            new JsonFormatProvider(), new NLangFormatProvider(), new POFormatProvider(), new ResXFormatProvider(),
            new TsProjFormatProvider()
        ];
        
        var formatFactory = new DefaultFormatFactory(providers);
        var formatProvider = formatFactory.GetFormatProvider(format);
        formatProvider.Should().BeOfType(providerType);
    }
    
    
    [Theory]
    [InlineData("ashlang", typeof(AshLangFormatProvider))]
    [InlineData("gengo", typeof(GengoFormatProvider))]
    [InlineData("javaProperties", typeof(JavaPropertiesFormatProvider))]
    [InlineData("json", typeof(JsonFormatProvider))]
    [InlineData("nlang", typeof(NLangFormatProvider))]
    [InlineData("po", typeof(POFormatProvider))]
    [InlineData("resx", typeof(ResXFormatProvider))]
    [InlineData("tsproj", typeof(TsProjFormatProvider))]
    public void GetFormatProvider_FromId(string formatId, Type providerType)
    {
        IEnumerable<IFormatProvider<IFormat>> providers =
        [
            new AshLangFormatProvider(), new GengoFormatProvider(), new JavaPropertiesFormatProvider(),
            new JsonFormatProvider(), new NLangFormatProvider(), new POFormatProvider(), new ResXFormatProvider(),
            new TsProjFormatProvider()
        ];
        
        var formatFactory = new DefaultFormatFactory(providers);
        var formatProvider = formatFactory.GetFormatProvider(formatId);
        formatProvider.Should().BeOfType(providerType);
    }
    
}