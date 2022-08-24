namespace Ashampoo.Translation.Systems.Formats.Abstractions;

public interface IFormatProvider
{
    /// <summary>
    /// Returns the id of the format provider.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Creates a new instance of the format.
    /// </summary>
    /// <returns></returns>
    IFormat Create();

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
    /// Returns the type of the format implementation.
    /// </summary>
    Type FormatType { get; }

    /// <summary>
    /// Returns the type of the format builder implementation.
    /// </summary>
    /// <value></value>
    Type FormatBuilderType { get; }

    /// <summary>
    /// Creates a new instance of the format builder.
    /// </summary>
    /// <returns></returns>
    IFormatBuilder GetFormatBuilder();
}