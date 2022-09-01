using Ashampoo.Translation.Systems.Formats.Abstractions;

namespace Ashampoo.Translation.Systems.Components.Services;

public interface IFormatService
{
    /// <summary>
    /// Create a new instance of IFormat from the given stream.
    /// </summary>
    /// <param name="stream">
    /// The stream to read the format from.
    /// </param>
    /// <param name="fileName">
    /// The name of the file the stream is from.
    /// </param>
    /// <param name="options">
    /// The options to use when creating the format.
    /// </param>
    /// <returns> A new instance of IFormat</returns>
    Task<IFormat?> ReadFromStreamAsync(Stream stream, string fileName, FormatOptionsCallback? options = null);

    /// <summary>
    /// Convert the given instance of IFormat into the format specified by id.
    /// </summary>
    /// <param name="format">
    /// The format to convert.
    /// </param>
    /// <param name="convertFormatId">
    /// The id of the format to convert to.
    /// </param>
    /// <param name="formatOptionsCallback">
    /// The callback to use when the options are insufficient to convert the format.
    /// </param>
    /// <param name="options">
    /// The options to use when converting the format.
    /// </param>
    /// <returns>A new instance of IFormat</returns>
    IFormat ConvertTo(IFormat format, string convertFormatId, FormatOptionsCallback? formatOptionsCallback = null,
        AssignOptions? options = null);
    
    /// <summary>
    /// Convert the given instance of IFormat into the format specified by id.
    /// </summary>
    /// <param name="format">
    /// The format to convert.
    /// </param>
    /// <param name="convertFormatId">
    /// The id of the format to convert to.
    /// </param>
    /// <param name="formatOptionsCallback">
    /// The callback to use when the options are insufficient to convert the format.
    /// </param>
    /// <param name="options">
    /// The options to use when converting the format.
    /// </param>
    /// <returns>A new instance of IFormat</returns>
    Task<IFormat> ConvertToAsync(IFormat format, string convertFormatId,
        FormatOptionsCallback? formatOptionsCallback = null, AssignOptions? options = null);

    /// <summary>
    /// Configures the options specified in the CallbackOptions
    /// </summary>
    /// <param name="options">
    /// The options to configure.
    /// </param>
    /// <param name="title">
    /// The title of the options dialog.
    /// </param>
    /// <returns>A tuple, with Item1 = sourceLanguage, Item2 = targetLanguage</returns>
    public Task ConfigureFormatOptionsAsync(FormatOptions options, string title = "Configure Format Options");

    /// <summary>
    /// Displays a dialog to select an <see cref="IFormat"/>.
    /// </summary>
    /// <returns>
    /// A string containing the id of the selected format.
    /// </returns>
    Task<string?> GetFormatIdAsync();
    
    /// <summary>
    /// Switches the language of a format with a new one.
    /// </summary>
    /// <param name="format">
    /// The format to switch the language of.
    /// </param>
    /// <param name="oldLanguage">
    /// The old language to switch.
    /// </param>
    /// <param name="newLanguage">
    /// The new language to switch to.
    /// </param>
    /// <returns></returns>
    public Task SwitchLanguageAsync(IFormat format, string oldLanguage, string? newLanguage = null);
}