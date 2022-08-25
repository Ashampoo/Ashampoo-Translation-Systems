using System.IO;
using Ashampoo.Translation.Systems.Formats.Abstractions;
using CommunityToolkit.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Ashampoo.Translation.Systems.Formats.TsProj.Tests;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddFormatFactory(configuration =>
        {
            var path = Path.GetDirectoryName(typeof(Startup).Assembly.Location);
            Guard.IsNotNullOrWhiteSpace(path, nameof(path));
            
            configuration.PluginPaths.Add(path);
        });
    }
}