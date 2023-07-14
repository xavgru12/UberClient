// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaDocumentation
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml.Serialization;

namespace System.Xml.Schema
{
  public class XmlSchemaDocumentation : XmlSchemaObject
  {
    private string language;
    private XmlNode[] markup;
    private string source;

    [XmlAnyElement]
    [XmlText]
    public XmlNode[] Markup
    {
      get => this.markup;
      set => this.markup = value;
    }

    [XmlAttribute("source", DataType = "anyURI")]
    public string Source
    {
      get => this.source;
      set => this.source = value;
    }

    [XmlAttribute("xml:lang")]
    public string Language
    {
      get => this.language;
      set => this.language = value;
    }

    internal static XmlSchemaDocumentation Read(
      XmlSchemaReader reader,
      ValidationEventHandler h,
      out bool skip)
    {
      skip = false;
      XmlSchemaDocumentation schemaDocumentation = new XmlSchemaDocumentation();
      reader.MoveToElement();
      if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "documentation")
      {
        XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaDocumentation.Read, name=" + reader.Name, (Exception) null);
        reader.Skip();
        return (XmlSchemaDocumentation) null;
      }
      schemaDocumentation.LineNumber = reader.LineNumber;
      schemaDocumentation.LinePosition = reader.LinePosition;
      schemaDocumentation.SourceUri = reader.BaseURI;
      while (reader.MoveToNextAttribute())
      {
        if (reader.Name == "source")
          schemaDocumentation.source = reader.Value;
        else if (reader.Name == "xml:lang")
          schemaDocumentation.language = reader.Value;
        else
          XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for documentation", (Exception) null);
      }
      reader.MoveToElement();
      if (reader.IsEmptyElement)
      {
        schemaDocumentation.Markup = new XmlNode[0];
        return schemaDocumentation;
      }
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.AppendChild(xmlDocument.ReadNode((XmlReader) reader));
      XmlNode firstChild = xmlDocument.FirstChild;
      if (firstChild != null && firstChild.ChildNodes != null)
      {
        schemaDocumentation.Markup = new XmlNode[firstChild.ChildNodes.Count];
        for (int i = 0; i < firstChild.ChildNodes.Count; ++i)
          schemaDocumentation.Markup[i] = firstChild.ChildNodes[i];
      }
      if (reader.NodeType == XmlNodeType.Element || reader.NodeType == XmlNodeType.EndElement)
        skip = true;
      return schemaDocumentation;
    }
  }
}
