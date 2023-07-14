// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaPatternFacet
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.Schema
{
  public class XmlSchemaPatternFacet : XmlSchemaFacet
  {
    private const string xmlname = "pattern";

    internal override XmlSchemaFacet.Facet ThisFacet => XmlSchemaFacet.Facet.pattern;

    internal static XmlSchemaPatternFacet Read(XmlSchemaReader reader, ValidationEventHandler h)
    {
      XmlSchemaPatternFacet xso = new XmlSchemaPatternFacet();
      reader.MoveToElement();
      if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "pattern")
      {
        XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaPatternFacet.Read, name=" + reader.Name, (Exception) null);
        reader.Skip();
        return (XmlSchemaPatternFacet) null;
      }
      xso.LineNumber = reader.LineNumber;
      xso.LinePosition = reader.LinePosition;
      xso.SourceUri = reader.BaseURI;
      while (reader.MoveToNextAttribute())
      {
        if (reader.Name == "id")
          xso.Id = reader.Value;
        else if (reader.Name == "value")
          xso.Value = reader.Value;
        else if (reader.NamespaceURI == string.Empty && reader.Name != "xmlns" || reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
          XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for pattern", (Exception) null);
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
          if (reader.LocalName != "pattern")
          {
            XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaPatternFacet.Read, name=" + reader.Name, (Exception) null);
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
