// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaMaxLengthFacet
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.Schema
{
  public class XmlSchemaMaxLengthFacet : XmlSchemaNumericFacet
  {
    private const string xmlname = "maxLength";

    internal override XmlSchemaFacet.Facet ThisFacet => XmlSchemaFacet.Facet.maxLength;

    internal static XmlSchemaMaxLengthFacet Read(XmlSchemaReader reader, ValidationEventHandler h)
    {
      XmlSchemaMaxLengthFacet xso = new XmlSchemaMaxLengthFacet();
      reader.MoveToElement();
      if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "maxLength")
      {
        XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaMaxLengthFacet.Read, name=" + reader.Name, (Exception) null);
        reader.Skip();
        return (XmlSchemaMaxLengthFacet) null;
      }
      xso.LineNumber = reader.LineNumber;
      xso.LinePosition = reader.LinePosition;
      xso.SourceUri = reader.BaseURI;
      while (reader.MoveToNextAttribute())
      {
        if (reader.Name == "id")
          xso.Id = reader.Value;
        else if (reader.Name == "fixed")
        {
          Exception innerExcpetion;
          xso.IsFixed = XmlSchemaUtil.ReadBoolAttribute((XmlReader) reader, out innerExcpetion);
          if (innerExcpetion != null)
            XmlSchemaObject.error(h, reader.Value + " is not a valid value for fixed attribute", innerExcpetion);
        }
        else if (reader.Name == "value")
          xso.Value = reader.Value;
        else if (reader.NamespaceURI == string.Empty && reader.Name != "xmlns" || reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
          XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for group", (Exception) null);
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
          if (reader.LocalName != "maxLength")
          {
            XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaMaxLengthFacet.Read, name=" + reader.Name, (Exception) null);
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
