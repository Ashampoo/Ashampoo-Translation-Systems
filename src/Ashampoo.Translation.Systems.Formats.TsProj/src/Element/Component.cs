using System.Xml.Serialization;

namespace Ashampoo.Translation.Systems.Formats.TsProj.Element;

public class Component
{
    [XmlAttribute("pluginguid")] public string? PluginGuid;

    [XmlElement("translation")] public List<Translation>? Translations;
}