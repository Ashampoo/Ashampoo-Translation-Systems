namespace Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

/// <summary>
/// Default implementation of the <see cref="ITranslationUnit"/> interface.
/// </summary>
public class DefaultTranslationUnit : AbstractTranslationUnit
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultTranslationUnit"/> class.
    /// </summary>
    /// <param name="id">
    /// The id of the translation unit.
    /// </param>
    public DefaultTranslationUnit(string id) : base(id)
    {
    }
}