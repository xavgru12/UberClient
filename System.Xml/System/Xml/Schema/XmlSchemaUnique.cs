// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaUnique
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.Schema
{
  public class XmlSchemaUnique : XmlSchemaIdentityConstraint
  {
    private const string xmlname = "unique";

    internal override int Compile(ValidationEventHandler h, XmlSchema schema) => base.Compile(h, schema);

    internal override int Validate(ValidationEventHandler h, XmlSchema schema) => this.errorCount;

    internal static XmlSchemaUnique Read(XmlSchemaReader reader, ValidationEventHandler h)
    {
      XmlSchemaUnique xso = new XmlSchemaUnique();
      reader.MoveToElement();
      if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "unique")
      {
        XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaUnique.Read, name=" + reader.Name, (Exception) null);
        reader.Skip();
        return (XmlSchemaUnique) null;
      }
      xso.LineNumber = reader.LineNumber;
      xso.LinePosition = reader.LinePosition;
      xso.SourceUri = reader.BaseURI;
      while (reader.MoveToNextAttribute())
      {
        if (reader.Name == "id")
          xso.Id = reader.Value;
        else if (reader.Name == "name")
          xso.Name = reader.Value;
        else if (reader.NamespaceURI == string.Empty && reader.Name != "xmlns" || reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
          XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for unique", (Exception) null);
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
          if (reader.LocalName != "unique")
          {
            XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaUnion.Read, name=" + reader.Name, (Exception) null);
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
        else if (num <= 2 && reader.LocalName == "selector")
        {
          num = 3;
          XmlSchemaXPath xmlSchemaXpath = XmlSchemaXPath.Read(reader, h, "selector");
          if (xmlSchemaXpath != null)
            xso.Selector = xmlSchemaXpath;
        }
        else if (num <= 3 && reader.LocalName == "field")
        {
          num = 3;
          if (xso.Selector == null)
            XmlSchemaObject.error(h, "selector must be defined before field declarations", (Exception) null);
          XmlSchemaXPath xmlSchemaXpath = XmlSchemaXPath.Read(reader, h, "field");
          if (xmlSchemaXpath != null)
            xso.Fields.Add((XmlSchemaObject) xmlSchemaXpath);
        }
        else
          reader.RaiseInvalidElementError();
      }
      return xso;
    }
  }
}
