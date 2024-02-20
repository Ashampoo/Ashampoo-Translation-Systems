using Ashampoo.Translation.Systems.Formats.Abstractions.Models;

namespace Ashampoo.Translation.Systems.Formats.Abstractions;

/// <summary>
/// Abstract base class for the <see cref="IFormatHeader"/>
/// </summary>
public abstract class AbstractFormatHeader : IFormatHeader
{
    /// <inheritdoc />
    public abstract Language TargetLanguage { get; set; }

    /// <inheritdoc />
    public abstract Language? SourceLanguage { get; set; }

    /// <inheritdoc />
    public abstract Dictionary<string, string> AdditionalHeaders { get; set; }
}