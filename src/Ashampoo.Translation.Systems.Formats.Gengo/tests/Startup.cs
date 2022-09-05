using System.IO;
using Ashampoo.Translation.Systems.Formats.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Diagnostics;

namespace Ashampoo.Translation.Systems.Formats.Gengo.Tests;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddFormatFactory().RegisterFormat<GengoFormat>();
    }
}