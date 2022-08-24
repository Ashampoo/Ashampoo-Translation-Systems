namespace Ashampoo.Translation.Systems.Formats.Abstractions;

/// <summary>
/// Abstract base class for the <see cref="IFormatHeader"/>
/// </summary>
public abstract class AbstractFormatHeader : Dictionary<string, string>, IFormatHeader
{
    /// <inheritdoc />
    public abstract string TargetLanguage { get; set; }

    /// <inheritdoc />
    public abstract string? SourceLanguage { get; set; }
}