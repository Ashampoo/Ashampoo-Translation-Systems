namespace Ashampoo.Translation.Systems.Formats.Abstractions;

/// <summary>
/// Interface for a <see cref="IFormatBuilder"/> for formats that only support one language.
/// </summary>
public interface IFormatBuilderWithTarget : IFormatBuilder
{
    /// <summary>
    /// Add a translation for the target language.
    /// </summary>
    /// <param name="id">
    /// The id of the translation.
    /// </param>
    /// <param name="target">
    /// The translation.
    /// </param>
    void Add(string id, string target);
}