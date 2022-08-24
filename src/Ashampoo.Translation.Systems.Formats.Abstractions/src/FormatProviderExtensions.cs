using Microsoft.Extensions.DependencyInjection;

namespace Ashampoo.Translation.Systems.Formats.Abstractions;

/// <summary>
/// Static class that contains extension methods for <see cref="IServiceCollection"/>.
/// </summary>
public static class FormatProviderExtensions
{
    /// <summary>
    /// Adds the format provider to the service collection.
    /// </summary>
    /// <param name="services">
    /// The service collection.
    /// </param>
    /// <param name="formatProviderFactory">
    /// Function that creates the format provider.
    /// </param>
    /// <returns></returns>
    public static IServiceCollection AddFormatProvider(this IServiceCollection services,
        Func<FormatProviderBuilder, IFormatProvider> formatProviderFactory)
    {
        services.AddSingleton(_ =>
        {
            var builder = new FormatProviderBuilder();
            return formatProviderFactory(builder);
        });
        return services;
    }
}