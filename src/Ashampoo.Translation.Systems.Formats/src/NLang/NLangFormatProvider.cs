using Ashampoo.Translation.Systems.Formats.Abstractions;

namespace Ashampoo.Translation.Systems.Formats.NLang;

/// <summary>
/// Implementation of <see cref="IFormatProvider"/> for the NLang format.
/// </summary>
public sealed class NLangFormatProvider : IFormatProvider<NLangFormat>
{
    /// <inheritdoc />
    public string Id { get; } = "nlang";

    /// <inheritdoc />
    public NLangFormat Create() => new();

    /// <inheritdoc />
    public bool SupportsFileName(string fileName)
    {
        return SupportedFileExtensions.Any(ext => fileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
    }

    /// <inheritdoc />
    public string[] SupportedFileExtensions { get; } = [".nlang3"];

    /// <inheritdoc />
    public IFormatBuilder<NLangFormat> GetFormatBuilder() => new NLangFormatBuilder();
}