namespace Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

/// <summary>
/// Interface for a container that contains <see cref="ITranslation"/>.
/// </summary>
public interface ITranslationUnit : IEnumerable<ITranslation>
{
    string Id { get; }
    int Count { get; }

    ITranslation this[string language] { get; set; }
    ITranslation? TryGet(string language);
}

/// <summary>
/// Interface for a container that contains <see cref="ITranslationUnit"/>.
/// </summary>
public interface ITranslationUnits : IEnumerable<ITranslationUnit>
{
    int Count { get; }
    ITranslationUnit? this[string id] { get; set; }
}