using Ashampoo.Translation.Systems.Formats.Abstractions;

namespace Ashampoo.Translation.Systems.TestBase;

public class MockHeader : AbstractFormatHeader
{
    public override string? SourceLanguage { get; set; } = "";
    public override string TargetLanguage { get; set; } = "";
}