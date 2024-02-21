using System.Collections.Generic;
using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Models;

namespace Ashampoo.Translation.Systems.TestBase;

public class MockHeader : AbstractFormatHeader
{
    public override Language? SourceLanguage { get; set; } = Language.Empty;
    public override Dictionary<string, string> AdditionalHeaders { get; set; } = new();
    public override Language TargetLanguage { get; set; } = Language.Empty;
}