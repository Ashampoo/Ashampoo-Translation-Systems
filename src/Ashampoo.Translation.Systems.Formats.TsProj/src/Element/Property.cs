using System.Xml.Serialization;

namespace Ashampoo.Translation.Systems.Formats.TsProj.Element;

/// <summary>
/// Represents a property element in a tsproj file.
/// </summary>
public class Property
{
    /// <summary>
    /// Gets or sets the name of the property.
    /// </summary>
    [XmlAttribute("name")] public string? Name;
}