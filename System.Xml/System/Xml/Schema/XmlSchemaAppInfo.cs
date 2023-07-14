// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaAppInfo
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml.Serialization;

namespace System.Xml.Schema
{
  public class XmlSchemaAppInfo : XmlSchemaObject
  {
    private XmlNode[] markup;
    private string source;

    [XmlAttribute("source", DataType = "anyURI")]
    public string Source
    {
      get => this.source;
      set => this.source = value;
    }

    [XmlAnyElement]
    [XmlText]
    public XmlNode[] Markup
    {
      get => this.markup;
      set => this.markup = value;
    }

    internal static XmlSchemaAppInfo Read(
      XmlSchemaReader reader,
      ValidationEventHandler h,
      out bool skip)
    {
      skip = false;
      XmlSchemaAppInfo xmlSchemaAppInfo = new XmlSchemaAppInfo();
      reader.MoveToElement();
      if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "appinfo")
      {
        XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaAppInfo.Read, name=" + reader.Name, (Exception) null);
        reader.SkipToEnd();
        return (XmlSchemaAppInfo) null;
      }
      xmlSchemaAppInfo.LineNumber = reader.LineNumber;
      xmlSchemaAppInfo.LinePosition = reader.LinePosition;
      xmlSchemaAppInfo.SourceUri = reader.BaseURI;
      while (reader.MoveToNextAttribute())
      {
        if (reader.Name == "source")
          xmlSchemaAppInfo.source = reader.Value;
        else
          XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for appinfo", (Exception) null);
      }
      reader.MoveToElement();
      if (reader.IsEmptyElement)
      {
        xmlSchemaAppInfo.Markup = new XmlNode[0];
        return xmlSchemaAppInfo;
      }
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.AppendChild(xmlDocument.ReadNode((XmlReader) reader));
      XmlNode firstChild = xmlDocument.FirstChild;
      if (firstChild != null && firstChild.ChildNodes != null)
      {
        xmlSchemaAppInfo.Markup = new XmlNode[firstChild.ChildNodes.Count];
        for (int i = 0; i < firstChild.ChildNodes.Count; ++i)
          xmlSchemaAppInfo.Markup[i] = firstChild.ChildNodes[i];
      }
      if (reader.NodeType == XmlNodeType.Element || reader.NodeType == XmlNodeType.EndElement)
        skip = true;
      return xmlSchemaAppInfo;
    }
  }
}
