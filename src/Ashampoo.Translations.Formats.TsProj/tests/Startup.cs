using Ashampoo.Translations.Formats.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Ashampoo.Translations.Formats.TsProj.Tests;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddFormatFactory().AddFormatProvider(builder =>
        {
            return builder.SetId("tsproj")
                .SetSupportedFileExtensions(new[] { ".tsproj" })
                .SetFormatType<TsProjFormat>()
                .SetFormatBuilder<TsProjFormatBuilder>()
                .Create();
        });
    }
}