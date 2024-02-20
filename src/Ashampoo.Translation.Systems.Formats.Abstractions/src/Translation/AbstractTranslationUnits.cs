namespace Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

/// <summary>
/// Abstract base class for the <see cref="ITranslationUnits"/> interface.
/// </summary>
public abstract class AbstractTranslationUnits
{
    /// <summary>
    /// Base constructor for the <see cref="AbstractTranslationUnits"/> class.
    /// </summary>
    /// <param name="collection">
    /// The collection of <see cref="ITranslationUnit"/> objects
    /// to add to the <see cref="AbstractTranslationUnits"/> object.
    /// </param>
    protected AbstractTranslationUnits(IEnumerable<ITranslationUnit> collection)
    {
    }

    /// <summary>
    /// Base constructor for the <see cref="AbstractTranslationUnits"/> class.
    /// </summary>
    protected AbstractTranslationUnits()
    {
    }

    /// <summary>
    /// The id of the <see cref="AbstractTranslationUnits"/> object.
    /// </summary>
    public virtual string Id { get; init; } = "";
}