using Ashampoo.Translation.Systems.Formats.Abstractions.Models;

namespace Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

/// <summary>
/// Default implementation of the <see cref="ITranslation"/> interface.
/// </summary>
public class DefaultTranslationString : AbstractTranslationString
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultTranslationString"/> class.
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
    public DefaultTranslationString(string id, string value, Language language, string? comment = null) : base(id, value,
        language, comment)
    {
    }
}