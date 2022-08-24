using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Ashampoo.Translation.Systems.Formats.ResX.Elements;

public class RawResXHeader : IXmlSerializable
{
    private const string NameStr = "name";
    private const string ValueStr = "value";
    private const string ResHeaderStr = "resheader";
    private const string VersionStr = "version";
    private const string ResMimeTypeStr = "resmimetype";
    private const string ReaderStr = "reader";
    private const string WriterStr = "writer";
    private const string Version = "2.0";
    private const string ResMimeType = "text/microsoft-resx";

    private string Value
    {
        get =>
            @"<xsd:schema id=""root"" xmlns="""" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:msdata=""urn:schemas-microsoft-com:xml-msdata"">
        <xsd:import namespace=""http://www.w3.org/XML/1998/namespace""/>
        <xsd:element name=""root"" msdata:IsDataSet=""true"">
            <xsd:complexType>
                <xsd:choice maxOccurs=""unbounded"">
                    <xsd:element name=""metadata"">
                        <xsd:complexType>
                            <xsd:sequence>
                            <xsd:element name=""value"" type=""xsd:string"" minOccurs=""0""/>
                            </xsd:sequence>
                            <xsd:attribute name=""name"" use=""required"" type=""xsd:string""/>
                            <xsd:attribute name=""type"" type=""xsd:string""/>
                            <xsd:attribute name=""mimetype"" type=""xsd:string""/>
                            <xsd:attribute ref=""xml:space""/>
                        </xsd:complexType>
                    </xsd:element>
                    <xsd:element name=""assembly"">
                      <xsd:complexType>
                        <xsd:attribute name=""alias"" type=""xsd:string""/>
                        <xsd:attribute name=""name"" type=""xsd:string""/>
                      </xsd:complexType>
                    </xsd:element>
                    <xsd:element name=""data"">
                        <xsd:complexType>
                            <xsd:sequence>
                                <xsd:element name=""value"" type=""xsd:string"" minOccurs=""0"" msdata:Ordinal=""1"" />
                                <xsd:element name=""comment"" type=""xsd:string"" minOccurs=""0"" msdata:Ordinal=""2"" />
                            </xsd:sequence>
                            <xsd:attribute name=""name"" type=""xsd:string"" use=""required"" msdata:Ordinal=""1"" />
                            <xsd:attribute name=""type"" type=""xsd:string"" msdata:Ordinal=""3"" />
                            <xsd:attribute name=""mimetype"" type=""xsd:string"" msdata:Ordinal=""4"" />
                            <xsd:attribute ref=""xml:space""/>
                        </xsd:complexType>
                    </xsd:element>
                    <xsd:element name=""resheader"">
                        <xsd:complexType>
                            <xsd:sequence>
                                <xsd:element name=""value"" type=""xsd:string"" minOccurs=""0"" msdata:Ordinal=""1"" />
                            </xsd:sequence>
                            <xsd:attribute name=""name"" type=""xsd:string"" use=""required"" />
                        </xsd:complexType>
                    </xsd:element>
                </xsd:choice>
            </xsd:complexType>
        </xsd:element>
    </xsd:schema>";
        set => _ = value; //do nothing
    }

    public XmlSchema GetSchema()
    {
        return null!;
    }

    public void ReadXml(XmlReader reader)
    {
        Value = reader.ReadInnerXml();
    }

    public void WriteXml(XmlWriter writer)
    {
        var readerSettings = new XmlReaderSettings
        {
            Async = true,
            IgnoreWhitespace = true
        };
        var reader = XmlReader.Create(new StringReader(Value), readerSettings);
        writer.WriteNodeAsync(reader, true);

        writer.WriteStartElement(ResHeaderStr);
        {
            writer.WriteAttributeString(NameStr, ResMimeTypeStr);
            writer.WriteStartElement(ValueStr);
            {
                writer.WriteString(ResMimeType);
            }
            writer.WriteEndElement();
        }
        writer.WriteEndElement();
        writer.WriteStartElement(ResHeaderStr);
        {
            writer.WriteAttributeString(NameStr, VersionStr);
            writer.WriteStartElement(ValueStr);
            {
                writer.WriteString(Version);
            }
            writer.WriteEndElement();
        }
        writer.WriteEndElement();
        writer.WriteStartElement(ResHeaderStr);
        {
            writer.WriteAttributeString(NameStr, ReaderStr);
            writer.WriteStartElement(ValueStr);
            {
                writer.WriteString(ResXConstants.ResHeaderReader);
            }
            writer.WriteEndElement();
        }
        writer.WriteEndElement();
        writer.WriteStartElement(ResHeaderStr);
        {
            writer.WriteAttributeString(NameStr, WriterStr);
            writer.WriteStartElement(ValueStr);
            {
                writer.WriteString(ResXConstants.ResHeaderWriter);
            }
            writer.WriteEndElement();
        }
        writer.WriteEndElement();
    }
}

public static class ResXConstants
{
    public const string ResHeaderReader =
        "System.Resources.ResXResourceReader, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";

    public const string ResHeaderWriter =
        "System.Resources.ResXResourceWriter, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
}