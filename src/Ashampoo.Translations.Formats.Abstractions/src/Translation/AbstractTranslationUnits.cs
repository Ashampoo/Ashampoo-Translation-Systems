namespace Ashampoo.Translations.Formats.Abstractions.Translation;

/// <summary>
/// Abstract base class for the <see cref="ITranslationUnits"/> interface.
/// </summary>
public abstract class AbstractTranslationUnits : HashSet<ITranslationUnit>
{
    protected AbstractTranslationUnits(IEnumerable<ITranslationUnit> collection)
        : base(collection, new TranslationUnitsEqualityComparer())
    {
    }

    protected AbstractTranslationUnits()
        : base(new TranslationUnitsEqualityComparer())
    {
    }

    public virtual string Id { get; init; } = "";

    /// <summary>
    /// Get or set the translation unit for the given id.
    /// </summary>
    /// <param name="id">The id of the <see cref="ITranslationUnit"/></param>
    /// <param name="value">The <see cref="ITranslationUnit"/> you are trying to set.</param>
    /// <exception cref="ArgumentNullException">The id is null or whitespace, or the  value is null.</exception>
    /// <exception cref="InvalidOperationException">The <see cref="ITranslationUnit"/> could not be added.</exception>
    public ITranslationUnit? this[string id]
    {
        //TODO: dont return null if not found, make try get instead
        get { return this.FirstOrDefault(x => x.Id == id); }
        set
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException($"AbstractTranslationUnits: {nameof(id)} can not be null.");
            if (value is null)
                throw new ArgumentNullException($"AbstractTranslationUnits: {nameof(value)} can not be null");

            if (Add(value)) return;
            RemoveWhere(x => x.Id == id);
            if (!Add(value))
                throw new InvalidOperationException(
                    $"AbstractTranslationUnits: not able to add translation unit {value.Id}.");
        }
    }
}

public class TranslationUnitsEqualityComparer : IEqualityComparer<ITranslationUnit>
{
    public bool Equals(ITranslationUnit? x, ITranslationUnit? y)
    {
        if (x is null || y is null) return false;
        return x.Id == y.Id; // TranslationUnit is unique by id
    }

    public int GetHashCode(ITranslationUnit obj)
    {
        return HashCode.Combine(obj.Id);
    }
}