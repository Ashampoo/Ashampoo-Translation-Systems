using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

namespace Ashampoo.Translation.Systems.Formats.Abstractions;

/// <summary>
/// Interface for a translation format.
/// </summary>
public interface IFormat : ITranslationUnits
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
    /// <exception cref="UnsupportedFormatException"></exception>
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
    FormatLanguageCount LanguageCount { get; }
}