namespace Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

/// <summary>
/// Abstract base class for the <see cref="ITranslation"/> interface.
/// </summary>
public abstract class AbstractTranslationString : ITranslation
{
    /// <inheritdoc />
    public virtual string Value { get; set; } = "";

    /// <inheritdoc />
    protected string Id { get; set; }

    /// <inheritdoc />
    public virtual string? Comment { get; set; }

    /// <inheritdoc />
    public virtual bool IsEmpty => string.IsNullOrWhiteSpace(Value);

    /// <inheritdoc />
    public virtual string Language { get; set; } = "";

    /// <summary>
    /// Base constructor for the <see cref="AbstractTranslationString"/> class.
    /// </summary>
    /// <param name="id">
    /// The id of the translation string.
    /// </param>
    /// <param name="value">
    /// The value of the translation string.
    /// </param>
    /// <param name="language">
    /// The language of the translation string.
    /// </param>
    /// <param name="comment">
    /// The comment of the translation string.
    /// </param>
    protected  AbstractTranslationString(string id, string value, string language, string? comment = null)
    {
        Id = id;
        Value = value;
        Language = language;
        Comment = comment;
    }
}