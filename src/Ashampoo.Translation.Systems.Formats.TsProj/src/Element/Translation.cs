using System.Xml.Serialization;

namespace Ashampoo.Translation.Systems.Formats.TsProj.Element;

public class Translation
{
    [XmlAttribute("id")] public string Id { get; set; } = "";

    [XmlElement("comment")] public string? Comment { get; set; }

    [XmlElement("source")] public string? Source { get; set; }

    [XmlElement("target")] public string Value { get; set; } = "";
}