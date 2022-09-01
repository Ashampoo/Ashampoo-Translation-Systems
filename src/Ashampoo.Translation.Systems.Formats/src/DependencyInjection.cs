using Ashampoo.Translation.Systems.Formats.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Ashampoo.Translation.Systems.Formats;

/// <summary>
/// This class provides extension methods for the <see cref="IServiceCollection"/> interface.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers all necessary services and the format implementations.
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
    public static IServiceCollection RegisterFormats(this IServiceCollection services)
    {
        services.AddFormatFactory(configuration =>
        {
            var path = Path.GetDirectoryName(typeof(DependencyInjection).Assembly.Location);

            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path), "Assembly location could not be found.");
    
            configuration.PluginPaths.Add(path);
        });
        return services;
    }   
}