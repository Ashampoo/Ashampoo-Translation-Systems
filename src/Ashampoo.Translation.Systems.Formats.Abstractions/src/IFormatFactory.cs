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
    IEnumerable<IFormatProvider> GetFormatProviders();

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
    IFormatProvider GetFormatProvider(IFormat format);
    
    /// <summary>
    /// Try to get the <see cref="IFormatProvider"/> for the specified <see cref="IFormat"/>.
    /// </summary>
    /// <param name="format">
    /// The format to get the format provider for.
    /// </param>
    /// <returns>
    /// The <see cref="IFormatProvider"/> for the specified <see cref="IFormat"/>, or null if not found.
    /// </returns>
    IFormatProvider? TryGetFormatProvider(IFormat format);
    
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
    IFormatProvider GetFormatProvider(string formatId);
    
    /// <summary>
    /// Try to get the <see cref="IFormatProvider"/> for the specified format id.
    /// </summary>
    /// <param name="formatId">
    /// The format id to get the format provider for.
    /// </param>
    /// <returns>
    /// The <see cref="IFormatProvider"/> for the specified format id, or null if not found.
    /// </returns>
    IFormatProvider? TryGetFormatProvider(string formatId);
    
    /// <summary>
    /// Get the <see cref="IFormatProvider"/> for the specified <see cref="Type"/>.
    /// </summary>
    /// <param name="formatType">
    /// The format type to get the format provider for.
    /// </param>
    /// <returns>
    /// The <see cref="IFormatProvider"/> for the specified <see cref="Type"/>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown if the specified <see cref="Type"/> is not a supported format.
    /// </exception>
    /// <exception cref="Exception">
    /// Thrown if the format provider for the specified format is not found.
    /// </exception>
    IFormatProvider GetFormatProvider(Type formatType);
    
    /// <summary>
    /// Try to get the <see cref="IFormatProvider"/> for the specified <see cref="Type"/>.
    /// </summary>
    /// <param name="formatType">
    /// The format type to get the format provider for.
    /// </param>
    /// <returns>
    /// The <see cref="IFormatProvider"/> for the specified <see cref="Type"/>, or null if not found.
    /// </returns>
    IFormatProvider? TryGetFormatProvider(Type formatType);
}