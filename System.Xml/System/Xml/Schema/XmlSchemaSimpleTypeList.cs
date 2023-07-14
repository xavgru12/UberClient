// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaSimpleTypeList
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml.Serialization;

namespace System.Xml.Schema
{
  public class XmlSchemaSimpleTypeList : XmlSchemaSimpleTypeContent
  {
    private const string xmlname = "list";
    private XmlSchemaSimpleType itemType;
    private XmlQualifiedName itemTypeName;
    private object validatedListItemType;
    private XmlSchemaSimpleType validatedListItemSchemaType;

    public XmlSchemaSimpleTypeList() => this.ItemTypeName = XmlQualifiedName.Empty;

    [XmlAttribute("itemType")]
    public XmlQualifiedName ItemTypeName
    {
      get => this.itemTypeName;
      set => this.itemTypeName = value;
    }

    [XmlElement("simpleType", Type = typeof (XmlSchemaSimpleType))]
    public XmlSchemaSimpleType ItemType
    {
      get => this.itemType;
      set => this.itemType = value;
    }

    [XmlIgnore]
    public XmlSchemaSimpleType BaseItemType
    {
      get => this.validatedListItemSchemaType;
      set
      {
      }
    }

    internal object ValidatedListItemType => this.validatedListItemType;

    internal override void SetParent(XmlSchemaObject parent)
    {
      base.SetParent(parent);
      if (this.ItemType == null)
        return;
      this.ItemType.SetParent((XmlSchemaObject) this);
    }

    internal override int Compile(ValidationEventHandler h, XmlSchema schema)
    {
      if (this.CompilationId == schema.CompilationId)
        return 0;
      this.errorCount = 0;
      if (this.ItemType != null && !this.ItemTypeName.IsEmpty)
        this.error(h, "both itemType and simpletype can't be present");
      if (this.ItemType == null && this.ItemTypeName.IsEmpty)
        this.error(h, "one of itemType or simpletype must be present");
      if (this.ItemType != null)
        this.errorCount += this.ItemType.Compile(h, schema);
      if (!XmlSchemaUtil.CheckQName(this.ItemTypeName))
        this.error(h, "BaseTypeName must be a XmlQualifiedName");
      XmlSchemaUtil.CompileID(this.Id, (XmlSchemaObject) this, schema.IDCollection, h);
      this.CompilationId = schema.CompilationId;
      return this.errorCount;
    }

    internal override int Validate(ValidationEventHandler h, XmlSchema schema)
    {
      if (this.IsValidated(schema.ValidationId))
        return this.errorCount;
      XmlSchemaSimpleType schemaSimpleType1 = this.itemType ?? schema.FindSchemaType(this.itemTypeName) as XmlSchemaSimpleType;
      if (schemaSimpleType1 != null)
      {
        this.errorCount += schemaSimpleType1.Validate(h, schema);
        this.validatedListItemType = (object) schemaSimpleType1;
      }
      else if (this.itemTypeName == XmlSchemaComplexType.AnyTypeName)
        this.validatedListItemType = (object) XmlSchemaSimpleType.AnySimpleType;
      else if (XmlSchemaUtil.IsBuiltInDatatypeName(this.itemTypeName))
      {
        this.validatedListItemType = (object) XmlSchemaDatatype.FromName(this.itemTypeName);
        if (this.validatedListItemType == null)
          this.error(h, "Invalid schema type name was specified: " + (object) this.itemTypeName);
      }
      else if (!schema.IsNamespaceAbsent(this.itemTypeName.Namespace))
        this.error(h, "Referenced base list item schema type " + (object) this.itemTypeName + " was not found.");
      if (!(this.validatedListItemType is XmlSchemaSimpleType schemaSimpleType2) && this.validatedListItemType != null)
        schemaSimpleType2 = XmlSchemaType.GetBuiltInSimpleType(((XmlSchemaDatatype) this.validatedListItemType).TypeCode);
      this.validatedListItemSchemaType = schemaSimpleType2;
      this.ValidationId = schema.ValidationId;
      return this.errorCount;
    }

    internal static XmlSchemaSimpleTypeList Read(XmlSchemaReader reader, ValidationEventHandler h)
    {
      XmlSchemaSimpleTypeList xso = new XmlSchemaSimpleTypeList();
      reader.MoveToElement();
      if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "list")
      {
        XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaSimpleTypeList.Read, name=" + reader.Name, (Exception) null);
        reader.Skip();
        return (XmlSchemaSimpleTypeList) null;
      }
      xso.LineNumber = reader.LineNumber;
      xso.LinePosition = reader.LinePosition;
      xso.SourceUri = reader.BaseURI;
      while (reader.MoveToNextAttribute())
      {
        if (reader.Name == "id")
          xso.Id = reader.Value;
        else if (reader.Name == "itemType")
        {
          Exception innerEx;
          xso.ItemTypeName = XmlSchemaUtil.ReadQNameAttribute((XmlReader) reader, out innerEx);
          if (innerEx != null)
            XmlSchemaObject.error(h, reader.Value + " is not a valid value for itemType attribute", innerEx);
        }
        else if (reader.NamespaceURI == string.Empty && reader.Name != "xmlns" || reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
          XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for list", (Exception) null);
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
          if (reader.LocalName != "list")
          {
            XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaSimpleTypeList.Read, name=" + reader.Name, (Exception) null);
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
        else if (num <= 2 && reader.LocalName == "simpleType")
        {
          num = 3;
          XmlSchemaSimpleType schemaSimpleType = XmlSchemaSimpleType.Read(reader, h);
          if (schemaSimpleType != null)
            xso.itemType = schemaSimpleType;
        }
        else
          reader.RaiseInvalidElementError();
      }
      return xso;
    }
  }
}
