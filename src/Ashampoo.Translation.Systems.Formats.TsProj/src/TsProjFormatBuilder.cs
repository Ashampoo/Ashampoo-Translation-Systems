using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Models;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using Ashampoo.Translation.Systems.Formats.TsProj.Element;
using CommunityToolkit.Diagnostics;

namespace Ashampoo.Translation.Systems.Formats.TsProj;

/// <summary>
/// Implementation of the <see cref="IFormatBuilderWithSourceAndTarget"/> interface for the <see cref="TsProj"/> format.
/// </summary>
public class TsProjFormatBuilder : IFormatBuilderWithSourceAndTarget
{
    private Language? _sourceLanguage;
    private Language? _targetLanguage;
    private readonly Dictionary<string, (string, string)> _translations = new();
    private Dictionary<string, string> _information = new();

    /// <inheritdoc />
    public void Add(string id, string source, string target)
    {
        _translations.Add(id, (source, target));
    }

    /// <inheritdoc />
    public IFormat Build()
    {
        Guard.IsNotNullOrWhiteSpace(_sourceLanguage.ToString(), nameof(_sourceLanguage)); // sourceLanguage is required
        Guard.IsNotNullOrWhiteSpace(_targetLanguage.ToString(), nameof(_targetLanguage)); // targetLanguage is required

        //Create new TsProj format and add translations
        var tsProjFormat = new TsProjFormat();
        var project = tsProjFormat.Project;

        project.SourceLanguage = _sourceLanguage.ToString(); // Set source language for xml object
        project.TargetLanguage = _targetLanguage.ToString()!; // Set target language for xml object
        tsProjFormat.Header.SourceLanguage = _sourceLanguage; // Set source language for format object
        tsProjFormat.Header.TargetLanguage = (Language)_targetLanguage!; // Set target language for format object
        
        // Add information to header
        var nameFound = _information.TryGetValue("Name", out var name);
        var versionFound = _information.TryGetValue("Version", out var version);
        var authorFound = _information.TryGetValue("Author", out var author);
        var mailFound = _information.TryGetValue("Mail", out var mail);
        var creationToolFound = _information.TryGetValue("CreationTool", out var creationTool);
        var creationToolVersionFound = _information.TryGetValue("CreationToolVersion", out var creationToolVersion);
        var countryNameFound = _information.TryGetValue("CountryName", out var countryName);
        
        if (nameFound) project.Name = name;
        if (versionFound) project.Version = version;
        if (authorFound) project.Author = author;
        if (mailFound) project.Mail = mail;
        if (creationToolFound) project.CreationTool = creationTool;
        if (creationToolVersionFound) project.CreationToolVersion = creationToolVersion;
        if (countryNameFound) project.CountryName = countryName;

        var component = new Component
        {
            PluginGuid = "F0D8F625-2EE3-4C84-96EC-BFBDD4946878", // Guid for the TsProj format
            Translations = new List<Element.Translation>()
        };
        project.Components = [component]; // Add component to project

        foreach (var keyValuePair in _translations)
        {
            var translationElement = new Element.Translation // Create new translation element for xml parsing
            {
                Id = keyValuePair.Key,
                Source = keyValuePair.Value.Item1,
                Value = keyValuePair.Value.Item2
            };
            component.Translations.Add(translationElement);

            var sourceTranslationString = new TranslationStringSource(translationElement) { Language = (Language)_sourceLanguage! }; // Create new source translation string
            var targetTranslationString = new TranslationStringTarget(translationElement) { Language = (Language)_targetLanguage }; // Create new target translation string

            var translationUnit = new DefaultTranslationUnit(keyValuePair.Key) // Create new translation unit
            {
                Translations =
                {
                    sourceTranslationString,
                    targetTranslationString
                }
            };

            tsProjFormat.TranslationUnits.Add(translationUnit);
        }

        return tsProjFormat;
    }

    /// <inheritdoc />
    public void SetSourceLanguage(Language language)
    {
        _sourceLanguage = language;
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