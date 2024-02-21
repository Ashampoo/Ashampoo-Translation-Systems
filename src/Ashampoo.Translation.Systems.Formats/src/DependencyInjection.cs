using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.AshLang;
using Ashampoo.Translation.Systems.Formats.Gengo;
using Ashampoo.Translation.Systems.Formats.JavaProperties;
using Ashampoo.Translation.Systems.Formats.Json;
using Ashampoo.Translation.Systems.Formats.NLang;
using Ashampoo.Translation.Systems.Formats.PO;
using Ashampoo.Translation.Systems.Formats.ResX;
using Ashampoo.Translation.Systems.Formats.TsProj;
using Microsoft.Extensions.DependencyInjection;

namespace Ashampoo.Translation.Systems.Formats;

/// <summary>
/// Static class that contains extension methods for <see cref="IServiceCollection"/>.
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
        services.AddSingleton<IFormatFactory, DefaultFormatFactory>();
        services
            .AddAshLangFormatFeatures()
            .AddGengoFormatFeatures()
            .AddJavaPropertiesFormatFeatures()
            .AddJsonFormatFeatures()
            .AddNLangFormatFeatures()
            .AddPOFormatFeatures()
            .AddResXFormatFeatures()
            .AddTsProjFormatFeatures();
        
        return services;
    }

}