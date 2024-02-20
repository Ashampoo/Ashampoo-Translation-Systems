namespace Ashampoo.Translation.Systems.Formats.Abstractions;

/// <summary>
/// Interface for a <see cref="IFormatBuilder"/> for formats that support two languages.
/// </summary>
public interface IFormatBuilderWithSourceAndTarget : IFormatBuilder
{
    /// <summary>
    /// Add a translation for the source and target language.
    /// </summary>
    /// <param name="id">
    /// The id of the translation.
    /// </param>
    /// <param name="source">
    /// The source translation.
    /// </param>
    /// <param name="target">
    /// The target translation.
    /// </param>
    void Add(string id, string source, string target);
    
    /// <summary>
    /// Set the source language.
    /// </summary>
    /// <param name="language">
    /// The language to set.
    /// </param>
    void SetSourceLanguage(string language);
}