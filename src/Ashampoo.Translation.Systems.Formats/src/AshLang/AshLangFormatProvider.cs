using Ashampoo.Translation.Systems.Formats.Abstractions;

namespace Ashampoo.Translation.Systems.Formats.AshLang;

/// <summary>
/// <see cref="IFormatProvider{T}"/> for the AshLang format.
/// </summary>
public sealed class AshLangFormatProvider : IFormatProvider<AshLangFormat>
{
    /// <inheritdoc />
    public string Id => "ashlang";

    /// <inheritdoc />
    public AshLangFormat Create() => new();

    /// <inheritdoc />
    public bool SupportsFileName(string fileName)
    {
        return SupportedFileExtensions.Any(ext => fileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
    }

    /// <inheritdoc />
    public IEnumerable<string> SupportedFileExtensions => [".ashlang"];

    /// <inheritdoc />
    public IFormatBuilder<AshLangFormat> GetFormatBuilder() => new AshLangFormatBuilder();
}