namespace Ashampoo.Translation.Systems.Formats.Abstractions;

/// <summary>
/// Default implementation of the <see cref="AbstractFormatHeader"/> .
/// </summary>
public class DefaultFormatHeader : AbstractFormatHeader
{
    public override string TargetLanguage { get; set; } = "";
    public override string? SourceLanguage { get; set; }
}