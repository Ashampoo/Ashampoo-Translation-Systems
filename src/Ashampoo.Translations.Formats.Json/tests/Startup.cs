using Ashampoo.Translations.Formats.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Ashampoo.Translations.Formats.Json.Tests;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddFormatFactory().AddFormatProvider(builder =>
        {
            return builder.SetId("json")
                .SetSupportedFileExtensions(new[] { ".json" })
                .SetFormatType<JsonFormat>()
                .SetFormatBuilder<JsonFormatBuilder>()
                .Create();
        });
    }
}