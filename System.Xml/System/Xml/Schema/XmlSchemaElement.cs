// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaElement
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
  public class XmlSchemaElement : XmlSchemaParticle
  {
    private const string xmlname = "element";
    private XmlSchemaDerivationMethod block;
    private XmlSchemaObjectCollection constraints;
    private string defaultValue;
    private object elementType;
    private XmlSchemaType elementSchemaType;
    private XmlSchemaDerivationMethod final;
    private string fixedValue;
    private XmlSchemaForm form;
    private bool isAbstract;
    private bool isNillable;
    private string name;
    private XmlQualifiedName refName;
    private XmlSchemaType schemaType;
    private XmlQualifiedName schemaTypeName;
    private XmlQualifiedName substitutionGroup;
    private XmlSchema schema;
    internal bool parentIsSchema;
    private XmlQualifiedName qName;
    private XmlSchemaDerivationMethod blockResolved;
    private XmlSchemaDerivationMethod finalResolved;
    private XmlSchemaElement referencedElement;
    private ArrayList substitutingElements = new ArrayList();
    private XmlSchemaElement substitutionGroupElement;
    private bool actualIsAbstract;
    private bool actualIsNillable;
    private string validatedDefaultValue;
    private string validatedFixedValue;

    public XmlSchemaElement()
    {
      this.block = XmlSchemaDerivationMethod.None;
      this.final = XmlSchemaDerivationMethod.None;
      this.constraints = new XmlSchemaObjectCollection();
      this.refName = XmlQualifiedName.Empty;
      this.schemaTypeName = XmlQualifiedName.Empty;
      this.substitutionGroup = XmlQualifiedName.Empty;
      this.InitPostCompileInformations();
    }

    private void InitPostCompileInformations()
    {
      this.qName = XmlQualifiedName.Empty;
      this.schema = (XmlSchema) null;
      this.blockResolved = XmlSchemaDerivationMethod.None;
      this.finalResolved = XmlSchemaDerivationMethod.None;
      this.referencedElement = (XmlSchemaElement) null;
      this.substitutingElements.Clear();
      this.substitutionGroupElement = (XmlSchemaElement) null;
      this.actualIsAbstract = false;
      this.actualIsNillable = false;
      this.validatedDefaultValue = (string) null;
      this.validatedFixedValue = (string) null;
    }

    [System.ComponentModel.DefaultValue(false)]
    [XmlAttribute("abstract")]
    public bool IsAbstract
    {
      get => this.isAbstract;
      set => this.isAbstract = value;
    }

    [XmlAttribute("block")]
    [System.ComponentModel.DefaultValue(XmlSchemaDerivationMethod.None)]
    public XmlSchemaDerivationMethod Block
    {
      get => this.block;
      set => this.block = value;
    }

    [XmlAttribute("default")]
    [System.ComponentModel.DefaultValue(null)]
    public string DefaultValue
    {
      get => this.defaultValue;
      set => this.defaultValue = value;
    }

    [XmlAttribute("final")]
    [System.ComponentModel.DefaultValue(XmlSchemaDerivationMethod.None)]
    public XmlSchemaDerivationMethod Final
    {
      get => this.final;
      set => this.final = value;
    }

    [XmlAttribute("fixed")]
    [System.ComponentModel.DefaultValue(null)]
    public string FixedValue
    {
      get => this.fixedValue;
      set => this.fixedValue = value;
    }

    [XmlAttribute("form")]
    [System.ComponentModel.DefaultValue(XmlSchemaForm.None)]
    public XmlSchemaForm Form
    {
      get => this.form;
      set => this.form = value;
    }

    [XmlAttribute("name")]
    [System.ComponentModel.DefaultValue("")]
    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    [XmlAttribute("nillable")]
    [System.ComponentModel.DefaultValue(false)]
    public bool IsNillable
    {
      get => this.isNillable;
      set => this.isNillable = value;
    }

    [XmlAttribute("ref")]
    public XmlQualifiedName RefName
    {
      get => this.refName;
      set => this.refName = value;
    }

    [XmlAttribute("substitutionGroup")]
    public XmlQualifiedName SubstitutionGroup
    {
      get => this.substitutionGroup;
      set => this.substitutionGroup = value;
    }

    [XmlAttribute("type")]
    public XmlQualifiedName SchemaTypeName
    {
      get => this.schemaTypeName;
      set => this.schemaTypeName = value;
    }

    [XmlElement("simpleType", typeof (XmlSchemaSimpleType))]
    [XmlElement("complexType", typeof (XmlSchemaComplexType))]
    public XmlSchemaType SchemaType
    {
      get => this.schemaType;
      set => this.schemaType = value;
    }

    [XmlElement("keyref", typeof (XmlSchemaKeyref))]
    [XmlElement("unique", typeof (XmlSchemaUnique))]
    [XmlElement("key", typeof (XmlSchemaKey))]
    public XmlSchemaObjectCollection Constraints => this.constraints;

    [XmlIgnore]
    public XmlQualifiedName QualifiedName => this.qName;

    [XmlIgnore]
    [Obsolete]
    public object ElementType => this.referencedElement != null ? this.referencedElement.ElementType : this.elementType;

    [XmlIgnore]
    public XmlSchemaType ElementSchemaType => this.referencedElement != null ? this.referencedElement.ElementSchemaType : this.elementSchemaType;

    [XmlIgnore]
    public XmlSchemaDerivationMethod BlockResolved => this.referencedElement != null ? this.referencedElement.BlockResolved : this.blockResolved;

    [XmlIgnore]
    public XmlSchemaDerivationMethod FinalResolved => this.referencedElement != null ? this.referencedElement.FinalResolved : this.finalResolved;

    internal bool ActualIsNillable => this.referencedElement != null ? this.referencedElement.ActualIsNillable : this.actualIsNillable;

    internal bool ActualIsAbstract => this.referencedElement != null ? this.referencedElement.ActualIsAbstract : this.actualIsAbstract;

    internal string ValidatedDefaultValue => this.referencedElement != null ? this.referencedElement.ValidatedDefaultValue : this.validatedDefaultValue;

    internal string ValidatedFixedValue => this.referencedElement != null ? this.referencedElement.ValidatedFixedValue : this.validatedFixedValue;

    internal ArrayList SubstitutingElements => this.referencedElement != null ? this.referencedElement.SubstitutingElements : this.substitutingElements;

    internal XmlSchemaElement SubstitutionGroupElement => this.referencedElement != null ? this.referencedElement.SubstitutionGroupElement : this.substitutionGroupElement;

    internal override void SetParent(XmlSchemaObject parent)
    {
      base.SetParent(parent);
      if (this.SchemaType != null)
        this.SchemaType.SetParent((XmlSchemaObject) this);
      foreach (XmlSchemaObject constraint in this.Constraints)
        constraint.SetParent((XmlSchemaObject) this);
    }

    internal override int Compile(ValidationEventHandler h, XmlSchema schema)
    {
      if (this.CompilationId == schema.CompilationId)
        return 0;
      this.InitPostCompileInformations();
      this.schema = schema;
      if (this.defaultValue != null && this.fixedValue != null)
        this.error(h, "both default and fixed can't be present");
      if (this.parentIsSchema || this.isRedefineChild)
      {
        if (this.refName != (XmlQualifiedName) null && !this.RefName.IsEmpty)
          this.error(h, "ref must be absent");
        if (this.name == null)
          this.error(h, "Required attribute name must be present");
        else if (!XmlSchemaUtil.CheckNCName(this.name))
          this.error(h, "attribute name must be NCName");
        else
          this.qName = new XmlQualifiedName(this.name, this.AncestorSchema.TargetNamespace);
        if (this.form != XmlSchemaForm.None)
          this.error(h, "form must be absent");
        if (this.MinOccursString != null)
          this.error(h, "minOccurs must be absent");
        if (this.MaxOccursString != null)
          this.error(h, "maxOccurs must be absent");
        XmlSchemaDerivationMethod derivationMethod = XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction;
        if (this.final == XmlSchemaDerivationMethod.All)
          this.finalResolved = derivationMethod;
        else if (this.final == XmlSchemaDerivationMethod.None)
        {
          this.finalResolved = XmlSchemaDerivationMethod.Empty;
        }
        else
        {
          if ((this.final | XmlSchemaUtil.FinalAllowed) != XmlSchemaUtil.FinalAllowed)
            this.error(h, "some values for final are invalid in this context");
          this.finalResolved = this.final & derivationMethod;
        }
        if (this.schemaType != null && this.schemaTypeName != (XmlQualifiedName) null && !this.schemaTypeName.IsEmpty)
          this.error(h, "both schemaType and content can't be present");
        if (this.schemaType != null)
        {
          if (this.schemaType is XmlSchemaSimpleType)
            this.errorCount += ((XmlSchemaSimpleType) this.schemaType).Compile(h, schema);
          else if (this.schemaType is XmlSchemaComplexType)
            this.errorCount += ((XmlSchemaComplexType) this.schemaType).Compile(h, schema);
          else
            this.error(h, "only simpletype or complextype is allowed");
        }
        if (this.schemaTypeName != (XmlQualifiedName) null && !this.schemaTypeName.IsEmpty && !XmlSchemaUtil.CheckQName(this.SchemaTypeName))
          this.error(h, "SchemaTypeName must be an XmlQualifiedName");
        if (this.SubstitutionGroup != (XmlQualifiedName) null && !this.SubstitutionGroup.IsEmpty && !XmlSchemaUtil.CheckQName(this.SubstitutionGroup))
          this.error(h, "SubstitutionGroup must be a valid XmlQualifiedName");
        foreach (XmlSchemaObject constraint in this.constraints)
        {
          switch (constraint)
          {
            case XmlSchemaUnique _:
              this.errorCount += ((XmlSchemaUnique) constraint).Compile(h, schema);
              continue;
            case XmlSchemaKey _:
              this.errorCount += ((XmlSchemaKey) constraint).Compile(h, schema);
              continue;
            case XmlSchemaKeyref _:
              this.errorCount += ((XmlSchemaKeyref) constraint).Compile(h, schema);
              continue;
            default:
              continue;
          }
        }
      }
      else
      {
        if (this.substitutionGroup != (XmlQualifiedName) null && !this.substitutionGroup.IsEmpty)
          this.error(h, "substitutionGroup must be absent");
        if (this.final != XmlSchemaDerivationMethod.None)
          this.error(h, "final must be absent");
        this.CompileOccurence(h, schema);
        if (this.refName == (XmlQualifiedName) null || this.RefName.IsEmpty)
        {
          string ns = string.Empty;
          if (this.form == XmlSchemaForm.Qualified || this.form == XmlSchemaForm.None && this.AncestorSchema.ElementFormDefault == XmlSchemaForm.Qualified)
            ns = this.AncestorSchema.TargetNamespace;
          if (this.name == null)
            this.error(h, "Required attribute name must be present");
          else if (!XmlSchemaUtil.CheckNCName(this.name))
            this.error(h, "attribute name must be NCName");
          else
            this.qName = new XmlQualifiedName(this.name, ns);
          if (this.schemaType != null && this.schemaTypeName != (XmlQualifiedName) null && !this.schemaTypeName.IsEmpty)
            this.error(h, "both schemaType and content can't be present");
          if (this.schemaType != null)
          {
            if (this.schemaType is XmlSchemaSimpleType)
              this.errorCount += ((XmlSchemaSimpleType) this.schemaType).Compile(h, schema);
            else if (this.schemaType is XmlSchemaComplexType)
              this.errorCount += ((XmlSchemaComplexType) this.schemaType).Compile(h, schema);
            else
              this.error(h, "only simpletype or complextype is allowed");
          }
          if (this.schemaTypeName != (XmlQualifiedName) null && !this.schemaTypeName.IsEmpty && !XmlSchemaUtil.CheckQName(this.SchemaTypeName))
            this.error(h, "SchemaTypeName must be an XmlQualifiedName");
          if (this.SubstitutionGroup != (XmlQualifiedName) null && !this.SubstitutionGroup.IsEmpty && !XmlSchemaUtil.CheckQName(this.SubstitutionGroup))
            this.error(h, "SubstitutionGroup must be a valid XmlQualifiedName");
          foreach (XmlSchemaObject constraint in this.constraints)
          {
            switch (constraint)
            {
              case XmlSchemaUnique _:
                this.errorCount += ((XmlSchemaUnique) constraint).Compile(h, schema);
                continue;
              case XmlSchemaKey _:
                this.errorCount += ((XmlSchemaKey) constraint).Compile(h, schema);
                continue;
              case XmlSchemaKeyref _:
                this.errorCount += ((XmlSchemaKeyref) constraint).Compile(h, schema);
                continue;
              default:
                continue;
            }
          }
        }
        else
        {
          if (!XmlSchemaUtil.CheckQName(this.RefName))
            this.error(h, "RefName must be a XmlQualifiedName");
          if (this.name != null)
            this.error(h, "name must not be present when ref is present");
          if (this.Constraints.Count != 0)
            this.error(h, "key, keyref and unique must be absent");
          if (this.isNillable)
            this.error(h, "nillable must be absent");
          if (this.defaultValue != null)
            this.error(h, "default must be absent");
          if (this.fixedValue != null)
            this.error(h, "fixed must be null");
          if (this.form != XmlSchemaForm.None)
            this.error(h, "form must be absent");
          if (this.block != XmlSchemaDerivationMethod.None)
            this.error(h, "block must be absent");
          if (this.schemaTypeName != (XmlQualifiedName) null && !this.schemaTypeName.IsEmpty)
            this.error(h, "type must be absent");
          if (this.SchemaType != null)
            this.error(h, "simpleType or complexType must be absent");
          this.qName = this.RefName;
        }
      }
      switch (this.block)
      {
        case XmlSchemaDerivationMethod.All:
          this.blockResolved = XmlSchemaDerivationMethod.All;
          break;
        case XmlSchemaDerivationMethod.None:
          this.blockResolved = XmlSchemaDerivationMethod.Empty;
          break;
        default:
          if ((this.block | XmlSchemaUtil.ElementBlockAllowed) != XmlSchemaUtil.ElementBlockAllowed)
            this.error(h, "Some of the values for block are invalid in this context");
          this.blockResolved = this.block;
          break;
      }
      if (this.Constraints != null)
      {
        XmlSchemaObjectTable table = new XmlSchemaObjectTable();
        foreach (XmlSchemaIdentityConstraint constraint in this.Constraints)
          XmlSchemaUtil.AddToTable(table, (XmlSchemaObject) constraint, constraint.QualifiedName, h);
      }
      XmlSchemaUtil.CompileID(this.Id, (XmlSchemaObject) this, schema.IDCollection, h);
      this.CompilationId = schema.CompilationId;
      return this.errorCount;
    }

    internal override XmlSchemaParticle GetOptimizedParticle(bool isTop)
    {
      if (this.OptimizedParticle != null)
        return this.OptimizedParticle;
      if (this.RefName != (XmlQualifiedName) null && this.RefName != XmlQualifiedName.Empty)
        this.referencedElement = this.schema.FindElement(this.RefName);
      if (this.ValidatedMaxOccurs == 0M)
        this.OptimizedParticle = XmlSchemaParticle.Empty;
      else if (this.SubstitutingElements != null && this.SubstitutingElements.Count > 0)
      {
        XmlSchemaChoice xmlSchemaChoice = new XmlSchemaChoice();
        xmlSchemaChoice.MinOccurs = this.MinOccurs;
        xmlSchemaChoice.MaxOccurs = this.MaxOccurs;
        xmlSchemaChoice.Compile((ValidationEventHandler) null, this.schema);
        XmlSchemaElement xmlSchemaElement = this.MemberwiseClone() as XmlSchemaElement;
        xmlSchemaElement.MinOccurs = 1M;
        xmlSchemaElement.MaxOccurs = 1M;
        xmlSchemaElement.substitutionGroupElement = (XmlSchemaElement) null;
        xmlSchemaElement.substitutingElements = (ArrayList) null;
        for (int index = 0; index < this.SubstitutingElements.Count; ++index)
        {
          XmlSchemaElement substitutingElement = this.SubstitutingElements[index] as XmlSchemaElement;
          this.AddSubstElementRecursively(xmlSchemaChoice.Items, substitutingElement);
          this.AddSubstElementRecursively(xmlSchemaChoice.CompiledItems, substitutingElement);
        }
        if (!xmlSchemaChoice.Items.Contains((XmlSchemaObject) xmlSchemaElement))
        {
          xmlSchemaChoice.Items.Add((XmlSchemaObject) xmlSchemaElement);
          xmlSchemaChoice.CompiledItems.Add((XmlSchemaObject) xmlSchemaElement);
        }
        this.OptimizedParticle = (XmlSchemaParticle) xmlSchemaChoice;
      }
      else
        this.OptimizedParticle = (XmlSchemaParticle) this;
      return this.OptimizedParticle;
    }

    private void AddSubstElementRecursively(XmlSchemaObjectCollection col, XmlSchemaElement el)
    {
      if (el.SubstitutingElements != null)
      {
        for (int index = 0; index < el.SubstitutingElements.Count; ++index)
          this.AddSubstElementRecursively(col, el.SubstitutingElements[index] as XmlSchemaElement);
      }
      if (col.Contains((XmlSchemaObject) el))
        return;
      col.Add((XmlSchemaObject) el);
    }

    internal void FillSubstitutionElementInfo()
    {
      if (this.substitutionGroupElement != null || !(this.SubstitutionGroup != XmlQualifiedName.Empty))
        return;
      XmlSchemaElement element = this.schema.FindElement(this.SubstitutionGroup);
      this.substitutionGroupElement = element;
      element?.substitutingElements.Add((object) this);
    }

    internal override int Validate(ValidationEventHandler h, XmlSchema schema)
    {
      if (this.IsValidated(schema.CompilationId))
        return this.errorCount;
      this.actualIsNillable = this.IsNillable;
      this.actualIsAbstract = this.IsAbstract;
      if (this.SubstitutionGroup != XmlQualifiedName.Empty)
        this.substitutionGroupElement?.Validate(h, schema);
      XmlSchemaDatatype xmlSchemaDatatype = (XmlSchemaDatatype) null;
      if (this.schemaType != null)
        this.elementType = (object) this.schemaType;
      else if (this.SchemaTypeName != XmlQualifiedName.Empty)
      {
        XmlSchemaType schemaType = schema.FindSchemaType(this.SchemaTypeName);
        if (schemaType != null)
        {
          schemaType.Validate(h, schema);
          this.elementType = (object) schemaType;
        }
        else if (this.SchemaTypeName == XmlSchemaComplexType.AnyTypeName)
          this.elementType = (object) XmlSchemaComplexType.AnyType;
        else if (XmlSchemaUtil.IsBuiltInDatatypeName(this.SchemaTypeName))
        {
          xmlSchemaDatatype = XmlSchemaDatatype.FromName(this.SchemaTypeName);
          if (xmlSchemaDatatype == null)
            this.error(h, "Invalid schema datatype was specified.");
          else
            this.elementType = (object) xmlSchemaDatatype;
        }
        else if (!schema.IsNamespaceAbsent(this.SchemaTypeName.Namespace))
          this.error(h, "Referenced element schema type " + (object) this.SchemaTypeName + " was not found in the corresponding schema.");
      }
      else if (this.RefName != XmlQualifiedName.Empty)
      {
        XmlSchemaElement element = schema.FindElement(this.RefName);
        if (element != null)
        {
          this.referencedElement = element;
          this.errorCount += element.Validate(h, schema);
        }
        else if (!schema.IsNamespaceAbsent(this.RefName.Namespace))
          this.error(h, "Referenced element " + (object) this.RefName + " was not found in the corresponding schema.");
      }
      if (this.referencedElement == null)
      {
        if (this.elementType == null && this.substitutionGroupElement != null)
          this.elementType = this.substitutionGroupElement.ElementType;
        if (this.elementType == null)
          this.elementType = (object) XmlSchemaComplexType.AnyType;
      }
      if (this.elementType is XmlSchemaType elementType)
      {
        this.errorCount += elementType.Validate(h, schema);
        xmlSchemaDatatype = elementType.Datatype;
      }
      if (this.SubstitutionGroup != XmlQualifiedName.Empty)
      {
        XmlSchemaElement element = schema.FindElement(this.SubstitutionGroup);
        if (element != null)
        {
          if (element.ElementType is XmlSchemaType)
          {
            if ((element.FinalResolved & XmlSchemaDerivationMethod.Substitution) != XmlSchemaDerivationMethod.Empty)
              this.error(h, "Substituted element blocks substitution.");
            if (elementType != null && (element.FinalResolved & elementType.DerivedBy) != XmlSchemaDerivationMethod.Empty)
              this.error(h, "Invalid derivation was found. Substituted element prohibits this derivation method: " + (object) elementType.DerivedBy + ".");
          }
          if (elementType is XmlSchemaComplexType schemaComplexType)
            schemaComplexType.ValidateTypeDerivationOK(element.ElementType, h, schema);
          else if (elementType is XmlSchemaSimpleType schemaSimpleType)
            schemaSimpleType.ValidateTypeDerivationOK(element.ElementType, h, schema, true);
        }
        else if (!schema.IsNamespaceAbsent(this.SubstitutionGroup.Namespace))
          this.error(h, "Referenced element type " + (object) this.SubstitutionGroup + " was not found in the corresponding schema.");
      }
      if (this.defaultValue != null || this.fixedValue != null)
      {
        this.ValidateElementDefaultValidImmediate(h, schema);
        if (xmlSchemaDatatype != null && xmlSchemaDatatype.TokenizedType == XmlTokenizedType.ID)
          this.error(h, "Element type is ID, which does not allows default or fixed values.");
      }
      foreach (XmlSchemaObject constraint in this.Constraints)
        constraint.Validate(h, schema);
      if (this.elementType != null)
      {
        this.elementSchemaType = this.elementType as XmlSchemaType;
        if (this.elementType == XmlSchemaSimpleType.AnySimpleType)
          this.elementSchemaType = (XmlSchemaType) XmlSchemaSimpleType.XsAnySimpleType;
        if (this.elementSchemaType == null)
          this.elementSchemaType = (XmlSchemaType) XmlSchemaType.GetBuiltInSimpleType(this.SchemaTypeName);
      }
      this.ValidationId = schema.ValidationId;
      return this.errorCount;
    }

    internal override bool ParticleEquals(XmlSchemaParticle other)
    {
      if (!(other is XmlSchemaElement xmlSchemaElement) || this.ValidatedMaxOccurs != xmlSchemaElement.ValidatedMaxOccurs || this.ValidatedMinOccurs != xmlSchemaElement.ValidatedMinOccurs || this.QualifiedName != xmlSchemaElement.QualifiedName || this.ElementType != xmlSchemaElement.ElementType || this.Constraints.Count != xmlSchemaElement.Constraints.Count)
        return false;
      for (int index1 = 0; index1 < this.Constraints.Count; ++index1)
      {
        XmlSchemaIdentityConstraint constraint1 = this.Constraints[index1] as XmlSchemaIdentityConstraint;
        XmlSchemaIdentityConstraint constraint2 = xmlSchemaElement.Constraints[index1] as XmlSchemaIdentityConstraint;
        if (constraint1.QualifiedName != constraint2.QualifiedName || constraint1.Selector.XPath != constraint2.Selector.XPath || constraint1.Fields.Count != constraint2.Fields.Count)
          return false;
        for (int index2 = 0; index2 < constraint1.Fields.Count; ++index2)
        {
          if ((constraint1.Fields[index2] as XmlSchemaXPath).XPath != (constraint2.Fields[index2] as XmlSchemaXPath).XPath)
            return false;
        }
      }
      return this.BlockResolved == xmlSchemaElement.BlockResolved && this.FinalResolved == xmlSchemaElement.FinalResolved && !(this.ValidatedDefaultValue != xmlSchemaElement.ValidatedDefaultValue) && !(this.ValidatedFixedValue != xmlSchemaElement.ValidatedFixedValue);
    }

    internal override bool ValidateDerivationByRestriction(
      XmlSchemaParticle baseParticle,
      ValidationEventHandler h,
      XmlSchema schema,
      bool raiseError)
    {
      switch (baseParticle)
      {
        case XmlSchemaElement baseElement:
          return this.ValidateDerivationByRestrictionNameAndTypeOK(baseElement, h, schema, raiseError);
        case XmlSchemaAny other:
          return other.ValidateWildcardAllowsNamespaceName(this.QualifiedName.Namespace, h, schema, raiseError) && this.ValidateOccurenceRangeOK((XmlSchemaParticle) other, h, schema, raiseError);
        default:
          XmlSchemaGroupBase xmlSchemaGroupBase = (XmlSchemaGroupBase) null;
          switch (baseParticle)
          {
            case XmlSchemaSequence _:
              xmlSchemaGroupBase = (XmlSchemaGroupBase) new XmlSchemaSequence();
              break;
            case XmlSchemaChoice _:
              xmlSchemaGroupBase = (XmlSchemaGroupBase) new XmlSchemaChoice();
              break;
            case XmlSchemaAll _:
              xmlSchemaGroupBase = (XmlSchemaGroupBase) new XmlSchemaAll();
              break;
          }
          if (xmlSchemaGroupBase == null)
            return true;
          xmlSchemaGroupBase.Items.Add((XmlSchemaObject) this);
          xmlSchemaGroupBase.Compile(h, schema);
          xmlSchemaGroupBase.Validate(h, schema);
          return xmlSchemaGroupBase.ValidateDerivationByRestriction(baseParticle, h, schema, raiseError);
      }
    }

    private bool ValidateDerivationByRestrictionNameAndTypeOK(
      XmlSchemaElement baseElement,
      ValidationEventHandler h,
      XmlSchema schema,
      bool raiseError)
    {
      if (this.QualifiedName != baseElement.QualifiedName)
      {
        if (raiseError)
          this.error(h, "Invalid derivation by restriction of particle was found. Both elements must have the same name.");
        return false;
      }
      if (this.isNillable && !baseElement.isNillable)
      {
        if (raiseError)
          this.error(h, "Invalid element derivation by restriction of particle was found. Base element is not nillable and derived type is nillable.");
        return false;
      }
      if (!this.ValidateOccurenceRangeOK((XmlSchemaParticle) baseElement, h, schema, raiseError))
        return false;
      if (baseElement.ValidatedFixedValue != null && baseElement.ValidatedFixedValue != this.ValidatedFixedValue)
      {
        if (raiseError)
          this.error(h, "Invalid element derivation by restriction of particle was found. Both fixed value must be the same.");
        return false;
      }
      if ((baseElement.BlockResolved | this.BlockResolved) != this.BlockResolved)
      {
        if (raiseError)
          this.error(h, "Invalid derivation by restriction of particle was found. Derived element must contain all of the base element's block value.");
        return false;
      }
      if (baseElement.ElementType != null)
      {
        if (this.ElementType is XmlSchemaComplexType elementType2)
        {
          elementType2.ValidateDerivationValidRestriction(baseElement.ElementType as XmlSchemaComplexType, h, schema);
          elementType2.ValidateTypeDerivationOK(baseElement.ElementType, h, schema);
        }
        else if (this.ElementType is XmlSchemaSimpleType elementType1)
          elementType1.ValidateTypeDerivationOK(baseElement.ElementType, h, schema, true);
        else if (baseElement.ElementType != XmlSchemaComplexType.AnyType && baseElement.ElementType != this.ElementType)
        {
          if (raiseError)
            this.error(h, "Invalid element derivation by restriction of particle was found. Both primitive types differ.");
          return false;
        }
      }
      return true;
    }

    internal override void CheckRecursion(int depth, ValidationEventHandler h, XmlSchema schema)
    {
      if (!(this.ElementType is XmlSchemaComplexType elementType) || elementType.Particle == null)
        return;
      elementType.Particle.CheckRecursion(depth + 1, h, schema);
    }

    internal override void ValidateUniqueParticleAttribution(
      XmlSchemaObjectTable qnames,
      ArrayList nsNames,
      ValidationEventHandler h,
      XmlSchema schema)
    {
      if (qnames.Contains(this.QualifiedName))
      {
        this.error(h, "Ambiguous element label was detected: " + (object) this.QualifiedName);
      }
      else
      {
        foreach (XmlSchemaAny nsName in nsNames)
        {
          if (!(nsName.ValidatedMaxOccurs == 0M))
          {
            if (nsName.HasValueAny || nsName.HasValueLocal && this.QualifiedName.Namespace == string.Empty || nsName.HasValueOther && this.QualifiedName.Namespace != this.QualifiedName.Namespace || nsName.HasValueTargetNamespace && this.QualifiedName.Namespace == this.QualifiedName.Namespace)
            {
              this.error(h, "Ambiguous element label which is contained by -any- particle was detected: " + (object) this.QualifiedName);
              break;
            }
            if (!nsName.HasValueOther)
            {
              bool flag = false;
              foreach (string resolvedNamespace in nsName.ResolvedNamespaces)
              {
                if (resolvedNamespace == this.QualifiedName.Namespace)
                {
                  flag = true;
                  break;
                }
              }
              if (flag)
              {
                this.error(h, "Ambiguous element label which is contained by -any- particle was detected: " + (object) this.QualifiedName);
                break;
              }
            }
            else if (nsName.TargetNamespace != this.QualifiedName.Namespace)
              this.error(h, string.Format("Ambiguous element label '{0}' which is contained by -any- particle with ##other value than '{1}' was detected: ", (object) this.QualifiedName.Namespace, (object) nsName.TargetNamespace));
          }
        }
        qnames.Add(this.QualifiedName, (XmlSchemaObject) this);
      }
    }

    internal override void ValidateUniqueTypeAttribution(
      XmlSchemaObjectTable labels,
      ValidationEventHandler h,
      XmlSchema schema)
    {
      if (!(labels[this.QualifiedName] is XmlSchemaElement label))
      {
        labels.Add(this.QualifiedName, (XmlSchemaObject) this);
      }
      else
      {
        if (label.ElementType == this.ElementType)
          return;
        this.error(h, "Different types are specified on the same named elements in the same sequence. Element name is " + (object) this.QualifiedName);
      }
    }

    private void ValidateElementDefaultValidImmediate(ValidationEventHandler h, XmlSchema schema)
    {
      XmlSchemaDatatype xmlSchemaDatatype = this.elementType as XmlSchemaDatatype;
      if (this.elementType is XmlSchemaSimpleType elementType)
        xmlSchemaDatatype = elementType.Datatype;
      if (xmlSchemaDatatype == null)
      {
        switch ((this.elementType as XmlSchemaComplexType).ContentType)
        {
          case XmlSchemaContentType.Empty:
          case XmlSchemaContentType.ElementOnly:
            this.error(h, "Element content type must be simple type or mixed.");
            break;
        }
        xmlSchemaDatatype = (XmlSchemaDatatype) XmlSchemaSimpleType.AnySimpleType;
      }
      XmlNamespaceManager nsmgr = (XmlNamespaceManager) null;
      if (xmlSchemaDatatype.TokenizedType == XmlTokenizedType.QName)
      {
        if (this.Namespaces != null)
        {
          foreach (XmlQualifiedName xmlQualifiedName in this.Namespaces.ToArray())
            nsmgr.AddNamespace(xmlQualifiedName.Name, xmlQualifiedName.Namespace);
        }
      }
      try
      {
        if (this.defaultValue != null)
        {
          this.validatedDefaultValue = xmlSchemaDatatype.Normalize(this.defaultValue);
          xmlSchemaDatatype.ParseValue(this.validatedDefaultValue, (XmlNameTable) null, (IXmlNamespaceResolver) nsmgr);
        }
      }
      catch (Exception ex)
      {
        XmlSchemaObject.error(h, "The Element's default value is invalid with respect to its type definition.", ex);
      }
      try
      {
        if (this.fixedValue == null)
          return;
        this.validatedFixedValue = xmlSchemaDatatype.Normalize(this.fixedValue);
        xmlSchemaDatatype.ParseValue(this.validatedFixedValue, (XmlNameTable) null, (IXmlNamespaceResolver) nsmgr);
      }
      catch (Exception ex)
      {
        XmlSchemaObject.error(h, "The Element's fixed value is invalid with its type definition.", ex);
      }
    }

    internal static XmlSchemaElement Read(XmlSchemaReader reader, ValidationEventHandler h)
    {
      XmlSchemaElement xso = new XmlSchemaElement();
      reader.MoveToElement();
      if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "element")
      {
        XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaElement.Read, name=" + reader.Name, (Exception) null);
        reader.Skip();
        return (XmlSchemaElement) null;
      }
      xso.LineNumber = reader.LineNumber;
      xso.LinePosition = reader.LinePosition;
      xso.SourceUri = reader.BaseURI;
      Exception innerException;
      while (reader.MoveToNextAttribute())
      {
        if (reader.Name == "abstract")
        {
          xso.IsAbstract = XmlSchemaUtil.ReadBoolAttribute((XmlReader) reader, out innerException);
          if (innerException != null)
            XmlSchemaObject.error(h, reader.Value + " is invalid value for abstract", innerException);
        }
        else if (reader.Name == "block")
        {
          xso.block = XmlSchemaUtil.ReadDerivationAttribute((XmlReader) reader, out innerException, "block", XmlSchemaUtil.ElementBlockAllowed);
          if (innerException != null)
            XmlSchemaObject.error(h, "some invalid values for block attribute were found", innerException);
        }
        else if (reader.Name == "default")
          xso.defaultValue = reader.Value;
        else if (reader.Name == "final")
        {
          xso.Final = XmlSchemaUtil.ReadDerivationAttribute((XmlReader) reader, out innerException, "final", XmlSchemaUtil.FinalAllowed);
          if (innerException != null)
            XmlSchemaObject.error(h, "some invalid values for final attribute were found", innerException);
        }
        else if (reader.Name == "fixed")
          xso.fixedValue = reader.Value;
        else if (reader.Name == "form")
        {
          xso.form = XmlSchemaUtil.ReadFormAttribute((XmlReader) reader, out innerException);
          if (innerException != null)
            XmlSchemaObject.error(h, reader.Value + " is an invalid value for form attribute", innerException);
        }
        else if (reader.Name == "id")
          xso.Id = reader.Value;
        else if (reader.Name == "maxOccurs")
        {
          try
          {
            xso.MaxOccursString = reader.Value;
          }
          catch (Exception ex)
          {
            XmlSchemaObject.error(h, reader.Value + " is an invalid value for maxOccurs", ex);
          }
        }
        else if (reader.Name == "minOccurs")
        {
          try
          {
            xso.MinOccursString = reader.Value;
          }
          catch (Exception ex)
          {
            XmlSchemaObject.error(h, reader.Value + " is an invalid value for minOccurs", ex);
          }
        }
        else if (reader.Name == "name")
          xso.Name = reader.Value;
        else if (reader.Name == "nillable")
        {
          xso.IsNillable = XmlSchemaUtil.ReadBoolAttribute((XmlReader) reader, out innerException);
          if (innerException != null)
            XmlSchemaObject.error(h, reader.Value + "is not a valid value for nillable", innerException);
        }
        else if (reader.Name == "ref")
        {
          xso.refName = XmlSchemaUtil.ReadQNameAttribute((XmlReader) reader, out innerException);
          if (innerException != null)
            XmlSchemaObject.error(h, reader.Value + " is not a valid value for ref attribute", innerException);
        }
        else if (reader.Name == "substitutionGroup")
        {
          xso.substitutionGroup = XmlSchemaUtil.ReadQNameAttribute((XmlReader) reader, out innerException);
          if (innerException != null)
            XmlSchemaObject.error(h, reader.Value + " is not a valid value for substitutionGroup attribute", innerException);
        }
        else if (reader.Name == "type")
        {
          xso.SchemaTypeName = XmlSchemaUtil.ReadQNameAttribute((XmlReader) reader, out innerException);
          if (innerException != null)
            XmlSchemaObject.error(h, reader.Value + " is not a valid value for type attribute", innerException);
        }
        else if (reader.NamespaceURI == string.Empty && reader.Name != "xmlns" || reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
          XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for element", (Exception) null);
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
          if (reader.LocalName != "element")
          {
            XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaElement.Read, name=" + reader.Name, (Exception) null);
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
            if (reader.LocalName == "simpleType")
            {
              num = 3;
              XmlSchemaSimpleType schemaSimpleType = XmlSchemaSimpleType.Read(reader, h);
              if (schemaSimpleType != null)
              {
                xso.SchemaType = (XmlSchemaType) schemaSimpleType;
                continue;
              }
              continue;
            }
            if (reader.LocalName == "complexType")
            {
              num = 3;
              XmlSchemaComplexType schemaComplexType = XmlSchemaComplexType.Read(reader, h);
              if (schemaComplexType != null)
              {
                xso.SchemaType = (XmlSchemaType) schemaComplexType;
                continue;
              }
              continue;
            }
          }
          if (num <= 3)
          {
            if (reader.LocalName == "unique")
            {
              num = 3;
              XmlSchemaUnique xmlSchemaUnique = XmlSchemaUnique.Read(reader, h);
              if (xmlSchemaUnique != null)
              {
                xso.constraints.Add((XmlSchemaObject) xmlSchemaUnique);
                continue;
              }
              continue;
            }
            if (reader.LocalName == "key")
            {
              num = 3;
              XmlSchemaKey xmlSchemaKey = XmlSchemaKey.Read(reader, h);
              if (xmlSchemaKey != null)
              {
                xso.constraints.Add((XmlSchemaObject) xmlSchemaKey);
                continue;
              }
              continue;
            }
            if (reader.LocalName == "keyref")
            {
              num = 3;
              XmlSchemaKeyref xmlSchemaKeyref = XmlSchemaKeyref.Read(reader, h);
              if (xmlSchemaKeyref != null)
              {
                xso.constraints.Add((XmlSchemaObject) xmlSchemaKeyref);
                continue;
              }
              continue;
            }
          }
          reader.RaiseInvalidElementError();
        }
      }
      return xso;
    }
  }
}
