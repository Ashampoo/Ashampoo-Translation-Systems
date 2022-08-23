using Ashampoo.Translations.Formats.Abstractions;

namespace Ashampoo.Translations.TestBase;

public class MockHeader : AbstractFormatHeader
{
    public override string? SourceLanguage { get; set; } = "";
    public override string TargetLanguage { get; set; } = "";
}