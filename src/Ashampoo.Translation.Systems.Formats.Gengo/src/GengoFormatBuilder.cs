using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using Microsoft.Toolkit.Diagnostics;

namespace Ashampoo.Translation.Systems.Formats.Gengo;

/// <summary>
/// Builder for the <see cref="GengoFormat"/>.
/// </summary>
public class GengoFormatBuilder : IFormatBuilderWithSourceAndTarget
{
    private string? _sourceLanguage;
    private string? _targetLanguage;
    private readonly Dictionary<string, (string, string)> _translations = new();

    /// <inheritdoc />
    public void Add(string id, string source, string target)
    {
        _translations.Add(id, (source, target));
    }

    /// <inheritdoc />
    public IFormat Build()
    {
        Guard.IsNotNullOrWhiteSpace(_sourceLanguage, nameof(_sourceLanguage));
        Guard.IsNotNullOrWhiteSpace(_targetLanguage, nameof(_targetLanguage));


        //Create new Gengo format and add translations
        var gengoFormat = new GengoFormat
        {
            Header =
            {
                SourceLanguage = _sourceLanguage,
                TargetLanguage = _targetLanguage
            }
        };

        // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
        foreach (var keyValuePair in _translations)
        {
            var sourceTranslationString =
                new DefaultTranslationString(keyValuePair.Key, keyValuePair.Value.Item1,
                    _sourceLanguage); //Create new translation string
            var targetTranslationString =
                new DefaultTranslationString(keyValuePair.Key, keyValuePair.Value.Item2,
                    _targetLanguage); //Create new translation string

            var translationUnit = new DefaultTranslationUnit(keyValuePair.Key) //Create new translation unit
            {
                Translations =
                {
                    sourceTranslationString,
                    targetTranslationString
                }
            };

            gengoFormat.TranslationUnits.Add(translationUnit); //Add translation unit to format
        }

        return gengoFormat;
    }

    /// <inheritdoc />
    public void SetSourceLanguage(string language)
    {
        _sourceLanguage = language;
    }

    /// <inheritdoc />
    public void SetTargetLanguage(string language)
    {
        _targetLanguage = language;
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