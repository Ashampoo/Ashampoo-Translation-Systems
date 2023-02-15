using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using CommunityToolkit.Diagnostics;

namespace Ashampoo.Translation.Systems.Formats.Json;

/// <summary>
/// Builder for the <see cref="JsonFormat"/>.
/// </summary>
public class JsonFormatBuilder : IFormatBuilderWithTarget
{
    private string? targetLanguage;
    private readonly Dictionary<string, string> translations = new();

    /// <inheritdoc />
    public void Add(string id, string target)
    {
        translations.Add(id, target);
    }

    /// <inheritdoc />
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
    public void SetTargetLanguage(string language)
    {
        targetLanguage = language;
    }
}