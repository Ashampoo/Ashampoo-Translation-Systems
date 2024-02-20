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
    
    ICollection<ITranslationUnit> TranslationUnits { get; }
}


public static class TranslationUnitCollectionExtensions
{
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

    public static ITranslationUnit GetTranslationUnit(this ICollection<ITranslationUnit> translationUnits, string id) =>
        translationUnits.First(t => t.Id == id);

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