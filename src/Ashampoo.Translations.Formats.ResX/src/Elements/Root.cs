using System.ComponentModel;
using System.Xml.Serialization;

namespace Ashampoo.Translations.Formats.ResX.Elements;

[XmlRoot("root")]
public class Root
{
    [XmlAnyElement] [Browsable(false)] [EditorBrowsable(EditorBrowsableState.Never)]
    public RawResXHeader ResXHeader = new();

    [XmlElement("data")] public List<Data>? Data;
}