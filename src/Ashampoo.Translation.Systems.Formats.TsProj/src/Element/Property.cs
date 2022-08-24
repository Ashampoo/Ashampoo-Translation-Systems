using System.Xml.Serialization;

namespace Ashampoo.Translation.Systems.Formats.TsProj.Element;

public class Property
{
    [XmlAttribute("name")] public string? Name;
}