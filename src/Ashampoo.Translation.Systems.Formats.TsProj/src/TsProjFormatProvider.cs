using Ashampoo.Translation.Systems.Formats.Abstractions;

namespace Ashampoo.Translation.Systems.Formats.TsProj;

/// <inheritdoc />
public sealed class TsProjFormatProvider : IFormatProvider<TsProjFormat>
{
    /// <inheritdoc />
    public string Id { get; } = "tsproj";

    /// <inheritdoc />
    public TsProjFormat Create() => new();

    /// <inheritdoc />
    public bool SupportsFileName(string fileName)
    {
        return SupportedFileExtensions.Any(ext => fileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
    }

    /// <inheritdoc />
    public string[] SupportedFileExtensions { get; } = [".tsproj"];

    /// <inheritdoc />
    public IFormatBuilder<TsProjFormat> GetFormatBuilder() => new TsProjFormatBuilder();
}