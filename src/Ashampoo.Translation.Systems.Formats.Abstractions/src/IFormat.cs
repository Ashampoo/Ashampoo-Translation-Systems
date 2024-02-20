using System.Diagnostics.CodeAnalysis;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

namespace Ashampoo.Translation.Systems.Formats.Abstractions;

/// <summary>
/// Interface for a translation format.
/// </summary>
public interface IFormat
{
    /// <summary>
    /// Reads the format from the given stream.
    /// </summary>
    /// <param name="stream">
    /// The stream to read from.
    /// </param>
    /// <param name="options">
    /// The options to use.
    /// </param>
    /// <exception cref="UnsupportedFormatException">
    /// Thrown if the format is not supported.
    /// </exception>
    void Read(Stream stream, FormatReadOptions? options = null) // TODO: require options
    {
        ReadAsync(stream, options).Wait();
    }
    
    /// <summary>
    /// Reads the format from the given stream asynchronously.
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    Task ReadAsync(Stream stream, FormatReadOptions? options = null); // TODO: require options

    /// <summary>
    /// Writes the format to the given stream.
    /// </summary>
    /// <param name="stream"></param>
    /// <exception cref="UnsupportedFormatException"></exception>
    void Write(Stream stream);

    /// <summary>
    /// Writes the format to the given stream asynchronously.
    /// </summary>
    /// <param name="stream"></param>
    /// <exception cref="UnsupportedFormatException"></exception>
    Task WriteAsync(Stream stream);

    /// <summary>
    /// A function to build a <see cref="IFormatProvider"/> for this format.
    /// </summary>
    /// <returns>
    /// A function that takes a <see cref="FormatProviderBuilder"/> and returns a new <see cref="IFormatProvider"/>.
    /// </returns>
    /// <exception cref="NotImplementedException">
    /// If the format does not support building a <see cref="IFormatProvider"/>.
    /// </exception>
    Func<FormatProviderBuilder, IFormatProvider> BuildFormatProvider();

    /// <summary>
    /// The <see cref="IFormatHeader"/> containing the header information for this format.
    /// </summary>
    IFormatHeader Header { get; }
    
    /// <summary>
    /// Information about how many languages the format can handle.
    /// </summary>
    LanguageSupport LanguageSupport { get; }
    
    /// <summary>
    /// Gets the collection of translation units associated with the format.
    /// </summary>
    ICollection<ITranslationUnit> TranslationUnits { get; }
}

/// <summary>
/// Provides extension methods for collections of translation units.
/// </summary>
public static class TranslationUnitCollectionExtensions
{
    /// <summary>
    /// Tries to get a translation unit from the collection by its ID.
    /// </summary>
    /// <param name="translationUnits">The collection of translation units.</param>
    /// <param name="id">The ID of the translation unit to get.</param>
    /// <param name="translationUnit">When this method returns, contains the translation unit with the specified ID, if found; otherwise, null.</param>
    /// <returns>true if a translation unit with the specified ID is found; otherwise, false.</returns>
    public static bool TryGetTranslationUnit(this ICollection<ITranslationUnit> translationUnits, string id,
        [NotNullWhen(true)] out ITranslationUnit? translationUnit)
    {
        var foundTranslationUnit = translationUnits.FirstOrDefault(t => t.Id == id);
        if (foundTranslationUnit is null)
        {
            translationUnit = null;
            return false;
        }

        translationUnit = foundTranslationUnit;
        return true;
    }

    /// <summary>
    /// Gets a translation unit from the collection by its ID.
    /// </summary>
    /// <param name="translationUnits">The collection of translation units.</param>
    /// <param name="id">The ID of the translation unit to get.</param>
    /// <returns>The translation unit with the specified ID.</returns>
    /// <exception cref="InvalidOperationException">No translation unit with the specified ID is found.</exception>
    public static ITranslationUnit GetTranslationUnit(this ICollection<ITranslationUnit> translationUnits, string id) =>
        translationUnits.First(t => t.Id == id);

    /// <summary>
    /// Adds a new translation to the collection or updates an existing one.
    /// </summary>
    /// <param name="translations">The collection of translations.</param>
    /// <param name="language">The language of the translation to add or update.</param>
    /// <param name="value">The new translation.</param>
    public static void AddOrUpdateTranslationUnit(this ICollection<ITranslation> translations, string language, ITranslation value)
    {
        var existingTranslation = translations.FirstOrDefault(t => t.Language == language);

        if (existingTranslation is not null)
        {
            translations.Remove(existingTranslation);
        }

        translations.Add(value);
    }
}