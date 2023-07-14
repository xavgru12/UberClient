// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaAttributeGroup
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml.Serialization;

namespace System.Xml.Schema
{
  public class XmlSchemaAttributeGroup : XmlSchemaAnnotated
  {
    private const string xmlname = "attributeGroup";
    private XmlSchemaAnyAttribute anyAttribute;
    private XmlSchemaObjectCollection attributes;
    private string name;
    private XmlSchemaAttributeGroup redefined;
    private XmlQualifiedName qualifiedName;
    private XmlSchemaObjectTable attributeUses;
    private XmlSchemaAnyAttribute anyAttributeUse;
    internal bool AttributeGroupRecursionCheck;

    public XmlSchemaAttributeGroup()
    {
      this.attributes = new XmlSchemaObjectCollection();
      this.qualifiedName = XmlQualifiedName.Empty;
    }

    [XmlAttribute("name")]
    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    [XmlElement("attribute", typeof (XmlSchemaAttribute))]
    [XmlElement("attributeGroup", typeof (XmlSchemaAttributeGroupRef))]
    public XmlSchemaObjectCollection Attributes => this.attributes;

    internal XmlSchemaObjectTable AttributeUses => this.attributeUses;

    internal XmlSchemaAnyAttribute AnyAttributeUse => this.anyAttributeUse;

    [XmlElement("anyAttribute")]
    public XmlSchemaAnyAttribute AnyAttribute
    {
      get => this.anyAttribute;
      set => this.anyAttribute = value;
    }

    [XmlIgnore]
    public XmlSchemaAttributeGroup RedefinedAttributeGroup => this.redefined;

    [XmlIgnore]
    public XmlQualifiedName QualifiedName => this.qualifiedName;

    internal override void SetParent(XmlSchemaObject parent)
    {
      base.SetParent(parent);
      if (this.AnyAttribute != null)
        this.AnyAttribute.SetParent((XmlSchemaObject) this);
      foreach (XmlSchemaObject attribute in this.Attributes)
        attribute.SetParent((XmlSchemaObject) this);
    }

    internal override int Compile(ValidationEventHandler h, XmlSchema schema)
    {
      if (this.CompilationId == schema.CompilationId)
        return this.errorCount;
      this.errorCount = 0;
      if (this.redefinedObject != null)
      {
        this.errorCount += this.redefined.Compile(h, schema);
        if (this.errorCount == 0)
          this.redefined = (XmlSchemaAttributeGroup) this.redefinedObject;
      }
      XmlSchemaUtil.CompileID(this.Id, (XmlSchemaObject) this, schema.IDCollection, h);
      if (this.Name == null || this.Name == string.Empty)
        this.error(h, "Name is required in top level simpletype");
      else if (!XmlSchemaUtil.CheckNCName(this.Name))
        this.error(h, "name attribute of a simpleType must be NCName");
      else
        this.qualifiedName = new XmlQualifiedName(this.Name, this.AncestorSchema.TargetNamespace);
      if (this.AnyAttribute != null)
        this.errorCount += this.AnyAttribute.Compile(h, schema);
      foreach (XmlSchemaObject attribute in this.Attributes)
      {
        switch (attribute)
        {
          case XmlSchemaAttribute _:
            this.errorCount += ((XmlSchemaAttribute) attribute).Compile(h, schema);
            continue;
          case XmlSchemaAttributeGroupRef _:
            this.errorCount += ((XmlSchemaAttributeGroupRef) attribute).Compile(h, schema);
            continue;
          default:
            this.error(h, "invalid type of object in Attributes property");
            continue;
        }
      }
      this.CompilationId = schema.CompilationId;
      return this.errorCount;
    }

    internal override int Validate(ValidationEventHandler h, XmlSchema schema)
    {
      if (this.IsValidated(schema.CompilationId))
        return this.errorCount;
      if (this.redefined == null && this.redefinedObject != null)
      {
        this.redefinedObject.Compile(h, schema);
        this.redefined = (XmlSchemaAttributeGroup) this.redefinedObject;
        this.redefined.Validate(h, schema);
      }
      XmlSchemaObjectCollection attributes = this.Attributes;
      this.attributeUses = new XmlSchemaObjectTable();
      this.errorCount += XmlSchemaUtil.ValidateAttributesResolved(this.attributeUses, h, schema, attributes, this.AnyAttribute, ref this.anyAttributeUse, this.redefined, false);
      this.ValidationId = schema.ValidationId;
      return this.errorCount;
    }

    internal static XmlSchemaAttributeGroup Read(XmlSchemaReader reader, ValidationEventHandler h)
    {
      XmlSchemaAttributeGroup xso = new XmlSchemaAttributeGroup();
      reader.MoveToElement();
      if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "attributeGroup")
      {
        XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaAttributeGroup.Read, name=" + reader.Name, (Exception) null);
        reader.SkipToEnd();
        return (XmlSchemaAttributeGroup) null;
      }
      xso.LineNumber = reader.LineNumber;
      xso.LinePosition = reader.LinePosition;
      xso.SourceUri = reader.BaseURI;
      while (reader.MoveToNextAttribute())
      {
        if (reader.Name == "id")
          xso.Id = reader.Value;
        else if (reader.Name == "name")
          xso.name = reader.Value;
        else if (reader.NamespaceURI == string.Empty && reader.Name != "xmlns" || reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
          XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for attributeGroup in this context", (Exception) null);
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
          if (reader.LocalName != "attributeGroup")
          {
            XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaAttributeGroup.Read, name=" + reader.Name, (Exception) null);
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
        {
          if (num <= 2)
          {
            if (reader.LocalName == "attribute")
            {
              num = 2;
              XmlSchemaAttribute xmlSchemaAttribute = XmlSchemaAttribute.Read(reader, h);
              if (xmlSchemaAttribute != null)
              {
                xso.Attributes.Add((XmlSchemaObject) xmlSchemaAttribute);
                continue;
              }
              continue;
            }
            if (reader.LocalName == "attributeGroup")
            {
              num = 2;
              XmlSchemaAttributeGroupRef attributeGroupRef = XmlSchemaAttributeGroupRef.Read(reader, h);
              if (attributeGroupRef != null)
              {
                xso.attributes.Add((XmlSchemaObject) attributeGroupRef);
                continue;
              }
              continue;
            }
          }
          if (num <= 3 && reader.LocalName == "anyAttribute")
          {
            num = 4;
            XmlSchemaAnyAttribute schemaAnyAttribute = XmlSchemaAnyAttribute.Read(reader, h);
            if (schemaAnyAttribute != null)
              xso.AnyAttribute = schemaAnyAttribute;
          }
          else
            reader.RaiseInvalidElementError();
        }
      }
      return xso;
    }
  }
}
