using Ashampoo.Translation.Systems.Formats.Abstractions.Models;

namespace Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

/// <summary>
/// Abstract base class for the <see cref="ITranslation"/> interface.
/// </summary>
public abstract class AbstractTranslationString : ITranslation
{
    /// <inheritdoc />
    public string Value { get; set; }

    /// <inheritdoc />
    public string? Comment { get; set; }

    /// <inheritdoc />
    public Language Language { get; set; }

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
    protected  AbstractTranslationString(string value, Language language, string? comment = null)
    {
        Value = value;
        Language = language;
        Comment = comment;
    }
}