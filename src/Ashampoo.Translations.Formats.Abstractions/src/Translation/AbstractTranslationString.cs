namespace Ashampoo.Translations.Formats.Abstractions.Translation;

/// <summary>
/// Abstract base class for the <see cref="ITranslationString"/> interface.
/// </summary>
public abstract class AbstractTranslationString : ITranslationString
{
    public virtual string Value { get; set; } = "";

    public virtual string Id { get; set; } = "";

    public virtual string? Comment { get; set; }

    public virtual bool IsEmpty => string.IsNullOrWhiteSpace(Value);

    public virtual string Language { get; set; } = "";

    protected  AbstractTranslationString(string id, string value, string language, string comment)
    {
        Id = id;
        Value = value;
        Language = language;
        Comment = comment;
    }

    protected AbstractTranslationString(string id, string value, string language)
    {
        Id = id;
        Value = value;
        Language = language;
    }
}