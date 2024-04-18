using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Models;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using Ashampoo.Translation.Systems.Formats.ResX.Elements;
using CommunityToolkit.Diagnostics;

namespace Ashampoo.Translation.Systems.Formats.ResX;

/// <summary>
/// Implementation of the <see cref="IFormatBuilderWithTarget{T}"/> interface for the ResX format.
/// </summary>
public class ResXFormatBuilder : IFormatBuilderWithTarget<ResXFormat>
{
    private Language? _targetLanguage;
    private readonly Dictionary<string, string> _translations = new();

    /// <inheritdoc />
    public ResXFormat Build()
    {
        Guard.IsNotNullOrWhiteSpace(_targetLanguage?.Value, nameof(_targetLanguage));

        var format = new ResXFormat
        {
            Header =
            {
                TargetLanguage = (Language)_targetLanguage!
            },
            XmlRoot =
            {
                Data = new List<Data>()
            }
        };

        foreach (var (id, value) in _translations)
        {
            var data = new Data
            {
                Name = id,
                Value = value
            };
            format.XmlRoot.Data.Add(data);

            var translationString = new DefaultTranslationString(value, (Language)_targetLanguage, []);
            var translationUnit = new DefaultTranslationUnit(id)
            {
                Translations =
                {
                    translationString
                }
            };
            format.TranslationUnits.Add(translationUnit);
        }

        return format;
    }

    /// <inheritdoc />
    public void Add(string id, string target)
    {
        _translations.Add(id, target);
    }

    /// <inheritdoc />
    public void SetTargetLanguage(Language language)
    {
        _targetLanguage = language;
    }
    
    /// <summary>
    /// This method does nothing, because ResXFormat does not support header information.
    /// </summary>
    /// <param name="header"></param>
    public void SetHeaderInformation(IFormatHeader header)
    {
        // Do nothing, ResXFormat does not support header information
    }

    /// <summary>
    /// This method does nothing, because ResXFormat does not support header information.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void AddHeaderInformation(string key, string value)
    {
        // Do nothing, ResXFormat does not support header information
    }
}