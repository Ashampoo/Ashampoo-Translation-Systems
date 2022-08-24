namespace Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

/// <summary>
/// Default implementation of the <see cref="ITranslationString"/> interface.
/// </summary>
public class DefaultTranslationString : AbstractTranslationString
{
    public DefaultTranslationString(string id, string value, string language, string? comment = null) : base(id, value,
        language, comment ?? "")
    {
    }
}