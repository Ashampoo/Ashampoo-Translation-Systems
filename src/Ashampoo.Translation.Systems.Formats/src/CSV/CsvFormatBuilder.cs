using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Models;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using CommunityToolkit.Diagnostics;

namespace Ashampoo.Translation.Systems.Formats.CSV;

/// <summary>
/// Builder for the <see cref="CsvFormat"/>.
/// </summary>
public sealed class CsvFormatBuilder : IFormatBuilderWithSourceAndTarget<CsvFormat>
{
    private Language _targetLanguage = Language.Empty;
    private Language _sourceLanguage = Language.Empty;
    private char Delimiter { get; set; } = ';';
    private readonly Dictionary<string, (string, string)> _translations = new();
    private Dictionary<string, string> CustomHeaderInformation { get; set; } = new();

    /// <inheritdoc />
    public CsvFormat Build()
    {
        Guard.IsNotNullOrWhiteSpace(_targetLanguage.Value);

        CsvFormat format = new(new CsvFormatHeader
        {
            TargetLanguage = _targetLanguage,
            SourceLanguage = _sourceLanguage,
            AdditionalHeaders = CustomHeaderInformation,
            Delimiter = Delimiter
        });
        
        foreach (var translation in _translations)
        {
            DefaultTranslationString sourceTranslationString = new(translation.Value.Item1, _sourceLanguage, []);
            DefaultTranslationString targetTranslationString = new(translation.Value.Item2, _targetLanguage, []);
            DefaultTranslationUnit unit = new(translation.Key)
            {
                Translations =
                {
                    sourceTranslationString,
                    targetTranslationString
                }
            };
            format.TranslationUnits.Add(unit);
        }

        return format;
    }

    /// <inheritdoc />
    public void SetHeaderInformation(IFormatHeader header)
    {
        CustomHeaderInformation = header.AdditionalHeaders;
    }

    /// <inheritdoc />
    public void AddHeaderInformation(string key, string value)
    {
        if (key is "delimiter" && !string.IsNullOrWhiteSpace(value))
        {
            Delimiter = value.ToCharArray().First();
        }
    }

    /// <inheritdoc />
    public void SetTargetLanguage(Language language)
    {
        _targetLanguage = language;
    }

    /// <inheritdoc />
    public void Add(string id, string source, string target)
    {
        _translations.Add(id, (source, target));
    }

    /// <inheritdoc />
    public void SetSourceLanguage(Language language)
    {
        _sourceLanguage = language;
    }
}