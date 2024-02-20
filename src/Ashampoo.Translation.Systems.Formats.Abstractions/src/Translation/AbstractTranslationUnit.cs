namespace Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

/// <summary>
/// Abstract base class for the <see cref="ITranslationUnit"/> interface.
/// </summary>
public abstract class AbstractTranslationUnit : ITranslationUnit
{
    /// <summary>
    /// Base constructor for the <see cref="AbstractTranslationUnit"/> class.
    /// </summary>
    /// <param name="id">
    /// The id of the translation unit.
    /// </param>
    protected AbstractTranslationUnit(string id)
    {
        Id = id;
    }

    /// <inheritdoc />
    public string Id { get; init; }

    /// <inheritdoc />
    public ICollection<ITranslation> Translations { get; } = new HashSet<ITranslation>();
}