using System.Xml.Serialization;

namespace Ashampoo.Translation.Systems.Formats.TsProj.Element;

[XmlRoot("project")]
public class Project
{
    [XmlAttribute("tsversion")] public int FormatVersion = 7;

    [XmlAttribute("target_language")] public string TargetLanguage = "";

    [XmlAttribute("author")] public string? Author;


    [XmlAttribute("mail")] public string? Mail;

    [XmlAttribute("name")] public string? Name;

    [XmlAttribute("comment")] public string? Comment;

    [XmlAttribute("version")] public string? Version;

    [XmlAttribute("source_language")] public string? SourceLanguage;

    [XmlAttribute("language_name")] public string? LanguageName;

    [XmlAttribute("country_name")] public string? CountryName;

    [XmlAttribute("creation_tool")] public string? CreationTool;

    [XmlAttribute("creation_tool_version")]
    public string? CreationToolVersion;

    [XmlElement("component")] public List<Component>? Components;

    [XmlElement("translation")] public List<Translation>? Translations;

    [XmlArray("properties")] public List<Property>? Properties;
}