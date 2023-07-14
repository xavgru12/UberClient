// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaSimpleContentRestriction
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml.Serialization;

namespace System.Xml.Schema
{
  public class XmlSchemaSimpleContentRestriction : XmlSchemaContent
  {
    private const string xmlname = "restriction";
    private XmlSchemaAnyAttribute any;
    private XmlSchemaObjectCollection attributes;
    private XmlSchemaSimpleType baseType;
    private XmlQualifiedName baseTypeName;
    private XmlSchemaObjectCollection facets;

    public XmlSchemaSimpleContentRestriction()
    {
      this.baseTypeName = XmlQualifiedName.Empty;
      this.attributes = new XmlSchemaObjectCollection();
      this.facets = new XmlSchemaObjectCollection();
    }

    [XmlAttribute("base")]
    public XmlQualifiedName BaseTypeName
    {
      get => this.baseTypeName;
      set => this.baseTypeName = value;
    }

    [XmlElement("simpleType", Type = typeof (XmlSchemaSimpleType))]
    public XmlSchemaSimpleType BaseType
    {
      get => this.baseType;
      set => this.baseType = value;
    }

    [XmlElement("whiteSpace", typeof (XmlSchemaWhiteSpaceFacet))]
    [XmlElement("minExclusive", typeof (XmlSchemaMinExclusiveFacet))]
    [XmlElement("minInclusive", typeof (XmlSchemaMinInclusiveFacet))]
    [XmlElement("maxExclusive", typeof (XmlSchemaMaxExclusiveFacet))]
    [XmlElement("maxInclusive", typeof (XmlSchemaMaxInclusiveFacet))]
    [XmlElement("totalDigits", typeof (XmlSchemaTotalDigitsFacet))]
    [XmlElement("fractionDigits", typeof (XmlSchemaFractionDigitsFacet))]
    [XmlElement("length", typeof (XmlSchemaLengthFacet))]
    [XmlElement("minLength", typeof (XmlSchemaMinLengthFacet))]
    [XmlElement("maxLength", typeof (XmlSchemaMaxLengthFacet))]
    [XmlElement("enumeration", typeof (XmlSchemaEnumerationFacet))]
    [XmlElement("pattern", typeof (XmlSchemaPatternFacet))]
    public XmlSchemaObjectCollection Facets => this.facets;

    [XmlElement("attributeGroup", typeof (XmlSchemaAttributeGroupRef))]
    [XmlElement("attribute", typeof (XmlSchemaAttribute))]
    public XmlSchemaObjectCollection Attributes => this.attributes;

    [XmlElement("anyAttribute")]
    public XmlSchemaAnyAttribute AnyAttribute
    {
      get => this.any;
      set => this.any = value;
    }

    internal override bool IsExtension => false;

    internal override void SetParent(XmlSchemaObject parent)
    {
      base.SetParent(parent);
      if (this.BaseType != null)
        this.BaseType.SetParent((XmlSchemaObject) this);
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
      if (this.BaseType != null)
        this.errorCount += this.BaseType.Compile(h, schema);
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
            this.error(h, attribute.GetType().ToString() + " is not valid in this place::SimpleContentRestriction");
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
      if (this.baseType != null)
      {
        this.baseType.Validate(h, schema);
        this.actualBaseSchemaType = (object) this.baseType;
      }
      else if (this.baseTypeName != XmlQualifiedName.Empty)
      {
        XmlSchemaType schemaType = schema.FindSchemaType(this.baseTypeName);
        if (schemaType != null)
        {
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
      }
      this.ValidationId = schema.ValidationId;
      return this.errorCount;
    }

    internal static XmlSchemaSimpleContentRestriction Read(
      XmlSchemaReader reader,
      ValidationEventHandler h)
    {
      XmlSchemaSimpleContentRestriction xso = new XmlSchemaSimpleContentRestriction();
      reader.MoveToElement();
      if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "restriction")
      {
        XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaComplexContentRestriction.Read, name=" + reader.Name, (Exception) null);
        reader.SkipToEnd();
        return (XmlSchemaSimpleContentRestriction) null;
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
          XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for restriction", (Exception) null);
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
          if (reader.LocalName != "restriction")
          {
            XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaSimpleContentRestriction.Read, name=" + reader.Name, (Exception) null);
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
            xso.baseType = schemaSimpleType;
        }
        else
        {
          if (num <= 3)
          {
            if (reader.LocalName == "minExclusive")
            {
              num = 3;
              XmlSchemaMinExclusiveFacet minExclusiveFacet = XmlSchemaMinExclusiveFacet.Read(reader, h);
              if (minExclusiveFacet != null)
              {
                xso.facets.Add((XmlSchemaObject) minExclusiveFacet);
                continue;
              }
              continue;
            }
            if (reader.LocalName == "minInclusive")
            {
              num = 3;
              XmlSchemaMinInclusiveFacet minInclusiveFacet = XmlSchemaMinInclusiveFacet.Read(reader, h);
              if (minInclusiveFacet != null)
              {
                xso.facets.Add((XmlSchemaObject) minInclusiveFacet);
                continue;
              }
              continue;
            }
            if (reader.LocalName == "maxExclusive")
            {
              num = 3;
              XmlSchemaMaxExclusiveFacet maxExclusiveFacet = XmlSchemaMaxExclusiveFacet.Read(reader, h);
              if (maxExclusiveFacet != null)
              {
                xso.facets.Add((XmlSchemaObject) maxExclusiveFacet);
                continue;
              }
              continue;
            }
            if (reader.LocalName == "maxInclusive")
            {
              num = 3;
              XmlSchemaMaxInclusiveFacet maxInclusiveFacet = XmlSchemaMaxInclusiveFacet.Read(reader, h);
              if (maxInclusiveFacet != null)
              {
                xso.facets.Add((XmlSchemaObject) maxInclusiveFacet);
                continue;
              }
              continue;
            }
            if (reader.LocalName == "totalDigits")
            {
              num = 3;
              XmlSchemaTotalDigitsFacet totalDigitsFacet = XmlSchemaTotalDigitsFacet.Read(reader, h);
              if (totalDigitsFacet != null)
              {
                xso.facets.Add((XmlSchemaObject) totalDigitsFacet);
                continue;
              }
              continue;
            }
            if (reader.LocalName == "fractionDigits")
            {
              num = 3;
              XmlSchemaFractionDigitsFacet fractionDigitsFacet = XmlSchemaFractionDigitsFacet.Read(reader, h);
              if (fractionDigitsFacet != null)
              {
                xso.facets.Add((XmlSchemaObject) fractionDigitsFacet);
                continue;
              }
              continue;
            }
            if (reader.LocalName == "length")
            {
              num = 3;
              XmlSchemaLengthFacet schemaLengthFacet = XmlSchemaLengthFacet.Read(reader, h);
              if (schemaLengthFacet != null)
              {
                xso.facets.Add((XmlSchemaObject) schemaLengthFacet);
                continue;
              }
              continue;
            }
            if (reader.LocalName == "minLength")
            {
              num = 3;
              XmlSchemaMinLengthFacet schemaMinLengthFacet = XmlSchemaMinLengthFacet.Read(reader, h);
              if (schemaMinLengthFacet != null)
              {
                xso.facets.Add((XmlSchemaObject) schemaMinLengthFacet);
                continue;
              }
              continue;
            }
            if (reader.LocalName == "maxLength")
            {
              num = 3;
              XmlSchemaMaxLengthFacet schemaMaxLengthFacet = XmlSchemaMaxLengthFacet.Read(reader, h);
              if (schemaMaxLengthFacet != null)
              {
                xso.facets.Add((XmlSchemaObject) schemaMaxLengthFacet);
                continue;
              }
              continue;
            }
            if (reader.LocalName == "enumeration")
            {
              num = 3;
              XmlSchemaEnumerationFacet enumerationFacet = XmlSchemaEnumerationFacet.Read(reader, h);
              if (enumerationFacet != null)
              {
                xso.facets.Add((XmlSchemaObject) enumerationFacet);
                continue;
              }
              continue;
            }
            if (reader.LocalName == "whiteSpace")
            {
              num = 3;
              XmlSchemaWhiteSpaceFacet schemaWhiteSpaceFacet = XmlSchemaWhiteSpaceFacet.Read(reader, h);
              if (schemaWhiteSpaceFacet != null)
              {
                xso.facets.Add((XmlSchemaObject) schemaWhiteSpaceFacet);
                continue;
              }
              continue;
            }
            if (reader.LocalName == "pattern")
            {
              num = 3;
              XmlSchemaPatternFacet schemaPatternFacet = XmlSchemaPatternFacet.Read(reader, h);
              if (schemaPatternFacet != null)
              {
                xso.facets.Add((XmlSchemaObject) schemaPatternFacet);
                continue;
              }
              continue;
            }
          }
          if (num <= 4)
          {
            if (reader.LocalName == "attribute")
            {
              num = 4;
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
              num = 4;
              XmlSchemaAttributeGroupRef attributeGroupRef = XmlSchemaAttributeGroupRef.Read(reader, h);
              if (attributeGroupRef != null)
              {
                xso.attributes.Add((XmlSchemaObject) attributeGroupRef);
                continue;
              }
              continue;
            }
          }
          if (num <= 5 && reader.LocalName == "anyAttribute")
          {
            num = 6;
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
