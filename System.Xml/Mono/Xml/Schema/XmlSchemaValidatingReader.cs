// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Schema.XmlSchemaValidatingReader
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
  internal class XmlSchemaValidatingReader : 
    XmlReader,
    IHasXmlParserContext,
    IXmlSchemaInfo,
    IXmlLineInfo,
    IXmlNamespaceResolver
  {
    private static readonly XmlSchemaAttribute[] emptyAttributeArray = new XmlSchemaAttribute[0];
    private XmlReader reader;
    private XmlSchemaValidationFlags options;
    private XmlSchemaValidator v;
    private XmlValueGetter getter;
    private XmlSchemaInfo xsinfo;
    private IXmlLineInfo readerLineInfo;
    private ValidationType validationType;
    private IXmlNamespaceResolver nsResolver;
    private XmlSchemaAttribute[] defaultAttributes = XmlSchemaValidatingReader.emptyAttributeArray;
    private int currentDefaultAttribute = -1;
    private ArrayList defaultAttributesCache = new ArrayList();
    private bool defaultAttributeConsumed;
    private XmlSchemaType currentAttrType;
    private bool validationDone;
    private XmlSchemaElement element;

    public XmlSchemaValidatingReader(XmlReader reader, XmlReaderSettings settings)
    {
      XmlSchemaValidatingReader nsmgr = this;
      if (!(reader is IXmlNamespaceResolver nsResolver))
        nsResolver = (IXmlNamespaceResolver) new XmlNamespaceManager(reader.NameTable);
      XmlSchemaSet schemas = settings.Schemas ?? new XmlSchemaSet();
      this.options = settings.ValidationFlags;
      this.reader = reader;
      this.v = new XmlSchemaValidator(reader.NameTable, schemas, nsResolver, this.options);
      if (reader.BaseURI != string.Empty)
        this.v.SourceUri = new Uri(reader.BaseURI);
      this.readerLineInfo = reader as IXmlLineInfo;
      this.getter = (XmlValueGetter) (() => nsmgr.v.CurrentAttributeType != null ? nsmgr.v.CurrentAttributeType.ParseValue(nsmgr.Value, nsmgr.NameTable, (IXmlNamespaceResolver) nsmgr) : (object) nsmgr.Value);
      this.xsinfo = new XmlSchemaInfo();
      this.v.LineInfoProvider = (IXmlLineInfo) this;
      this.v.ValidationEventSender = (object) reader;
      this.nsResolver = nsResolver;
      this.ValidationEventHandler += (System.Xml.Schema.ValidationEventHandler) ((o, e) => settings.OnValidationError(o, e));
      this.v.XmlResolver = settings == null || settings.Schemas == null ? (XmlResolver) new XmlUrlResolver() : settings.Schemas.XmlResolver;
      this.v.Initialize();
    }

    public event System.Xml.Schema.ValidationEventHandler ValidationEventHandler
    {
      add => this.v.ValidationEventHandler += value;
      remove => this.v.ValidationEventHandler -= value;
    }

    int IXmlLineInfo.LineNumber => this.readerLineInfo != null ? this.readerLineInfo.LineNumber : 0;

    int IXmlLineInfo.LinePosition => this.readerLineInfo != null ? this.readerLineInfo.LinePosition : 0;

    bool IXmlLineInfo.HasLineInfo() => this.readerLineInfo != null && this.readerLineInfo.HasLineInfo();

    public XmlSchemaType ElementSchemaType => this.element != null ? this.element.ElementSchemaType : (XmlSchemaType) null;

    private void ResetStateOnRead()
    {
      this.currentDefaultAttribute = -1;
      this.defaultAttributeConsumed = false;
      this.currentAttrType = (XmlSchemaType) null;
      this.defaultAttributes = XmlSchemaValidatingReader.emptyAttributeArray;
      this.v.CurrentAttributeType = (XmlSchemaDatatype) null;
    }

    public int LineNumber => this.readerLineInfo != null ? this.readerLineInfo.LineNumber : 0;

    public int LinePosition => this.readerLineInfo != null ? this.readerLineInfo.LinePosition : 0;

    public XmlSchemaType SchemaType
    {
      get
      {
        if (this.ReadState != ReadState.Interactive)
          return (XmlSchemaType) null;
        switch (this.NodeType)
        {
          case XmlNodeType.Element:
            return this.ElementSchemaType != null ? this.ElementSchemaType : (XmlSchemaType) null;
          case XmlNodeType.Attribute:
            if (this.currentAttrType != null || !(this.ElementSchemaType is XmlSchemaComplexType elementSchemaType) || !(elementSchemaType.AttributeUses[new XmlQualifiedName(this.LocalName, this.NamespaceURI)] is XmlSchemaAttribute attributeUse))
              return this.currentAttrType;
            this.currentAttrType = (XmlSchemaType) attributeUse.AttributeSchemaType;
            return this.currentAttrType;
          default:
            return (XmlSchemaType) null;
        }
      }
    }

    public ValidationType ValidationType
    {
      get => this.validationType;
      set
      {
        if (this.ReadState != ReadState.Initial)
          throw new InvalidOperationException("ValidationType must be set before reading.");
        this.validationType = value;
      }
    }

    public IDictionary<string, string> GetNamespacesInScope(XmlNamespaceScope scope) => this.reader is IXmlNamespaceResolver reader ? reader.GetNamespacesInScope(scope) : throw new NotSupportedException("The input XmlReader does not implement IXmlNamespaceResolver and thus this validating reader cannot collect in-scope namespaces.");

    public string LookupPrefix(string ns) => this.nsResolver.LookupPrefix(ns);

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

    public override string Prefix
    {
      get
      {
        if (this.currentDefaultAttribute < 0)
          return this.reader.Prefix;
        return this.defaultAttributeConsumed ? string.Empty : this.nsResolver.LookupPrefix(this.defaultAttributes[this.currentDefaultAttribute].QualifiedName.Namespace) ?? string.Empty;
      }
    }

    public override char QuoteChar => this.reader.QuoteChar;

    public override ReadState ReadState => this.reader.ReadState;

    public override IXmlSchemaInfo SchemaInfo => (IXmlSchemaInfo) this;

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
      XmlConvert.VerifyName(name);
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
          this.currentAttrType = (XmlSchemaType) null;
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
          this.currentAttrType = (XmlSchemaType) null;
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
          this.currentAttrType = (XmlSchemaType) null;
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
      this.currentAttrType = (XmlSchemaType) null;
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
          this.currentAttrType = (XmlSchemaType) null;
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
          this.currentAttrType = (XmlSchemaType) null;
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

    public override bool Read()
    {
      if (!this.reader.Read())
      {
        if (!this.validationDone)
        {
          this.v.EndValidation();
          this.validationDone = true;
        }
        return false;
      }
      this.ResetStateOnRead();
      XmlNodeType nodeType = this.reader.NodeType;
      switch (nodeType)
      {
        case XmlNodeType.Element:
          string attribute1 = this.reader.GetAttribute("schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
          string attribute2 = this.reader.GetAttribute("noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
          this.v.ValidateElement(this.reader.LocalName, this.reader.NamespaceURI, this.xsinfo, this.reader.GetAttribute("type", "http://www.w3.org/2001/XMLSchema-instance"), this.reader.GetAttribute("nil", "http://www.w3.org/2001/XMLSchema-instance"), attribute1, attribute2);
          if (this.reader.MoveToFirstAttribute())
          {
            do
            {
              string namespaceUri = this.reader.NamespaceURI;
              if (namespaceUri != null)
              {
                // ISSUE: reference to a compiler-generated field
                if (XmlSchemaValidatingReader.\u003C\u003Ef__switch\u0024map1 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  XmlSchemaValidatingReader.\u003C\u003Ef__switch\u0024map1 = new Dictionary<string, int>(2)
                  {
                    {
                      "http://www.w3.org/2001/XMLSchema-instance",
                      0
                    },
                    {
                      "http://www.w3.org/2000/xmlns/",
                      1
                    }
                  };
                }
                int num1;
                // ISSUE: reference to a compiler-generated field
                if (XmlSchemaValidatingReader.\u003C\u003Ef__switch\u0024map1.TryGetValue(namespaceUri, out num1))
                {
                  switch (num1)
                  {
                    case 0:
                      string localName = this.reader.LocalName;
                      if (localName != null)
                      {
                        // ISSUE: reference to a compiler-generated field
                        if (XmlSchemaValidatingReader.\u003C\u003Ef__switch\u0024map0 == null)
                        {
                          // ISSUE: reference to a compiler-generated field
                          XmlSchemaValidatingReader.\u003C\u003Ef__switch\u0024map0 = new Dictionary<string, int>(4)
                          {
                            {
                              "schemaLocation",
                              0
                            },
                            {
                              "noNamespaceSchemaLocation",
                              0
                            },
                            {
                              "nil",
                              0
                            },
                            {
                              "type",
                              0
                            }
                          };
                        }
                        int num2;
                        // ISSUE: reference to a compiler-generated field
                        if (!XmlSchemaValidatingReader.\u003C\u003Ef__switch\u0024map0.TryGetValue(localName, out num2) || num2 != 0)
                          break;
                        goto label_17;
                      }
                      else
                        break;
                    case 1:
                      goto label_17;
                  }
                }
              }
              this.v.ValidateAttribute(this.reader.LocalName, this.reader.NamespaceURI, this.getter, this.xsinfo);
label_17:;
            }
            while (this.reader.MoveToNextAttribute());
            this.reader.MoveToElement();
          }
          this.v.GetUnspecifiedDefaultAttributes(this.defaultAttributesCache);
          this.defaultAttributes = (XmlSchemaAttribute[]) this.defaultAttributesCache.ToArray(typeof (XmlSchemaAttribute));
          this.v.ValidateEndOfAttributes(this.xsinfo);
          this.defaultAttributesCache.Clear();
          if (!this.reader.IsEmptyElement)
            goto label_23;
          else
            break;
        case XmlNodeType.Text:
          this.v.ValidateText(this.getter);
          goto label_23;
        default:
          switch (nodeType - 13)
          {
            case XmlNodeType.None:
            case XmlNodeType.Element:
              this.v.ValidateWhitespace(this.getter);
              goto label_23;
            case XmlNodeType.Attribute:
              break;
            default:
              goto label_23;
          }
          break;
      }
      this.v.ValidateEndElement(this.xsinfo);
label_23:
      return true;
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

    public override void ResolveEntity() => this.reader.ResolveEntity();

    public bool IsNil => this.xsinfo.IsNil;

    public XmlSchemaSimpleType MemberType => this.xsinfo.MemberType;

    public XmlSchemaAttribute SchemaAttribute => this.xsinfo.SchemaAttribute;

    public XmlSchemaElement SchemaElement => this.xsinfo.SchemaElement;

    public XmlSchemaValidity Validity => this.xsinfo.Validity;
  }
}
