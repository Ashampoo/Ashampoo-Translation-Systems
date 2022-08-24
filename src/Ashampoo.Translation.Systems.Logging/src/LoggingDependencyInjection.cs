using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Ashampoo.Translation.Systems.Logging;

internal record Dummy;

public static class LoggingDependencyInjection
{
    /// <summary>
    /// Add necessary logging services to the service collection.
    /// </summary>
    /// <param name="services">
    /// The service collection to add services to.
    /// </param>
    /// <returns>
    /// The service collection for chaining.
    /// </returns>
    public static IServiceCollection AddLoggingServices(this IServiceCollection services)
    {
        services.AddMediatR(typeof(Dummy).Assembly);

        return services;
    }
}