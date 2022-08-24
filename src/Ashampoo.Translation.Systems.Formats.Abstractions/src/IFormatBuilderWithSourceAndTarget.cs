namespace Ashampoo.Translation.Systems.Formats.Abstractions;

public interface IFormatBuilderWithSourceAndTarget : IFormatBuilder
{
    void Add(string id, string source, string target);
    void SetSourceLanguage(string language);
    void SetTargetLanguage(string language);
}