namespace Ashampoo.Translation.Systems.Formats.Abstractions;

/// <summary>
/// Interface for a provider for a specific <see cref="IFormat"/>.
/// Contains additional information about the format.
/// </summary>
public interface IFormatProvider<out T> where T : class, IFormat
{
    /// <summary>
    /// Returns the id of the format provider.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Creates a new instance of the format.
    /// </summary>
    /// <returns></returns>
    T Create();

    /// <summary>
    /// Returns true if the format provider supports the given file name, otherwise false.
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    bool SupportsFileName(string fileName);

    /// <summary>
    /// Returns the supported file extensions of the format provider.
    /// </summary>
    string[] SupportedFileExtensions { get; }

    /// <summary>
    /// Creates a new instance of the format builder.
    /// </summary>
    /// <returns></returns>
    IFormatBuilder<T> GetFormatBuilder();
}