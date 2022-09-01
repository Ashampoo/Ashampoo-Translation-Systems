using Ashampoo.Translation.Systems.Formats.Abstractions;

namespace Ashampoo.Translation.Systems.Components.Services;

public interface IFileService
{
    /// <summary>
    /// Write the stream to a File.
    /// </summary>
    /// <param name="stream">
    /// The stream to write the file to.
    /// </param>
    /// <param name="fileName">
    /// The name of the file.
    /// </param>
    /// <param name="fileExtension">
    /// The extensions of the file.
    /// </param>
    /// <returns></returns>
    Task SaveFile(Stream stream, string fileName, string[]? fileExtension = null);
    
    /// <summary>
    /// Save multiple formats in a zip file.
    /// </summary>
    /// <param name="formats">
    /// The formats to save.
    /// </param>
    /// <param name="fileName">
    /// The name of the file to save.
    /// </param>
    /// <param name="fileExtensions">
    /// The file extensions for the format files.
    /// </param>
    /// <returns></returns>
    Task SaveFormatsAsync(IEnumerable<IFormat> formats, string fileName, string[]? fileExtensions = null);
}