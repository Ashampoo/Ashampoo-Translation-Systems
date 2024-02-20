using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ashampoo.Translation.Systems.Formats.Abstractions;

/// <summary>
/// Static class that contains extension methods for <see cref="IServiceCollection"/>.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Register the <see cref="DefaultFormatFactory"/> in the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> to add the <see cref="DefaultFormatFactory"/> to.
    /// </param>
    /// <returns>
    /// The <see cref="IServiceCollection"/> so that additional calls can be chained.
    /// </returns>
    public static IServiceCollection AddFormatFactory(this IServiceCollection services)
    {
        services.AddSingleton<IFormatFactory, DefaultFormatFactory>();

        return services;
    }

    /// <summary>
    /// Options class for configuring the service registration of <see cref="DefaultFormatFactory"/>.
    /// </summary>
    public class FormatFactoryOptions
    {
        /// <summary>
        /// A list of <see cref="string">strings</see> representing
        /// file paths to plugins that will be loaded.
        /// </summary>
        public List<string> PluginPaths { get; } = [];
    }

    /// <summary>
    /// Extension method that registers the <see cref="DefaultFormatFactory"/> in the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> to add the <see cref="DefaultFormatFactory"/> to.
    /// </param>
    /// <param name="configuration">
    /// The options to configure the <see cref="DefaultFormatFactory"/>.
    /// </param>
    /// <returns>
    /// The <see cref="IServiceCollection"/> so that additional calls can be chained.
    /// </returns>
    private static IServiceCollection AddFormatFactory(this IServiceCollection services,
        Action<FormatFactoryOptions> configuration)
    {
        FormatFactoryOptions options = new();
        configuration(options);

        services.AddSingleton<IFormatFactory>(sp =>
        {
            var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<FormatProviderLoader>();

            var loader = new FormatProviderLoader(logger);
            foreach (var path in options.PluginPaths)
            {
                loader.LoadPlugins(path);
            }

            return new DefaultFormatFactory(loader.FormatProviders);
        });
        return services;
    }
    
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

    /// <summary>
    /// Register the given <see cref="IFormat"/> with the service collection.
    /// </summary>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> to add the <see cref="IFormat"/> to.
    /// </param>
    /// <typeparam name="T">
    /// The type of <see cref="IFormat"/> to register.
    /// </typeparam>
    /// <returns>
    /// The <see cref="IServiceCollection"/> so that additional calls can be chained.
    /// </returns>
    public static IServiceCollection RegisterFormat<T>(this IServiceCollection services) where T : IFormat
    {
        var format = Activator.CreateInstance<T>();
        services.AddFormatProvider(format.BuildFormatProvider());

        return services;
    }
}