// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlValidatingReader
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml;
using Mono.Xml.Schema;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Schema;

namespace System.Xml
{
  [Obsolete("Use XmlReader created by XmlReader.Create() method using appropriate XmlReaderSettings instead.")]
  public class XmlValidatingReader : 
    XmlReader,
    IHasXmlParserContext,
    IXmlLineInfo,
    IXmlNamespaceResolver
  {
    private EntityHandling entityHandling;
    private XmlReader sourceReader;
    private XmlTextReader xmlTextReader;
    private XmlReader validatingReader;
    private XmlResolver resolver;
    private bool resolverSpecified;
    private ValidationType validationType;
    private XmlSchemaCollection schemas;
    private DTDValidatingReader dtdReader;
    private IHasXmlSchemaInfo schemaInfo;
    private StringBuilder storedCharacters;

    public XmlValidatingReader(XmlReader reader)
    {
      this.sourceReader = reader;
      this.xmlTextReader = reader as XmlTextReader;
      if (this.xmlTextReader == null)
        this.resolver = (XmlResolver) new XmlUrlResolver();
      this.entityHandling = EntityHandling.ExpandEntities;
      this.validationType = ValidationType.Auto;
      this.storedCharacters = new StringBuilder();
    }

    public XmlValidatingReader(Stream xmlFragment, XmlNodeType fragType, XmlParserContext context)
      : this((XmlReader) new XmlTextReader(xmlFragment, fragType, context))
    {
    }

    public XmlValidatingReader(string xmlFragment, XmlNodeType fragType, XmlParserContext context)
      : this((XmlReader) new XmlTextReader(xmlFragment, fragType, context))
    {
    }

    public event System.Xml.Schema.ValidationEventHandler ValidationEventHandler;

    XmlParserContext IHasXmlParserContext.ParserContext
    {
      get
      {
        if (this.dtdReader != null)
          return this.dtdReader.ParserContext;
        return this.sourceReader is IHasXmlParserContext sourceReader ? sourceReader.ParserContext : (XmlParserContext) null;
      }
    }

    IDictionary<string, string> IXmlNamespaceResolver.GetNamespacesInScope(XmlNamespaceScope scope) => ((IHasXmlParserContext) this).ParserContext.NamespaceManager.GetNamespacesInScope(scope);

    string IXmlNamespaceResolver.LookupPrefix(string ns) => (this.validatingReader == null ? this.validatingReader as IXmlNamespaceResolver : this.sourceReader as IXmlNamespaceResolver)?.LookupNamespace(ns);

    public override int AttributeCount => this.validatingReader == null ? 0 : this.validatingReader.AttributeCount;

    public override string BaseURI => this.validatingReader == null ? this.sourceReader.BaseURI : this.validatingReader.BaseURI;

    public override bool CanReadBinaryContent => true;

    public override bool CanResolveEntity => true;

    public override int Depth => this.validatingReader == null ? 0 : this.validatingReader.Depth;

    public Encoding Encoding => this.xmlTextReader != null ? this.xmlTextReader.Encoding : throw new NotSupportedException("Encoding is supported only for XmlTextReader.");

    public EntityHandling EntityHandling
    {
      get => this.entityHandling;
      set
      {
        this.entityHandling = value;
        if (this.dtdReader == null)
          return;
        this.dtdReader.EntityHandling = value;
      }
    }

    public override bool EOF => this.validatingReader != null && this.validatingReader.EOF;

    public override bool HasValue => this.validatingReader != null && this.validatingReader.HasValue;

    public override bool IsDefault => this.validatingReader != null && this.validatingReader.IsDefault;

    public override bool IsEmptyElement => this.validatingReader != null && this.validatingReader.IsEmptyElement;

    public int LineNumber => this.IsDefault || !(this.validatingReader is IXmlLineInfo validatingReader) ? 0 : validatingReader.LineNumber;

    public int LinePosition => this.IsDefault || !(this.validatingReader is IXmlLineInfo validatingReader) ? 0 : validatingReader.LinePosition;

    public override string LocalName
    {
      get
      {
        if (this.validatingReader == null)
          return string.Empty;
        return this.Namespaces ? this.validatingReader.LocalName : this.validatingReader.Name;
      }
    }

    public override string Name => this.validatingReader == null ? string.Empty : this.validatingReader.Name;

    public bool Namespaces
    {
      get => this.xmlTextReader == null || this.xmlTextReader.Namespaces;
      set
      {
        if (this.ReadState != ReadState.Initial)
          throw new InvalidOperationException("Namespaces have to be set before reading.");
        if (this.xmlTextReader == null)
          throw new NotSupportedException("Property 'Namespaces' is supported only for XmlTextReader.");
        this.xmlTextReader.Namespaces = value;
      }
    }

    public override string NamespaceURI => this.validatingReader == null || !this.Namespaces ? string.Empty : this.validatingReader.NamespaceURI;

    public override XmlNameTable NameTable => this.validatingReader == null ? this.sourceReader.NameTable : this.validatingReader.NameTable;

    public override XmlNodeType NodeType => this.validatingReader == null ? XmlNodeType.None : this.validatingReader.NodeType;

    public override string Prefix => this.validatingReader == null ? string.Empty : this.validatingReader.Prefix;

    public override char QuoteChar => this.validatingReader == null ? this.sourceReader.QuoteChar : this.validatingReader.QuoteChar;

    public XmlReader Reader => this.sourceReader;

    public override ReadState ReadState => this.validatingReader == null ? ReadState.Initial : this.validatingReader.ReadState;

    internal XmlResolver Resolver
    {
      get
      {
        if (this.xmlTextReader != null)
          return this.xmlTextReader.Resolver;
        return this.resolverSpecified ? this.resolver : (XmlResolver) null;
      }
    }

    public XmlSchemaCollection Schemas
    {
      get
      {
        if (this.schemas == null)
          this.schemas = new XmlSchemaCollection(this.NameTable);
        return this.schemas;
      }
    }

    public object SchemaType => this.schemaInfo.SchemaType;

    [MonoTODO]
    public override XmlReaderSettings Settings => this.validatingReader == null ? this.sourceReader.Settings : this.validatingReader.Settings;

    [MonoTODO]
    public ValidationType ValidationType
    {
      get => this.validationType;
      set
      {
        if (this.ReadState != ReadState.Initial)
          throw new InvalidOperationException("ValidationType cannot be set after the first call to Read method.");
        switch (this.validationType)
        {
          case ValidationType.None:
          case ValidationType.Auto:
          case ValidationType.DTD:
          case ValidationType.Schema:
            this.validationType = value;
            break;
          case ValidationType.XDR:
            throw new NotSupportedException();
        }
      }
    }

    public override string Value => this.validatingReader == null ? string.Empty : this.validatingReader.Value;

    public override string XmlLang => this.validatingReader == null ? string.Empty : this.validatingReader.XmlLang;

    public XmlResolver XmlResolver
    {
      set
      {
        this.resolverSpecified = true;
        this.resolver = value;
        if (this.xmlTextReader != null)
          this.xmlTextReader.XmlResolver = value;
        if (this.validatingReader is XsdValidatingReader validatingReader1)
          validatingReader1.XmlResolver = value;
        if (!(this.validatingReader is DTDValidatingReader validatingReader2))
          return;
        validatingReader2.XmlResolver = value;
      }
    }

    public override XmlSpace XmlSpace => this.validatingReader == null ? XmlSpace.None : this.validatingReader.XmlSpace;

    public override void Close()
    {
      if (this.validatingReader == null)
        this.sourceReader.Close();
      else
        this.validatingReader.Close();
    }

    public override string GetAttribute(int i) => this.validatingReader != null ? this.validatingReader[i] : throw new IndexOutOfRangeException("Reader is not started.");

    public override string GetAttribute(string name) => this.validatingReader == null ? (string) null : this.validatingReader[name];

    public override string GetAttribute(string localName, string namespaceName) => this.validatingReader == null ? (string) null : this.validatingReader[localName, namespaceName];

    public bool HasLineInfo() => this.validatingReader is IXmlLineInfo validatingReader && validatingReader.HasLineInfo();

    public override string LookupNamespace(string prefix) => this.validatingReader != null ? this.validatingReader.LookupNamespace(prefix) : this.sourceReader.LookupNamespace(prefix);

    public override void MoveToAttribute(int i)
    {
      if (this.validatingReader == null)
        throw new IndexOutOfRangeException("Reader is not started.");
      this.validatingReader.MoveToAttribute(i);
    }

    public override bool MoveToAttribute(string name) => this.validatingReader != null && this.validatingReader.MoveToAttribute(name);

    public override bool MoveToAttribute(string localName, string namespaceName) => this.validatingReader != null && this.validatingReader.MoveToAttribute(localName, namespaceName);

    public override bool MoveToElement() => this.validatingReader != null && this.validatingReader.MoveToElement();

    public override bool MoveToFirstAttribute() => this.validatingReader != null && this.validatingReader.MoveToFirstAttribute();

    public override bool MoveToNextAttribute() => this.validatingReader != null && this.validatingReader.MoveToNextAttribute();

    [MonoTODO]
    public override bool Read()
    {
      if (this.validatingReader == null)
      {
        switch (this.ValidationType)
        {
          case ValidationType.None:
          case ValidationType.Auto:
          case ValidationType.Schema:
            this.dtdReader = new DTDValidatingReader(this.sourceReader, this);
            XsdValidatingReader validatingReader = new XsdValidatingReader((XmlReader) this.dtdReader);
            validatingReader.ValidationEventHandler += new System.Xml.Schema.ValidationEventHandler(this.OnValidationEvent);
            validatingReader.ValidationType = this.ValidationType;
            validatingReader.Schemas = this.Schemas.SchemaSet;
            validatingReader.XmlResolver = this.Resolver;
            this.validatingReader = (XmlReader) validatingReader;
            this.dtdReader.XmlResolver = this.Resolver;
            break;
          case ValidationType.DTD:
            this.validatingReader = (XmlReader) (this.dtdReader = new DTDValidatingReader(this.sourceReader, this));
            this.dtdReader.XmlResolver = this.Resolver;
            break;
          case ValidationType.XDR:
            throw new NotSupportedException();
        }
        this.schemaInfo = this.validatingReader as IHasXmlSchemaInfo;
      }
      return this.validatingReader.Read();
    }

    public override bool ReadAttributeValue() => this.validatingReader != null && this.validatingReader.ReadAttributeValue();

    public override string ReadString() => base.ReadString();

    public object ReadTypedValue()
    {
      if (this.dtdReader == null)
        return (object) null;
      if (!(this.schemaInfo.SchemaType is XmlSchemaDatatype xmlSchemaDatatype) && this.schemaInfo.SchemaType is XmlSchemaType schemaType)
        xmlSchemaDatatype = schemaType.Datatype;
      if (xmlSchemaDatatype == null)
        return (object) null;
      switch (this.NodeType)
      {
        case XmlNodeType.Element:
          if (this.IsEmptyElement)
            return (object) null;
          this.storedCharacters.Length = 0;
          bool flag = true;
          do
          {
            this.Read();
            XmlNodeType nodeType = this.NodeType;
            switch (nodeType)
            {
              case XmlNodeType.Text:
              case XmlNodeType.CDATA:
                this.storedCharacters.Append(this.Value);
                goto case XmlNodeType.Comment;
              case XmlNodeType.Comment:
                continue;
              default:
                if (nodeType != XmlNodeType.Whitespace && nodeType != XmlNodeType.SignificantWhitespace)
                {
                  flag = false;
                  goto case XmlNodeType.Comment;
                }
                else
                  goto case XmlNodeType.Text;
            }
          }
          while (flag && !this.EOF);
          return xmlSchemaDatatype.ParseValue(this.storedCharacters.ToString(), this.NameTable, (IXmlNamespaceResolver) this.dtdReader.ParserContext.NamespaceManager);
        case XmlNodeType.Attribute:
          return xmlSchemaDatatype.ParseValue(this.Value, this.NameTable, (IXmlNamespaceResolver) this.dtdReader.ParserContext.NamespaceManager);
        default:
          return (object) null;
      }
    }

    public override void ResolveEntity() => this.validatingReader.ResolveEntity();

    internal void OnValidationEvent(object o, ValidationEventArgs e)
    {
      if (this.ValidationEventHandler != null)
        this.ValidationEventHandler(o, e);
      else if (this.ValidationType != ValidationType.None && e.Severity == XmlSeverityType.Error)
        throw e.Exception;
    }

    [MonoTODO]
    public override int ReadContentAsBase64(byte[] buffer, int offset, int length) => this.validatingReader != null ? this.validatingReader.ReadContentAsBase64(buffer, offset, length) : this.sourceReader.ReadContentAsBase64(buffer, offset, length);

    [MonoTODO]
    public override int ReadContentAsBinHex(byte[] buffer, int offset, int length) => this.validatingReader != null ? this.validatingReader.ReadContentAsBinHex(buffer, offset, length) : this.sourceReader.ReadContentAsBinHex(buffer, offset, length);

    [MonoTODO]
    public override int ReadElementContentAsBase64(byte[] buffer, int offset, int length) => this.validatingReader != null ? this.validatingReader.ReadElementContentAsBase64(buffer, offset, length) : this.sourceReader.ReadElementContentAsBase64(buffer, offset, length);

    [MonoTODO]
    public override int ReadElementContentAsBinHex(byte[] buffer, int offset, int length) => this.validatingReader != null ? this.validatingReader.ReadElementContentAsBinHex(buffer, offset, length) : this.sourceReader.ReadElementContentAsBinHex(buffer, offset, length);
  }
}
