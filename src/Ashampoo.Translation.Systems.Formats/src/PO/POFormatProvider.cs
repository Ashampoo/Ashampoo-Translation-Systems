using Ashampoo.Translation.Systems.Formats.Abstractions;

namespace Ashampoo.Translation.Systems.Formats.PO;

/// <summary>
/// Implementation of the <see cref="IFormatProvider{T}"/> for the PO format.
/// </summary>
// ReSharper disable once InconsistentNaming
public sealed class POFormatProvider : IFormatProvider<POFormat>
{
    /// <inheritdoc />
    public string Id => "po";

    /// <inheritdoc />
    public POFormat Create() => new();

    /// <inheritdoc />
    public bool SupportsFileName(string fileName)
    {
        return SupportedFileExtensions.Any(ext => fileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
    }

    /// <inheritdoc />
    public IEnumerable<string> SupportedFileExtensions { get; } = [".po"];

    /// <inheritdoc />
    public IFormatBuilder<POFormat> GetFormatBuilder() => new POFormatBuilder();
}