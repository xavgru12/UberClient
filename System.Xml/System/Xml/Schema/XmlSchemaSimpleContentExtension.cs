// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaSimpleContentExtension
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml.Serialization;

namespace System.Xml.Schema
{
  public class XmlSchemaSimpleContentExtension : XmlSchemaContent
  {
    private const string xmlname = "extension";
    private XmlSchemaAnyAttribute any;
    private XmlSchemaObjectCollection attributes;
    private XmlQualifiedName baseTypeName;

    public XmlSchemaSimpleContentExtension()
    {
      this.baseTypeName = XmlQualifiedName.Empty;
      this.attributes = new XmlSchemaObjectCollection();
    }

    [XmlAttribute("base")]
    public XmlQualifiedName BaseTypeName
    {
      get => this.baseTypeName;
      set => this.baseTypeName = value;
    }

    [XmlElement("attribute", typeof (XmlSchemaAttribute))]
    [XmlElement("attributeGroup", typeof (XmlSchemaAttributeGroupRef))]
    public XmlSchemaObjectCollection Attributes => this.attributes;

    [XmlElement("anyAttribute")]
    public XmlSchemaAnyAttribute AnyAttribute
    {
      get => this.any;
      set => this.any = value;
    }

    internal override bool IsExtension => true;

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
        return 0;
      if (this.isRedefinedComponent)
      {
        if (this.Annotation != null)
          this.Annotation.isRedefinedComponent = true;
        if (this.AnyAttribute != null)
          this.AnyAttribute.isRedefinedComponent = true;
        foreach (XmlSchemaObject attribute in this.Attributes)
          attribute.isRedefinedComponent = true;
      }
      if (this.BaseTypeName == (XmlQualifiedName) null || this.BaseTypeName.IsEmpty)
        this.error(h, "base must be present, as a QName");
      else if (!XmlSchemaUtil.CheckQName(this.BaseTypeName))
        this.error(h, "BaseTypeName must be a QName");
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
            this.error(h, attribute.GetType().ToString() + " is not valid in this place::SimpleConentExtension");
            continue;
        }
      }
      XmlSchemaUtil.CompileID(this.Id, (XmlSchemaObject) this, schema.IDCollection, h);
      this.CompilationId = schema.CompilationId;
      return this.errorCount;
    }

    internal override XmlQualifiedName GetBaseTypeName() => this.baseTypeName;

    internal override XmlSchemaParticle GetParticle() => (XmlSchemaParticle) null;

    internal override int Validate(ValidationEventHandler h, XmlSchema schema)
    {
      if (this.IsValidated(schema.ValidationId))
        return this.errorCount;
      XmlSchemaType schemaType = schema.FindSchemaType(this.baseTypeName);
      if (schemaType != null)
      {
        if (schemaType is XmlSchemaComplexType schemaComplexType && schemaComplexType.ContentModel is XmlSchemaComplexContent)
          this.error(h, "Specified type is complex type which contains complex content.");
        schemaType.Validate(h, schema);
        this.actualBaseSchemaType = (object) schemaType;
      }
      else if (this.baseTypeName == XmlSchemaComplexType.AnyTypeName)
        this.actualBaseSchemaType = (object) XmlSchemaComplexType.AnyType;
      else if (XmlSchemaUtil.IsBuiltInDatatypeName(this.baseTypeName))
      {
        this.actualBaseSchemaType = (object) XmlSchemaDatatype.FromName(this.baseTypeName);
        if (this.actualBaseSchemaType == null)
          this.error(h, "Invalid schema datatype name is specified.");
      }
      else if (!schema.IsNamespaceAbsent(this.baseTypeName.Namespace))
        this.error(h, "Referenced base schema type " + (object) this.baseTypeName + " was not found in the corresponding schema.");
      this.ValidationId = schema.ValidationId;
      return this.errorCount;
    }

    internal static XmlSchemaSimpleContentExtension Read(
      XmlSchemaReader reader,
      ValidationEventHandler h)
    {
      XmlSchemaSimpleContentExtension xso = new XmlSchemaSimpleContentExtension();
      reader.MoveToElement();
      if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "extension")
      {
        XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaAttributeGroup.Read, name=" + reader.Name, (Exception) null);
        reader.Skip();
        return (XmlSchemaSimpleContentExtension) null;
      }
      xso.LineNumber = reader.LineNumber;
      xso.LinePosition = reader.LinePosition;
      xso.SourceUri = reader.BaseURI;
      while (reader.MoveToNextAttribute())
      {
        if (reader.Name == "base")
        {
          Exception innerEx;
          xso.baseTypeName = XmlSchemaUtil.ReadQNameAttribute((XmlReader) reader, out innerEx);
          if (innerEx != null)
            XmlSchemaObject.error(h, reader.Value + " is not a valid value for base attribute", innerEx);
        }
        else if (reader.Name == "id")
          xso.Id = reader.Value;
        else if (reader.NamespaceURI == string.Empty && reader.Name != "xmlns" || reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
          XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for extension in this context", (Exception) null);
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
          if (reader.LocalName != "extension")
          {
            XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaSimpleContentExtension.Read, name=" + reader.Name, (Exception) null);
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
