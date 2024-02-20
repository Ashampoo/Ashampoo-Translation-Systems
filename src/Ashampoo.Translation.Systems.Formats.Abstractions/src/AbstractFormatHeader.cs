namespace Ashampoo.Translation.Systems.Formats.Abstractions;

/// <summary>
/// Abstract base class for the <see cref="IFormatHeader"/>
/// </summary>
public abstract class AbstractFormatHeader : IFormatHeader
{
    /// <inheritdoc />
    public abstract string TargetLanguage { get; set; }

    /// <inheritdoc />
    public abstract string? SourceLanguage { get; set; }

    /// <inheritdoc />
    public Dictionary<string, string> AdditionalHeaders { get; set; } = new();
}