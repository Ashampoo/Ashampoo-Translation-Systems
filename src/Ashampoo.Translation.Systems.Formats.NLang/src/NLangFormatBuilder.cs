using Ashampoo.Translation.Systems.Formats.Abstractions;
using CommunityToolkit.Diagnostics;

namespace Ashampoo.Translation.Systems.Formats.NLang;

/// <summary>
/// Builder for <see cref="NLangFormat"/>.
/// </summary>
public class NLangFormatBuilder : IFormatBuilderWithTarget
{
    private string? _targetLanguage;
    private readonly Dictionary<string, string> _translations = new();

    /// <inheritdoc />
    public void Add(string id, string target)
    {
        _translations.Add(id, target);
    }

    /// <inheritdoc />
    public IFormat Build()
    {
        Guard.IsNotNullOrWhiteSpace(_targetLanguage, nameof(_targetLanguage));

        //Create new NLang format and add translations
        var nLangFormat = new NLangFormat
        {
            Header =
            {
                TargetLanguage = _targetLanguage
            }
        };

        foreach (var translation in _translations)
        {
            var translationUnit = new TranslationUnit(translation.Key);
            var translationString = new TranslationString(translation.Key, translation.Value, _targetLanguage);
            translationUnit.Translations.Add(translationString);
            nLangFormat.TranslationUnits.Add(translationUnit);
        }

        return nLangFormat;
    }

    /// <inheritdoc />
    public void SetTargetLanguage(string language)
    {
        _targetLanguage = language;
    }
    
    /// <summary>
    /// This method does nothing, because NLangFormat does not support header information.
    /// </summary>
    /// <param name="header"></param>
    public void SetHeaderInformation(IFormatHeader header)
    {
        // Do nothing, NLangFormat does not support header information
    }

    /// <summary>
    /// This method does nothing, because NLangFormat does not support header information.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void AddHeaderInformation(string key, string value)
    {
        // Do nothing, NLangFormat does not support header information
    }
}