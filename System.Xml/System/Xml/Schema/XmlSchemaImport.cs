// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaImport
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml.Serialization;

namespace System.Xml.Schema
{
  public class XmlSchemaImport : XmlSchemaExternal
  {
    private const string xmlname = "import";
    private XmlSchemaAnnotation annotation;
    private string nameSpace;

    [XmlAttribute("namespace", DataType = "anyURI")]
    public string Namespace
    {
      get => this.nameSpace;
      set => this.nameSpace = value;
    }

    [XmlElement("annotation", Type = typeof (XmlSchemaAnnotation))]
    public XmlSchemaAnnotation Annotation
    {
      get => this.annotation;
      set => this.annotation = value;
    }

    internal static XmlSchemaImport Read(XmlSchemaReader reader, ValidationEventHandler h)
    {
      XmlSchemaImport xso = new XmlSchemaImport();
      reader.MoveToElement();
      if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "import")
      {
        XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaImport.Read, name=" + reader.Name, (Exception) null);
        reader.SkipToEnd();
        return (XmlSchemaImport) null;
      }
      xso.LineNumber = reader.LineNumber;
      xso.LinePosition = reader.LinePosition;
      xso.SourceUri = reader.BaseURI;
      while (reader.MoveToNextAttribute())
      {
        if (reader.Name == "id")
          xso.Id = reader.Value;
        else if (reader.Name == "namespace")
          xso.nameSpace = reader.Value;
        else if (reader.Name == "schemaLocation")
          xso.SchemaLocation = reader.Value;
        else if (reader.NamespaceURI == string.Empty && reader.Name != "xmlns" || reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
          XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for import", (Exception) null);
        else
          XmlSchemaUtil.ReadUnhandledAttribute((XmlReader) reader, (XmlSchemaObject) xso);
      }
      reader.MoveToElement();
      if (reader.IsEmptyElement)
        return xso;
      int num = 1;
      while (reader.ReadNextElement())
      {
        if (reader.NodeType == XmlNodeType.EndElement)
        {
          if (reader.LocalName != "import")
          {
            XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaImport.Read, name=" + reader.Name, (Exception) null);
            break;
          }
          break;
        }
        if (num <= 1 && reader.LocalName == "annotation")
        {
          num = 2;
          XmlSchemaAnnotation schemaAnnotation = XmlSchemaAnnotation.Read(reader, h);
          if (schemaAnnotation != null)
            xso.Annotation = schemaAnnotation;
        }
        else
          reader.RaiseInvalidElementError();
      }
      return xso;
    }
  }
}
