using Ashampoo.Translations.Formats.Abstractions;
using CommunityToolkit.Diagnostics;

namespace Ashampoo.Translations.Formats.NLang;

public class NLangFormatBuilder : IFormatBuilderWithTarget
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

        //Create new NLang format and add translations
        var nLangFormat = new NLangFormat
        {
            Header =
            {
                TargetLanguage = targetLanguage
            }
        };

        foreach (var translation in translations)
        {
            var translationUnit = new TranslationUnit(translation.Key);
            var translationString = new TranslationString(translation.Key, translation.Value, targetLanguage);
            translationUnit.Add(translationString);
            nLangFormat.Add(translationUnit);
        }

        return nLangFormat;
    }

    public void SetTargetLanguage(string language)
    {
        targetLanguage = language;
    }
}