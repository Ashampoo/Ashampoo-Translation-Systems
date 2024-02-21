using Ashampoo.Translation.Systems.Formats.Abstractions.Models;

namespace Ashampoo.Translation.Systems.Formats.Abstractions;

/// <summary>
/// This class is used to provide options for reading a <see cref="IFormat"/> from a file.
/// </summary>
public class FormatReadOptions
{
    /// <summary>
    /// The target language of the format.
    /// </summary>
    public Language TargetLanguage { get; init; } = Language.Empty;
    /// <summary>
    /// The source language of the format.
    /// </summary>
    public Language? SourceLanguage { get; init; }

    /// <summary>
    /// Indicates whether the read has been cancelled.
    /// </summary>
    public bool IsCancelled { get; set; }
    
    /// <summary>
    /// This callback is called, when options are insufficient to read a <see cref="IFormat"/>.
    /// </summary>
    public FormatOptionsCallback? FormatOptionsCallback { get; init; }
}