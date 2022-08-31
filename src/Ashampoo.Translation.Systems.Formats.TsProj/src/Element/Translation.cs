using System.Xml.Serialization;

namespace Ashampoo.Translation.Systems.Formats.TsProj.Element;

/// <summary>
/// Represents a translation element in a tsproj file.
/// </summary>
public class Translation
{
    /// <summary>
    /// Gets or sets the id of the translation.
    /// </summary>
    [XmlAttribute("id")] public string Id { get; set; } = "";

    /// <summary>
    /// Gets or sets the comment of the translation.
    /// </summary>
    [XmlElement("comment")] public string? Comment { get; set; }

    /// <summary>
    /// Gets or sets the source value of the translation.
    /// </summary>
    [XmlElement("source")] public string? Source { get; set; }

    /// <summary>
    /// Gets or sets the target value of the translation.
    /// </summary>
    [XmlElement("target")] public string Value { get; set; } = "";
}