using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Models;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using CommunityToolkit.Diagnostics;

namespace Ashampoo.Translation.Systems.Formats.CSV;

internal sealed class CsvFormatBuilder : IFormatBuilderWithSourceAndTarget<CsvFormat>
{
    private Language _targetLanguage = Language.Empty;
    private Language _sourceLanguage = Language.Empty;
    private readonly Dictionary<string, (string, string)> _translations = new();
    private readonly Dictionary<string, string> _customHeaderInformation = new();
    
    public CsvFormat Build()
    {
        Guard.IsNotNullOrWhiteSpace(_targetLanguage.Value);

        CsvFormat format = new()
        {
            Header =
            {
                TargetLanguage = _targetLanguage,
                SourceLanguage = _sourceLanguage,
                AdditionalHeaders = _customHeaderInformation
            }
        };

        foreach (var translation in _translations)
        {
            DefaultTranslationString sourceTranslationString = new(translation.Value.Item1, _targetLanguage, []);
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

    public void SetHeaderInformation(IFormatHeader header)
    {
        throw new NotImplementedException();
    }

    public void AddHeaderInformation(string key, string value)
    {
        if (key is "delimiter")
        {
            if (_customHeaderInformation.TryAdd(key, value)) return;
            _customHeaderInformation.Remove(key);
            _customHeaderInformation.Add(key, value);
        }
        else
        {
            throw new ArgumentException("Custom header only supports delimiter as a key!", nameof(key));
        }
    }

    public void SetTargetLanguage(Language language)
    {
        _targetLanguage = language;
    }

    public void Add(string id, string source, string target)
    {
        _translations.Add(id, (source, target));
    }

    public void SetSourceLanguage(Language language)
    {
        _sourceLanguage = language;
    }
}