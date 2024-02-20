using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using Ashampoo.Translation.Systems.Formats.ResX.Elements;
using CommunityToolkit.Diagnostics;

namespace Ashampoo.Translation.Systems.Formats.ResX;

/// <summary>
/// Implementation of the <see cref="IFormatBuilderWithTarget"/> interface for the ResX format.
/// </summary>
public class ResXFormatBuilder : IFormatBuilderWithTarget
{
    private string? targetLanguage;
    private readonly Dictionary<string, string> translations = new();

    /// <inheritdoc />
    public IFormat Build()
    {
        Guard.IsNotNullOrWhiteSpace(targetLanguage, nameof(targetLanguage));

        var format = new ResXFormat
        {
            Header =
            {
                TargetLanguage = targetLanguage
            },
            XmlRoot =
            {
                Data = new List<Data>()
            }
        };

        foreach (var (id, value) in translations)
        {
            var data = new Data
            {
                Name = id,
                Value = value
            };
            format.XmlRoot.Data.Add(data);

            var translationString = new DefaultTranslationString(id, value, targetLanguage);
            var translationUnit = new DefaultTranslationUnit(id)
            {
                Translations =
                {
                    translationString
                }
            };
            format.Add(translationUnit);
        }

        return format;
    }

    /// <inheritdoc />
    public void Add(string id, string target)
    {
        translations.Add(id, target);
    }

    /// <inheritdoc />
    public void SetTargetLanguage(string language)
    {
        targetLanguage = language;
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