namespace Ashampoo.Translation.Systems.Formats.Abstractions;

/// <summary>
/// Format factory to handle format providers.
/// </summary>
public interface IFormatFactory
{
    /// <summary>
    /// Returns a collection of all available format providers.
    /// </summary>
    /// <returns></returns>
    IEnumerable<IFormatProvider> GetFormatProviders();

    /// <summary>
    /// Creates a new instance of a format by its id.
    /// </summary>
    /// <param name="formatId"></param>
    /// <returns></returns>
    IFormat CreateFormat(string formatId);

    /// <summary>
    /// Tries to create a new instance of a format by the given filename.
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    IFormat? TryCreateFormatByFileName(string fileName);

    IFormatProvider GetFormatProvider(IFormat format);
    IFormatProvider? TryGetFormatProvider(IFormat format);

    IFormatProvider GetFormatProvider(string formatId);
    IFormatProvider? TryGetFormatProvider(string formatId);

    IFormatProvider GetFormatProvider(Type formatType);
    IFormatProvider? TryGetFormatProvider(Type formatType);
}