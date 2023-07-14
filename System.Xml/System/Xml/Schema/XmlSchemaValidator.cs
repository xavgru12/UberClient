// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaValidator
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml.Schema;
using System.Collections;
using System.IO;
using System.Text;

namespace System.Xml.Schema
{
  public sealed class XmlSchemaValidator
  {
    private static readonly XmlSchemaAttribute[] emptyAttributeArray = new XmlSchemaAttribute[0];
    private object nominalEventSender;
    private IXmlLineInfo lineInfo;
    private IXmlNamespaceResolver nsResolver;
    private Uri sourceUri;
    private XmlNameTable nameTable;
    private XmlSchemaSet schemas;
    private XmlResolver xmlResolver = (XmlResolver) new XmlUrlResolver();
    private XmlSchemaObject startType;
    private XmlSchemaValidationFlags options;
    private XmlSchemaValidator.Transition transition;
    private XsdParticleStateManager state;
    private ArrayList occuredAtts = new ArrayList();
    private XmlSchemaAttribute[] defaultAttributes = XmlSchemaValidator.emptyAttributeArray;
    private ArrayList defaultAttributesCache = new ArrayList();
    private XsdIDManager idManager = new XsdIDManager();
    private ArrayList keyTables = new ArrayList();
    private ArrayList currentKeyFieldConsumers = new ArrayList();
    private ArrayList tmpKeyrefPool;
    private ArrayList elementQNameStack = new ArrayList();
    private StringBuilder storedCharacters = new StringBuilder();
    private bool shouldValidateCharacters;
    private int depth;
    private int xsiNilDepth = -1;
    private int skipValidationDepth = -1;
    internal XmlSchemaDatatype CurrentAttributeType;

    public XmlSchemaValidator(
      XmlNameTable nameTable,
      XmlSchemaSet schemas,
      IXmlNamespaceResolver nsResolver,
      XmlSchemaValidationFlags options)
    {
      this.nameTable = nameTable;
      this.schemas = schemas;
      this.nsResolver = nsResolver;
      this.options = options;
    }

    public event System.Xml.Schema.ValidationEventHandler ValidationEventHandler;

    public object ValidationEventSender
    {
      get => this.nominalEventSender;
      set => this.nominalEventSender = value;
    }

    public IXmlLineInfo LineInfoProvider
    {
      get => this.lineInfo;
      set => this.lineInfo = value;
    }

    public XmlResolver XmlResolver
    {
      set => this.xmlResolver = value;
    }

    public Uri SourceUri
    {
      get => this.sourceUri;
      set => this.sourceUri = value;
    }

    private string BaseUri => this.sourceUri != (Uri) null ? this.sourceUri.AbsoluteUri : string.Empty;

    private XsdValidationContext Context => this.state.Context;

    private bool IgnoreWarnings => (this.options & XmlSchemaValidationFlags.ReportValidationWarnings) == XmlSchemaValidationFlags.None;

    private bool IgnoreIdentity => (this.options & XmlSchemaValidationFlags.ProcessIdentityConstraints) == XmlSchemaValidationFlags.None;

    public XmlSchemaAttribute[] GetExpectedAttributes()
    {
      if (!(this.Context.ActualType is XmlSchemaComplexType actualType))
        return XmlSchemaValidator.emptyAttributeArray;
      ArrayList arrayList = new ArrayList();
      foreach (DictionaryEntry attributeUse in actualType.AttributeUses)
      {
        if (!this.occuredAtts.Contains((object) (XmlQualifiedName) attributeUse.Key))
          arrayList.Add(attributeUse.Value);
      }
      return (XmlSchemaAttribute[]) arrayList.ToArray(typeof (XmlSchemaAttribute));
    }

    private void CollectAtomicParticles(XmlSchemaParticle p, ArrayList al)
    {
      if (p is XmlSchemaGroupBase)
      {
        foreach (XmlSchemaParticle p1 in ((XmlSchemaGroupBase) p).Items)
          this.CollectAtomicParticles(p1, al);
      }
      else
        al.Add((object) p);
    }

    [MonoTODO]
    public XmlSchemaParticle[] GetExpectedParticles()
    {
      ArrayList al1 = new ArrayList();
      this.Context.State.GetExpectedParticles(al1);
      ArrayList al2 = new ArrayList();
      foreach (XmlSchemaParticle p in al1)
        this.CollectAtomicParticles(p, al2);
      return (XmlSchemaParticle[]) al2.ToArray(typeof (XmlSchemaParticle));
    }

    public void GetUnspecifiedDefaultAttributes(ArrayList defaultAttributeList)
    {
      if (defaultAttributeList == null)
        throw new ArgumentNullException(nameof (defaultAttributeList));
      if (this.transition != XmlSchemaValidator.Transition.StartTag)
        throw new InvalidOperationException("Method 'GetUnsoecifiedDefaultAttributes' works only when the validator state is inside a start tag.");
      foreach (XmlSchemaAttribute expectedAttribute in this.GetExpectedAttributes())
      {
        if (expectedAttribute.ValidatedDefaultValue != null || expectedAttribute.ValidatedFixedValue != null)
          defaultAttributeList.Add((object) expectedAttribute);
      }
      defaultAttributeList.AddRange((ICollection) this.defaultAttributes);
    }

    public void AddSchema(XmlSchema schema)
    {
      if (schema == null)
        throw new ArgumentNullException(nameof (schema));
      this.schemas.Add(schema);
      this.schemas.Compile();
    }

    public void Initialize()
    {
      this.transition = XmlSchemaValidator.Transition.Content;
      this.state = new XsdParticleStateManager();
      if (this.schemas.IsCompiled)
        return;
      this.schemas.Compile();
    }

    public void Initialize(XmlSchemaObject partialValidationType)
    {
      this.startType = partialValidationType != null ? partialValidationType : throw new ArgumentNullException(nameof (partialValidationType));
      this.Initialize();
    }

    public void EndValidation()
    {
      this.CheckState(XmlSchemaValidator.Transition.Content);
      this.transition = XmlSchemaValidator.Transition.Finished;
      if (this.schemas.Count == 0)
        return;
      if (this.depth > 0)
        throw new InvalidOperationException(string.Format("There are {0} open element(s). ValidateEndElement() must be called for each open element.", (object) this.depth));
      if (this.IgnoreIdentity || !this.idManager.HasMissingIDReferences())
        return;
      this.HandleError("There are missing ID references: " + this.idManager.GetMissingIDString());
    }

    [MonoTODO]
    public void SkipToEndElement(XmlSchemaInfo info)
    {
      this.CheckState(XmlSchemaValidator.Transition.Content);
      if (this.schemas.Count == 0)
        return;
      this.state.PopContext();
    }

    public object ValidateAttribute(
      string localName,
      string ns,
      string attributeValue,
      XmlSchemaInfo info)
    {
      if (attributeValue == null)
        throw new ArgumentNullException(nameof (attributeValue));
      return this.ValidateAttribute(localName, ns, (XmlValueGetter) (() => (object) attributeValue), info);
    }

    public object ValidateAttribute(
      string localName,
      string ns,
      XmlValueGetter attributeValue,
      XmlSchemaInfo info)
    {
      if (localName == null)
        throw new ArgumentNullException(nameof (localName));
      if (ns == null)
        throw new ArgumentNullException(nameof (ns));
      if (attributeValue == null)
        throw new ArgumentNullException(nameof (attributeValue));
      this.CheckState(XmlSchemaValidator.Transition.StartTag);
      XmlQualifiedName xmlQualifiedName = new XmlQualifiedName(localName, ns);
      if (this.occuredAtts.Contains((object) xmlQualifiedName))
        throw new InvalidOperationException(string.Format("Attribute '{0}' has already been validated in the same element.", (object) xmlQualifiedName));
      this.occuredAtts.Add((object) xmlQualifiedName);
      if (ns == "http://www.w3.org/2000/xmlns/")
        return (object) null;
      if (this.schemas.Count == 0)
        return (object) null;
      if (this.Context.Element != null && this.Context.XsiType == null)
      {
        if (this.Context.ActualType is XmlSchemaComplexType)
          return this.AssessAttributeElementLocallyValidType(localName, ns, attributeValue, info);
        this.HandleError("Current simple type cannot accept attributes other than schema instance namespace.");
      }
      return (object) null;
    }

    public void ValidateElement(string localName, string ns, XmlSchemaInfo info) => this.ValidateElement(localName, ns, info, (string) null, (string) null, (string) null, (string) null);

    public void ValidateElement(
      string localName,
      string ns,
      XmlSchemaInfo info,
      string xsiType,
      string xsiNil,
      string schemaLocation,
      string noNsSchemaLocation)
    {
      if (localName == null)
        throw new ArgumentNullException(nameof (localName));
      if (ns == null)
        throw new ArgumentNullException(nameof (ns));
      this.CheckState(XmlSchemaValidator.Transition.Content);
      this.transition = XmlSchemaValidator.Transition.StartTag;
      if (schemaLocation != null)
        this.HandleSchemaLocation(schemaLocation);
      if (noNsSchemaLocation != null)
        this.HandleNoNSSchemaLocation(noNsSchemaLocation);
      this.elementQNameStack.Add((object) new XmlQualifiedName(localName, ns));
      if (this.schemas.Count == 0)
        return;
      if (!this.IgnoreIdentity)
        this.idManager.OnStartElement();
      this.defaultAttributes = XmlSchemaValidator.emptyAttributeArray;
      if (this.skipValidationDepth < 0 || this.depth <= this.skipValidationDepth)
      {
        if (this.shouldValidateCharacters)
          this.ValidateEndSimpleContent((XmlSchemaInfo) null);
        this.AssessOpenStartElementSchemaValidity(localName, ns);
      }
      if (xsiNil != null)
        this.HandleXsiNil(xsiNil, info);
      if (xsiType != null)
        this.HandleXsiType(xsiType);
      if (this.xsiNilDepth < this.depth)
        this.shouldValidateCharacters = true;
      if (info == null)
        return;
      info.IsNil = this.xsiNilDepth >= 0;
      info.SchemaElement = this.Context.Element;
      info.SchemaType = this.Context.ActualSchemaType;
      info.SchemaAttribute = (XmlSchemaAttribute) null;
      info.IsDefault = false;
      info.MemberType = (XmlSchemaSimpleType) null;
    }

    public object ValidateEndElement(XmlSchemaInfo info) => this.ValidateEndElement(info, (object) null);

    [MonoTODO]
    public object ValidateEndElement(XmlSchemaInfo info, object var)
    {
      if (this.transition == XmlSchemaValidator.Transition.StartTag)
        this.ValidateEndOfAttributes(info);
      this.CheckState(XmlSchemaValidator.Transition.Content);
      this.elementQNameStack.RemoveAt(this.elementQNameStack.Count - 1);
      if (this.schemas.Count == 0)
        return (object) null;
      if (this.depth == 0)
        throw new InvalidOperationException("There was no corresponding call to 'ValidateElement' method.");
      --this.depth;
      object obj = (object) null;
      if (this.depth == this.skipValidationDepth)
        this.skipValidationDepth = -1;
      else if (this.skipValidationDepth < 0 || this.depth <= this.skipValidationDepth)
        obj = this.AssessEndElementSchemaValidity(info);
      return obj;
    }

    public void ValidateEndOfAttributes(XmlSchemaInfo info)
    {
      try
      {
        this.CheckState(XmlSchemaValidator.Transition.StartTag);
        this.transition = XmlSchemaValidator.Transition.Content;
        if (this.schemas.Count == 0)
          return;
        if (this.skipValidationDepth < 0 || this.depth <= this.skipValidationDepth)
          this.AssessCloseStartElementSchemaValidity(info);
        ++this.depth;
      }
      finally
      {
        this.occuredAtts.Clear();
      }
    }

    public void ValidateText(string value)
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      this.ValidateText((XmlValueGetter) (() => (object) value));
    }

    public void ValidateText(XmlValueGetter getter)
    {
      if (getter == null)
        throw new ArgumentNullException(nameof (getter));
      this.CheckState(XmlSchemaValidator.Transition.Content);
      if (this.schemas.Count == 0 || this.skipValidationDepth >= 0 && this.depth > this.skipValidationDepth)
        return;
      if (this.Context.ActualType is XmlSchemaComplexType actualType)
      {
        switch (actualType.ContentType)
        {
          case XmlSchemaContentType.Empty:
            this.HandleError("Not allowed character content was found.");
            break;
          case XmlSchemaContentType.ElementOnly:
            string str = this.storedCharacters.ToString();
            if (str.Length > 0 && !XmlChar.IsWhitespace(str))
            {
              this.HandleError("Not allowed character content was found.");
              break;
            }
            break;
        }
      }
      this.ValidateCharacters(getter);
    }

    public void ValidateWhitespace(string value)
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      this.ValidateWhitespace((XmlValueGetter) (() => (object) value));
    }

    public void ValidateWhitespace(XmlValueGetter getter) => this.ValidateText(getter);

    private void HandleError(string message) => this.HandleError(message, (Exception) null, false);

    private void HandleError(string message, Exception innerException) => this.HandleError(message, innerException, false);

    private void HandleError(string message, Exception innerException, bool isWarning)
    {
      if (isWarning && this.IgnoreWarnings)
        return;
      this.HandleError(new XmlSchemaValidationException(message, this.nominalEventSender, this.BaseUri, (XmlSchemaObject) null, innerException), isWarning);
    }

    private void HandleError(XmlSchemaValidationException exception) => this.HandleError(exception, false);

    private void HandleError(XmlSchemaValidationException exception, bool isWarning)
    {
      if (isWarning && this.IgnoreWarnings)
        return;
      if (this.ValidationEventHandler == null)
        throw exception;
      this.ValidationEventHandler(this.nominalEventSender, new ValidationEventArgs((XmlSchemaException) exception, exception.Message, !isWarning ? XmlSeverityType.Error : XmlSeverityType.Warning));
    }

    private void CheckState(XmlSchemaValidator.Transition expected)
    {
      if (this.transition == expected)
        return;
      if (this.transition == XmlSchemaValidator.Transition.None)
        throw new InvalidOperationException("Initialize() must be called before processing validation.");
      throw new InvalidOperationException(string.Format("Unexpected attempt to validate state transition from {0} to {1}.", (object) this.transition, (object) expected));
    }

    private XmlSchemaElement FindElement(string name, string ns) => (XmlSchemaElement) this.schemas.GlobalElements[new XmlQualifiedName(name, ns)];

    private XmlSchemaType FindType(XmlQualifiedName qname) => (XmlSchemaType) this.schemas.GlobalTypes[qname];

    private void ValidateStartElementParticle(string localName, string ns)
    {
      if (this.Context.State == null)
        return;
      this.Context.XsiType = (object) null;
      this.state.CurrentElement = (XmlSchemaElement) null;
      this.Context.EvaluateStartElement(localName, ns);
      if (this.Context.IsInvalid)
        this.HandleError("Invalid start element: " + ns + ":" + localName);
      this.Context.PushCurrentElement(this.state.CurrentElement);
    }

    private void AssessOpenStartElementSchemaValidity(string localName, string ns)
    {
      if (this.xsiNilDepth >= 0 && this.xsiNilDepth < this.depth)
        this.HandleError("Element item appeared, while current element context is nil.");
      this.ValidateStartElementParticle(localName, ns);
      if (this.Context.Element == null)
      {
        this.state.CurrentElement = this.FindElement(localName, ns);
        this.Context.PushCurrentElement(this.state.CurrentElement);
      }
      if (this.IgnoreIdentity)
        return;
      this.ValidateKeySelectors();
      this.ValidateKeyFields(false, this.xsiNilDepth == this.depth, this.Context.ActualType, (string) null, (string) null, (object) null);
    }

    private void AssessCloseStartElementSchemaValidity(XmlSchemaInfo info)
    {
      if (this.Context.XsiType != null)
        this.AssessCloseStartElementLocallyValidType(info);
      else if (this.Context.Element != null)
      {
        this.AssessElementLocallyValidElement();
        if (this.Context.Element.ElementType != null)
          this.AssessCloseStartElementLocallyValidType(info);
      }
      if (this.Context.Element == null)
      {
        switch (this.state.ProcessContents)
        {
          case XmlSchemaContentProcessing.Skip:
          case XmlSchemaContentProcessing.Lax:
            break;
          default:
            XmlQualifiedName elementQname = (XmlQualifiedName) this.elementQNameStack[this.elementQNameStack.Count - 1];
            if (this.Context.XsiType == null && (this.schemas.Contains(elementQname.Namespace) || !this.schemas.MissedSubComponents(elementQname.Namespace)))
            {
              this.HandleError("Element declaration for " + (object) elementQname + " is missing.");
              break;
            }
            break;
        }
      }
      this.state.PushContext();
      XsdValidationState xsdValidationState = (XsdValidationState) null;
      if (this.state.ProcessContents == XmlSchemaContentProcessing.Skip)
        this.skipValidationDepth = this.depth;
      else
        xsdValidationState = !(this.Context.ActualType is XmlSchemaComplexType actualType) ? (this.state.ProcessContents != XmlSchemaContentProcessing.Lax ? this.state.Create((XmlSchemaObject) XmlSchemaParticle.Empty) : this.state.Create((XmlSchemaObject) XmlSchemaAny.AnyTypeContent)) : this.state.Create((XmlSchemaObject) actualType.ValidatableParticle);
      this.Context.State = xsdValidationState;
    }

    private void AssessElementLocallyValidElement()
    {
      XmlSchemaElement element = this.Context.Element;
      XmlQualifiedName elementQname = (XmlQualifiedName) this.elementQNameStack[this.elementQNameStack.Count - 1];
      if (element == null)
        this.HandleError("Element declaration is required for " + (object) elementQname);
      if (!element.ActualIsAbstract)
        return;
      this.HandleError("Abstract element declaration was specified for " + (object) elementQname);
    }

    private void AssessCloseStartElementLocallyValidType(XmlSchemaInfo info)
    {
      object actualType = this.Context.ActualType;
      if (actualType == null)
      {
        this.HandleError("Schema type does not exist.");
      }
      else
      {
        XmlSchemaComplexType cType = actualType as XmlSchemaComplexType;
        if (actualType is XmlSchemaSimpleType || cType == null)
          return;
        this.AssessCloseStartElementLocallyValidComplexType(cType, info);
      }
    }

    private void AssessCloseStartElementLocallyValidComplexType(
      XmlSchemaComplexType cType,
      XmlSchemaInfo info)
    {
      if (cType.IsAbstract)
      {
        this.HandleError("Target complex type is abstract.");
      }
      else
      {
        foreach (XmlSchemaAttribute expectedAttribute in this.GetExpectedAttributes())
        {
          if (expectedAttribute.ValidatedUse == XmlSchemaUse.Required && expectedAttribute.ValidatedFixedValue == null)
            this.HandleError("Required attribute " + (object) expectedAttribute.QualifiedName + " was not found.");
          else if (expectedAttribute.ValidatedDefaultValue != null || expectedAttribute.ValidatedFixedValue != null)
            this.defaultAttributesCache.Add((object) expectedAttribute);
        }
        this.defaultAttributes = this.defaultAttributesCache.Count != 0 ? (XmlSchemaAttribute[]) this.defaultAttributesCache.ToArray(typeof (XmlSchemaAttribute)) : XmlSchemaValidator.emptyAttributeArray;
        this.defaultAttributesCache.Clear();
        if (!this.IgnoreIdentity)
        {
          foreach (XmlSchemaAttribute defaultAttribute in this.defaultAttributes)
          {
            if (!(defaultAttribute.AttributeType is XmlSchemaDatatype dt))
              dt = defaultAttribute.AttributeSchemaType.Datatype;
            string message = this.idManager.AssessEachAttributeIdentityConstraint(dt, (object) (defaultAttribute.ValidatedFixedValue ?? defaultAttribute.ValidatedDefaultValue), ((XmlQualifiedName) this.elementQNameStack[this.elementQNameStack.Count - 1]).Name);
            if (message != null)
              this.HandleError(message);
          }
        }
        if (this.IgnoreIdentity)
          return;
        foreach (XmlSchemaAttribute defaultAttribute in this.defaultAttributes)
          this.ValidateKeyFieldsAttribute(defaultAttribute, (object) (defaultAttribute.ValidatedFixedValue ?? defaultAttribute.ValidatedDefaultValue));
      }
    }

    private object AssessAttributeElementLocallyValidType(
      string localName,
      string ns,
      XmlValueGetter getter,
      XmlSchemaInfo info)
    {
      XmlSchemaComplexType actualType = this.Context.ActualType as XmlSchemaComplexType;
      XmlQualifiedName qname = new XmlQualifiedName(localName, ns);
      XmlSchemaObject attributeDeclaration = XmlSchemaUtil.FindAttributeDeclaration(ns, this.schemas, actualType, qname);
      if (attributeDeclaration == null)
        this.HandleError("Attribute declaration was not found for " + (object) qname);
      if (!(attributeDeclaration is XmlSchemaAttribute attr))
        return (object) null;
      this.AssessAttributeLocallyValidUse(attr);
      return this.AssessAttributeLocallyValid(attr, info, getter);
    }

    private object AssessAttributeLocallyValid(
      XmlSchemaAttribute attr,
      XmlSchemaInfo info,
      XmlValueGetter getter)
    {
      if (attr.AttributeType == null)
        this.HandleError("Attribute type is missing for " + (object) attr.QualifiedName);
      if (!(attr.AttributeType is XmlSchemaDatatype dt))
        dt = ((XmlSchemaType) attr.AttributeType).Datatype;
      object obj = (object) null;
      if (dt == XmlSchemaSimpleType.AnySimpleType)
      {
        if (attr.ValidatedFixedValue == null)
          goto label_14;
      }
      try
      {
        this.CurrentAttributeType = dt;
        obj = getter();
      }
      catch (Exception ex)
      {
        this.HandleError(string.Format("Attribute value is invalid against its data type {0}", (object) (XmlTokenizedType) (dt == null ? 0 : (int) dt.TokenizedType)), ex);
      }
      if (attr.AttributeType is XmlSchemaSimpleType attributeType)
        this.ValidateRestrictedSimpleTypeValue(attributeType, ref dt, new XmlAtomicValue(obj, (XmlSchemaType) attr.AttributeSchemaType).Value);
      if (attr.ValidatedFixedValue != null)
      {
        if (!XmlSchemaUtil.AreSchemaDatatypeEqual(attr.AttributeSchemaType, attr.ValidatedFixedTypedValue, attr.AttributeSchemaType, obj))
          this.HandleError(string.Format("The value of the attribute {0} does not match with its fixed value '{1}' in the space of type {2}", (object) attr.QualifiedName, (object) attr.ValidatedFixedValue, (object) dt));
        obj = attr.ValidatedFixedTypedValue;
      }
label_14:
      if (!this.IgnoreIdentity)
      {
        string message = this.idManager.AssessEachAttributeIdentityConstraint(dt, obj, ((XmlQualifiedName) this.elementQNameStack[this.elementQNameStack.Count - 1]).Name);
        if (message != null)
          this.HandleError(message);
      }
      if (!this.IgnoreIdentity)
        this.ValidateKeyFieldsAttribute(attr, obj);
      return obj;
    }

    private void AssessAttributeLocallyValidUse(XmlSchemaAttribute attr)
    {
      if (attr.ValidatedUse != XmlSchemaUse.Prohibited)
        return;
      this.HandleError("Attribute " + (object) attr.QualifiedName + " is prohibited in this context.");
    }

    private object AssessEndElementSchemaValidity(XmlSchemaInfo info)
    {
      object obj = this.ValidateEndSimpleContent(info);
      this.ValidateEndElementParticle();
      if (!this.IgnoreIdentity)
        this.ValidateEndElementKeyConstraints();
      if (this.xsiNilDepth == this.depth)
        this.xsiNilDepth = -1;
      return obj;
    }

    private void ValidateEndElementParticle()
    {
      if (this.Context.State != null && !this.Context.EvaluateEndElement())
        this.HandleError("Invalid end element. There are still required content items.");
      this.Context.PopCurrentElement();
      this.state.PopContext();
      this.Context.XsiType = (object) null;
    }

    private void ValidateCharacters(XmlValueGetter getter)
    {
      if (this.xsiNilDepth >= 0 && this.xsiNilDepth < this.depth)
        this.HandleError("Element item appeared, while current element context is nil.");
      if (!this.shouldValidateCharacters)
        return;
      this.CurrentAttributeType = (XmlSchemaDatatype) null;
      this.storedCharacters.Append(getter());
    }

    private object ValidateEndSimpleContent(XmlSchemaInfo info)
    {
      object obj = (object) null;
      if (this.shouldValidateCharacters)
        obj = this.ValidateEndSimpleContentCore(info);
      this.shouldValidateCharacters = false;
      this.storedCharacters.Length = 0;
      return obj;
    }

    private object ValidateEndSimpleContentCore(XmlSchemaInfo info)
    {
      if (this.Context.ActualType == null)
        return (object) null;
      string validatedDefaultValue = this.storedCharacters.ToString();
      object obj = (object) null;
      if (validatedDefaultValue.Length == 0 && this.Context.Element != null && this.Context.Element.ValidatedDefaultValue != null)
        validatedDefaultValue = this.Context.Element.ValidatedDefaultValue;
      XmlSchemaDatatype dt = this.Context.ActualType as XmlSchemaDatatype;
      XmlSchemaSimpleType actualType1 = this.Context.ActualType as XmlSchemaSimpleType;
      if (dt == null)
      {
        if (actualType1 != null)
        {
          dt = actualType1.Datatype;
        }
        else
        {
          XmlSchemaComplexType actualType2 = this.Context.ActualType as XmlSchemaComplexType;
          dt = actualType2.Datatype;
          switch (actualType2.ContentType)
          {
            case XmlSchemaContentType.Empty:
              if (validatedDefaultValue.Length > 0)
              {
                this.HandleError("Character content not allowed in an empty model.");
                break;
              }
              break;
            case XmlSchemaContentType.ElementOnly:
              if (validatedDefaultValue.Length > 0 && !XmlChar.IsWhitespace(validatedDefaultValue))
              {
                this.HandleError("Character content not allowed in an elementOnly model.");
                break;
              }
              break;
          }
        }
      }
      if (dt != null)
      {
        if (this.Context.Element != null && this.Context.Element.ValidatedFixedValue != null && validatedDefaultValue != this.Context.Element.ValidatedFixedValue)
          this.HandleError("Fixed value constraint was not satisfied.");
        obj = this.AssessStringValid(actualType1, dt, validatedDefaultValue);
      }
      if (!this.IgnoreIdentity)
        this.ValidateSimpleContentIdentity(dt, validatedDefaultValue);
      this.shouldValidateCharacters = false;
      if (info != null)
      {
        info.IsNil = this.xsiNilDepth >= 0;
        info.SchemaElement = (XmlSchemaElement) null;
        info.SchemaType = this.Context.ActualType as XmlSchemaType;
        if (info.SchemaType == null)
          info.SchemaType = (XmlSchemaType) XmlSchemaType.GetBuiltInSimpleType(dt.TypeCode);
        info.SchemaAttribute = (XmlSchemaAttribute) null;
        info.IsDefault = false;
        info.MemberType = (XmlSchemaSimpleType) null;
      }
      return obj;
    }

    private object AssessStringValid(XmlSchemaSimpleType st, XmlSchemaDatatype dt, string value)
    {
      XmlSchemaDatatype xmlSchemaDatatype1 = dt;
      object obj = (object) null;
      if (st != null)
      {
        string str = xmlSchemaDatatype1.Normalize(value);
        switch (st.DerivedBy)
        {
          case XmlSchemaDerivationMethod.Restriction:
            if (st.Content is XmlSchemaSimpleTypeRestriction content1)
            {
              if (st.BaseXmlSchemaType is XmlSchemaSimpleType baseXmlSchemaType)
                obj = this.AssessStringValid(baseXmlSchemaType, dt, value);
              if (!content1.ValidateValueWithFacets(value, this.nameTable, this.nsResolver))
              {
                this.HandleError("Specified value was invalid against the facets.");
                break;
              }
            }
            xmlSchemaDatatype1 = st.Datatype;
            break;
          case XmlSchemaDerivationMethod.List:
            XmlSchemaSimpleTypeList content2 = st.Content as XmlSchemaSimpleTypeList;
            string[] strArray = str.Split(XmlChar.WhitespaceChars);
            object[] objArray = new object[strArray.Length];
            XmlSchemaDatatype validatedListItemType1 = content2.ValidatedListItemType as XmlSchemaDatatype;
            XmlSchemaSimpleType validatedListItemType2 = content2.ValidatedListItemType as XmlSchemaSimpleType;
            for (int index = 0; index < strArray.Length; ++index)
            {
              string s = strArray[index];
              if (!(s == string.Empty))
              {
                if (validatedListItemType1 != null)
                {
                  try
                  {
                    objArray[index] = validatedListItemType1.ParseValue(s, this.nameTable, this.nsResolver);
                  }
                  catch (Exception ex)
                  {
                    this.HandleError("List type value contains one or more invalid values.", ex);
                    break;
                  }
                }
                else
                  this.AssessStringValid(validatedListItemType2, validatedListItemType2.Datatype, s);
              }
            }
            obj = (object) objArray;
            break;
          case XmlSchemaDerivationMethod.Union:
            XmlSchemaSimpleTypeUnion content3 = st.Content as XmlSchemaSimpleTypeUnion;
            string s1 = str;
            bool flag = false;
            foreach (object validatedType in content3.ValidatedTypes)
            {
              XmlSchemaDatatype xmlSchemaDatatype2 = validatedType as XmlSchemaDatatype;
              XmlSchemaSimpleType st1 = validatedType as XmlSchemaSimpleType;
              if (xmlSchemaDatatype2 != null)
              {
                try
                {
                  obj = xmlSchemaDatatype2.ParseValue(s1, this.nameTable, this.nsResolver);
                }
                catch (Exception ex)
                {
                  continue;
                }
              }
              else
              {
                try
                {
                  obj = this.AssessStringValid(st1, st1.Datatype, s1);
                }
                catch (XmlSchemaValidationException ex)
                {
                  continue;
                }
              }
              flag = true;
              break;
            }
            if (!flag)
            {
              this.HandleError("Union type value contains one or more invalid values.");
              break;
            }
            break;
        }
      }
      if (xmlSchemaDatatype1 != null)
      {
        try
        {
          obj = xmlSchemaDatatype1.ParseValue(value, this.nameTable, this.nsResolver);
        }
        catch (Exception ex)
        {
          this.HandleError(string.Format("Invalidly typed data was specified."), ex);
        }
      }
      return obj;
    }

    private void ValidateRestrictedSimpleTypeValue(
      XmlSchemaSimpleType st,
      ref XmlSchemaDatatype dt,
      string normalized)
    {
      switch (st.DerivedBy)
      {
        case XmlSchemaDerivationMethod.Restriction:
          if (st.Content is XmlSchemaSimpleTypeRestriction content1)
          {
            if (st.BaseXmlSchemaType is XmlSchemaSimpleType baseXmlSchemaType)
              this.AssessStringValid(baseXmlSchemaType, dt, normalized);
            if (!content1.ValidateValueWithFacets(normalized, this.nameTable, this.nsResolver))
            {
              this.HandleError("Specified value was invalid against the facets.");
              break;
            }
          }
          dt = st.Datatype;
          break;
        case XmlSchemaDerivationMethod.List:
          XmlSchemaSimpleTypeList content2 = st.Content as XmlSchemaSimpleTypeList;
          string[] strArray = normalized.Split(XmlChar.WhitespaceChars);
          XmlSchemaDatatype validatedListItemType1 = content2.ValidatedListItemType as XmlSchemaDatatype;
          XmlSchemaSimpleType validatedListItemType2 = content2.ValidatedListItemType as XmlSchemaSimpleType;
          for (int index = 0; index < strArray.Length; ++index)
          {
            string s = strArray[index];
            if (!(s == string.Empty))
            {
              if (validatedListItemType1 != null)
              {
                try
                {
                  validatedListItemType1.ParseValue(s, this.nameTable, this.nsResolver);
                }
                catch (Exception ex)
                {
                  this.HandleError("List type value contains one or more invalid values.", ex);
                  break;
                }
              }
              else
                this.AssessStringValid(validatedListItemType2, validatedListItemType2.Datatype, s);
            }
          }
          break;
        case XmlSchemaDerivationMethod.Union:
          XmlSchemaSimpleTypeUnion content3 = st.Content as XmlSchemaSimpleTypeUnion;
          string s1 = normalized;
          bool flag = false;
          foreach (object validatedType in content3.ValidatedTypes)
          {
            XmlSchemaDatatype xmlSchemaDatatype = validatedType as XmlSchemaDatatype;
            XmlSchemaSimpleType st1 = validatedType as XmlSchemaSimpleType;
            if (xmlSchemaDatatype != null)
            {
              try
              {
                xmlSchemaDatatype.ParseValue(s1, this.nameTable, this.nsResolver);
              }
              catch (Exception ex)
              {
                continue;
              }
            }
            else
            {
              try
              {
                this.AssessStringValid(st1, st1.Datatype, s1);
              }
              catch (XmlSchemaValidationException ex)
              {
                continue;
              }
            }
            flag = true;
            break;
          }
          if (flag)
            break;
          this.HandleError("Union type value contains one or more invalid values.");
          break;
      }
    }

    private XsdKeyTable CreateNewKeyTable(XmlSchemaIdentityConstraint ident)
    {
      XsdKeyTable newKeyTable = new XsdKeyTable(ident);
      newKeyTable.StartDepth = this.depth;
      this.keyTables.Add((object) newKeyTable);
      return newKeyTable;
    }

    private void ValidateKeySelectors()
    {
      if (this.tmpKeyrefPool != null)
        this.tmpKeyrefPool.Clear();
      if (this.Context.Element != null && this.Context.Element.Constraints.Count > 0)
      {
        for (int index = 0; index < this.Context.Element.Constraints.Count; ++index)
        {
          XmlSchemaIdentityConstraint constraint = (XmlSchemaIdentityConstraint) this.Context.Element.Constraints[index];
          XsdKeyTable newKeyTable = this.CreateNewKeyTable(constraint);
          if (constraint is XmlSchemaKeyref)
          {
            if (this.tmpKeyrefPool == null)
              this.tmpKeyrefPool = new ArrayList();
            this.tmpKeyrefPool.Add((object) newKeyTable);
          }
        }
      }
      for (int index = 0; index < this.keyTables.Count; ++index)
      {
        XsdKeyTable keyTable = (XsdKeyTable) this.keyTables[index];
        if (keyTable.SelectorMatches(this.elementQNameStack, this.depth) != null)
        {
          XsdKeyEntry entry = new XsdKeyEntry(keyTable, this.depth, this.lineInfo);
          keyTable.Entries.Add(entry);
        }
      }
    }

    private void ValidateKeyFieldsAttribute(XmlSchemaAttribute attr, object value) => this.ValidateKeyFields(true, false, attr.AttributeType, attr.QualifiedName.Name, attr.QualifiedName.Namespace, value);

    private void ValidateKeyFields(
      bool isAttr,
      bool isNil,
      object schemaType,
      string attrName,
      string attrNs,
      object value)
    {
      for (int index = 0; index < this.keyTables.Count; ++index)
      {
        XsdKeyTable keyTable = (XsdKeyTable) this.keyTables[index];
        for (int i = 0; i < keyTable.Entries.Count; ++i)
        {
          this.CurrentAttributeType = (XmlSchemaDatatype) null;
          try
          {
            keyTable.Entries[i].ProcessMatch(isAttr, this.elementQNameStack, this.nominalEventSender, this.nameTable, this.BaseUri, schemaType, this.nsResolver, this.lineInfo, !isAttr ? this.depth : this.depth + 1, attrName, attrNs, value, isNil, this.currentKeyFieldConsumers);
          }
          catch (XmlSchemaValidationException ex)
          {
            this.HandleError(ex);
          }
        }
      }
    }

    private void ValidateEndElementKeyConstraints()
    {
      for (int index1 = 0; index1 < this.keyTables.Count; ++index1)
      {
        XsdKeyTable keyTable = this.keyTables[index1] as XsdKeyTable;
        if (keyTable.StartDepth == this.depth)
        {
          this.ValidateEndKeyConstraint(keyTable);
        }
        else
        {
          for (int index2 = 0; index2 < keyTable.Entries.Count; ++index2)
          {
            XsdKeyEntry entry = keyTable.Entries[index2];
            if (entry.StartDepth == this.depth)
            {
              if (entry.KeyFound)
                keyTable.FinishedEntries.Add(entry);
              else if (keyTable.SourceSchemaIdentity is XmlSchemaKey)
                this.HandleError("Key sequence is missing.");
              keyTable.Entries.RemoveAt(index2);
              --index2;
            }
            else
            {
              for (int i = 0; i < entry.KeyFields.Count; ++i)
              {
                XsdKeyEntryField keyField = entry.KeyFields[i];
                if (!keyField.FieldFound && keyField.FieldFoundDepth == this.depth)
                {
                  keyField.FieldFoundDepth = 0;
                  keyField.FieldFoundPath = (XsdIdentityPath) null;
                }
              }
            }
          }
        }
      }
      for (int index = 0; index < this.keyTables.Count; ++index)
      {
        if ((this.keyTables[index] as XsdKeyTable).StartDepth == this.depth)
        {
          this.keyTables.RemoveAt(index);
          --index;
        }
      }
    }

    private void ValidateEndKeyConstraint(XsdKeyTable seq)
    {
      ArrayList arrayList = new ArrayList();
      for (int i = 0; i < seq.Entries.Count; ++i)
      {
        XsdKeyEntry entry = seq.Entries[i];
        if (!entry.KeyFound && seq.SourceSchemaIdentity is XmlSchemaKey)
          arrayList.Add((object) ("line " + (object) entry.SelectorLineNumber + "position " + (object) entry.SelectorLinePosition));
      }
      if (arrayList.Count > 0)
        this.HandleError("Invalid identity constraints were found. Key was not found. " + string.Join(", ", arrayList.ToArray(typeof (string)) as string[]));
      arrayList.Clear();
      if (!(seq.SourceSchemaIdentity is XmlSchemaKeyref sourceSchemaIdentity))
        return;
      for (int index = this.keyTables.Count - 1; index >= 0; --index)
      {
        XsdKeyTable keyTable = this.keyTables[index] as XsdKeyTable;
        if (keyTable.SourceSchemaIdentity == sourceSchemaIdentity.Target)
        {
          seq.ReferencedKey = keyTable;
          for (int i1 = 0; i1 < seq.FinishedEntries.Count; ++i1)
          {
            XsdKeyEntry finishedEntry1 = seq.FinishedEntries[i1];
            for (int i2 = 0; i2 < keyTable.FinishedEntries.Count; ++i2)
            {
              XsdKeyEntry finishedEntry2 = keyTable.FinishedEntries[i2];
              if (finishedEntry1.CompareIdentity(finishedEntry2))
              {
                finishedEntry1.KeyRefFound = true;
                break;
              }
            }
          }
        }
      }
      if (seq.ReferencedKey == null)
        this.HandleError("Target key was not found.");
      for (int i = 0; i < seq.FinishedEntries.Count; ++i)
      {
        XsdKeyEntry finishedEntry = seq.FinishedEntries[i];
        if (!finishedEntry.KeyRefFound)
          arrayList.Add((object) (" line " + (object) finishedEntry.SelectorLineNumber + ", position " + (object) finishedEntry.SelectorLinePosition));
      }
      if (arrayList.Count <= 0)
        return;
      this.HandleError("Invalid identity constraints were found. Referenced key was not found: " + string.Join(" / ", arrayList.ToArray(typeof (string)) as string[]));
    }

    private void ValidateSimpleContentIdentity(XmlSchemaDatatype dt, string value)
    {
      if (this.currentKeyFieldConsumers == null)
        return;
      while (this.currentKeyFieldConsumers.Count > 0)
      {
        XsdKeyEntryField keyFieldConsumer = this.currentKeyFieldConsumers[0] as XsdKeyEntryField;
        if (keyFieldConsumer.Identity != null)
          this.HandleError("Two or more identical field was found. Former value is '" + keyFieldConsumer.Identity + "' .");
        object identity = (object) null;
        if (dt != null)
        {
          try
          {
            identity = dt.ParseValue(value, this.nameTable, this.nsResolver);
          }
          catch (Exception ex)
          {
            this.HandleError("Identity value is invalid against its data type " + (object) dt.TokenizedType, ex);
          }
        }
        if (identity == null)
          identity = (object) value;
        if (!keyFieldConsumer.SetIdentityField(identity, this.depth == this.xsiNilDepth, dt as XsdAnySimpleType, this.depth, this.lineInfo))
          this.HandleError("Two or more identical key value was found: '" + value + "' .");
        this.currentKeyFieldConsumers.RemoveAt(0);
      }
    }

    private object GetXsiType(string name)
    {
      XmlQualifiedName qname = XmlQualifiedName.Parse(name, this.nsResolver, true);
      return !(qname == XmlSchemaComplexType.AnyTypeName) ? (!XmlSchemaUtil.IsBuiltInDatatypeName(qname) ? (object) this.FindType(qname) : (object) XmlSchemaDatatype.FromName(qname)) : (object) XmlSchemaComplexType.AnyType;
    }

    private void HandleXsiType(string typename)
    {
      XmlSchemaElement element = this.Context.Element;
      object xsiType = this.GetXsiType(typename);
      if (xsiType == null)
      {
        this.HandleError("The instance type was not found: " + typename);
      }
      else
      {
        if (xsiType is XmlSchemaType xmlSchemaType && this.Context.Element != null)
        {
          if (element.ElementType is XmlSchemaType elementType && (xmlSchemaType.DerivedBy & elementType.FinalResolved) != XmlSchemaDerivationMethod.Empty)
            this.HandleError("The instance type is prohibited by the type of the context element.");
          if (elementType != xsiType && (xmlSchemaType.DerivedBy & element.BlockResolved) != XmlSchemaDerivationMethod.Empty)
            this.HandleError("The instance type is prohibited by the context element.");
        }
        if (xsiType is XmlSchemaComplexType schemaComplexType && schemaComplexType.IsAbstract)
        {
          this.HandleError("The instance type is abstract: " + typename);
        }
        else
        {
          if (element != null)
            this.AssessLocalTypeDerivationOK(xsiType, element.ElementType, element.BlockResolved);
          this.Context.XsiType = xsiType;
        }
      }
    }

    private void AssessLocalTypeDerivationOK(
      object xsiType,
      object baseType,
      XmlSchemaDerivationMethod flag)
    {
      XmlSchemaType xmlSchemaType = xsiType as XmlSchemaType;
      XmlSchemaComplexType schemaComplexType1 = baseType as XmlSchemaComplexType;
      XmlSchemaComplexType schemaComplexType2 = xmlSchemaType as XmlSchemaComplexType;
      if (xsiType != baseType)
      {
        if (schemaComplexType1 != null)
          flag |= schemaComplexType1.BlockResolved;
        if (flag == XmlSchemaDerivationMethod.All)
        {
          this.HandleError("Prohibited element type substitution.");
          return;
        }
        if (xmlSchemaType != null && (flag & xmlSchemaType.DerivedBy) != XmlSchemaDerivationMethod.Empty)
        {
          this.HandleError("Prohibited element type substitution.");
          return;
        }
      }
      if (schemaComplexType2 != null)
      {
        try
        {
          schemaComplexType2.ValidateTypeDerivationOK(baseType, (System.Xml.Schema.ValidationEventHandler) null, (XmlSchema) null);
        }
        catch (XmlSchemaValidationException ex)
        {
          this.HandleError(ex);
        }
      }
      else
      {
        switch (xsiType)
        {
          case XmlSchemaSimpleType schemaSimpleType:
            try
            {
              schemaSimpleType.ValidateTypeDerivationOK(baseType, (System.Xml.Schema.ValidationEventHandler) null, (XmlSchema) null, true);
              break;
            }
            catch (XmlSchemaValidationException ex)
            {
              this.HandleError(ex);
              break;
            }
          case XmlSchemaDatatype _:
            break;
          default:
            this.HandleError("Primitive data type cannot be derived type using xsi:type specification.");
            break;
        }
      }
    }

    private void HandleXsiNil(string value, XmlSchemaInfo info)
    {
      XmlSchemaElement element = this.Context.Element;
      if (!element.ActualIsNillable)
      {
        this.HandleError(string.Format("Current element '{0}' is not nillable and thus does not allow occurence of 'nil' attribute.", (object) this.Context.Element.QualifiedName));
      }
      else
      {
        value = value.Trim(XmlChar.WhitespaceChars);
        if (!(value == "true"))
          return;
        if (element.ValidatedFixedValue != null)
          this.HandleError("Schema instance nil was specified, where the element declaration for " + (object) element.QualifiedName + "has fixed value constraints.");
        this.xsiNilDepth = this.depth;
        if (info == null)
          return;
        info.IsNil = true;
      }
    }

    private XmlSchema ReadExternalSchema(string uri)
    {
      Uri absoluteUri = new Uri(this.SourceUri, uri.Trim(XmlChar.WhitespaceChars));
      XmlTextReader rdr = (XmlTextReader) null;
      try
      {
        rdr = new XmlTextReader(absoluteUri.ToString(), (Stream) this.xmlResolver.GetEntity(absoluteUri, (string) null, typeof (Stream)), this.nameTable);
        return XmlSchema.Read((XmlReader) rdr, this.ValidationEventHandler);
      }
      finally
      {
        rdr?.Close();
      }
    }

    private void HandleSchemaLocation(string schemaLocation)
    {
      if (this.xmlResolver == null)
        return;
      bool flag = false;
      string[] strArray;
      try
      {
        schemaLocation = XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.Token).Datatype.ParseValue(schemaLocation, (XmlNameTable) null, (IXmlNamespaceResolver) null) as string;
        strArray = schemaLocation.Split(XmlChar.WhitespaceChars);
      }
      catch (Exception ex)
      {
        this.HandleError("Invalid schemaLocation attribute format.", ex, true);
        strArray = new string[0];
      }
      if (strArray.Length % 2 != 0)
        this.HandleError("Invalid schemaLocation attribute format.");
      for (int index = 0; index < strArray.Length; index += 2)
      {
        XmlSchema schema;
        try
        {
          schema = this.ReadExternalSchema(strArray[index + 1]);
        }
        catch (Exception ex)
        {
          this.HandleError("Could not resolve schema location URI: " + schemaLocation, ex, true);
          continue;
        }
        if (schema.TargetNamespace == null)
          schema.TargetNamespace = strArray[index];
        else if (schema.TargetNamespace != strArray[index])
          this.HandleError("Specified schema has different target namespace.");
        if (schema != null && !this.schemas.Contains(schema.TargetNamespace))
        {
          flag = true;
          this.schemas.Add(schema);
        }
      }
      if (!flag)
        return;
      this.schemas.Compile();
    }

    private void HandleNoNSSchemaLocation(string noNsSchemaLocation)
    {
      if (this.xmlResolver == null)
        return;
      XmlSchema schema = (XmlSchema) null;
      bool flag = false;
      try
      {
        schema = this.ReadExternalSchema(noNsSchemaLocation);
      }
      catch (Exception ex)
      {
        this.HandleError("Could not resolve schema location URI: " + noNsSchemaLocation, ex, true);
      }
      if (schema != null && schema.TargetNamespace != null)
        this.HandleError("Specified schema has different target namespace.");
      if (schema != null && !this.schemas.Contains(schema.TargetNamespace))
      {
        flag = true;
        this.schemas.Add(schema);
      }
      if (!flag)
        return;
      this.schemas.Compile();
    }

    private enum Transition
    {
      None,
      Content,
      StartTag,
      Finished,
    }
  }
}
