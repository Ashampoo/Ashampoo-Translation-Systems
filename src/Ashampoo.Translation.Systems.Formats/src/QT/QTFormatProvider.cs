using Ashampoo.Translation.Systems.Formats.Abstractions;

namespace Ashampoo.Translation.Systems.Formats.QT;

/// <summary>
/// Implementation of <see cref="IFormatProvider{T}"/> interface for the QT format.
/// </summary>
public class QtFormatProvider : IFormatProvider<QtFormat>
{
    /// <inheritdoc />
    public string Id => "QT";

    /// <inheritdoc />
    public QtFormat Create() => new();

    /// <inheritdoc />
    public bool SupportsFileName(string fileName)
    {
        return SupportedFileExtensions.Any(ext => ext.EndsWith(fileName, StringComparison.OrdinalIgnoreCase));
    }

    /// <inheritdoc />
    public IEnumerable<string> SupportedFileExtensions { get; } = [".ts"];

    /// <inheritdoc />
    public IFormatBuilder<QtFormat> GetFormatBuilder() => new QtFormatBuilder();
}