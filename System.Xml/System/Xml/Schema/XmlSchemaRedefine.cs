// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaRedefine
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml.Serialization;

namespace System.Xml.Schema
{
  public class XmlSchemaRedefine : XmlSchemaExternal
  {
    private const string xmlname = "redefine";
    private XmlSchemaObjectTable attributeGroups;
    private XmlSchemaObjectTable groups;
    private XmlSchemaObjectCollection items;
    private XmlSchemaObjectTable schemaTypes;

    public XmlSchemaRedefine()
    {
      this.attributeGroups = new XmlSchemaObjectTable();
      this.groups = new XmlSchemaObjectTable();
      this.items = new XmlSchemaObjectCollection((XmlSchemaObject) this);
      this.schemaTypes = new XmlSchemaObjectTable();
    }

    [XmlElement("simpleType", typeof (XmlSchemaSimpleType))]
    [XmlElement("group", typeof (XmlSchemaGroup))]
    [XmlElement("complexType", typeof (XmlSchemaComplexType))]
    [XmlElement("attributeGroup", typeof (XmlSchemaAttributeGroup))]
    [XmlElement("annotation", typeof (XmlSchemaAnnotation))]
    public XmlSchemaObjectCollection Items => this.items;

    [XmlIgnore]
    public XmlSchemaObjectTable AttributeGroups => this.attributeGroups;

    [XmlIgnore]
    public XmlSchemaObjectTable SchemaTypes => this.schemaTypes;

    [XmlIgnore]
    public XmlSchemaObjectTable Groups => this.groups;

    internal override void SetParent(XmlSchemaObject parent)
    {
      base.SetParent(parent);
      foreach (XmlSchemaObject xmlSchemaObject in this.Items)
      {
        xmlSchemaObject.SetParent((XmlSchemaObject) this);
        xmlSchemaObject.isRedefinedComponent = true;
        xmlSchemaObject.isRedefineChild = true;
      }
    }

    internal static XmlSchemaRedefine Read(XmlSchemaReader reader, ValidationEventHandler h)
    {
      XmlSchemaRedefine xso = new XmlSchemaRedefine();
      reader.MoveToElement();
      if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "redefine")
      {
        XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaRedefine.Read, name=" + reader.Name, (Exception) null);
        reader.Skip();
        return (XmlSchemaRedefine) null;
      }
      xso.LineNumber = reader.LineNumber;
      xso.LinePosition = reader.LinePosition;
      xso.SourceUri = reader.BaseURI;
      while (reader.MoveToNextAttribute())
      {
        if (reader.Name == "id")
          xso.Id = reader.Value;
        else if (reader.Name == "schemaLocation")
          xso.SchemaLocation = reader.Value;
        else if (reader.NamespaceURI == string.Empty && reader.Name != "xmlns" || reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
          XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for redefine", (Exception) null);
        else
          XmlSchemaUtil.ReadUnhandledAttribute((XmlReader) reader, (XmlSchemaObject) xso);
      }
      reader.MoveToElement();
      if (reader.IsEmptyElement)
        return xso;
      while (reader.ReadNextElement())
      {
        if (reader.NodeType == XmlNodeType.EndElement)
        {
          if (reader.LocalName != "redefine")
          {
            XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaRedefine.Read, name=" + reader.Name, (Exception) null);
            break;
          }
          break;
        }
        if (reader.LocalName == "annotation")
        {
          XmlSchemaAnnotation schemaAnnotation = XmlSchemaAnnotation.Read(reader, h);
          if (schemaAnnotation != null)
            xso.items.Add((XmlSchemaObject) schemaAnnotation);
        }
        else if (reader.LocalName == "simpleType")
        {
          XmlSchemaSimpleType schemaSimpleType = XmlSchemaSimpleType.Read(reader, h);
          if (schemaSimpleType != null)
            xso.items.Add((XmlSchemaObject) schemaSimpleType);
        }
        else if (reader.LocalName == "complexType")
        {
          XmlSchemaComplexType schemaComplexType = XmlSchemaComplexType.Read(reader, h);
          if (schemaComplexType != null)
            xso.items.Add((XmlSchemaObject) schemaComplexType);
        }
        else if (reader.LocalName == "group")
        {
          XmlSchemaGroup xmlSchemaGroup = XmlSchemaGroup.Read(reader, h);
          if (xmlSchemaGroup != null)
            xso.items.Add((XmlSchemaObject) xmlSchemaGroup);
        }
        else if (reader.LocalName == "attributeGroup")
        {
          XmlSchemaAttributeGroup schemaAttributeGroup = XmlSchemaAttributeGroup.Read(reader, h);
          if (schemaAttributeGroup != null)
            xso.items.Add((XmlSchemaObject) schemaAttributeGroup);
        }
        else
          reader.RaiseInvalidElementError();
      }
      return xso;
    }
  }
}
