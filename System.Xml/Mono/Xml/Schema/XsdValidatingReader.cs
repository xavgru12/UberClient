// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Schema.XsdValidatingReader
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
  internal class XsdValidatingReader : 
    XmlReader,
    IHasXmlParserContext,
    IHasXmlSchemaInfo,
    IXmlLineInfo
  {
    private static readonly XmlSchemaAttribute[] emptyAttributeArray = new XmlSchemaAttribute[0];
    private XmlReader reader;
    private XmlResolver resolver;
    private IHasXmlSchemaInfo sourceReaderSchemaInfo;
    private IXmlLineInfo readerLineInfo;
    private ValidationType validationType;
    private XmlSchemaSet schemas = new XmlSchemaSet();
    private bool namespaces = true;
    private bool validationStarted;
    private bool checkIdentity = true;
    private XsdIDManager idManager = new XsdIDManager();
    private bool checkKeyConstraints = true;
    private ArrayList keyTables = new ArrayList();
    private ArrayList currentKeyFieldConsumers;
    private ArrayList tmpKeyrefPool;
    private ArrayList elementQNameStack = new ArrayList();
    private XsdParticleStateManager state = new XsdParticleStateManager();
    private int skipValidationDepth = -1;
    private int xsiNilDepth = -1;
    private StringBuilder storedCharacters = new StringBuilder();
    private bool shouldValidateCharacters;
    private XmlSchemaAttribute[] defaultAttributes = XsdValidatingReader.emptyAttributeArray;
    private int currentDefaultAttribute = -1;
    private ArrayList defaultAttributesCache = new ArrayList();
    private bool defaultAttributeConsumed;
    private object currentAttrType;
    public ValidationEventHandler ValidationEventHandler;

    public XsdValidatingReader(XmlReader reader)
    {
      this.reader = reader;
      this.readerLineInfo = reader as IXmlLineInfo;
      this.sourceReaderSchemaInfo = reader as IHasXmlSchemaInfo;
      this.schemas.ValidationEventHandler += this.ValidationEventHandler;
    }

    private XsdValidationContext Context => this.state.Context;

    internal ArrayList CurrentKeyFieldConsumers
    {
      get
      {
        if (this.currentKeyFieldConsumers == null)
          this.currentKeyFieldConsumers = new ArrayList();
        return this.currentKeyFieldConsumers;
      }
    }

    public int XsiNilDepth => this.xsiNilDepth;

    public bool Namespaces
    {
      get => this.namespaces;
      set => this.namespaces = value;
    }

    public XmlResolver XmlResolver
    {
      set => this.resolver = value;
    }

    public XmlSchemaSet Schemas
    {
      get => this.schemas;
      set
      {
        if (this.validationStarted)
          throw new InvalidOperationException("Schemas must be set before the first call to Read().");
        this.schemas = value;
      }
    }

    public object SchemaType
    {
      get
      {
        if (this.ReadState != ReadState.Interactive)
          return (object) null;
        switch (this.NodeType)
        {
          case XmlNodeType.Element:
            return this.Context.ActualType != null ? this.Context.ActualType : this.SourceReaderSchemaType;
          case XmlNodeType.Attribute:
            if (this.currentAttrType == null)
            {
              if (this.Context.ActualType is XmlSchemaComplexType actualType)
              {
                if (actualType.AttributeUses[new XmlQualifiedName(this.LocalName, this.NamespaceURI)] is XmlSchemaAttribute attributeUse)
                  this.currentAttrType = attributeUse.AttributeType;
                return this.currentAttrType;
              }
              this.currentAttrType = this.SourceReaderSchemaType;
            }
            return this.currentAttrType;
          default:
            return this.SourceReaderSchemaType;
        }
      }
    }

    private object SourceReaderSchemaType => this.sourceReaderSchemaInfo != null ? this.sourceReaderSchemaInfo.SchemaType : (object) null;

    public ValidationType ValidationType
    {
      get => this.validationType;
      set
      {
        if (this.validationStarted)
          throw new InvalidOperationException("ValidationType must be set before reading.");
        this.validationType = value;
      }
    }

    public object ReadTypedValue()
    {
      object obj = XmlSchemaUtil.ReadTypedValue((XmlReader) this, this.SchemaType, (IXmlNamespaceResolver) this.NamespaceManager, this.storedCharacters);
      this.storedCharacters.Length = 0;
      return obj;
    }

    public override int AttributeCount => this.reader.AttributeCount + this.defaultAttributes.Length;

    public override string BaseURI => this.reader.BaseURI;

    public override bool CanResolveEntity => this.reader.CanResolveEntity;

    public override int Depth
    {
      get
      {
        if (this.currentDefaultAttribute < 0)
          return this.reader.Depth;
        return this.defaultAttributeConsumed ? this.reader.Depth + 2 : this.reader.Depth + 1;
      }
    }

    public override bool EOF => this.reader.EOF;

    public override bool HasValue => this.currentDefaultAttribute >= 0 || this.reader.HasValue;

    public override bool IsDefault => this.currentDefaultAttribute >= 0 || this.reader.IsDefault;

    public override bool IsEmptyElement => this.currentDefaultAttribute < 0 && this.reader.IsEmptyElement;

    public override string this[int i] => this.GetAttribute(i);

    public override string this[string name] => this.GetAttribute(name);

    public override string this[string localName, string ns] => this.GetAttribute(localName, ns);

    public int LineNumber => this.readerLineInfo != null ? this.readerLineInfo.LineNumber : 0;

    public int LinePosition => this.readerLineInfo != null ? this.readerLineInfo.LinePosition : 0;

    public override string LocalName
    {
      get
      {
        if (this.currentDefaultAttribute < 0)
          return this.reader.LocalName;
        return this.defaultAttributeConsumed ? string.Empty : this.defaultAttributes[this.currentDefaultAttribute].QualifiedName.Name;
      }
    }

    public override string Name
    {
      get
      {
        if (this.currentDefaultAttribute < 0)
          return this.reader.Name;
        if (this.defaultAttributeConsumed)
          return string.Empty;
        XmlQualifiedName qualifiedName = this.defaultAttributes[this.currentDefaultAttribute].QualifiedName;
        string prefix = this.Prefix;
        return prefix == string.Empty ? qualifiedName.Name : prefix + ":" + qualifiedName.Name;
      }
    }

    public override string NamespaceURI
    {
      get
      {
        if (this.currentDefaultAttribute < 0)
          return this.reader.NamespaceURI;
        return this.defaultAttributeConsumed ? string.Empty : this.defaultAttributes[this.currentDefaultAttribute].QualifiedName.Namespace;
      }
    }

    public override XmlNameTable NameTable => this.reader.NameTable;

    public override XmlNodeType NodeType
    {
      get
      {
        if (this.currentDefaultAttribute < 0)
          return this.reader.NodeType;
        return this.defaultAttributeConsumed ? XmlNodeType.Text : XmlNodeType.Attribute;
      }
    }

    public XmlParserContext ParserContext => XmlSchemaUtil.GetParserContext(this.reader);

    internal XmlNamespaceManager NamespaceManager => this.ParserContext != null ? this.ParserContext.NamespaceManager : (XmlNamespaceManager) null;

    public override string Prefix
    {
      get
      {
        if (this.currentDefaultAttribute < 0)
          return this.reader.Prefix;
        if (this.defaultAttributeConsumed)
          return string.Empty;
        XmlQualifiedName qualifiedName = this.defaultAttributes[this.currentDefaultAttribute].QualifiedName;
        return (this.NamespaceManager == null ? (string) null : this.NamespaceManager.LookupPrefix(qualifiedName.Namespace, false)) ?? string.Empty;
      }
    }

    public override char QuoteChar => this.reader.QuoteChar;

    public override ReadState ReadState => this.reader.ReadState;

    public override string Value => this.currentDefaultAttribute < 0 ? this.reader.Value : this.defaultAttributes[this.currentDefaultAttribute].ValidatedDefaultValue ?? this.defaultAttributes[this.currentDefaultAttribute].ValidatedFixedValue;

    public override string XmlLang
    {
      get
      {
        string xmlLang = this.reader.XmlLang;
        if (xmlLang != null)
          return xmlLang;
        int defaultAttribute = this.FindDefaultAttribute("lang", "http://www.w3.org/XML/1998/namespace");
        return defaultAttribute < 0 ? (string) null : this.defaultAttributes[defaultAttribute].ValidatedDefaultValue ?? this.defaultAttributes[defaultAttribute].ValidatedFixedValue;
      }
    }

    public override XmlSpace XmlSpace
    {
      get
      {
        XmlSpace xmlSpace = this.reader.XmlSpace;
        if (xmlSpace != XmlSpace.None)
          return xmlSpace;
        int defaultAttribute = this.FindDefaultAttribute("space", "http://www.w3.org/XML/1998/namespace");
        if (defaultAttribute < 0)
          return XmlSpace.None;
        return (XmlSpace) Enum.Parse(typeof (XmlSpace), this.defaultAttributes[defaultAttribute].ValidatedDefaultValue ?? this.defaultAttributes[defaultAttribute].ValidatedFixedValue, false);
      }
    }

    private void HandleError(string error) => this.HandleError(error, (Exception) null);

    private void HandleError(string error, Exception innerException) => this.HandleError(error, innerException, false);

    private void HandleError(string error, Exception innerException, bool isWarning)
    {
      if (this.ValidationType == ValidationType.None)
        return;
      this.HandleError(new XmlSchemaValidationException(error, (object) this, this.BaseURI, (XmlSchemaObject) null, innerException), isWarning);
    }

    private void HandleError(XmlSchemaValidationException schemaException) => this.HandleError(schemaException, false);

    private void HandleError(XmlSchemaValidationException schemaException, bool isWarning)
    {
      if (this.ValidationType == ValidationType.None)
        return;
      ValidationEventArgs e = new ValidationEventArgs((XmlSchemaException) schemaException, schemaException.Message, !isWarning ? XmlSeverityType.Error : XmlSeverityType.Warning);
      if (this.ValidationEventHandler != null)
        this.ValidationEventHandler((object) this, e);
      else if (e.Severity == XmlSeverityType.Error)
        throw e.Exception;
    }

    private XmlSchemaElement FindElement(string name, string ns) => (XmlSchemaElement) this.schemas.GlobalElements[new XmlQualifiedName(name, ns)];

    private XmlSchemaType FindType(XmlQualifiedName qname) => (XmlSchemaType) this.schemas.GlobalTypes[qname];

    private void ValidateStartElementParticle()
    {
      if (this.Context.State == null)
        return;
      this.Context.XsiType = (object) null;
      this.state.CurrentElement = (XmlSchemaElement) null;
      this.Context.EvaluateStartElement(this.reader.LocalName, this.reader.NamespaceURI);
      if (this.Context.IsInvalid)
        this.HandleError("Invalid start element: " + this.reader.NamespaceURI + ":" + this.reader.LocalName);
      this.Context.PushCurrentElement(this.state.CurrentElement);
    }

    private void ValidateEndElementParticle()
    {
      if (this.Context.State != null && !this.Context.EvaluateEndElement())
        this.HandleError("Invalid end element: " + this.reader.Name);
      this.Context.PopCurrentElement();
      this.state.PopContext();
    }

    private void ValidateCharacters()
    {
      if (this.xsiNilDepth >= 0 && this.xsiNilDepth < this.reader.Depth)
        this.HandleError("Element item appeared, while current element context is nil.");
      if (!this.shouldValidateCharacters)
        return;
      this.storedCharacters.Append(this.reader.Value);
    }

    private void ValidateEndSimpleContent()
    {
      if (this.shouldValidateCharacters)
        this.ValidateEndSimpleContentCore();
      this.shouldValidateCharacters = false;
      this.storedCharacters.Length = 0;
    }

    private void ValidateEndSimpleContentCore()
    {
      if (this.Context.ActualType == null)
        return;
      string validatedDefaultValue = this.storedCharacters.ToString();
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
                this.HandleError("Character content not allowed.");
                break;
              }
              break;
            case XmlSchemaContentType.ElementOnly:
              if (validatedDefaultValue.Length > 0 && !XmlChar.IsWhitespace(validatedDefaultValue))
              {
                this.HandleError("Character content not allowed.");
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
        this.AssessStringValid(actualType1, dt, validatedDefaultValue);
      }
      if (this.checkKeyConstraints)
        this.ValidateSimpleContentIdentity(dt, validatedDefaultValue);
      this.shouldValidateCharacters = false;
    }

    private void AssessStringValid(XmlSchemaSimpleType st, XmlSchemaDatatype dt, string value)
    {
      XmlSchemaDatatype dt1 = dt;
      if (st != null)
      {
        string normalized = dt1.Normalize(value);
        this.ValidateRestrictedSimpleTypeValue(st, ref dt1, normalized);
      }
      if (dt1 == null)
        return;
      try
      {
        dt1.ParseValue(value, this.NameTable, (IXmlNamespaceResolver) this.NamespaceManager);
      }
      catch (Exception ex)
      {
        this.HandleError("Invalidly typed data was specified.", ex);
      }
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
            if (!content1.ValidateValueWithFacets(normalized, this.NameTable, (IXmlNamespaceResolver) this.NamespaceManager))
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
                  validatedListItemType1.ParseValue(s, this.NameTable, (IXmlNamespaceResolver) this.NamespaceManager);
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
                xmlSchemaDatatype.ParseValue(s1, this.NameTable, (IXmlNamespaceResolver) this.NamespaceManager);
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

    private object GetXsiType(string name)
    {
      XmlQualifiedName qname = XmlQualifiedName.Parse(name, (XmlReader) this);
      return !(qname == XmlSchemaComplexType.AnyTypeName) ? (!XmlSchemaUtil.IsBuiltInDatatypeName(qname) ? (object) this.FindType(qname) : (object) XmlSchemaDatatype.FromName(qname)) : (object) XmlSchemaComplexType.AnyType;
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
          schemaComplexType2.ValidateTypeDerivationOK(baseType, (ValidationEventHandler) null, (XmlSchema) null);
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
              schemaSimpleType.ValidateTypeDerivationOK(baseType, (ValidationEventHandler) null, (XmlSchema) null, true);
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

    private void AssessStartElementSchemaValidity()
    {
      if (this.xsiNilDepth >= 0 && this.xsiNilDepth < this.reader.Depth)
        this.HandleError("Element item appeared, while current element context is nil.");
      this.ValidateStartElementParticle();
      string xsiNilValue = this.reader.GetAttribute("nil", "http://www.w3.org/2001/XMLSchema-instance");
      if (xsiNilValue != null)
        xsiNilValue = xsiNilValue.Trim(XmlChar.WhitespaceChars);
      if (xsiNilValue == "true" && this.xsiNilDepth < 0)
        this.xsiNilDepth = this.reader.Depth;
      string name = this.reader.GetAttribute("type", "http://www.w3.org/2001/XMLSchema-instance");
      if (name != null)
      {
        name = name.Trim(XmlChar.WhitespaceChars);
        object xsiType = this.GetXsiType(name);
        if (xsiType == null)
        {
          this.HandleError("The instance type was not found: " + name + " .");
        }
        else
        {
          if (xsiType is XmlSchemaType xmlSchemaType && this.Context.Element != null)
          {
            if (this.Context.Element.ElementType is XmlSchemaType elementType && (xmlSchemaType.DerivedBy & elementType.FinalResolved) != XmlSchemaDerivationMethod.Empty)
              this.HandleError("The instance type is prohibited by the type of the context element.");
            if (elementType != xsiType && (xmlSchemaType.DerivedBy & this.Context.Element.BlockResolved) != XmlSchemaDerivationMethod.Empty)
              this.HandleError("The instance type is prohibited by the context element.");
          }
          if (xsiType is XmlSchemaComplexType schemaComplexType && schemaComplexType.IsAbstract)
          {
            this.HandleError("The instance type is abstract: " + name + " .");
          }
          else
          {
            if (this.Context.Element != null)
              this.AssessLocalTypeDerivationOK(xsiType, this.Context.Element.ElementType, this.Context.Element.BlockResolved);
            this.AssessStartElementLocallyValidType(xsiType);
            this.Context.XsiType = xsiType;
          }
        }
      }
      if (this.Context.Element == null)
      {
        this.state.CurrentElement = this.FindElement(this.reader.LocalName, this.reader.NamespaceURI);
        this.Context.PushCurrentElement(this.state.CurrentElement);
      }
      if (this.Context.Element != null)
      {
        if (this.Context.XsiType == null)
          this.AssessElementLocallyValidElement(xsiNilValue);
      }
      else
      {
        switch (this.state.ProcessContents)
        {
          case XmlSchemaContentProcessing.Skip:
          case XmlSchemaContentProcessing.Lax:
            break;
          default:
            if (name == null && (this.schemas.Contains(this.reader.NamespaceURI) || !this.schemas.MissedSubComponents(this.reader.NamespaceURI)))
            {
              this.HandleError("Element declaration for " + (object) new XmlQualifiedName(this.reader.LocalName, this.reader.NamespaceURI) + " is missing.");
              break;
            }
            break;
        }
      }
      this.state.PushContext();
      XsdValidationState xsdValidationState = (XsdValidationState) null;
      if (this.state.ProcessContents == XmlSchemaContentProcessing.Skip)
        this.skipValidationDepth = this.reader.Depth;
      else
        xsdValidationState = !(this.SchemaType is XmlSchemaComplexType schemaType) ? (this.state.ProcessContents != XmlSchemaContentProcessing.Lax ? this.state.Create((XmlSchemaObject) XmlSchemaParticle.Empty) : this.state.Create((XmlSchemaObject) XmlSchemaAny.AnyTypeContent)) : this.state.Create((XmlSchemaObject) schemaType.ValidatableParticle);
      this.Context.State = xsdValidationState;
      if (!this.checkKeyConstraints)
        return;
      this.ValidateKeySelectors();
      this.ValidateKeyFields();
    }

    private void AssessElementLocallyValidElement(string xsiNilValue)
    {
      XmlSchemaElement element = this.Context.Element;
      XmlQualifiedName xmlQualifiedName = new XmlQualifiedName(this.reader.LocalName, this.reader.NamespaceURI);
      if (element == null)
        this.HandleError("Element declaration is required for " + (object) xmlQualifiedName);
      if (element.ActualIsAbstract)
        this.HandleError("Abstract element declaration was specified for " + (object) xmlQualifiedName);
      if (!element.ActualIsNillable && xsiNilValue != null)
        this.HandleError("This element declaration is not nillable: " + (object) xmlQualifiedName);
      else if (xsiNilValue == "true" && element.ValidatedFixedValue != null)
        this.HandleError("Schema instance nil was specified, where the element declaration for " + (object) xmlQualifiedName + "has fixed value constraints.");
      string attribute = this.reader.GetAttribute("type", "http://www.w3.org/2001/XMLSchema-instance");
      if (attribute != null)
      {
        this.Context.XsiType = this.GetXsiType(attribute);
        this.AssessLocalTypeDerivationOK(this.Context.XsiType, element.ElementType, element.BlockResolved);
      }
      else
        this.Context.XsiType = (object) null;
      if (element.ElementType == null)
        return;
      this.AssessStartElementLocallyValidType(this.SchemaType);
    }

    private void AssessStartElementLocallyValidType(object schemaType)
    {
      if (schemaType == null)
      {
        this.HandleError("Schema type does not exist.");
      }
      else
      {
        XmlSchemaComplexType cType = schemaType as XmlSchemaComplexType;
        if (schemaType is XmlSchemaSimpleType)
        {
          while (this.reader.MoveToNextAttribute())
          {
            if (!(this.reader.NamespaceURI == "http://www.w3.org/2000/xmlns/"))
            {
              if (this.reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema-instance")
                this.HandleError("Current simple type cannot accept attributes other than schema instance namespace.");
              string localName = this.reader.LocalName;
              if (localName != null)
              {
                // ISSUE: reference to a compiler-generated field
                if (XsdValidatingReader.\u003C\u003Ef__switch\u0024map3 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  XsdValidatingReader.\u003C\u003Ef__switch\u0024map3 = new Dictionary<string, int>(4)
                  {
                    {
                      "type",
                      0
                    },
                    {
                      "nil",
                      0
                    },
                    {
                      "schemaLocation",
                      0
                    },
                    {
                      "noNamespaceSchemaLocation",
                      0
                    }
                  };
                }
                int num;
                // ISSUE: reference to a compiler-generated field
                if (XsdValidatingReader.\u003C\u003Ef__switch\u0024map3.TryGetValue(localName, out num) && num == 0)
                  continue;
              }
              this.HandleError("Unknown schema instance namespace attribute: " + this.reader.LocalName);
            }
          }
          this.reader.MoveToElement();
        }
        else
        {
          if (cType == null)
            return;
          if (cType.IsAbstract)
            this.HandleError("Target complex type is abstract.");
          else
            this.AssessElementLocallyValidComplexType(cType);
        }
      }
    }

    private void AssessElementLocallyValidComplexType(XmlSchemaComplexType cType)
    {
      if (cType.IsAbstract)
        this.HandleError("Target complex type is abstract.");
      if (this.reader.MoveToFirstAttribute())
      {
        do
        {
          string namespaceUri = this.reader.NamespaceURI;
          if (namespaceUri != null)
          {
            // ISSUE: reference to a compiler-generated field
            if (XsdValidatingReader.\u003C\u003Ef__switch\u0024map4 == null)
            {
              // ISSUE: reference to a compiler-generated field
              XsdValidatingReader.\u003C\u003Ef__switch\u0024map4 = new Dictionary<string, int>(2)
              {
                {
                  "http://www.w3.org/2000/xmlns/",
                  0
                },
                {
                  "http://www.w3.org/2001/XMLSchema-instance",
                  0
                }
              };
            }
            int num;
            // ISSUE: reference to a compiler-generated field
            if (XsdValidatingReader.\u003C\u003Ef__switch\u0024map4.TryGetValue(namespaceUri, out num) && num == 0)
              goto label_11;
          }
          XmlQualifiedName qname = new XmlQualifiedName(this.reader.LocalName, this.reader.NamespaceURI);
          XmlSchemaObject attributeDeclaration = XmlSchemaUtil.FindAttributeDeclaration(this.reader.NamespaceURI, this.schemas, cType, qname);
          if (attributeDeclaration == null)
            this.HandleError("Attribute declaration was not found for " + (object) qname);
          if (attributeDeclaration is XmlSchemaAttribute attr)
          {
            this.AssessAttributeLocallyValidUse(attr);
            this.AssessAttributeLocallyValid(attr);
          }
label_11:;
        }
        while (this.reader.MoveToNextAttribute());
        this.reader.MoveToElement();
      }
      foreach (DictionaryEntry attributeUse in cType.AttributeUses)
      {
        XmlSchemaAttribute xmlSchemaAttribute = (XmlSchemaAttribute) attributeUse.Value;
        if (this.reader[xmlSchemaAttribute.QualifiedName.Name, xmlSchemaAttribute.QualifiedName.Namespace] == null)
        {
          if (xmlSchemaAttribute.ValidatedUse == XmlSchemaUse.Required && xmlSchemaAttribute.ValidatedFixedValue == null)
            this.HandleError("Required attribute " + (object) xmlSchemaAttribute.QualifiedName + " was not found.");
          else if (xmlSchemaAttribute.ValidatedDefaultValue != null || xmlSchemaAttribute.ValidatedFixedValue != null)
            this.defaultAttributesCache.Add((object) xmlSchemaAttribute);
        }
      }
      this.defaultAttributes = this.defaultAttributesCache.Count != 0 ? (XmlSchemaAttribute[]) this.defaultAttributesCache.ToArray(typeof (XmlSchemaAttribute)) : XsdValidatingReader.emptyAttributeArray;
      this.defaultAttributesCache.Clear();
    }

    private void AssessAttributeLocallyValid(XmlSchemaAttribute attr)
    {
      if (attr.AttributeType == null)
        this.HandleError("Attribute type is missing for " + (object) attr.QualifiedName);
      if (!(attr.AttributeType is XmlSchemaDatatype dt))
        dt = ((XmlSchemaType) attr.AttributeType).Datatype;
      if (dt == XmlSchemaSimpleType.AnySimpleType && attr.ValidatedFixedValue == null)
        return;
      string str = dt.Normalize(this.reader.Value);
      object parsedValue = (object) null;
      if (attr.AttributeType is XmlSchemaSimpleType attributeType)
        this.ValidateRestrictedSimpleTypeValue(attributeType, ref dt, str);
      try
      {
        parsedValue = dt.ParseValue(str, this.reader.NameTable, (IXmlNamespaceResolver) this.NamespaceManager);
      }
      catch (Exception ex)
      {
        this.HandleError("Attribute value is invalid against its data type " + (object) dt.TokenizedType, ex);
      }
      if (attr.ValidatedFixedValue != null && attr.ValidatedFixedValue != str)
      {
        this.HandleError("The value of the attribute " + (object) attr.QualifiedName + " does not match with its fixed value.");
        parsedValue = dt.ParseValue(attr.ValidatedFixedValue, this.reader.NameTable, (IXmlNamespaceResolver) this.NamespaceManager);
      }
      if (!this.checkIdentity)
        return;
      string error = this.idManager.AssessEachAttributeIdentityConstraint(dt, parsedValue, ((XmlQualifiedName) this.elementQNameStack[this.elementQNameStack.Count - 1]).Name);
      if (error == null)
        return;
      this.HandleError(error);
    }

    private void AssessAttributeLocallyValidUse(XmlSchemaAttribute attr)
    {
      if (attr.ValidatedUse != XmlSchemaUse.Prohibited)
        return;
      this.HandleError("Attribute " + (object) attr.QualifiedName + " is prohibited in this context.");
    }

    private void AssessEndElementSchemaValidity()
    {
      this.ValidateEndSimpleContent();
      this.ValidateEndElementParticle();
      if (this.checkKeyConstraints)
        this.ValidateEndElementKeyConstraints();
      if (this.xsiNilDepth != this.reader.Depth)
        return;
      this.xsiNilDepth = -1;
    }

    private void ValidateEndElementKeyConstraints()
    {
      for (int index1 = 0; index1 < this.keyTables.Count; ++index1)
      {
        XsdKeyTable keyTable = this.keyTables[index1] as XsdKeyTable;
        if (keyTable.StartDepth == this.reader.Depth)
        {
          this.EndIdentityValidation(keyTable);
        }
        else
        {
          for (int index2 = 0; index2 < keyTable.Entries.Count; ++index2)
          {
            XsdKeyEntry entry = keyTable.Entries[index2];
            if (entry.StartDepth == this.reader.Depth)
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
                if (!keyField.FieldFound && keyField.FieldFoundDepth == this.reader.Depth)
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
        if ((this.keyTables[index] as XsdKeyTable).StartDepth == this.reader.Depth)
        {
          this.keyTables.RemoveAt(index);
          --index;
        }
      }
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
        if (keyTable.SelectorMatches(this.elementQNameStack, this.reader.Depth) != null)
        {
          XsdKeyEntry entry = new XsdKeyEntry(keyTable, this.reader.Depth, this.readerLineInfo);
          keyTable.Entries.Add(entry);
        }
      }
    }

    private void ValidateKeyFields()
    {
      for (int index = 0; index < this.keyTables.Count; ++index)
      {
        XsdKeyTable keyTable = (XsdKeyTable) this.keyTables[index];
        for (int i = 0; i < keyTable.Entries.Count; ++i)
        {
          try
          {
            this.ProcessKeyEntry(keyTable.Entries[i]);
          }
          catch (XmlSchemaValidationException ex)
          {
            this.HandleError(ex);
          }
        }
      }
    }

    private void ProcessKeyEntry(XsdKeyEntry entry)
    {
      bool isXsiNil = this.XsiNilDepth == this.Depth;
      entry.ProcessMatch(false, this.elementQNameStack, (object) this, this.NameTable, this.BaseURI, this.SchemaType, (IXmlNamespaceResolver) this.NamespaceManager, this.readerLineInfo, this.Depth, (string) null, (string) null, (object) null, isXsiNil, this.CurrentKeyFieldConsumers);
      if (!this.MoveToFirstAttribute())
        return;
      try
      {
        do
        {
          string namespaceUri = this.NamespaceURI;
          if (namespaceUri != null)
          {
            // ISSUE: reference to a compiler-generated field
            if (XsdValidatingReader.\u003C\u003Ef__switch\u0024map5 == null)
            {
              // ISSUE: reference to a compiler-generated field
              XsdValidatingReader.\u003C\u003Ef__switch\u0024map5 = new Dictionary<string, int>(2)
              {
                {
                  "http://www.w3.org/2000/xmlns/",
                  0
                },
                {
                  "http://www.w3.org/2001/XMLSchema-instance",
                  0
                }
              };
            }
            int num;
            // ISSUE: reference to a compiler-generated field
            if (XsdValidatingReader.\u003C\u003Ef__switch\u0024map5.TryGetValue(namespaceUri, out num) && num == 0)
              goto label_13;
          }
          XmlSchemaDatatype xmlSchemaDatatype = this.SchemaType as XmlSchemaDatatype;
          XmlSchemaSimpleType schemaType = this.SchemaType as XmlSchemaSimpleType;
          if (xmlSchemaDatatype == null && schemaType != null)
            xmlSchemaDatatype = schemaType.Datatype;
          object attrValue = (object) null;
          if (xmlSchemaDatatype != null)
            attrValue = xmlSchemaDatatype.ParseValue(this.Value, this.NameTable, (IXmlNamespaceResolver) this.NamespaceManager);
          if (attrValue == null)
            attrValue = (object) this.Value;
          entry.ProcessMatch(true, this.elementQNameStack, (object) this, this.NameTable, this.BaseURI, this.SchemaType, (IXmlNamespaceResolver) this.NamespaceManager, this.readerLineInfo, this.Depth, this.LocalName, this.NamespaceURI, attrValue, false, this.CurrentKeyFieldConsumers);
label_13:;
        }
        while (this.MoveToNextAttribute());
      }
      finally
      {
        this.MoveToElement();
      }
    }

    private XsdKeyTable CreateNewKeyTable(XmlSchemaIdentityConstraint ident)
    {
      XsdKeyTable newKeyTable = new XsdKeyTable(ident);
      newKeyTable.StartDepth = this.reader.Depth;
      this.keyTables.Add((object) newKeyTable);
      return newKeyTable;
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
            identity = dt.ParseValue(value, this.NameTable, (IXmlNamespaceResolver) this.NamespaceManager);
          }
          catch (Exception ex)
          {
            this.HandleError("Identity value is invalid against its data type " + (object) dt.TokenizedType, ex);
          }
        }
        if (identity == null)
          identity = (object) value;
        if (!keyFieldConsumer.SetIdentityField(identity, this.reader.Depth == this.xsiNilDepth, dt as XsdAnySimpleType, this.Depth, this.readerLineInfo))
          this.HandleError("Two or more identical key value was found: '" + value + "' .");
        this.currentKeyFieldConsumers.RemoveAt(0);
      }
    }

    private void EndIdentityValidation(XsdKeyTable seq)
    {
      ArrayList arrayList = (ArrayList) null;
      for (int i = 0; i < seq.Entries.Count; ++i)
      {
        XsdKeyEntry entry = seq.Entries[i];
        if (!entry.KeyFound && seq.SourceSchemaIdentity is XmlSchemaKey)
        {
          if (arrayList == null)
            arrayList = new ArrayList();
          arrayList.Add((object) ("line " + (object) entry.SelectorLineNumber + "position " + (object) entry.SelectorLinePosition));
        }
      }
      if (arrayList != null)
        this.HandleError("Invalid identity constraints were found. Key was not found. " + string.Join(", ", arrayList.ToArray(typeof (string)) as string[]));
      if (!(seq.SourceSchemaIdentity is XmlSchemaKeyref sourceSchemaIdentity))
        return;
      this.EndKeyrefValidation(seq, sourceSchemaIdentity.Target);
    }

    private void EndKeyrefValidation(XsdKeyTable seq, XmlSchemaIdentityConstraint targetIdent)
    {
      for (int index = this.keyTables.Count - 1; index >= 0; --index)
      {
        XsdKeyTable keyTable = this.keyTables[index] as XsdKeyTable;
        if (keyTable.SourceSchemaIdentity == targetIdent)
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
      ArrayList arrayList = (ArrayList) null;
      for (int i = 0; i < seq.FinishedEntries.Count; ++i)
      {
        XsdKeyEntry finishedEntry = seq.FinishedEntries[i];
        if (!finishedEntry.KeyRefFound)
        {
          if (arrayList == null)
            arrayList = new ArrayList();
          arrayList.Add((object) (" line " + (object) finishedEntry.SelectorLineNumber + ", position " + (object) finishedEntry.SelectorLinePosition));
        }
      }
      if (arrayList == null)
        return;
      this.HandleError("Invalid identity constraints were found. Referenced key was not found: " + string.Join(" / ", arrayList.ToArray(typeof (string)) as string[]));
    }

    public override void Close() => this.reader.Close();

    public override string GetAttribute(int i)
    {
      switch (this.reader.NodeType)
      {
        case XmlNodeType.DocumentType:
        case XmlNodeType.XmlDeclaration:
          return this.reader.GetAttribute(i);
        default:
          if (this.reader.AttributeCount > i)
            this.reader.GetAttribute(i);
          int index = i - this.reader.AttributeCount;
          if (i < this.AttributeCount)
            return this.defaultAttributes[index].DefaultValue;
          throw new ArgumentOutOfRangeException(nameof (i), (object) i, "Specified attribute index is out of range.");
      }
    }

    public override string GetAttribute(string name)
    {
      switch (this.reader.NodeType)
      {
        case XmlNodeType.DocumentType:
        case XmlNodeType.XmlDeclaration:
          return this.reader.GetAttribute(name);
        default:
          string attribute = this.reader.GetAttribute(name);
          if (attribute != null)
            return attribute;
          XmlQualifiedName xmlQualifiedName = this.SplitQName(name);
          return this.GetDefaultAttribute(xmlQualifiedName.Name, xmlQualifiedName.Namespace);
      }
    }

    private XmlQualifiedName SplitQName(string name)
    {
      if (!XmlChar.IsName(name))
        throw new ArgumentException("Invalid name was specified.", nameof (name));
      Exception innerEx = (Exception) null;
      XmlQualifiedName qname = XmlSchemaUtil.ToQName(this.reader, name, out innerEx);
      return innerEx != null ? XmlQualifiedName.Empty : qname;
    }

    public override string GetAttribute(string localName, string ns)
    {
      switch (this.reader.NodeType)
      {
        case XmlNodeType.DocumentType:
        case XmlNodeType.XmlDeclaration:
          return this.reader.GetAttribute(localName, ns);
        default:
          return this.reader.GetAttribute(localName, ns) ?? this.GetDefaultAttribute(localName, ns);
      }
    }

    private string GetDefaultAttribute(string localName, string ns)
    {
      int defaultAttribute = this.FindDefaultAttribute(localName, ns);
      return defaultAttribute < 0 ? (string) null : this.defaultAttributes[defaultAttribute].ValidatedDefaultValue ?? this.defaultAttributes[defaultAttribute].ValidatedFixedValue;
    }

    private int FindDefaultAttribute(string localName, string ns)
    {
      for (int defaultAttribute1 = 0; defaultAttribute1 < this.defaultAttributes.Length; ++defaultAttribute1)
      {
        XmlSchemaAttribute defaultAttribute2 = this.defaultAttributes[defaultAttribute1];
        if (defaultAttribute2.QualifiedName.Name == localName && (ns == null || defaultAttribute2.QualifiedName.Namespace == ns))
          return defaultAttribute1;
      }
      return -1;
    }

    public bool HasLineInfo() => this.readerLineInfo != null && this.readerLineInfo.HasLineInfo();

    public override string LookupNamespace(string prefix) => this.reader.LookupNamespace(prefix);

    public override void MoveToAttribute(int i)
    {
      switch (this.reader.NodeType)
      {
        case XmlNodeType.DocumentType:
        case XmlNodeType.XmlDeclaration:
          this.reader.MoveToAttribute(i);
          break;
        default:
          this.currentAttrType = (object) null;
          if (i < this.reader.AttributeCount)
          {
            this.reader.MoveToAttribute(i);
            this.currentDefaultAttribute = -1;
            this.defaultAttributeConsumed = false;
          }
          if (i >= this.AttributeCount)
            throw new ArgumentOutOfRangeException(nameof (i), (object) i, "Attribute index is out of range.");
          this.currentDefaultAttribute = i - this.reader.AttributeCount;
          this.defaultAttributeConsumed = false;
          break;
      }
    }

    public override bool MoveToAttribute(string name)
    {
      switch (this.reader.NodeType)
      {
        case XmlNodeType.DocumentType:
        case XmlNodeType.XmlDeclaration:
          return this.reader.MoveToAttribute(name);
        default:
          this.currentAttrType = (object) null;
          if (!this.reader.MoveToAttribute(name))
            return this.MoveToDefaultAttribute(name, (string) null);
          this.currentDefaultAttribute = -1;
          this.defaultAttributeConsumed = false;
          return true;
      }
    }

    public override bool MoveToAttribute(string localName, string ns)
    {
      switch (this.reader.NodeType)
      {
        case XmlNodeType.DocumentType:
        case XmlNodeType.XmlDeclaration:
          return this.reader.MoveToAttribute(localName, ns);
        default:
          this.currentAttrType = (object) null;
          if (!this.reader.MoveToAttribute(localName, ns))
            return this.MoveToDefaultAttribute(localName, ns);
          this.currentDefaultAttribute = -1;
          this.defaultAttributeConsumed = false;
          return true;
      }
    }

    private bool MoveToDefaultAttribute(string localName, string ns)
    {
      int defaultAttribute = this.FindDefaultAttribute(localName, ns);
      if (defaultAttribute < 0)
        return false;
      this.currentDefaultAttribute = defaultAttribute;
      this.defaultAttributeConsumed = false;
      return true;
    }

    public override bool MoveToElement()
    {
      this.currentDefaultAttribute = -1;
      this.defaultAttributeConsumed = false;
      this.currentAttrType = (object) null;
      return this.reader.MoveToElement();
    }

    public override bool MoveToFirstAttribute()
    {
      switch (this.reader.NodeType)
      {
        case XmlNodeType.DocumentType:
        case XmlNodeType.XmlDeclaration:
          return this.reader.MoveToFirstAttribute();
        default:
          this.currentAttrType = (object) null;
          if (this.reader.AttributeCount > 0)
          {
            bool firstAttribute = this.reader.MoveToFirstAttribute();
            if (firstAttribute)
            {
              this.currentDefaultAttribute = -1;
              this.defaultAttributeConsumed = false;
            }
            return firstAttribute;
          }
          if (this.defaultAttributes.Length <= 0)
            return false;
          this.currentDefaultAttribute = 0;
          this.defaultAttributeConsumed = false;
          return true;
      }
    }

    public override bool MoveToNextAttribute()
    {
      switch (this.reader.NodeType)
      {
        case XmlNodeType.DocumentType:
        case XmlNodeType.XmlDeclaration:
          return this.reader.MoveToNextAttribute();
        default:
          this.currentAttrType = (object) null;
          if (this.currentDefaultAttribute >= 0)
          {
            if (this.defaultAttributes.Length == this.currentDefaultAttribute + 1)
              return false;
            ++this.currentDefaultAttribute;
            this.defaultAttributeConsumed = false;
            return true;
          }
          if (this.reader.MoveToNextAttribute())
          {
            this.currentDefaultAttribute = -1;
            this.defaultAttributeConsumed = false;
            return true;
          }
          if (this.defaultAttributes.Length <= 0)
            return false;
          this.currentDefaultAttribute = 0;
          this.defaultAttributeConsumed = false;
          return true;
      }
    }

    private XmlSchema ReadExternalSchema(string uri)
    {
      Uri absoluteUri = this.resolver.ResolveUri(!(this.BaseURI != string.Empty) ? (Uri) null : new Uri(this.BaseURI), uri);
      string url = !(absoluteUri != (Uri) null) ? string.Empty : absoluteUri.ToString();
      XmlTextReader rdr = (XmlTextReader) null;
      try
      {
        rdr = new XmlTextReader(url, (Stream) this.resolver.GetEntity(absoluteUri, (string) null, typeof (Stream)), this.NameTable);
        return XmlSchema.Read((XmlReader) rdr, this.ValidationEventHandler);
      }
      finally
      {
        rdr?.Close();
      }
    }

    private void ExamineAdditionalSchema()
    {
      if (this.resolver == null || this.ValidationType == ValidationType.None)
        return;
      XmlSchema schema = (XmlSchema) null;
      string attribute1 = this.reader.GetAttribute("schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
      bool flag = false;
      if (attribute1 != null)
      {
        string[] strArray;
        try
        {
          strArray = XmlSchemaDatatype.FromName("token", "http://www.w3.org/2001/XMLSchema").Normalize(attribute1).Split(XmlChar.WhitespaceChars);
        }
        catch (Exception ex)
        {
          if (this.schemas.Count == 0)
            this.HandleError("Invalid schemaLocation attribute format.", ex, true);
          strArray = new string[0];
        }
        if (strArray.Length % 2 != 0 && this.schemas.Count == 0)
          this.HandleError("Invalid schemaLocation attribute format.");
        int index = 0;
        do
        {
          try
          {
            for (; index < strArray.Length; index += 2)
            {
              schema = this.ReadExternalSchema(strArray[index + 1]);
              if (schema.TargetNamespace == null)
                schema.TargetNamespace = strArray[index];
              else if (schema.TargetNamespace != strArray[index])
                this.HandleError("Specified schema has different target namespace.");
              if (schema != null)
              {
                if (!this.schemas.Contains(schema.TargetNamespace))
                {
                  flag = true;
                  this.schemas.Add(schema);
                }
                schema = (XmlSchema) null;
              }
            }
          }
          catch (Exception ex)
          {
            if (!this.schemas.Contains(strArray[index]))
              this.HandleError(string.Format("Could not resolve schema location URI: {0}", index + 1 >= strArray.Length ? (object) string.Empty : (object) strArray[index + 1]), (Exception) null, true);
            index += 2;
          }
        }
        while (index < strArray.Length);
      }
      string attribute2 = this.reader.GetAttribute("noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
      if (attribute2 != null)
      {
        try
        {
          schema = this.ReadExternalSchema(attribute2);
        }
        catch (Exception ex)
        {
          if (this.schemas.Count != 0)
            this.HandleError("Could not resolve schema location URI: " + attribute2, (Exception) null, true);
        }
        if (schema != null && schema.TargetNamespace != null)
          this.HandleError("Specified schema has different target namespace.");
      }
      if (schema != null && !this.schemas.Contains(schema.TargetNamespace))
      {
        flag = true;
        this.schemas.Add(schema);
      }
      if (!flag)
        return;
      this.schemas.Compile();
    }

    public override bool Read()
    {
      this.validationStarted = true;
      this.currentDefaultAttribute = -1;
      this.defaultAttributeConsumed = false;
      this.currentAttrType = (object) null;
      this.defaultAttributes = XsdValidatingReader.emptyAttributeArray;
      bool flag = this.reader.Read();
      if (this.reader.Depth == 0 && this.reader.NodeType == XmlNodeType.Element)
      {
        if (this.reader is DTDValidatingReader reader && reader.DTD == null)
          this.reader = (XmlReader) reader.Source;
        this.ExamineAdditionalSchema();
      }
      if (this.schemas.Count == 0)
        return flag;
      if (!this.schemas.IsCompiled)
        this.schemas.Compile();
      if (this.checkIdentity)
        this.idManager.OnStartElement();
      if (!flag && this.checkIdentity && this.idManager.HasMissingIDReferences())
        this.HandleError("There are missing ID references: " + this.idManager.GetMissingIDString());
      XmlNodeType nodeType = this.reader.NodeType;
      switch (nodeType)
      {
        case XmlNodeType.Element:
          if (this.checkKeyConstraints)
            this.elementQNameStack.Add((object) new XmlQualifiedName(this.reader.LocalName, this.reader.NamespaceURI));
          if (this.skipValidationDepth < 0 || this.reader.Depth <= this.skipValidationDepth)
          {
            this.ValidateEndSimpleContent();
            this.AssessStartElementSchemaValidity();
          }
          if (!this.reader.IsEmptyElement)
          {
            if (this.xsiNilDepth < this.reader.Depth)
            {
              this.shouldValidateCharacters = true;
              goto label_34;
            }
            else
              goto label_34;
          }
          else
            break;
        case XmlNodeType.Text:
        case XmlNodeType.CDATA:
label_27:
          if (this.skipValidationDepth < 0 || this.reader.Depth <= this.skipValidationDepth)
          {
            if (this.Context.ActualType is XmlSchemaComplexType actualType)
            {
              switch (actualType.ContentType)
              {
                case XmlSchemaContentType.Empty:
                  this.HandleError(string.Format("Not allowed character content is found (current element content model '{0}' is empty).", (object) actualType.QualifiedName));
                  break;
                case XmlSchemaContentType.ElementOnly:
                  if (this.reader.NodeType != XmlNodeType.Whitespace)
                  {
                    this.HandleError(string.Format("Not allowed character content is found (current content model '{0}' is element-only).", (object) actualType.QualifiedName));
                    break;
                  }
                  break;
              }
            }
            this.ValidateCharacters();
            goto label_34;
          }
          else
            goto label_34;
        default:
          switch (nodeType - 13)
          {
            case XmlNodeType.None:
            case XmlNodeType.Element:
              goto label_27;
            case XmlNodeType.Attribute:
              break;
            default:
              goto label_34;
          }
          break;
      }
      if (this.reader.Depth == this.skipValidationDepth)
        this.skipValidationDepth = -1;
      else if (this.skipValidationDepth < 0 || this.reader.Depth <= this.skipValidationDepth)
        this.AssessEndElementSchemaValidity();
      if (this.checkKeyConstraints)
        this.elementQNameStack.RemoveAt(this.elementQNameStack.Count - 1);
label_34:
      return flag;
    }

    public override bool ReadAttributeValue()
    {
      if (this.currentDefaultAttribute < 0)
        return this.reader.ReadAttributeValue();
      if (this.defaultAttributeConsumed)
        return false;
      this.defaultAttributeConsumed = true;
      return true;
    }

    public override string ReadString() => base.ReadString();

    public override void ResolveEntity() => this.reader.ResolveEntity();
  }
}
