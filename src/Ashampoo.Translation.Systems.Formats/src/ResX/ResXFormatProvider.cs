using Ashampoo.Translation.Systems.Formats.Abstractions;

namespace Ashampoo.Translation.Systems.Formats.ResX;

/// <summary>
/// Implementation of <see cref="IFormatProvider{T}"/> interface for the ResX format.
/// </summary>
public sealed class ResXFormatProvider : IFormatProvider<ResXFormat>
{
    /// <inheritdoc />
    public string Id => "resx";

    /// <inheritdoc />
    public ResXFormat Create() => new();

    /// <inheritdoc />
    public bool SupportsFileName(string fileName)
    {
        return SupportedFileExtensions.Any(ext => fileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
    }

    /// <inheritdoc />
    public IEnumerable<string> SupportedFileExtensions { get; } = [".resx"];

    /// <inheritdoc />
    public IFormatBuilder<ResXFormat> GetFormatBuilder() => new ResXFormatBuilder();
}