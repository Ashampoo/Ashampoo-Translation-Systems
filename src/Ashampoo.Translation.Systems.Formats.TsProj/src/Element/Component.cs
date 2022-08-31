using System.Xml.Serialization;

namespace Ashampoo.Translation.Systems.Formats.TsProj.Element;

/// <summary>
/// Represents a component element in a tsproj file.
/// </summary>
public class Component
{
    /// <summary>
    /// Gets or sets the plugin guid of the component.
    /// </summary>
    [XmlAttribute("pluginguid")] public string? PluginGuid;

    /// <summary>
    /// The translations of the component.
    /// </summary>
    [XmlElement("translation")] public List<Translation>? Translations;
}