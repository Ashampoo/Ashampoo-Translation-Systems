namespace Ashampoo.Translation.Systems.Formats.Abstractions;

/// <summary>
/// This class is used to provide options for reading a <see cref="IFormat"/> from a file.
/// </summary>
public class FormatReadOptions
{
    public string? TargetLanguage { get; init; }
    public string? SourceLanguage { get; init; }

    public bool IsCancelled { get; set; }
    
    /// <summary>
    /// This callback is called, when options are insufficient to read a <see cref="IFormat"/>.
    /// </summary>
    public FormatOptionsCallback? FormatOptionsCallback { get; init; }
}