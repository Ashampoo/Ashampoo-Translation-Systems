using Ashampoo.Translation.Systems.Formats.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Ashampoo.Translation.Systems.Formats.CSV;

internal static class DependencyInjectionExtension
{
    public static IServiceCollection AddCsvFormat(this IServiceCollection services)
    {
        return services.AddSingleton<CsvFormatProvider>()
            .AddSingleton<IFormatProvider<IFormat>>(sp => sp.GetRequiredService<CsvFormatProvider>())
            .AddSingleton<IFormatProvider<CsvFormat>>(sp => sp.GetRequiredService<CsvFormatProvider>());
    }
}