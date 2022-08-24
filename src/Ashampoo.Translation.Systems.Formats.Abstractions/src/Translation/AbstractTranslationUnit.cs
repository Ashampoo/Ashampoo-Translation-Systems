namespace Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

/// <summary>
/// Abstract base class for the <see cref="ITranslationUnit"/> interface.
/// </summary>
public abstract class AbstractTranslationUnit : HashSet<ITranslation>, ITranslationUnit
{
    protected AbstractTranslationUnit(string id)
        : base(new TranslationUnitEqualityComparer())
    {
        Id = id;
    }

    public string Id { get; init; }
    
    /// <summary>
    /// Get or set the translation for the given language.
    /// </summary>
    /// <param name="language">The language of the <see cref="ITranslation"/> you are trying to get or set.</param>
    /// <param name="value">The <see cref="ITranslation"/> you are trying to set.</param>
    /// <exception cref="ArgumentNullException">The <paramref name="language"/> is null or whitespace or the <paramref name="value"/> ist null.</exception>
    /// <exception cref="InvalidOperationException">The <see cref="ITranslation"/> could not be added.</exception>
    public ITranslation this[string language]
    {
        get { return this.First(x => x.Language == language); }
        set
        {
            if (string.IsNullOrWhiteSpace(language))
                throw new ArgumentNullException($"AbstractTranslationUnit: {nameof(language)} can not be null.");
            if (value is null)
                throw new ArgumentNullException($"AbstractTranslationUnit: {nameof(value)} can not be null.");

            if (Add(value)) return;

            RemoveWhere(x => x.Language == language);
            if (!Add(value))
                throw new InvalidOperationException(
                    $"AbstractTranslationUnit: not able to add translation {value.Id}.");
        }
    }
    
    /// <summary>
    /// Try to get the translation for the given language.
    /// </summary>
    /// <param name="language">The language of the <see cref="ITranslation"/> you are trying to get.</param>
    /// <returns>The <see cref="ITranslation"/> for the given language, or null if it could not be found.</returns>
    public ITranslation? TryGet(string language)
    {
        return this.FirstOrDefault(x => x.Language == language);
    }
}

internal class TranslationUnitEqualityComparer : IEqualityComparer<ITranslation>
{
    public bool Equals(ITranslation? x, ITranslation? y)
    {
        if (x is null || y is null) return false;
        return x.Language == y.Language; // Translation is unique by language.
    }

    public int GetHashCode(ITranslation obj)
    {
        return HashCode.Combine(obj.Language);
    }
}