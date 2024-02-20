namespace Ashampoo.Translation.Systems.Formats.Abstractions;

/// <summary>
/// Default implementation of the <see cref="AbstractFormatHeader"/> .
/// </summary>
public class DefaultFormatHeader : AbstractFormatHeader
{
    /// <inheritdoc />
    public override string TargetLanguage { get; set; } = "";

    /// <inheritdoc />
    public override string? SourceLanguage { get; set; }

    /// <inheritdoc />
    public override Dictionary<string, string> AdditionalHeaders { get; set; } = new();
}