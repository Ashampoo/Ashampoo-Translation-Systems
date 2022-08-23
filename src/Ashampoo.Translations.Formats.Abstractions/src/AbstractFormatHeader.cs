namespace Ashampoo.Translations.Formats.Abstractions;

/// <summary>
/// Abstract base class for the <see cref="IFormatHeader"/>
/// </summary>
public abstract class AbstractFormatHeader : Dictionary<string, string>, IFormatHeader
{
    public abstract string TargetLanguage { get; set; }
    public abstract string? SourceLanguage { get; set; }
}