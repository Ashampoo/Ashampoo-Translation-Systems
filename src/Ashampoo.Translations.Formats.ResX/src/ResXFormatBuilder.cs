using Ashampoo.Translations.Formats.Abstractions;
using Ashampoo.Translations.Formats.Abstractions.Translation;
using Ashampoo.Translations.Formats.ResX.Elements;
using Microsoft.Toolkit.Diagnostics;

namespace Ashampoo.Translations.Formats.ResX;

/// <summary>
/// Implementation of the <see cref="IFormatBuilderWithTarget"/> interface for the ResX format.
/// </summary>
public class ResXFormatBuilder : IFormatBuilderWithTarget
{
    private string? targetLanguage;
    private readonly Dictionary<string, string> translations = new();

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
            var translationUnit = new DefaultTranslationUnit(id) { translationString };
            format.Add(translationUnit);
        }

        return format;
    }

    public void Add(string id, string target)
    {
        translations.Add(id, target);
    }

    public void SetTargetLanguage(string language)
    {
        targetLanguage = language;
    }
}