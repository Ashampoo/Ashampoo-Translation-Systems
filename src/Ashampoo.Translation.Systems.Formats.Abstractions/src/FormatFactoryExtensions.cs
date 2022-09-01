using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ashampoo.Translation.Systems.Formats.Abstractions;

/// <summary>
/// Static class that contains extension methods for <see cref="IServiceCollection"/>.
/// </summary>
public static class FormatFactoryExtensions
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
        public List<string> PluginPaths { get; } = new();
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
    public static IServiceCollection AddFormatFactory(this IServiceCollection services,
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
}