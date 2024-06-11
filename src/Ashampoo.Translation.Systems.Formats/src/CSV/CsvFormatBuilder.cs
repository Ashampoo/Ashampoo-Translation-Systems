using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Models;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using CommunityToolkit.Diagnostics;

namespace Ashampoo.Translation.Systems.Formats.CSV;

/// <summary>
/// Builder for the <see cref="CsvFormat"/>.
/// </summary>
public sealed class CsvFormatBuilder : IFormatBuilderWithSourceAndTarget<CsvFormat>
{
    private Language _targetLanguage = Language.Empty;
    private Language _sourceLanguage = Language.Empty;
    private readonly Dictionary<string, (string, string)> _translations = new();
    private Dictionary<string, string> CustomHeaderInformation { get; set; } = new();

    /// <inheritdoc />
    public CsvFormat Build()
    {
        Guard.IsNotNullOrWhiteSpace(_targetLanguage.Value);

        CsvFormat format = new()
        {
            Header =
            {
                TargetLanguage = _targetLanguage,
                SourceLanguage = _sourceLanguage,
                AdditionalHeaders = CustomHeaderInformation
            }
        };

        foreach (var translation in _translations)
        {
            DefaultTranslationString sourceTranslationString = new(translation.Value.Item1, _targetLanguage, []);
            DefaultTranslationString targetTranslationString = new(translation.Value.Item2, _targetLanguage, []);
            DefaultTranslationUnit unit = new(translation.Key)
            {
                Translations =
                {
                    sourceTranslationString,
                    targetTranslationString
                }
            };
            format.TranslationUnits.Add(unit);
        }

        return format;
    }

    /// <inheritdoc />
    public void SetHeaderInformation(IFormatHeader header)
    {
        _sourceLanguage = header.SourceLanguage ?? new Language("en-US");
        _targetLanguage = header.TargetLanguage;
        CustomHeaderInformation = header.AdditionalHeaders;
    }

    /// <inheritdoc />
    public void AddHeaderInformation(string key, string value)
    {
        if (key is "delimiter")
        {
            if (CustomHeaderInformation.TryAdd(key, value)) return;
            CustomHeaderInformation.Remove(key);
            CustomHeaderInformation.Add(key, value);
        }
        else
        {
            throw new ArgumentException("Custom header only supports delimiter as a key!", nameof(key));
        }
    }

    /// <inheritdoc />
    public void SetTargetLanguage(Language language)
    {
        _targetLanguage = language;
    }

    /// <inheritdoc />
    public void Add(string id, string source, string target)
    {
        _translations.Add(id, (source, target));
    }

    /// <inheritdoc />
    public void SetSourceLanguage(Language language)
    {
        _sourceLanguage = language;
    }
}