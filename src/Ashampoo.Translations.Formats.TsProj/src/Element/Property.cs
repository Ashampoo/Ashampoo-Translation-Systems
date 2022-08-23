using System.Xml.Serialization;

namespace Ashampoo.Translations.Formats.TsProj.Element;

public class Property
{
    [XmlAttribute("name")] public string? Name;
}