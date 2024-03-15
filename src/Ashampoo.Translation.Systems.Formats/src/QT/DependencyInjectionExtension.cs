using Ashampoo.Translation.Systems.Formats.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Ashampoo.Translation.Systems.Formats.QT;

/// <summary>
/// Static class that contains extension methods for <see cref="IServiceCollection"/>.
/// </summary>
public static class DependencyInjectionExtension
{
    /// <summary>
    /// Registers all necessary services for the QT format.
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
    public static IServiceCollection AddQtFormat(this IServiceCollection services)
    {
        return services.AddSingleton<QtFormatProvider>()
            .AddSingleton<IFormatProvider<IFormat>>(sp => sp.GetRequiredService<QtFormatProvider>())
            .AddSingleton<IFormatProvider<QtFormat>>(sp => sp.GetRequiredService<QtFormatProvider>());
    }
}