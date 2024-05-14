using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Models;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using CommunityToolkit.Diagnostics;

namespace Ashampoo.Translation.Systems.Formats.CSV;

internal sealed class CsvFormatBuilder : IFormatBuilderWithTarget<CsvFormat>
{
    private Language _targetLanguage = Language.Empty;
    private readonly Dictionary<string, string> _translations = new();
    
    public CsvFormat Build()
    {
        Guard.IsNotNullOrWhiteSpace(_targetLanguage.Value);

        CsvFormat format = new()
        {
            Header =
            {
                TargetLanguage = _targetLanguage
            }
        };

        foreach (var translation in _translations)
        {
            DefaultTranslationUnit unit = new(translation.Key);
            DefaultTranslationString translationString = new(translation.Value, _targetLanguage, []);
            unit.Translations.Add(translationString);
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
        throw new NotImplementedException();
    }

    public void SetTargetLanguage(Language language)
    {
        _targetLanguage = language;
    }

    public void Add(string id, string target)
    {
        _translations.Add(id, target);
    }
}