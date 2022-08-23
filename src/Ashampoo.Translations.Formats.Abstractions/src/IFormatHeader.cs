namespace Ashampoo.Translations.Formats.Abstractions;

public interface IFormatHeader : IDictionary<string, string>
{
    string TargetLanguage { get; set; }
    string? SourceLanguage { get; set; }
}