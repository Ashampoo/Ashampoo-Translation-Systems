using Microsoft.Extensions.DependencyInjection;

namespace Ashampoo.Translation.Systems.Formats.QT;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddQtFormat(this IServiceCollection services)
    {
        return services;
    }
}