using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.Logging;

namespace Ashampoo.Translation.Systems.Formats.Abstractions;

internal class FormatProviderLoader
{
    private readonly ILogger<FormatProviderLoader> _logger;
    public List<IFormatProvider> FormatProviders { get; } = new();

    public FormatProviderLoader(ILogger<FormatProviderLoader> logger)
    {
        _logger = logger;
    }

    public void LoadPlugins(string path)
    {
        // Load all the assemblies in the given path
        var fileNames =
            Directory.GetFiles(path, "Ashampoo.Translation.Systems.Formats.*.dll", SearchOption.TopDirectoryOnly)
                .Select(Path.GetFileNameWithoutExtension).ToArray();

        foreach (var filename in fileNames)
        {
            if (filename != null)
            {
                var assembly = LoadAssembly(filename);
                var provider = CreateFormatProvider(assembly);
                FormatProviders.AddRange(provider);
            }
        }
    }

    private Assembly LoadAssembly(string fileName)
    {
        _logger.LogInformation("loading assembly {FileName}", fileName);

        var assemblyName = new AssemblyName(fileName);
        return AssemblyLoadContext.Default.LoadFromAssemblyName(assemblyName);
    }

    private IEnumerable<IFormatProvider> CreateFormatProvider(Assembly assembly)
    {
        var builder = new FormatProviderBuilder();
        foreach (var type in assembly.GetTypes())
        {
            if (typeof(IFormat).IsAssignableFrom(type) && type is { IsInterface: false, IsAbstract: false })
            {
                if (Activator.CreateInstance(type) is not IFormat format) continue;
                yield return format.BuildFormatProvider()(builder);
            }
        }
    }
}

internal class PluginLoadContext : AssemblyLoadContext
{
    private readonly AssemblyDependencyResolver _resolver;

    public PluginLoadContext(string pluginPath)
    {
        _resolver = new AssemblyDependencyResolver(pluginPath);
    }

    protected override Assembly? Load(AssemblyName assemblyName)
    {
        var assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);

        return assemblyPath != null ? LoadFromAssemblyPath(assemblyPath) : null;
    }

    protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
    {
        var libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);

        return libraryPath != null ? LoadUnmanagedDllFromPath(libraryPath) : IntPtr.Zero;
    }
}