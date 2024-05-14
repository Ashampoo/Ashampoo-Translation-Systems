using Microsoft.Extensions.DependencyInjection;

namespace Ashampoo.Translation.Systems.Formats.CSV;

internal static class DependencyInjectionExtension
{
    public static IServiceCollection AddCsvFormat(this IServiceCollection services)
    {
        return services;
    }
}