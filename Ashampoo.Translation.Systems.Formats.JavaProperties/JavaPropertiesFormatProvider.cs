using Ashampoo.Translation.Systems.Formats.Abstractions;

namespace Ashampoo.Translation.Systems.Formats.JavaProperties;

/// <summary>
/// The format provider for the Java properties format.
/// </summary>
public sealed class JavaPropertiesFormatProvider : IFormatProvider<JavaPropertiesFormat>
{
    /// <inheritdoc />
    public string Id { get; } = "javaProperties";
    /// <inheritdoc />
    public JavaPropertiesFormat Create() => new();

    /// <inheritdoc />
    public bool SupportsFileName(string fileName)
    {
        return SupportedFileExtensions.Any(ext => fileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
    }

    /// <inheritdoc />
    public string[] SupportedFileExtensions { get; } = [".properties"];
    /// <inheritdoc />
    public IFormatBuilder<JavaPropertiesFormat> GetFormatBuilder() => new JavaPropertiesFormatBuilder();
}