using Ashampoo.Translations.Formats.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Ashampoo.Translations.Formats.PO.Tests;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddFormatFactory().AddFormatProvider(builder =>
        {
            return builder.SetId("po")
                .SetSupportedFileExtensions(new[] { ".po" })
                .SetFormatType<POFormat>()
                .SetFormatBuilder<POFormatBuilder>()
                .Create();
        });
    }
}