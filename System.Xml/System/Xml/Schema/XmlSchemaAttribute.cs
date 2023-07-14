// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaAttribute
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml.Serialization;

namespace System.Xml.Schema
{
  public class XmlSchemaAttribute : XmlSchemaAnnotated
  {
    private const string xmlname = "attribute";
    private object attributeType;
    private XmlSchemaSimpleType attributeSchemaType;
    private string defaultValue;
    private string fixedValue;
    private string validatedDefaultValue;
    private string validatedFixedValue;
    private object validatedFixedTypedValue;
    private XmlSchemaForm form;
    private string name;
    private string targetNamespace;
    private XmlQualifiedName qualifiedName;
    private XmlQualifiedName refName;
    private XmlSchemaSimpleType schemaType;
    private XmlQualifiedName schemaTypeName;
    private XmlSchemaUse use;
    private XmlSchemaUse validatedUse;
    private XmlSchemaAttribute referencedAttribute;

    public XmlSchemaAttribute()
    {
      this.form = XmlSchemaForm.None;
      this.use = XmlSchemaUse.None;
      this.schemaTypeName = XmlQualifiedName.Empty;
      this.qualifiedName = XmlQualifiedName.Empty;
      this.refName = XmlQualifiedName.Empty;
    }

    internal bool ParentIsSchema => this.Parent is XmlSchema;

    [System.ComponentModel.DefaultValue(null)]
    [XmlAttribute("default")]
    public string DefaultValue
    {
      get => this.defaultValue;
      set
      {
        this.fixedValue = (string) null;
        this.defaultValue = value;
      }
    }

    [XmlAttribute("fixed")]
    [System.ComponentModel.DefaultValue(null)]
    public string FixedValue
    {
      get => this.fixedValue;
      set
      {
        this.defaultValue = (string) null;
        this.fixedValue = value;
      }
    }

    [System.ComponentModel.DefaultValue(XmlSchemaForm.None)]
    [XmlAttribute("form")]
    public XmlSchemaForm Form
    {
      get => this.form;
      set => this.form = value;
    }

    [XmlAttribute("name")]
    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    [XmlAttribute("ref")]
    public XmlQualifiedName RefName
    {
      get => this.refName;
      set => this.refName = value;
    }

    [XmlAttribute("type")]
    public XmlQualifiedName SchemaTypeName
    {
      get => this.schemaTypeName;
      set => this.schemaTypeName = value;
    }

    [XmlElement("simpleType")]
    public XmlSchemaSimpleType SchemaType
    {
      get => this.schemaType;
      set => this.schemaType = value;
    }

    [System.ComponentModel.DefaultValue(XmlSchemaUse.None)]
    [XmlAttribute("use")]
    public XmlSchemaUse Use
    {
      get => this.use;
      set => this.use = value;
    }

    [XmlIgnore]
    public XmlQualifiedName QualifiedName => this.qualifiedName;

    [Obsolete]
    [XmlIgnore]
    public object AttributeType => this.referencedAttribute != null ? this.referencedAttribute.AttributeType : this.attributeType;

    [XmlIgnore]
    public XmlSchemaSimpleType AttributeSchemaType => this.referencedAttribute != null ? this.referencedAttribute.AttributeSchemaType : this.attributeSchemaType;

    internal string ValidatedDefaultValue => this.validatedDefaultValue;

    internal string ValidatedFixedValue => this.validatedFixedValue;

    internal object ValidatedFixedTypedValue => this.validatedFixedTypedValue;

    internal XmlSchemaUse ValidatedUse => this.validatedUse;

    internal override void SetParent(XmlSchemaObject parent)
    {
      base.SetParent(parent);
      if (this.schemaType == null)
        return;
      this.schemaType.SetParent((XmlSchemaObject) this);
    }

    internal override int Compile(ValidationEventHandler h, XmlSchema schema)
    {
      if (this.CompilationId == schema.CompilationId)
        return 0;
      this.errorCount = 0;
      if (this.ParentIsSchema || this.isRedefineChild)
      {
        if (this.RefName != (XmlQualifiedName) null && !this.RefName.IsEmpty)
          this.error(h, "ref must be absent in the top level <attribute>");
        if (this.Form != XmlSchemaForm.None)
          this.error(h, "form must be absent in the top level <attribute>");
        if (this.Use != XmlSchemaUse.None)
          this.error(h, "use must be absent in the top level <attribute>");
        this.targetNamespace = this.AncestorSchema.TargetNamespace;
        this.CompileCommon(h, schema, true);
      }
      else if (this.RefName == (XmlQualifiedName) null || this.RefName.IsEmpty)
      {
        this.targetNamespace = this.form == XmlSchemaForm.Qualified || this.form == XmlSchemaForm.None && schema.AttributeFormDefault == XmlSchemaForm.Qualified ? this.AncestorSchema.TargetNamespace : string.Empty;
        this.CompileCommon(h, schema, true);
      }
      else
      {
        if (this.name != null)
          this.error(h, "name must be absent if ref is present");
        if (this.form != XmlSchemaForm.None)
          this.error(h, "form must be absent if ref is present");
        if (this.schemaType != null)
          this.error(h, "simpletype must be absent if ref is present");
        if (this.schemaTypeName != (XmlQualifiedName) null && !this.schemaTypeName.IsEmpty)
          this.error(h, "type must be absent if ref is present");
        this.CompileCommon(h, schema, false);
      }
      this.CompilationId = schema.CompilationId;
      return this.errorCount;
    }

    private void CompileCommon(ValidationEventHandler h, XmlSchema schema, bool refIsNotPresent)
    {
      if (refIsNotPresent)
      {
        if (this.Name == null)
          this.error(h, "Required attribute name must be present");
        else if (!XmlSchemaUtil.CheckNCName(this.Name))
          this.error(h, "attribute name must be NCName");
        else if (this.Name == "xmlns")
          this.error(h, "attribute name must not be xmlns");
        else
          this.qualifiedName = new XmlQualifiedName(this.Name, this.targetNamespace);
        if (this.SchemaType != null)
        {
          if (this.SchemaTypeName != (XmlQualifiedName) null && !this.SchemaTypeName.IsEmpty)
            this.error(h, "attribute can't have both a type and <simpleType> content");
          this.errorCount += this.SchemaType.Compile(h, schema);
        }
        if (this.SchemaTypeName != (XmlQualifiedName) null && !XmlSchemaUtil.CheckQName(this.SchemaTypeName))
          this.error(h, this.SchemaTypeName.ToString() + " is not a valid QName");
      }
      else
        this.qualifiedName = !(this.RefName == (XmlQualifiedName) null) && !this.RefName.IsEmpty ? this.RefName : throw new InvalidOperationException("Error: Should Never Happen. refname must be present");
      if (this.AncestorSchema.TargetNamespace == "http://www.w3.org/2001/XMLSchema-instance" && this.Name != "nil" && this.Name != "type" && this.Name != "schemaLocation" && this.Name != "noNamespaceSchemaLocation")
        this.error(h, "targetNamespace can't be http://www.w3.org/2001/XMLSchema-instance");
      if (this.DefaultValue != null && this.FixedValue != null)
        this.error(h, "default and fixed must not both be present in an Attribute");
      if (this.DefaultValue != null && this.Use != XmlSchemaUse.None && this.Use != XmlSchemaUse.Optional)
        this.error(h, "if default is present, use must be optional");
      XmlSchemaUtil.CompileID(this.Id, (XmlSchemaObject) this, schema.IDCollection, h);
    }

    internal override int Validate(ValidationEventHandler h, XmlSchema schema)
    {
      if (this.IsValidated(schema.ValidationId))
        return this.errorCount;
      if (this.SchemaType != null)
      {
        this.SchemaType.Validate(h, schema);
        this.attributeType = (object) this.SchemaType;
      }
      else if (this.SchemaTypeName != (XmlQualifiedName) null && this.SchemaTypeName != XmlQualifiedName.Empty)
      {
        XmlSchemaType schemaType = schema.FindSchemaType(this.SchemaTypeName);
        if (schemaType is XmlSchemaComplexType)
          this.error(h, "An attribute can't have complexType Content");
        else if (schemaType != null)
        {
          this.errorCount += schemaType.Validate(h, schema);
          this.attributeType = (object) schemaType;
        }
        else if (this.SchemaTypeName == XmlSchemaComplexType.AnyTypeName)
          this.attributeType = (object) XmlSchemaComplexType.AnyType;
        else if (XmlSchemaUtil.IsBuiltInDatatypeName(this.SchemaTypeName))
        {
          this.attributeType = (object) XmlSchemaDatatype.FromName(this.SchemaTypeName);
          if (this.attributeType == null)
            this.error(h, "Invalid xml schema namespace datatype was specified.");
        }
        else if (!schema.IsNamespaceAbsent(this.SchemaTypeName.Namespace))
          this.error(h, "Referenced schema type " + (object) this.SchemaTypeName + " was not found in the corresponding schema.");
      }
      if (this.RefName != (XmlQualifiedName) null && this.RefName != XmlQualifiedName.Empty)
      {
        this.referencedAttribute = schema.FindAttribute(this.RefName);
        if (this.referencedAttribute != null)
          this.errorCount += this.referencedAttribute.Validate(h, schema);
        else if (!schema.IsNamespaceAbsent(this.RefName.Namespace))
          this.error(h, "Referenced attribute " + (object) this.RefName + " was not found in the corresponding schema.");
      }
      if (this.attributeType == null)
        this.attributeType = (object) XmlSchemaSimpleType.AnySimpleType;
      if (this.defaultValue != null || this.fixedValue != null)
      {
        if (!(this.attributeType is XmlSchemaDatatype xmlSchemaDatatype))
          xmlSchemaDatatype = ((XmlSchemaType) this.attributeType).Datatype;
        if (xmlSchemaDatatype.TokenizedType == XmlTokenizedType.QName)
        {
          this.error(h, "By the defection of the W3C XML Schema specification, it is impossible to supply QName default or fixed values.");
        }
        else
        {
          try
          {
            if (this.defaultValue != null)
            {
              this.validatedDefaultValue = xmlSchemaDatatype.Normalize(this.defaultValue);
              xmlSchemaDatatype.ParseValue(this.validatedDefaultValue, (XmlNameTable) null, (IXmlNamespaceResolver) null);
            }
          }
          catch (Exception ex)
          {
            XmlSchemaObject.error(h, "The Attribute's default value is invalid with its type definition.", ex);
          }
          try
          {
            if (this.fixedValue != null)
            {
              this.validatedFixedValue = xmlSchemaDatatype.Normalize(this.fixedValue);
              this.validatedFixedTypedValue = xmlSchemaDatatype.ParseValue(this.validatedFixedValue, (XmlNameTable) null, (IXmlNamespaceResolver) null);
            }
          }
          catch (Exception ex)
          {
            XmlSchemaObject.error(h, "The Attribute's fixed value is invalid with its type definition.", ex);
          }
        }
      }
      this.validatedUse = this.Use != XmlSchemaUse.None ? this.Use : XmlSchemaUse.Optional;
      if (this.attributeType != null)
      {
        this.attributeSchemaType = this.attributeType as XmlSchemaSimpleType;
        if (this.attributeType == XmlSchemaSimpleType.AnySimpleType)
          this.attributeSchemaType = XmlSchemaSimpleType.XsAnySimpleType;
        if (this.attributeSchemaType == null)
          this.attributeSchemaType = XmlSchemaType.GetBuiltInSimpleType(this.SchemaTypeName);
      }
      this.ValidationId = schema.ValidationId;
      return this.errorCount;
    }

    internal bool AttributeEquals(XmlSchemaAttribute other) => !(this.Id != other.Id) && !(this.QualifiedName != other.QualifiedName) && this.AttributeType == other.AttributeType && this.ValidatedUse == other.ValidatedUse && !(this.ValidatedDefaultValue != other.ValidatedDefaultValue) && !(this.ValidatedFixedValue != other.ValidatedFixedValue);

    internal static XmlSchemaAttribute Read(XmlSchemaReader reader, ValidationEventHandler h)
    {
      XmlSchemaAttribute xso = new XmlSchemaAttribute();
      reader.MoveToElement();
      if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "attribute")
      {
        XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaAttribute.Read, name=" + reader.Name, (Exception) null);
        reader.SkipToEnd();
        return (XmlSchemaAttribute) null;
      }
      xso.LineNumber = reader.LineNumber;
      xso.LinePosition = reader.LinePosition;
      xso.SourceUri = reader.BaseURI;
      while (reader.MoveToNextAttribute())
      {
        if (reader.Name == "default")
          xso.defaultValue = reader.Value;
        else if (reader.Name == "fixed")
          xso.fixedValue = reader.Value;
        else if (reader.Name == "form")
        {
          Exception innerExcpetion;
          xso.form = XmlSchemaUtil.ReadFormAttribute((XmlReader) reader, out innerExcpetion);
          if (innerExcpetion != null)
            XmlSchemaObject.error(h, reader.Value + " is not a valid value for form attribute", innerExcpetion);
        }
        else if (reader.Name == "id")
          xso.Id = reader.Value;
        else if (reader.Name == "name")
          xso.name = reader.Value;
        else if (reader.Name == "ref")
        {
          Exception innerEx;
          xso.refName = XmlSchemaUtil.ReadQNameAttribute((XmlReader) reader, out innerEx);
          if (innerEx != null)
            XmlSchemaObject.error(h, reader.Value + " is not a valid value for ref attribute", innerEx);
        }
        else if (reader.Name == "type")
        {
          Exception innerEx;
          xso.schemaTypeName = XmlSchemaUtil.ReadQNameAttribute((XmlReader) reader, out innerEx);
          if (innerEx != null)
            XmlSchemaObject.error(h, reader.Value + " is not a valid value for type attribute", innerEx);
        }
        else if (reader.Name == "use")
        {
          Exception innerExcpetion;
          xso.use = XmlSchemaUtil.ReadUseAttribute((XmlReader) reader, out innerExcpetion);
          if (innerExcpetion != null)
            XmlSchemaObject.error(h, reader.Value + " is not a valid value for use attribute", innerExcpetion);
        }
        else if (reader.NamespaceURI == string.Empty && reader.Name != "xmlns" || reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
          XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for attribute", (Exception) null);
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
          if (reader.LocalName != "attribute")
          {
            XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaAttribute.Read, name=" + reader.Name, (Exception) null);
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
            xso.schemaType = schemaSimpleType;
        }
        else
          reader.RaiseInvalidElementError();
      }
      return xso;
    }
  }
}
