using Ashampoo.Translations.Formats.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Ashampoo.Translations.Formats.Gengo.Tests;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddFormatFactory().AddFormatProvider(builder =>
        {
            return builder.SetId("gengo")
                .SetSupportedFileExtensions(new[] { ".xlsx", ".xls" })
                .SetFormatType<GengoFormat>()
                .SetFormatBuilder<GengoFormatBuilder>()
                .Create();
        });
    }
}