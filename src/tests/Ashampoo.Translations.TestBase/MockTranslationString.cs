using Ashampoo.Translations.Formats.Abstractions.Translation;

namespace Ashampoo.Translations.TestBase;

public class MockTranslationString : ITranslationString
{
    public string Value { get; set; }

    public string Id { get; }

    public string? Comment { get; set; }

    public bool IsEmpty => string.IsNullOrWhiteSpace(Value);

    public string Language { get; set; }

    public MockTranslationString(string id, string value, string language = "")
    {
        Id = id;
        Value = value;
        Language = language;
    }
}