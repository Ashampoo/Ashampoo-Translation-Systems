using Ashampoo.Translation.Systems.Formats.Abstractions.Models;

namespace Ashampoo.Translation.Systems.Formats.Abstractions;

/// <summary>
/// Interface for a builder for a <see cref="IFormat"/>.
/// </summary>
public interface IFormatBuilder<out T> where T : class, IFormat
{
    /// <summary>
    /// Builds the instance of <see cref="IFormat"/>.
    /// </summary>
    /// <returns>
    /// The instance of <see cref="IFormat"/>.
    /// </returns>
    T Build(IFormatBuilderOptions? options = null);
    
    /// <summary>
    /// Set the header information. All information will be added to the header and will overwrite
    /// existing information. All previous information will be removed.
    /// </summary>
    /// <param name="header">
    /// The <see cref="IFormatHeader"/> containing the information.
    /// </param>
    void SetHeaderInformation(IFormatHeader header);
    
    /// <summary>
    /// Add a header information. If the key already exists, the value will be overwritten.
    /// </summary>
    /// <param name="key">
    /// The key of the information.
    /// </param>
    /// <param name="value">
    /// The value of the information.
    /// </param>
    void AddHeaderInformation(string key, string value);
    
    /// <summary>
    /// Set the target language.
    /// </summary>
    /// <param name="language">
    /// The language to set.
    /// </param>
    void SetTargetLanguage(Language language);
}