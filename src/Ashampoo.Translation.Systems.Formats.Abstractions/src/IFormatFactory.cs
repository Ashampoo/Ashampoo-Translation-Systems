namespace Ashampoo.Translation.Systems.Formats.Abstractions;

/// <summary>
/// Interface for a factory for <see cref="IFormat"/>.
/// </summary>
public interface IFormatFactory
{
    /// <summary>
    /// Get a collection of all available format providers.
    /// </summary>
    /// <returns>
    /// A collection of all available format providers.
    /// </returns>
    IEnumerable<IFormatProvider<IFormat>> GetFormatProviders();

    /// <summary>
    /// Creates a new instance of <see cref="IFormat" /> by the given <paramref name="formatId" />. 
    /// </summary>
    /// <param name="formatId">
    /// The format id.
    /// </param>
    /// <returns>
    /// The <see cref="IFormat" />.
    /// </returns>
    IFormat CreateFormat(string formatId);

    /// <summary>
    /// Try to create a format from a file, via the file name.
    /// </summary>
    /// <param name="fileName">
    /// The file name.
    /// </param>
    /// <returns>
    /// An instance of <see cref="IFormat"/> if the file name is supported by a format, otherwise null.
    /// </returns>
    IFormat? TryCreateFormatByFileName(string fileName);
    
    /// <summary>
    /// Gets the format provider for the specified <see cref="IFormat"/> .
    /// </summary>
    /// <param name="format">
    /// The format.
    /// </param>
    /// <returns></returns>
    /// <exception cref="Exception">
    /// Thrown if the format provider for the specified format is not found.
    /// </exception>
    IFormatProvider<T> GetFormatProvider<T>(T format) where T : class, IFormat;
    
    /// <summary>
    /// Try to get the <see cref="IFormatProvider"/> for the specified <see cref="IFormat"/>.
    /// </summary>
    /// <param name="format">
    /// The format to get the format provider for.
    /// </param>
    /// <returns>
    /// The <see cref="IFormatProvider"/> for the specified <see cref="IFormat"/>, or null if not found.
    /// </returns>
    IFormatProvider<T>? TryGetFormatProvider<T>(T format) where T : class, IFormat ;
    
    /// <summary>
    /// Gets the format provider for a format with the specified id.
    /// </summary>
    /// <param name="formatId">
    /// The format id.
    /// </param>
    /// <returns></returns>
    /// <exception cref="Exception">
    /// Thrown if the format provider for the specified format is not found.
    /// </exception>
    IFormatProvider<IFormat> GetFormatProvider(string formatId);
    
    /// <summary>
    /// Try to get the <see cref="IFormatProvider"/> for the specified format id.
    /// </summary>
    /// <param name="formatId">
    /// The format id to get the format provider for.
    /// </param>
    /// <returns>
    /// The <see cref="IFormatProvider"/> for the specified format id, or null if not found.
    /// </returns>
    IFormatProvider<IFormat>? TryGetFormatProvider(string formatId);
}