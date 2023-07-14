// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaAnnotation
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
  public class XmlSchemaAnnotation : XmlSchemaObject
  {
    private const string xmlname = "annotation";
    private string id;
    private XmlSchemaObjectCollection items;
    private XmlAttribute[] unhandledAttributes;

    public XmlSchemaAnnotation() => this.items = new XmlSchemaObjectCollection();

    [XmlAttribute("id", DataType = "ID")]
    public string Id
    {
      get => this.id;
      set => this.id = value;
    }

    [XmlElement("appinfo", typeof (XmlSchemaAppInfo))]
    [XmlElement("documentation", typeof (XmlSchemaDocumentation))]
    public XmlSchemaObjectCollection Items => this.items;

    [XmlAnyAttribute]
    public XmlAttribute[] UnhandledAttributes
    {
      get
      {
        if (this.unhandledAttributeList != null)
        {
          this.unhandledAttributes = (XmlAttribute[]) this.unhandledAttributeList.ToArray(typeof (XmlAttribute));
          this.unhandledAttributeList = (ArrayList) null;
        }
        return this.unhandledAttributes;
      }
      set
      {
        this.unhandledAttributes = value;
        this.unhandledAttributeList = (ArrayList) null;
      }
    }

    internal override int Compile(ValidationEventHandler h, XmlSchema schema)
    {
      if (this.CompilationId == schema.CompilationId)
        return 0;
      this.CompilationId = schema.CompilationId;
      return 0;
    }

    internal override int Validate(ValidationEventHandler h, XmlSchema schema) => 0;

    internal static XmlSchemaAnnotation Read(XmlSchemaReader reader, ValidationEventHandler h)
    {
      XmlSchemaAnnotation xso = new XmlSchemaAnnotation();
      reader.MoveToElement();
      if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "annotation")
      {
        XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaAnnotation.Read, name=" + reader.Name, (Exception) null);
        reader.SkipToEnd();
        return (XmlSchemaAnnotation) null;
      }
      xso.LineNumber = reader.LineNumber;
      xso.LinePosition = reader.LinePosition;
      xso.SourceUri = reader.BaseURI;
      while (reader.MoveToNextAttribute())
      {
        if (reader.Name == "id")
          xso.Id = reader.Value;
        else if (reader.NamespaceURI == string.Empty && reader.Name != "xmlns" || reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
          XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for annotation", (Exception) null);
        else
          XmlSchemaUtil.ReadUnhandledAttribute((XmlReader) reader, (XmlSchemaObject) xso);
      }
      reader.MoveToElement();
      if (reader.IsEmptyElement)
        return xso;
      bool skip = false;
      string str1 = (string) null;
      while (!reader.EOF)
      {
        if (skip)
          skip = false;
        else
          reader.ReadNextElement();
        if (reader.NodeType == XmlNodeType.EndElement)
        {
          bool flag = true;
          string str2 = "annotation";
          if (str1 != null)
          {
            str2 = str1;
            str1 = (string) null;
            flag = false;
          }
          if (reader.LocalName != str2)
            XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaAnnotation.Read, name=" + reader.Name + ",expected=" + str2, (Exception) null);
          if (flag)
            break;
        }
        else if (reader.LocalName == "appinfo")
        {
          XmlSchemaAppInfo xmlSchemaAppInfo = XmlSchemaAppInfo.Read(reader, h, out skip);
          if (xmlSchemaAppInfo != null)
            xso.items.Add((XmlSchemaObject) xmlSchemaAppInfo);
        }
        else if (reader.LocalName == "documentation")
        {
          XmlSchemaDocumentation schemaDocumentation = XmlSchemaDocumentation.Read(reader, h, out skip);
          if (schemaDocumentation != null)
            xso.items.Add((XmlSchemaObject) schemaDocumentation);
        }
        else
          reader.RaiseInvalidElementError();
      }
      return xso;
    }
  }
}
