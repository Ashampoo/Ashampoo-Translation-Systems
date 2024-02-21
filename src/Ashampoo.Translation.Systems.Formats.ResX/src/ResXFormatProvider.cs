using Ashampoo.Translation.Systems.Formats.Abstractions;

namespace Ashampoo.Translation.Systems.Formats.ResX;

/// <summary>
/// Implementation of <see cref="IFormatProvider"/> interface for the ResX format.
/// </summary>
public sealed class ResXFormatProvider : IFormatProvider<ResXFormat>
{
    /// <inheritdoc />
    public string Id { get; } = "resx";

    /// <inheritdoc />
    public ResXFormat Create() => new();

    /// <inheritdoc />
    public bool SupportsFileName(string fileName)
    {
        return SupportedFileExtensions.Any(ext => fileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
    }

    /// <inheritdoc />
    public string[] SupportedFileExtensions { get; } = [".resx"];

    /// <inheritdoc />
    public IFormatBuilder<ResXFormat> GetFormatBuilder() => new ResXFormatBuilder();
}