using System.ComponentModel;
using System.Xml.Serialization;

namespace Ashampoo.Translation.Systems.Formats.ResX.Elements;

/// <summary>
/// Represents the root element of a ResX file.
/// </summary>
[XmlRoot("root")]
public class Root
{
    /// <summary>
    /// The header of the ResX file.
    /// </summary>
    [XmlAnyElement] [Browsable(false)] [EditorBrowsable(EditorBrowsableState.Never)]
    public RawResXHeader ResXHeader = new();

    /// <summary>
    /// The data of the ResX file.
    /// </summary>
    [XmlElement("data")] public List<Data>? Data;
}