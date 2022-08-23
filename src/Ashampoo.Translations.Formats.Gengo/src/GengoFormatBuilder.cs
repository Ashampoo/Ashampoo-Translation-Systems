using Ashampoo.Translations.Formats.Abstractions;
using Ashampoo.Translations.Formats.Abstractions.Translation;
using Microsoft.Toolkit.Diagnostics;

namespace Ashampoo.Translations.Formats.Gengo;

public class GengoFormatBuilder : IFormatBuilderWithSourceAndTarget
{
    private string? sourceLanguage;
    private string? targetLanguage;
    private readonly Dictionary<string, (string, string)> translations = new();

    public void Add(string id, string source, string target)
    {
        translations.Add(id, (source, target));
    }

    public IFormat Build()
    {
        Guard.IsNotNullOrWhiteSpace(sourceLanguage, nameof(sourceLanguage));
        Guard.IsNotNullOrWhiteSpace(targetLanguage, nameof(targetLanguage));


        //Create new Gengo format and add translations
        var gengoFormat = new GengoFormat
        {
            Header =
            {
                SourceLanguage = sourceLanguage,
                TargetLanguage = targetLanguage
            }
        };

        // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
        foreach (var keyValuePair in translations)
        {
            var sourceTranslationString =
                new DefaultTranslationString(keyValuePair.Key, keyValuePair.Value.Item1, sourceLanguage); //Create new translation string
            var targetTranslationString =
                new DefaultTranslationString(keyValuePair.Key, keyValuePair.Value.Item2, targetLanguage); //Create new translation string

            var translationUnit = new DefaultTranslationUnit(keyValuePair.Key) //Create new translation unit
            {
                sourceTranslationString,
                targetTranslationString
            };

            gengoFormat.Add(translationUnit); //Add translation unit to format
        }

        return gengoFormat;
    }

    public void SetSourceLanguage(string language)
    {
        sourceLanguage = language;
    }

    public void SetTargetLanguage(string language)
    {
        targetLanguage = language;
    }
}