using Ashampoo.Translation.Systems.Formats.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Ashampoo.Translation.Systems.Formats.NLang.Tests;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddFormatFactory().AddFormatProvider(builder =>
        {
            return builder.SetId("nlang")
                .SetSupportedFileExtensions(new[] { ".nlang3" })
                .SetFormatType<NLangFormat>()
                .SetFormatBuilder<NLangFormatBuilder>()
                .Create();
        });
    }
}