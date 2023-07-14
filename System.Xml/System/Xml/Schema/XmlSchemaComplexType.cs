// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaComplexType
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.ComponentModel;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
  public class XmlSchemaComplexType : XmlSchemaType
  {
    private const string xmlname = "complexType";
    private XmlSchemaAnyAttribute anyAttribute;
    private XmlSchemaObjectCollection attributes;
    private XmlSchemaObjectTable attributeUses;
    private XmlSchemaAnyAttribute attributeWildcard;
    private XmlSchemaDerivationMethod block;
    private XmlSchemaDerivationMethod blockResolved;
    private XmlSchemaContentModel contentModel;
    private XmlSchemaParticle validatableParticle;
    private XmlSchemaParticle contentTypeParticle;
    private bool isAbstract;
    private bool isMixed;
    private XmlSchemaParticle particle;
    private XmlSchemaContentType resolvedContentType;
    internal bool ValidatedIsAbstract;
    private static XmlSchemaComplexType anyType;
    internal static readonly XmlQualifiedName AnyTypeName = new XmlQualifiedName(nameof (anyType), "http://www.w3.org/2001/XMLSchema");
    private Guid CollectProcessId;

    public XmlSchemaComplexType()
    {
      this.attributes = new XmlSchemaObjectCollection();
      this.block = XmlSchemaDerivationMethod.None;
      this.attributeUses = new XmlSchemaObjectTable();
      this.validatableParticle = XmlSchemaParticle.Empty;
      this.contentTypeParticle = this.validatableParticle;
    }

    internal bool ParentIsSchema => this.Parent is XmlSchema;

    internal static XmlSchemaComplexType AnyType
    {
      get
      {
        if (XmlSchemaComplexType.anyType == null)
        {
          XmlSchemaComplexType.anyType = new XmlSchemaComplexType();
          XmlSchemaComplexType.anyType.Name = "anyType";
          XmlSchemaComplexType.anyType.QNameInternal = new XmlQualifiedName("anyType", "http://www.w3.org/2001/XMLSchema");
          XmlSchemaComplexType.anyType.validatableParticle = !XmlSchemaUtil.StrictMsCompliant ? (XmlSchemaParticle) XmlSchemaAny.AnyTypeContent : XmlSchemaParticle.Empty;
          XmlSchemaComplexType.anyType.contentTypeParticle = XmlSchemaComplexType.anyType.validatableParticle;
          XmlSchemaComplexType.anyType.DatatypeInternal = (XmlSchemaDatatype) XmlSchemaSimpleType.AnySimpleType;
          XmlSchemaComplexType.anyType.isMixed = true;
          XmlSchemaComplexType.anyType.resolvedContentType = XmlSchemaContentType.Mixed;
        }
        return XmlSchemaComplexType.anyType;
      }
    }

    [DefaultValue(false)]
    [XmlAttribute("abstract")]
    public bool IsAbstract
    {
      get => this.isAbstract;
      set => this.isAbstract = value;
    }

    [DefaultValue(XmlSchemaDerivationMethod.None)]
    [XmlAttribute("block")]
    public XmlSchemaDerivationMethod Block
    {
      get => this.block;
      set => this.block = value;
    }

    [DefaultValue(false)]
    [XmlAttribute("mixed")]
    public override bool IsMixed
    {
      get => this.isMixed;
      set => this.isMixed = value;
    }

    [XmlElement("complexContent", typeof (XmlSchemaComplexContent))]
    [XmlElement("simpleContent", typeof (XmlSchemaSimpleContent))]
    public XmlSchemaContentModel ContentModel
    {
      get => this.contentModel;
      set => this.contentModel = value;
    }

    [XmlElement("choice", typeof (XmlSchemaChoice))]
    [XmlElement("group", typeof (XmlSchemaGroupRef))]
    [XmlElement("sequence", typeof (XmlSchemaSequence))]
    [XmlElement("all", typeof (XmlSchemaAll))]
    public XmlSchemaParticle Particle
    {
      get => this.particle;
      set => this.particle = value;
    }

    [XmlElement("attribute", typeof (XmlSchemaAttribute))]
    [XmlElement("attributeGroup", typeof (XmlSchemaAttributeGroupRef))]
    public XmlSchemaObjectCollection Attributes => this.attributes;

    [XmlElement("anyAttribute")]
    public XmlSchemaAnyAttribute AnyAttribute
    {
      get => this.anyAttribute;
      set => this.anyAttribute = value;
    }

    [XmlIgnore]
    public XmlSchemaContentType ContentType => this.resolvedContentType;

    [XmlIgnore]
    public XmlSchemaParticle ContentTypeParticle => this.contentTypeParticle;

    [XmlIgnore]
    public XmlSchemaDerivationMethod BlockResolved => this.blockResolved;

    [XmlIgnore]
    public XmlSchemaObjectTable AttributeUses => this.attributeUses;

    [XmlIgnore]
    public XmlSchemaAnyAttribute AttributeWildcard => this.attributeWildcard;

    internal XmlSchemaParticle ValidatableParticle => this.contentTypeParticle;

    internal override void SetParent(XmlSchemaObject parent)
    {
      base.SetParent(parent);
      if (this.ContentModel != null)
        this.ContentModel.SetParent((XmlSchemaObject) this);
      if (this.Particle != null)
        this.Particle.SetParent((XmlSchemaObject) this);
      if (this.AnyAttribute != null)
        this.AnyAttribute.SetParent((XmlSchemaObject) this);
      foreach (XmlSchemaObject attribute in this.Attributes)
        attribute.SetParent((XmlSchemaObject) this);
    }

    internal override int Compile(ValidationEventHandler h, XmlSchema schema)
    {
      if (this.CompilationId == schema.CompilationId)
        return this.errorCount;
      this.ValidatedIsAbstract = this.isAbstract;
      this.attributeUses.Clear();
      if (this.isRedefinedComponent)
      {
        if (this.Annotation != null)
          this.Annotation.isRedefinedComponent = true;
        if (this.AnyAttribute != null)
          this.AnyAttribute.isRedefinedComponent = true;
        foreach (XmlSchemaObject attribute in this.Attributes)
          attribute.isRedefinedComponent = true;
        if (this.ContentModel != null)
          this.ContentModel.isRedefinedComponent = true;
        if (this.Particle != null)
          this.Particle.isRedefinedComponent = true;
      }
      if (this.ParentIsSchema || this.isRedefineChild)
      {
        if (this.Name == null || this.Name == string.Empty)
          this.error(h, "name must be present in a top level complex type");
        else if (!XmlSchemaUtil.CheckNCName(this.Name))
          this.error(h, "name must be a NCName");
        else
          this.QNameInternal = new XmlQualifiedName(this.Name, this.AncestorSchema.TargetNamespace);
        if (this.Block != XmlSchemaDerivationMethod.None)
        {
          if (this.Block == XmlSchemaDerivationMethod.All)
          {
            this.blockResolved = XmlSchemaDerivationMethod.All;
          }
          else
          {
            if ((this.Block & XmlSchemaUtil.ComplexTypeBlockAllowed) != this.Block)
              this.error(h, "Invalid block specification.");
            this.blockResolved = this.Block & XmlSchemaUtil.ComplexTypeBlockAllowed;
          }
        }
        else
        {
          switch (schema.BlockDefault)
          {
            case XmlSchemaDerivationMethod.All:
              this.blockResolved = XmlSchemaDerivationMethod.All;
              break;
            case XmlSchemaDerivationMethod.None:
              this.blockResolved = XmlSchemaDerivationMethod.Empty;
              break;
            default:
              this.blockResolved = schema.BlockDefault & XmlSchemaUtil.ComplexTypeBlockAllowed;
              break;
          }
        }
        if (this.Final != XmlSchemaDerivationMethod.None)
        {
          if (this.Final == XmlSchemaDerivationMethod.All)
            this.finalResolved = XmlSchemaDerivationMethod.All;
          else if ((this.Final & XmlSchemaUtil.FinalAllowed) != this.Final)
            this.error(h, "Invalid final specification.");
          else
            this.finalResolved = this.Final;
        }
        else
        {
          switch (schema.FinalDefault)
          {
            case XmlSchemaDerivationMethod.All:
              this.finalResolved = XmlSchemaDerivationMethod.All;
              break;
            case XmlSchemaDerivationMethod.None:
              this.finalResolved = XmlSchemaDerivationMethod.Empty;
              break;
            default:
              this.finalResolved = schema.FinalDefault & XmlSchemaUtil.FinalAllowed;
              break;
          }
        }
      }
      else
      {
        if (this.isAbstract)
          this.error(h, "abstract must be false in a local complex type");
        if (this.Name != null)
          this.error(h, "name must be absent in a local complex type");
        if (this.Final != XmlSchemaDerivationMethod.None)
          this.error(h, "final must be absent in a local complex type");
        if (this.block != XmlSchemaDerivationMethod.None)
          this.error(h, "block must be absent in a local complex type");
      }
      if (this.contentModel != null)
      {
        if (this.anyAttribute != null || this.Attributes.Count != 0 || this.Particle != null)
          this.error(h, "attributes, particles or anyattribute is not allowed if ContentModel is present");
        this.errorCount += this.contentModel.Compile(h, schema);
        if (this.ContentModel is XmlSchemaSimpleContent contentModel && !(contentModel.Content is XmlSchemaSimpleContentExtension) && contentModel.Content is XmlSchemaSimpleContentRestriction content && content.BaseType != null)
        {
          content.BaseType.Compile(h, schema);
          this.BaseXmlSchemaTypeInternal = (XmlSchemaType) content.BaseType;
        }
      }
      else
      {
        if (this.Particle != null)
          this.errorCount += this.Particle.Compile(h, schema);
        if (this.anyAttribute != null)
          this.AnyAttribute.Compile(h, schema);
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
              this.error(h, attribute.GetType().ToString() + " is not valid in this place::ComplexType");
              continue;
          }
        }
      }
      XmlSchemaUtil.CompileID(this.Id, (XmlSchemaObject) this, schema.IDCollection, h);
      this.CompilationId = schema.CompilationId;
      return this.errorCount;
    }

    private void CollectSchemaComponent(ValidationEventHandler h, XmlSchema schema)
    {
      if (this.CollectProcessId == schema.CompilationId)
        return;
      if (this.contentModel != null)
      {
        this.BaseSchemaTypeName = this.contentModel.Content == null ? XmlQualifiedName.Empty : this.contentModel.Content.GetBaseTypeName();
        this.BaseXmlSchemaTypeInternal = schema.FindSchemaType(this.BaseSchemaTypeName);
      }
      if (this.isRedefineChild && this.BaseXmlSchemaType != null && this.QualifiedName == this.BaseSchemaTypeName)
      {
        XmlSchemaType redefinedObject = (XmlSchemaType) this.redefinedObject;
        if (redefinedObject == null)
          this.error(h, "Redefinition base type was not found.");
        else
          this.BaseXmlSchemaTypeInternal = redefinedObject;
      }
      if (this.contentModel != null && this.contentModel.Content != null)
        this.resolvedDerivedBy = !this.contentModel.Content.IsExtension ? XmlSchemaDerivationMethod.Restriction : XmlSchemaDerivationMethod.Extension;
      else
        this.resolvedDerivedBy = XmlSchemaDerivationMethod.Empty;
    }

    private void FillContentTypeParticle(ValidationEventHandler h, XmlSchema schema)
    {
      if (this.ContentModel != null)
        this.CollectContentTypeFromContentModel(h, schema);
      else
        this.CollectContentTypeFromImmediateContent();
      this.contentTypeParticle = this.validatableParticle.GetOptimizedParticle(true);
      if (this.contentTypeParticle == XmlSchemaParticle.Empty && this.resolvedContentType == XmlSchemaContentType.ElementOnly)
        this.resolvedContentType = XmlSchemaContentType.Empty;
      this.CollectProcessId = schema.CompilationId;
    }

    private void CollectContentTypeFromImmediateContent()
    {
      if (this.Particle != null)
        this.validatableParticle = this.Particle;
      if (this == XmlSchemaComplexType.AnyType)
      {
        this.resolvedContentType = XmlSchemaContentType.Mixed;
      }
      else
      {
        this.resolvedContentType = this.validatableParticle != XmlSchemaParticle.Empty ? (!this.IsMixed ? XmlSchemaContentType.ElementOnly : XmlSchemaContentType.Mixed) : (!this.IsMixed ? XmlSchemaContentType.Empty : XmlSchemaContentType.TextOnly);
        if (this == XmlSchemaComplexType.AnyType)
          return;
        this.BaseXmlSchemaTypeInternal = (XmlSchemaType) XmlSchemaComplexType.AnyType;
      }
    }

    private void CollectContentTypeFromContentModel(ValidationEventHandler h, XmlSchema schema)
    {
      if (this.ContentModel.Content == null)
      {
        this.validatableParticle = XmlSchemaParticle.Empty;
        this.resolvedContentType = XmlSchemaContentType.Empty;
      }
      else
      {
        if (this.ContentModel.Content is XmlSchemaComplexContentExtension)
          this.CollectContentTypeFromComplexExtension(h, schema);
        if (!(this.ContentModel.Content is XmlSchemaComplexContentRestriction))
          return;
        this.CollectContentTypeFromComplexRestriction();
      }
    }

    private void CollectContentTypeFromComplexExtension(ValidationEventHandler h, XmlSchema schema)
    {
      XmlSchemaComplexContentExtension content = (XmlSchemaComplexContentExtension) this.ContentModel.Content;
      if (this.BaseXmlSchemaType is XmlSchemaComplexType schemaComplexType)
        schemaComplexType.CollectSchemaComponent(h, schema);
      if (this.BaseSchemaTypeName == XmlSchemaComplexType.AnyTypeName)
        schemaComplexType = XmlSchemaComplexType.AnyType;
      if (schemaComplexType == null)
      {
        this.validatableParticle = XmlSchemaParticle.Empty;
        this.resolvedContentType = XmlSchemaContentType.Empty;
      }
      else
      {
        if (content.Particle == null || content.Particle == XmlSchemaParticle.Empty)
        {
          if (schemaComplexType == null)
          {
            this.validatableParticle = XmlSchemaParticle.Empty;
            this.resolvedContentType = XmlSchemaContentType.Empty;
          }
          else
          {
            this.validatableParticle = schemaComplexType.ValidatableParticle;
            this.resolvedContentType = schemaComplexType.resolvedContentType;
            if (this.resolvedContentType == XmlSchemaContentType.Empty)
              this.resolvedContentType = this.GetComplexContentType(this.contentModel);
          }
        }
        else if (schemaComplexType.validatableParticle == XmlSchemaParticle.Empty || schemaComplexType == XmlSchemaComplexType.AnyType)
        {
          this.validatableParticle = content.Particle;
          this.resolvedContentType = this.GetComplexContentType(this.contentModel);
        }
        else
        {
          XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
          this.CopyInfo((XmlSchemaParticle) xmlSchemaSequence);
          xmlSchemaSequence.Items.Add((XmlSchemaObject) schemaComplexType.validatableParticle);
          xmlSchemaSequence.Items.Add((XmlSchemaObject) content.Particle);
          xmlSchemaSequence.Compile(h, schema);
          xmlSchemaSequence.Validate(h, schema);
          this.validatableParticle = (XmlSchemaParticle) xmlSchemaSequence;
          this.resolvedContentType = this.GetComplexContentType(this.contentModel);
        }
        if (this.validatableParticle != null)
          return;
        this.validatableParticle = XmlSchemaParticle.Empty;
      }
    }

    private void CollectContentTypeFromComplexRestriction()
    {
      XmlSchemaComplexContentRestriction content = (XmlSchemaComplexContentRestriction) this.ContentModel.Content;
      bool flag = false;
      if (content.Particle == null)
        flag = true;
      else if (content.Particle is XmlSchemaGroupBase particle)
      {
        if (!(particle is XmlSchemaChoice) && particle.Items.Count == 0)
          flag = true;
        else if (particle is XmlSchemaChoice && particle.Items.Count == 0 && particle.ValidatedMinOccurs == 0M)
          flag = true;
      }
      if (flag)
      {
        this.resolvedContentType = XmlSchemaContentType.Empty;
        this.validatableParticle = XmlSchemaParticle.Empty;
      }
      else
      {
        this.resolvedContentType = this.GetComplexContentType(this.contentModel);
        this.validatableParticle = content.Particle;
      }
    }

    private XmlSchemaContentType GetComplexContentType(XmlSchemaContentModel content) => this.IsMixed || ((XmlSchemaComplexContent) content).IsMixed ? XmlSchemaContentType.Mixed : XmlSchemaContentType.ElementOnly;

    internal override int Validate(ValidationEventHandler h, XmlSchema schema)
    {
      if (this.IsValidated(schema.ValidationId))
        return this.errorCount;
      this.ValidationId = schema.ValidationId;
      this.CollectSchemaComponent(h, schema);
      this.ValidateContentFirstPass(h, schema);
      this.FillContentTypeParticle(h, schema);
      if (this.ContentModel != null)
        this.ValidateContentModel(h, schema);
      else
        this.ValidateImmediateAttributes(h, schema);
      if (this.ContentTypeParticle != null && this.contentTypeParticle.GetOptimizedParticle(true) is XmlSchemaAll optimizedParticle && (optimizedParticle.ValidatedMaxOccurs != 1M || this.contentTypeParticle.ValidatedMaxOccurs != 1M))
        this.error(h, "Particle whose term is -all- and consists of complex type content particle must have maxOccurs = 1.");
      if (schema.Schemas.CompilationSettings != null && schema.Schemas.CompilationSettings.EnableUpaCheck)
        this.contentTypeParticle.ValidateUniqueParticleAttribution(new XmlSchemaObjectTable(), new ArrayList(), h, schema);
      this.contentTypeParticle.ValidateUniqueTypeAttribution(new XmlSchemaObjectTable(), h, schema);
      XmlSchemaAttribute xmlSchemaAttribute1 = (XmlSchemaAttribute) null;
      foreach (DictionaryEntry attributeUse in this.attributeUses)
      {
        XmlSchemaAttribute xmlSchemaAttribute2 = (XmlSchemaAttribute) attributeUse.Value;
        if (!(xmlSchemaAttribute2.AttributeType is XmlSchemaDatatype xmlSchemaDatatype) || xmlSchemaDatatype.TokenizedType == XmlTokenizedType.ID)
        {
          if (xmlSchemaDatatype == null)
            xmlSchemaDatatype = ((XmlSchemaType) xmlSchemaAttribute2.AttributeType).Datatype;
          if (xmlSchemaDatatype != null && xmlSchemaDatatype.TokenizedType == XmlTokenizedType.ID)
          {
            if (xmlSchemaAttribute1 != null)
              this.error(h, "Two or more ID typed attribute declarations in a complex type are found.");
            else
              xmlSchemaAttribute1 = xmlSchemaAttribute2;
          }
        }
      }
      this.ValidationId = schema.ValidationId;
      return this.errorCount;
    }

    private void ValidateImmediateAttributes(ValidationEventHandler h, XmlSchema schema)
    {
      this.attributeUses = new XmlSchemaObjectTable();
      XmlSchemaUtil.ValidateAttributesResolved(this.attributeUses, h, schema, this.attributes, this.anyAttribute, ref this.attributeWildcard, (XmlSchemaAttributeGroup) null, false);
    }

    private void ValidateContentFirstPass(ValidationEventHandler h, XmlSchema schema)
    {
      if (this.ContentModel != null)
      {
        this.errorCount += this.contentModel.Validate(h, schema);
        if (this.BaseXmlSchemaTypeInternal == null)
          return;
        this.errorCount += this.BaseXmlSchemaTypeInternal.Validate(h, schema);
      }
      else
      {
        if (this.Particle == null)
          return;
        this.errorCount += this.particle.Validate(h, schema);
        if (!(this.Particle is XmlSchemaGroupRef particle))
          return;
        if (particle.TargetGroup != null)
        {
          this.errorCount += particle.TargetGroup.Validate(h, schema);
        }
        else
        {
          if (schema.IsNamespaceAbsent(particle.RefName.Namespace))
            return;
          this.error(h, "Referenced group " + (object) particle.RefName + " was not found in the corresponding schema.");
        }
      }
    }

    private void ValidateContentModel(ValidationEventHandler h, XmlSchema schema)
    {
      XmlSchemaType schemaTypeInternal = this.BaseXmlSchemaTypeInternal;
      XmlSchemaComplexContentExtension content1 = this.contentModel.Content as XmlSchemaComplexContentExtension;
      XmlSchemaComplexContentRestriction content2 = this.contentModel.Content as XmlSchemaComplexContentRestriction;
      XmlSchemaSimpleContentExtension content3 = this.contentModel.Content as XmlSchemaSimpleContentExtension;
      XmlSchemaSimpleContentRestriction content4 = this.contentModel.Content as XmlSchemaSimpleContentRestriction;
      XmlSchemaAnyAttribute schemaAnyAttribute = (XmlSchemaAnyAttribute) null;
      XmlSchemaAnyAttribute other = (XmlSchemaAnyAttribute) null;
      if (this.ValidateRecursionCheck())
        this.error(h, "Circular definition of schema types was found.");
      if (schemaTypeInternal != null)
        this.DatatypeInternal = schemaTypeInternal.Datatype;
      else if (this.BaseSchemaTypeName == XmlSchemaComplexType.AnyTypeName)
        this.DatatypeInternal = (XmlSchemaDatatype) XmlSchemaSimpleType.AnySimpleType;
      else if (XmlSchemaUtil.IsBuiltInDatatypeName(this.BaseSchemaTypeName))
        this.DatatypeInternal = XmlSchemaDatatype.FromName(this.BaseSchemaTypeName);
      XmlSchemaComplexType schemaComplexType = schemaTypeInternal as XmlSchemaComplexType;
      XmlSchemaSimpleType baseType = schemaTypeInternal as XmlSchemaSimpleType;
      if (schemaTypeInternal != null && (schemaTypeInternal.FinalResolved & this.resolvedDerivedBy) != XmlSchemaDerivationMethod.Empty)
        this.error(h, "Specified derivation is specified as final by derived schema type.");
      if (baseType != null && this.resolvedDerivedBy == XmlSchemaDerivationMethod.Restriction)
        this.error(h, "If the base schema type is a simple type, then this type must be extension.");
      if (content1 != null || content2 != null)
      {
        if (this.BaseSchemaTypeName == XmlSchemaComplexType.AnyTypeName)
          schemaComplexType = XmlSchemaComplexType.AnyType;
        else if (XmlSchemaUtil.IsBuiltInDatatypeName(this.BaseSchemaTypeName))
          this.error(h, "Referenced base schema type is XML Schema datatype.");
        else if (schemaComplexType == null && !schema.IsNamespaceAbsent(this.BaseSchemaTypeName.Namespace))
          this.error(h, "Referenced base schema type " + (object) this.BaseSchemaTypeName + " was not complex type or not found in the corresponding schema.");
      }
      else
      {
        this.resolvedContentType = XmlSchemaContentType.TextOnly;
        if (this.BaseSchemaTypeName == XmlSchemaComplexType.AnyTypeName)
          schemaComplexType = XmlSchemaComplexType.AnyType;
        if (schemaComplexType != null && schemaComplexType.ContentType != XmlSchemaContentType.TextOnly)
          this.error(h, "Base schema complex type of a simple content must be simple content type. Base type is " + (object) this.BaseSchemaTypeName);
        else if (content3 == null && baseType != null && this.BaseSchemaTypeName.Namespace != "http://www.w3.org/2001/XMLSchema")
          this.error(h, "If a simple content is not an extension, base schema type must be complex type. Base type is " + (object) this.BaseSchemaTypeName);
        else if (!XmlSchemaUtil.IsBuiltInDatatypeName(this.BaseSchemaTypeName) && schemaTypeInternal == null && !schema.IsNamespaceAbsent(this.BaseSchemaTypeName.Namespace))
          this.error(h, "Referenced base schema type " + (object) this.BaseSchemaTypeName + " was not found in the corresponding schema.");
        if (schemaComplexType != null)
        {
          if (schemaComplexType.ContentType != XmlSchemaContentType.TextOnly && (content4 == null || schemaComplexType.ContentType != XmlSchemaContentType.Mixed || schemaComplexType.Particle == null || !schemaComplexType.Particle.ValidateIsEmptiable() || content4.BaseType == null))
            this.error(h, "Base complex type of a simple content restriction must be text only.");
        }
        else if (content3 == null || schemaComplexType != null)
          this.error(h, "Not allowed base type of a simple content restriction.");
      }
      if (content1 != null)
      {
        schemaAnyAttribute = content1.AnyAttribute;
        if (schemaComplexType != null)
        {
          foreach (DictionaryEntry attributeUse in schemaComplexType.AttributeUses)
          {
            XmlSchemaAttribute xmlSchemaAttribute = (XmlSchemaAttribute) attributeUse.Value;
            XmlSchemaUtil.AddToTable(this.attributeUses, (XmlSchemaObject) xmlSchemaAttribute, xmlSchemaAttribute.QualifiedName, h);
          }
          other = schemaComplexType.AttributeWildcard;
        }
        this.errorCount += XmlSchemaUtil.ValidateAttributesResolved(this.attributeUses, h, schema, content1.Attributes, content1.AnyAttribute, ref this.attributeWildcard, (XmlSchemaAttributeGroup) null, true);
        if (schemaComplexType != null)
          this.ValidateComplexBaseDerivationValidExtension(schemaComplexType, h, schema);
        else if (baseType != null)
          this.ValidateSimpleBaseDerivationValidExtension((object) baseType, h, schema);
      }
      if (content2 != null)
      {
        if (schemaComplexType == null)
          schemaComplexType = XmlSchemaComplexType.AnyType;
        schemaAnyAttribute = content2.AnyAttribute;
        this.attributeWildcard = schemaAnyAttribute;
        if (schemaComplexType != null)
          other = schemaComplexType.AttributeWildcard;
        if (other != null && schemaAnyAttribute != null)
          schemaAnyAttribute.ValidateWildcardSubset(other, h, schema);
        this.errorCount += XmlSchemaUtil.ValidateAttributesResolved(this.attributeUses, h, schema, content2.Attributes, content2.AnyAttribute, ref this.attributeWildcard, (XmlSchemaAttributeGroup) null, false);
        foreach (DictionaryEntry attributeUse in schemaComplexType.AttributeUses)
        {
          XmlSchemaAttribute xmlSchemaAttribute = (XmlSchemaAttribute) attributeUse.Value;
          if (this.attributeUses[xmlSchemaAttribute.QualifiedName] == null)
            XmlSchemaUtil.AddToTable(this.attributeUses, (XmlSchemaObject) xmlSchemaAttribute, xmlSchemaAttribute.QualifiedName, h);
        }
        this.ValidateDerivationValidRestriction(schemaComplexType, h, schema);
      }
      if (content3 != null)
      {
        this.errorCount += XmlSchemaUtil.ValidateAttributesResolved(this.attributeUses, h, schema, content3.Attributes, content3.AnyAttribute, ref this.attributeWildcard, (XmlSchemaAttributeGroup) null, true);
        schemaAnyAttribute = content3.AnyAttribute;
        if (schemaComplexType != null)
        {
          other = schemaComplexType.AttributeWildcard;
          foreach (DictionaryEntry attributeUse in schemaComplexType.AttributeUses)
          {
            XmlSchemaAttribute xmlSchemaAttribute = (XmlSchemaAttribute) attributeUse.Value;
            XmlSchemaUtil.AddToTable(this.attributeUses, (XmlSchemaObject) xmlSchemaAttribute, xmlSchemaAttribute.QualifiedName, h);
          }
        }
        if (other != null && schemaAnyAttribute != null)
          schemaAnyAttribute.ValidateWildcardSubset(other, h, schema);
      }
      if (content4 != null)
      {
        other = schemaComplexType == null ? (XmlSchemaAnyAttribute) null : schemaComplexType.AttributeWildcard;
        schemaAnyAttribute = content4.AnyAttribute;
        if (schemaAnyAttribute != null && other != null)
          schemaAnyAttribute.ValidateWildcardSubset(other, h, schema);
        this.errorCount += XmlSchemaUtil.ValidateAttributesResolved(this.attributeUses, h, schema, content4.Attributes, content4.AnyAttribute, ref this.attributeWildcard, (XmlSchemaAttributeGroup) null, false);
      }
      if (schemaAnyAttribute != null)
        this.attributeWildcard = schemaAnyAttribute;
      else
        this.attributeWildcard = other;
    }

    internal void ValidateTypeDerivationOK(object b, ValidationEventHandler h, XmlSchema schema)
    {
      if (this == XmlSchemaComplexType.AnyType && this.BaseXmlSchemaType == this)
        return;
      XmlSchemaType xmlSchemaType = b as XmlSchemaType;
      if (b == this)
        return;
      if (xmlSchemaType != null && (this.resolvedDerivedBy & xmlSchemaType.FinalResolved) != XmlSchemaDerivationMethod.Empty)
        this.error(h, "Derivation type " + (object) this.resolvedDerivedBy + " is prohibited by the base type.");
      if (this.BaseSchemaType == b)
        return;
      if (this.BaseSchemaType == null || this.BaseXmlSchemaType == XmlSchemaComplexType.AnyType)
        this.error(h, "Derived type's base schema type is anyType.");
      else if (this.BaseXmlSchemaType is XmlSchemaComplexType baseXmlSchemaType1)
      {
        baseXmlSchemaType1.ValidateTypeDerivationOK(b, h, schema);
      }
      else
      {
        if (!(this.BaseXmlSchemaType is XmlSchemaSimpleType baseXmlSchemaType))
          return;
        baseXmlSchemaType.ValidateTypeDerivationOK(b, h, schema, true);
      }
    }

    internal void ValidateComplexBaseDerivationValidExtension(
      XmlSchemaComplexType baseComplexType,
      ValidationEventHandler h,
      XmlSchema schema)
    {
      if ((baseComplexType.FinalResolved & XmlSchemaDerivationMethod.Extension) != XmlSchemaDerivationMethod.Empty)
        this.error(h, "Derivation by extension is prohibited.");
      foreach (DictionaryEntry attributeUse in baseComplexType.AttributeUses)
      {
        XmlSchemaAttribute xmlSchemaAttribute = (XmlSchemaAttribute) attributeUse.Value;
        if (!(this.AttributeUses[xmlSchemaAttribute.QualifiedName] is XmlSchemaAttribute))
          this.error(h, "Invalid complex type derivation by extension was found. Missing attribute was found: " + (object) xmlSchemaAttribute.QualifiedName + " .");
      }
      if (this.AnyAttribute != null)
      {
        if (baseComplexType.AnyAttribute == null)
          this.error(h, "Invalid complex type derivation by extension was found. Base complex type does not have an attribute wildcard.");
        else
          baseComplexType.AnyAttribute.ValidateWildcardSubset(this.AnyAttribute, h, schema);
      }
      if (baseComplexType.ContentType == XmlSchemaContentType.Empty)
        return;
      if (this.ContentType != baseComplexType.ContentType)
      {
        this.error(h, "Base complex type has different content type " + (object) baseComplexType.ContentType + ".");
      }
      else
      {
        if (this.contentTypeParticle != null && this.contentTypeParticle.ParticleEquals(baseComplexType.ContentTypeParticle))
          return;
        XmlSchemaSequence contentTypeParticle = this.contentTypeParticle as XmlSchemaSequence;
        if (this.contentTypeParticle == XmlSchemaParticle.Empty || contentTypeParticle != null && !(this.contentTypeParticle.ValidatedMinOccurs != 1M) && !(this.contentTypeParticle.ValidatedMaxOccurs != 1M))
          return;
        this.error(h, "Invalid complex content extension was found.");
      }
    }

    internal void ValidateSimpleBaseDerivationValidExtension(
      object baseType,
      ValidationEventHandler h,
      XmlSchema schema)
    {
      if (baseType is XmlSchemaSimpleType schemaSimpleType && (schemaSimpleType.FinalResolved & XmlSchemaDerivationMethod.Extension) != XmlSchemaDerivationMethod.Empty)
        this.error(h, "Extension is prohibited by the base type.");
      if (!(baseType is XmlSchemaDatatype xmlSchemaDatatype))
        xmlSchemaDatatype = schemaSimpleType.Datatype;
      if (xmlSchemaDatatype == this.Datatype)
        return;
      this.error(h, "To extend simple type, a complex type must have the same content type as the base type.");
    }

    internal void ValidateDerivationValidRestriction(
      XmlSchemaComplexType baseType,
      ValidationEventHandler h,
      XmlSchema schema)
    {
      if (baseType == null)
        this.error(h, "Base schema type is not a complex type.");
      else if ((baseType.FinalResolved & XmlSchemaDerivationMethod.Restriction) != XmlSchemaDerivationMethod.Empty)
      {
        this.error(h, "Prohibited derivation by restriction by base schema type.");
      }
      else
      {
        foreach (DictionaryEntry attributeUse1 in this.AttributeUses)
        {
          XmlSchemaAttribute xmlSchemaAttribute = (XmlSchemaAttribute) attributeUse1.Value;
          if (baseType.AttributeUses[xmlSchemaAttribute.QualifiedName] is XmlSchemaAttribute attributeUse2)
          {
            if (attributeUse2.ValidatedUse != XmlSchemaUse.Optional && xmlSchemaAttribute.ValidatedUse != XmlSchemaUse.Required)
              this.error(h, "Invalid attribute derivation by restriction was found for " + (object) xmlSchemaAttribute.QualifiedName + " .");
            XmlSchemaSimpleType attributeType1 = xmlSchemaAttribute.AttributeType as XmlSchemaSimpleType;
            XmlSchemaSimpleType attributeType2 = attributeUse2.AttributeType as XmlSchemaSimpleType;
            bool flag = false;
            if (attributeType1 != null)
              attributeType1.ValidateDerivationValid((object) attributeType2, (XmlSchemaObjectCollection) null, h, schema);
            else if (attributeType1 == null && attributeType2 != null)
            {
              flag = true;
            }
            else
            {
              Type type1 = xmlSchemaAttribute.AttributeType.GetType();
              Type type2 = attributeUse2.AttributeType.GetType();
              if (type1 != type2 && type1.IsSubclassOf(type2))
                flag = true;
            }
            if (flag)
              this.error(h, "Invalid attribute derivation by restriction because of its type: " + (object) xmlSchemaAttribute.QualifiedName + " .");
            if (attributeUse2.ValidatedFixedValue != null && xmlSchemaAttribute.ValidatedFixedValue != attributeUse2.ValidatedFixedValue)
              this.error(h, "Invalid attribute derivation by restriction because of its fixed value constraint: " + (object) xmlSchemaAttribute.QualifiedName + " .");
          }
          else if (baseType.AttributeWildcard != null && !baseType.AttributeWildcard.ValidateWildcardAllowsNamespaceName(xmlSchemaAttribute.QualifiedName.Namespace, schema) && !schema.IsNamespaceAbsent(xmlSchemaAttribute.QualifiedName.Namespace))
            this.error(h, "Invalid attribute derivation by restriction was found for " + (object) xmlSchemaAttribute.QualifiedName + " .");
        }
        if (this.AttributeWildcard != null && baseType != XmlSchemaComplexType.AnyType)
        {
          if (baseType.AttributeWildcard == null)
            this.error(h, "Invalid attribute derivation by restriction because of attribute wildcard.");
          else
            this.AttributeWildcard.ValidateWildcardSubset(baseType.AttributeWildcard, h, schema);
        }
        if (this == XmlSchemaComplexType.AnyType)
          return;
        if (this.contentTypeParticle == XmlSchemaParticle.Empty)
        {
          if (this.ContentType != XmlSchemaContentType.Empty)
          {
            if (baseType.ContentType != XmlSchemaContentType.Mixed || baseType.ContentTypeParticle.ValidateIsEmptiable())
              return;
            this.error(h, "Invalid content type derivation.");
          }
          else
          {
            if (baseType.ContentTypeParticle == XmlSchemaParticle.Empty || baseType.ContentTypeParticle.ValidateIsEmptiable())
              return;
            this.error(h, "Invalid content type derivation.");
          }
        }
        else
        {
          if (baseType.ContentTypeParticle == null || this.contentTypeParticle.ParticleEquals(baseType.ContentTypeParticle))
            return;
          this.contentTypeParticle.ValidateDerivationByRestriction(baseType.ContentTypeParticle, h, schema, true);
        }
      }
    }

    internal static XmlSchemaComplexType Read(XmlSchemaReader reader, ValidationEventHandler h)
    {
      XmlSchemaComplexType xso = new XmlSchemaComplexType();
      reader.MoveToElement();
      if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "complexType")
      {
        XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaComplexType.Read, name=" + reader.Name, (Exception) null);
        reader.SkipToEnd();
        return (XmlSchemaComplexType) null;
      }
      xso.LineNumber = reader.LineNumber;
      xso.LinePosition = reader.LinePosition;
      xso.SourceUri = reader.BaseURI;
      Exception innerExcpetion;
      while (reader.MoveToNextAttribute())
      {
        if (reader.Name == "abstract")
        {
          xso.IsAbstract = XmlSchemaUtil.ReadBoolAttribute((XmlReader) reader, out innerExcpetion);
          if (innerExcpetion != null)
            XmlSchemaObject.error(h, reader.Value + " is invalid value for abstract", innerExcpetion);
        }
        else if (reader.Name == "block")
        {
          xso.block = XmlSchemaUtil.ReadDerivationAttribute((XmlReader) reader, out innerExcpetion, "block", XmlSchemaUtil.ComplexTypeBlockAllowed);
          if (innerExcpetion != null)
            XmlSchemaObject.error(h, "some invalid values for block attribute were found", innerExcpetion);
        }
        else if (reader.Name == "final")
        {
          xso.Final = XmlSchemaUtil.ReadDerivationAttribute((XmlReader) reader, out innerExcpetion, "final", XmlSchemaUtil.FinalAllowed);
          if (innerExcpetion != null)
            XmlSchemaObject.error(h, "some invalid values for final attribute were found", innerExcpetion);
        }
        else if (reader.Name == "id")
          xso.Id = reader.Value;
        else if (reader.Name == "mixed")
        {
          xso.isMixed = XmlSchemaUtil.ReadBoolAttribute((XmlReader) reader, out innerExcpetion);
          if (innerExcpetion != null)
            XmlSchemaObject.error(h, reader.Value + " is invalid value for mixed", innerExcpetion);
        }
        else if (reader.Name == "name")
          xso.Name = reader.Value;
        else if (reader.NamespaceURI == string.Empty && reader.Name != "xmlns" || reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
          XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for complexType", (Exception) null);
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
          if (reader.LocalName != "complexType")
          {
            XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaComplexType.Read, name=" + reader.Name, (Exception) null);
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
            if (reader.LocalName == "simpleContent")
            {
              num = 6;
              XmlSchemaSimpleContent schemaSimpleContent = XmlSchemaSimpleContent.Read(reader, h);
              if (schemaSimpleContent != null)
              {
                xso.ContentModel = (XmlSchemaContentModel) schemaSimpleContent;
                continue;
              }
              continue;
            }
            if (reader.LocalName == "complexContent")
            {
              num = 6;
              XmlSchemaComplexContent schemaComplexContent = XmlSchemaComplexContent.Read(reader, h);
              if (schemaComplexContent != null)
              {
                xso.contentModel = (XmlSchemaContentModel) schemaComplexContent;
                continue;
              }
              continue;
            }
          }
          if (num <= 3)
          {
            if (reader.LocalName == "group")
            {
              num = 4;
              XmlSchemaGroupRef xmlSchemaGroupRef = XmlSchemaGroupRef.Read(reader, h);
              if (xmlSchemaGroupRef != null)
              {
                xso.particle = (XmlSchemaParticle) xmlSchemaGroupRef;
                continue;
              }
              continue;
            }
            if (reader.LocalName == "all")
            {
              num = 4;
              XmlSchemaAll xmlSchemaAll = XmlSchemaAll.Read(reader, h);
              if (xmlSchemaAll != null)
              {
                xso.particle = (XmlSchemaParticle) xmlSchemaAll;
                continue;
              }
              continue;
            }
            if (reader.LocalName == "choice")
            {
              num = 4;
              XmlSchemaChoice xmlSchemaChoice = XmlSchemaChoice.Read(reader, h);
              if (xmlSchemaChoice != null)
              {
                xso.particle = (XmlSchemaParticle) xmlSchemaChoice;
                continue;
              }
              continue;
            }
            if (reader.LocalName == "sequence")
            {
              num = 4;
              XmlSchemaSequence xmlSchemaSequence = XmlSchemaSequence.Read(reader, h);
              if (xmlSchemaSequence != null)
              {
                xso.particle = (XmlSchemaParticle) xmlSchemaSequence;
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
