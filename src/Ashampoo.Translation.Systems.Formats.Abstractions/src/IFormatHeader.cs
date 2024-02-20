using Ashampoo.Translation.Systems.Formats.Abstractions.Models;

namespace Ashampoo.Translation.Systems.Formats.Abstractions;

/// <summary>
/// Interface for a header containing information about an <see cref="IFormat"/>.
/// </summary>
public interface IFormatHeader
{
    /// <summary>
    /// The target language of the <see cref="IFormat"/>.
    /// </summary>
    Language TargetLanguage { get; set; }
    
    /// <summary>
    /// The source language of the <see cref="IFormat"/>.
    /// </summary>
    Language? SourceLanguage { get; set; }
    
    /// <summary>
    /// Gets or sets the additional headers associated with the format.
    /// </summary>
    Dictionary<string, string> AdditionalHeaders { get; set; }
}