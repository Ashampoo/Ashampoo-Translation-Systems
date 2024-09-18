using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Models;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using Ashampoo.Translation.Systems.Formats.AshLang.Chunk;
using CommunityToolkit.Diagnostics;

namespace Ashampoo.Translation.Systems.Formats.AshLang;

/// <summary>
/// Builder for <see cref="AshLangFormat"/>
/// </summary>
public class AshLangFormatBuilder : IFormatBuilderWithSourceAndTarget<AshLangFormat>
{
    private static readonly Language SourceLanguage = new("en-US"); // AshLang source is always in English
    private Language _targetLanguage = Language.Empty;
    private readonly Dictionary<string, (string source, string target)> _translations = new();
    private Dictionary<string, string> _information = new();

    /// <inheritdoc />
    public AshLangFormat Build(IFormatBuilderOptions? options = null)
    {
        Guard.IsNotNullOrWhiteSpace(_targetLanguage.Value, nameof(_targetLanguage));

        var ashLang = new AshLangFormat
        {
            Header =
            {
                TargetLanguage = _targetLanguage
            }
        };

        var appIdChunk = ashLang.Chunks.OfType<AppIdChunk>().FirstOrDefault();
        if (appIdChunk is not null && _information.TryGetValue("Name", out var name))
        {
            appIdChunk.Name = name;
            _information.Remove("Name");
        }

        var translationChunk = (TranslationChunk)ashLang.Chunks.Last();

        foreach (var (id, (source, target)) in _translations)
        {
            var translation = new TranslationChunk.Translation(0, id, target, source, "");
            translationChunk.Translations.Add(translation);

            var translationUnit = new DefaultTranslationUnit(id)
            {
                Translations =
                {
                    new SourceTranslationString(SourceLanguage, translation),
                    new TargetTranslationString(_targetLanguage, translation)
                }
            };
            ashLang.TranslationUnits.Add(translationUnit);
        }


        return ashLang;
    }

    /// <inheritdoc />
    public void Add(string id, string source, string target)
    {
        _translations.Add(id, (source, target));
    }

    /// <inheritdoc />
    public void SetSourceLanguage(Language language)
    {
        // AshLang source is always in English
    }

    /// <inheritdoc />
    public void SetTargetLanguage(Language language)
    {
        _targetLanguage = language;
    }

    /// <inheritdoc />
    public void SetHeaderInformation(IFormatHeader header)
    {
        _information = new Dictionary<string, string>(header.AdditionalHeaders);
    }

    /// <inheritdoc />
    public void AddHeaderInformation(string key, string value)
    {
        _information.Add(key, value);
    }
}