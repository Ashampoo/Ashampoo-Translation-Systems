using Ashampoo.Translation.Systems.Formats.Abstractions.Models;

namespace Ashampoo.Translation.Systems.Formats.Abstractions;

/// <summary>
/// Default implementation of the <see cref="AbstractFormatHeader"/> .
/// </summary>
public class DefaultFormatHeader : AbstractFormatHeader
{
    /// <inheritdoc />
    public override Language TargetLanguage { get; set; } = Language.Empty;

    /// <inheritdoc />
    public override Language? SourceLanguage { get; set; }

    /// <inheritdoc />
    public override Dictionary<string, string> AdditionalHeaders { get; set; } = [];
}