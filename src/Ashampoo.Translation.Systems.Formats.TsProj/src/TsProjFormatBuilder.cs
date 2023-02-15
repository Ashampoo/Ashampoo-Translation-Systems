using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using Ashampoo.Translation.Systems.Formats.TsProj.Element;
using CommunityToolkit.Diagnostics;

namespace Ashampoo.Translation.Systems.Formats.TsProj;

/// <summary>
/// Implementation of the <see cref="IFormatBuilderWithSourceAndTarget"/> interface for the <see cref="TsProj"/> format.
/// </summary>
public class TsProjFormatBuilder : IFormatBuilderWithSourceAndTarget
{
    private string? sourceLanguage;
    private string? targetLanguage;
    private readonly Dictionary<string, (string, string)> translations = new();
    private Dictionary<string, string> information = new();

    /// <inheritdoc />
    public void Add(string id, string source, string target)
    {
        translations.Add(id, (source, target));
    }

    /// <inheritdoc />
    public IFormat Build()
    {
        Guard.IsNotNullOrWhiteSpace(sourceLanguage, nameof(sourceLanguage)); // sourceLanguage is required
        Guard.IsNotNullOrWhiteSpace(targetLanguage, nameof(targetLanguage)); // targetLanguage is required

        //Create new TsProj format and add translations
        var tsProjFormat = new TsProjFormat();
        var project = tsProjFormat.Project;

        project.SourceLanguage = sourceLanguage; // Set source language for xml object
        project.TargetLanguage = targetLanguage; // Set target language for xml object
        tsProjFormat.Header.SourceLanguage = sourceLanguage; // Set source language for format object
        tsProjFormat.Header.TargetLanguage = targetLanguage; // Set target language for format object
        
        // Add information to header
        var nameFound = information.TryGetValue("Name", out var name);
        var versionFound = information.TryGetValue("Version", out var version);
        var authorFound = information.TryGetValue("Author", out var author);
        var mailFound = information.TryGetValue("Mail", out var mail);
        var creationToolFound = information.TryGetValue("CreationTool", out var creationTool);
        var creationToolVersionFound = information.TryGetValue("CreationToolVersion", out var creationToolVersion);
        var countryNameFound = information.TryGetValue("CountryName", out var countryName);
        
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
        project.Components = new List<Component> { component }; // Add component to project

        foreach (var keyValuePair in translations)
        {
            var translationElement = new Element.Translation // Create new translation element for xml parsing
            {
                Id = keyValuePair.Key,
                Source = keyValuePair.Value.Item1,
                Value = keyValuePair.Value.Item2
            };
            component.Translations.Add(translationElement);

            var sourceTranslationString = new TranslationStringSource(translationElement) { Language = sourceLanguage }; // Create new source translation string
            var targetTranslationString = new TranslationStringTarget(translationElement) { Language = targetLanguage }; // Create new target translation string

            var translationUnit = new DefaultTranslationUnit(keyValuePair.Key) // Create new translation unit
            {
                sourceTranslationString,
                targetTranslationString
            };

            tsProjFormat.Add(translationUnit);
        }

        return tsProjFormat;
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
    
    /// <inheritdoc />
    public void SetHeaderInformation(IFormatHeader header)
    {
        information = new Dictionary<string, string>(header);
    }

    /// <inheritdoc />
    public void AddHeaderInformation(string key, string value)
    {
        information.Add(key, value);
    }
}