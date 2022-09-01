using Ashampoo.Translation.Systems.Components.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Ashampoo.Translation.Systems.Components;


internal record Dummy;

/// <summary>
/// Extension methods for the <see cref="IServiceCollection"/>
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds all required services from the component library to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddBaseComponents(this IServiceCollection services)
    {
        services.AddScoped<IFileService, WebFileService>();
        services.AddScoped<IFormatService, FormatService>();
        services.AddMediatR(typeof(Dummy).Assembly);
        
        return services;
    }
}