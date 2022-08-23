using Ashampoo.Translations.Formats.Abstractions;
using Ashampoo.Translations.Formats.Abstractions.Translation;
using Microsoft.Toolkit.Diagnostics;

namespace Ashampoo.Translations.Formats.Json;

public class JsonFormatBuilder : IFormatBuilderWithTarget
{
    private string? targetLanguage;
    private readonly Dictionary<string, string> translations = new();

    public void Add(string id, string target)
    {
        translations.Add(id, target);
    }

    public IFormat Build()
    {
        Guard.IsNotNullOrWhiteSpace(targetLanguage, nameof(targetLanguage));

        //create new json format and add translations
        var jsonFormat = new JsonFormat
        {
            Header =
            {
                TargetLanguage = targetLanguage
            }
        };

        foreach (var translation in translations)
        {
            var translationUnit = new DefaultTranslationUnit(translation.Key);
            var translationString = new DefaultTranslationString(translation.Key, translation.Value, targetLanguage);
            translationUnit.Add(translationString);
            jsonFormat.Add(translationUnit);
        }

        return jsonFormat;
    }

    public void SetTargetLanguage(string language)
    {
        targetLanguage = language;
    }
}