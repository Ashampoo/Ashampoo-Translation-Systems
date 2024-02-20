using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using Microsoft.Toolkit.Diagnostics;

namespace Ashampoo.Translation.Systems.Formats.Gengo;

/// <summary>
/// Builder for the <see cref="GengoFormat"/>.
/// </summary>
public class GengoFormatBuilder : IFormatBuilderWithSourceAndTarget
{
    private string? sourceLanguage;
    private string? targetLanguage;
    private readonly Dictionary<string, (string, string)> translations = new();

    /// <inheritdoc />
    public void Add(string id, string source, string target)
    {
        translations.Add(id, (source, target));
    }

    /// <inheritdoc />
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
                new DefaultTranslationString(keyValuePair.Key, keyValuePair.Value.Item1,
                    sourceLanguage); //Create new translation string
            var targetTranslationString =
                new DefaultTranslationString(keyValuePair.Key, keyValuePair.Value.Item2,
                    targetLanguage); //Create new translation string

            var translationUnit = new DefaultTranslationUnit(keyValuePair.Key) //Create new translation unit
            {
                Translations =
                {
                    sourceTranslationString,
                    targetTranslationString
                }
            };

            gengoFormat.Add(translationUnit); //Add translation unit to format
        }

        return gengoFormat;
    }

    /// <inheritdoc />
    public void SetSourceLanguage(string language)
    {
        sourceLanguage = language;
    }

    /// <inheritdoc />
    public void SetTargetLanguage(string language)
    {
        targetLanguage = language;
    }
    
    /// <summary>
    /// This method is not supported because <see cref="GengoFormat"/> does not support header information,
    /// it will do nothing.
    /// </summary>
    /// <param name="header">
    /// The <see cref="IFormatHeader"/> containing the information.
    /// </param>
    public void SetHeaderInformation(IFormatHeader header)
    {
        // Do nothing, Gengo does not support header information
    }

    /// <summary>
    /// This method is not supported because <see cref="GengoFormat"/> does not support header information,
    /// it will do nothing.
    /// </summary>
    /// <param name="key">
    /// The key of the header information.
    /// </param>
    /// <param name="value">
    /// The value of the header information.
    /// </param>
    public void AddHeaderInformation(string key, string value)
    {
        // Do nothing, Gengo does not support header information
    }
}