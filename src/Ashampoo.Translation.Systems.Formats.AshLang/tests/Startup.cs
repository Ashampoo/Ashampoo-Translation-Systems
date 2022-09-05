using System;
using System.IO;
using Ashampoo.Translation.Systems.Formats.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ashampoo.Translation.Systems.Formats.AshLang.Tests;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // services.AddFormatFactory().AddFormatProvider(builder =>
        // {
        //     return builder.SetId("ashlang")
        //         .SetSupportedFileExtensions(new[] { ".ashlang" })
        //         .SetFormatType<AshLangFormat>()
        //         .SetFormatBuilder<AshLangFormatBuilder>()
        //         .Create();
        // });

        services.AddLogging(b => b.AddConsole());

        services.AddFormatFactory().RegisterFormat<AshLangFormat>();;
    }
}