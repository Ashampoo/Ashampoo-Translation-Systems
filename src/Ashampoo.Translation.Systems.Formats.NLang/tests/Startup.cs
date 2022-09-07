using System.IO;
using Ashampoo.Translation.Systems.Formats.Abstractions;
using CommunityToolkit.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Ashampoo.Translation.Systems.Formats.NLang.Tests;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddFormatFactory().RegisterFormat<NLangFormat>();
    }
}