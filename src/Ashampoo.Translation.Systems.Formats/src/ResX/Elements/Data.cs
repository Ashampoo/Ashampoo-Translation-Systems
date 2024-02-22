using System.Xml.Serialization;

namespace Ashampoo.Translation.Systems.Formats.ResX.Elements;

/// <summary>
/// Represents a data node in a ResX file.
/// </summary>
public class Data
{
    /// <summary>
    /// Gets or sets the name of the data node.
    /// </summary>
    [XmlAttribute("name")] public string? Name;
    /// <summary>
    /// Gets or sets the xmlspace of the data node.
    /// </summary>
    [XmlAttribute("xml:space")] public string? XmlSpace;
    /// <summary>
    /// Gets or sets the value of the data node.
    /// </summary>
    [XmlElement("value")] public string? Value;
    /// <summary>
    /// Gets or sets the comment of the data node.
    /// </summary>
    [XmlElement("comment")] public string? Comment;
}