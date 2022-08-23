namespace Ashampoo.Translations.Formats.Abstractions;

public interface IFormatBuilderWithTarget : IFormatBuilder
{
    void Add(string id, string target);
    void SetTargetLanguage(string language);
}