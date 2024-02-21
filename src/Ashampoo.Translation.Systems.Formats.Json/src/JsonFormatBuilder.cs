using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Models;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using CommunityToolkit.Diagnostics;

namespace Ashampoo.Translation.Systems.Formats.Json;

/// <summary>
/// Builder for the <see cref="JsonFormat"/>.
/// </summary>
public class JsonFormatBuilder : IFormatBuilderWithTarget
{
    private Language? _targetLanguage;
    private readonly Dictionary<string, string> _translations = new();

    /// <inheritdoc />
    public void Add(string id, string target)
    {
        _translations.Add(id, target);
    }

    /// <inheritdoc />
    public IFormat Build()
    {
        Guard.IsNotNullOrWhiteSpace(_targetLanguage.ToString(), nameof(_targetLanguage));

        //create new json format and add translations
        var jsonFormat = new JsonFormat
        {
            Header =
            {
                TargetLanguage = (Language)_targetLanguage!
            }
        };

        foreach (var translation in _translations)
        {
            var translationUnit = new DefaultTranslationUnit(translation.Key);
            var translationString = new DefaultTranslationString(translation.Key, translation.Value, (Language)_targetLanguage);
            translationUnit.Translations.Add(translationString);
            jsonFormat.TranslationUnits.Add(translationUnit);
        }

        return jsonFormat;
    }

    /// <summary>
    /// This method does nothing, because JsonFormat does not support header information.
    /// </summary>
    /// <param name="header"></param>
    public void SetHeaderInformation(IFormatHeader header)
    {
        // Do nothing, JsonFormat does not support header information
    }

    /// <summary>
    /// This method does nothing, because JsonFormat does not support header information.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void AddHeaderInformation(string key, string value)
    {
        // Do nothing, JsonFormat does not support header information
    }

    /// <inheritdoc />
    public void SetTargetLanguage(Language language)
    {
        _targetLanguage = language;
    }
}