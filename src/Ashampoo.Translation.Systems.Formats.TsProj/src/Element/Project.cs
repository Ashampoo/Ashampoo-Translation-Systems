using System.Xml.Serialization;

namespace Ashampoo.Translation.Systems.Formats.TsProj.Element;

/// <summary>
/// Represents a project element in a tsproj file.
/// </summary>
[XmlRoot("project")]
public class Project
{
    /// <summary>
    /// Gets or sets the version of the file.
    /// </summary>
    [XmlAttribute("tsversion")] public int FormatVersion = 7;

    /// <summary>
    /// Gets or sets the target language of the format.
    /// </summary>
    [XmlAttribute("target_language")] public string TargetLanguage = "";

    /// <summary>
    /// Gets or sets the author of the file.
    /// </summary>
    [XmlAttribute("author")] public string? Author;


    /// <summary>
    /// Gets or sets the mail fo the author.
    /// </summary>
    [XmlAttribute("mail")] public string? Mail;

    /// <summary>
    /// Gets or sets the name of the project.
    /// </summary>
    [XmlAttribute("name")] public string? Name;

    /// <summary>
    /// Gets or sets the comment of the project.
    /// </summary>
    [XmlAttribute("comment")] public string? Comment;

    /// <summary>
    /// Gets or sets the version of the project.
    /// </summary>
    [XmlAttribute("version")] public string? Version;

    /// <summary>
    /// Gets or sets the source language of the project.
    /// </summary>
    [XmlAttribute("source_language")] public string? SourceLanguage;

    /// <summary>
    /// Gets or sets the language name of the project.
    /// </summary>
    [XmlAttribute("language_name")] public string? LanguageName;

    /// <summary>
    /// Gets or sets the country name of the project.
    /// </summary>
    [XmlAttribute("country_name")] public string? CountryName;

    /// <summary>
    /// Gets or sets the creation tool of the project.
    /// </summary>
    [XmlAttribute("creation_tool")] public string? CreationTool;

    /// <summary>
    /// Gets or sets the creation tool version of the project.
    /// </summary>
    [XmlAttribute("creation_tool_version")]
    public string? CreationToolVersion;

    /// <summary>
    /// The components of a project.
    /// </summary>
    [XmlElement("component")] public List<Component>? Components;

    /// <summary>
    /// The translations of a project.
    /// </summary>
    [XmlElement("translation")] public List<Translation>? Translations;

    /// <summary>
    /// The properties of a format.w
    /// </summary>
    [XmlArray("properties")] public List<Property>? Properties;
}