using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Models;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using CommunityToolkit.Diagnostics;

namespace Ashampoo.Translation.Systems.Formats.JavaProperties;

public class JavaPropertiesBuilder : IFormatBuilderWithTarget
{
    private Language _targetLanguage = Language.Empty;
    private readonly Dictionary<string, string> _translations = new();
    
    /// <inheritdoc/>
    public IFormat Build()
    {
        Guard.IsNotNullOrWhiteSpace(_targetLanguage.Value);

        JavaPropertiesFormat format = new()
        {
            Header =
            {
                TargetLanguage = _targetLanguage
            }
        };

        foreach (var translation in _translations)
        {
            DefaultTranslationUnit unit = new(translation.Key);
            DefaultTranslationString translationString = new(translation.Key, translation.Value, _targetLanguage);
            unit.Translations.Add(translationString);
            format.TranslationUnits.Add(unit);
        }

        return format;
    }
    
    /// <inheritdoc/>
    public void Add(string id, string target)
    {
        _translations.Add(id, target);
    }

    /// <inheritdoc/>
    public void SetTargetLanguage(Language language)
    {
        _targetLanguage = language;
    }

    /// <summary>
    /// This is not supported by the JavaProperties format
    /// </summary>
    /// <param name="header"></param>
    /// <exception cref="NotSupportedException"></exception>
    public void SetHeaderInformation(IFormatHeader header)
    {
        throw new NotSupportedException("Header information's are not supported by the JavaProperties format");
    }

    /// <summary>
    /// This is not supported by the JavaProperties format
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <exception cref="NotSupportedException"></exception>
    public void AddHeaderInformation(string key, string value)
    {
        throw new NotSupportedException("Header information's are not supported by the JavaProperties format");
    }
}