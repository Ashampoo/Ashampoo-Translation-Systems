using Ashampoo.Translation.Systems.Formats.Abstractions;

namespace Ashampoo.Translation.Systems.Formats.QT;

public class QTFormatProvider : IFormatProvider<QTFormat>
{
    /// <inheritdoc />
    public string Id => "ts";

    /// <inheritdoc />
    public QTFormat Create() => new();

    /// <inheritdoc />
    public bool SupportsFileName(string fileName)
    {
        return SupportedFileExtensions.Any(ext => ext.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
    }

    /// <inheritdoc />
    public IEnumerable<string> SupportedFileExtensions { get; } = [".ts"];

    /// <inheritdoc />
    public IFormatBuilder<QTFormat> GetFormatBuilder() => new QTFormatBuilder();
}