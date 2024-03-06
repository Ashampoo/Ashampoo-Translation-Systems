using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Models;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using CommunityToolkit.Diagnostics;

namespace Ashampoo.Translation.Systems.Formats.QT;

public class QTFormatBuilder : IFormatBuilderWithTarget<QTFormat>
{
    private Language _targetLanguage = Language.Empty;
    private readonly Dictionary<string, string> _translations = new();

    /// <inheritdoc />
    public QTFormat Build()
    {
        Guard.IsNotNullOrWhiteSpace(_targetLanguage.Value);
        
        var format = new QTFormat()
        {
            Header =
            {
                TargetLanguage = _targetLanguage
            }
        };

        foreach (var (id, value) in _translations)
        {
            var unit = new DefaultTranslationUnit(id);
            var translation = new DefaultTranslationString(value, _targetLanguage, []);
            unit.Translations.Add(translation);
            format.TranslationUnits.Add(unit);
        }

        return format;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="header"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void SetHeaderInformation(IFormatHeader header)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void AddHeaderInformation(string key, string value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public void SetTargetLanguage(Language language)
    {
        _targetLanguage = language;
    }

    /// <inheritdoc />
    public void Add(string id, string target)
    {
        _translations.TryAdd(id, target);
    }
}