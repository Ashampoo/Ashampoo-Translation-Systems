using Ashampoo.Translation.Systems.Formats.Abstractions.TranslationFilter;

namespace Ashampoo.Translation.Systems.Formats.Abstractions;

/// <summary>
/// This class is used to store options when converting an implementation of <see cref="IFormat"/> into another implementation:
/// </summary>
public class AssignOptions
{
    /// <summary>
    /// A <see cref="ITranslationFilter"/> that is used to filter out translations while converting formats.
    /// </summary>
    public ITranslationFilter? Filter { get; set; }
    
    /// <summary>
    /// Specifies what language should be used for the source language while converting between formats.
    /// </summary>
    public string? SourceLanguage { get; set; }
    
    /// <summary>
    /// Specifies what language should be used for the target language while converting between formats.
    /// </summary>
    public string? TargetLanguage { get; set; }
    
    /// <summary>
    /// This callback is used by the converting format, when options are insufficient.
    /// </summary>
    public FormatOptionsCallback? FormatOptionsCallback { get; init; }
}