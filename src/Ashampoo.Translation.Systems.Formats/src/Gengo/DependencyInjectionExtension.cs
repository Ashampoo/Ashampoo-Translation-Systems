using Ashampoo.Translation.Systems.Formats.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Ashampoo.Translation.Systems.Formats.Gengo;

/// <summary>
/// Static class that contains extension methods for <see cref="IServiceCollection"/>.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers all necessary services for the Gengo format.
    /// </summary>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> to register the services with.
    /// </param>
    /// <returns>
    /// The <see cref="IServiceCollection"/> for chaining.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if something went wrong during the registration.
    /// </exception>
    public static IServiceCollection AddGengoFormatFeatures(this IServiceCollection services)
    {
        services.AddSingleton<GengoFormatProvider>()
            .AddSingleton<IFormatProvider<IFormat>>(sp => sp.GetRequiredService<GengoFormatProvider>())
            .AddSingleton<IFormatProvider<GengoFormat>>(sp => sp.GetRequiredService<GengoFormatProvider>());
        
        return services;
    }
}