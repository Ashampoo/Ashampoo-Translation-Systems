using Ashampoo.Translation.Systems.Formats.Abstractions.Models;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

namespace Ashampoo.Translation.Systems.TestBase;

public class MockTranslationString : ITranslation
{
    public string Value { get; set; }

    public string Id { get; }

    public string? Comment { get; set; }

    public bool IsEmpty => string.IsNullOrWhiteSpace(Value);

    public Language Language { get; set; }

    public MockTranslationString(string id, string value, Language language = new())
    {
        Id = id;
        Value = value;
        Language = language;
    }
}