using System.Xml.Serialization;

namespace Ashampoo.Translations.Formats.ResX.Elements;

public class Data
{
    [XmlAttribute("name")] public string? Name;
    [XmlAttribute("xml:space")] public string? XmlSpace;
    [XmlElement("value")] public string? Value;
    [XmlElement("comment")] public string? Comment;
}