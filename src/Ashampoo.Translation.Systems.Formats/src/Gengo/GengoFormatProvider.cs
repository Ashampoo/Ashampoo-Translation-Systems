using Ashampoo.Translation.Systems.Formats.Abstractions;

namespace Ashampoo.Translation.Systems.Formats.Gengo;

/// <summary>
/// <see cref="IFormatProvider{T}"/> for the Gengo format.
/// </summary>
public sealed class GengoFormatProvider : IFormatProvider<GengoFormat>
{
    /// <inheritdoc />
    public string Id => "gengo";

    /// <inheritdoc />
    public GengoFormat Create() => new();

    /// <inheritdoc />
    public bool SupportsFileName(string fileName)
    {
        return SupportedFileExtensions.Any(ext => fileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
    }

    /// <inheritdoc />
    public IEnumerable<string> SupportedFileExtensions { get; } = [".xlsx", ".xls"];

    /// <inheritdoc />
    public IFormatBuilder<GengoFormat> GetFormatBuilder() => new GengoFormatBuilder();
}