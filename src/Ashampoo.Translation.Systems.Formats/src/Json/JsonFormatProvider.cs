using Ashampoo.Translation.Systems.Formats.Abstractions;

namespace Ashampoo.Translation.Systems.Formats.Json;

/// <summary>
/// The <see cref="IFormatProvider{T}"/> for the <see cref="JsonFormat"/>.
/// </summary>
public sealed class JsonFormatProvider : IFormatProvider<JsonFormat>
{
    /// <inheritdoc />
    public string Id => "json";

    /// <inheritdoc />
    public JsonFormat Create() => new();

    /// <inheritdoc />
    public bool SupportsFileName(string fileName)
    {
        return SupportedFileExtensions.Any(ext => fileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
    }

    /// <inheritdoc />
    public IEnumerable<string> SupportedFileExtensions { get; } = [".json"];

    /// <inheritdoc />
    public IFormatBuilder<JsonFormat> GetFormatBuilder() => new JsonFormatBuilder();
}