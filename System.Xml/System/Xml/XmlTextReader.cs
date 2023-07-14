// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlTextReader
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace System.Xml
{
  public class XmlTextReader : XmlReader, IHasXmlParserContext, IXmlLineInfo, IXmlNamespaceResolver
  {
    private XmlTextReader entity;
    private Mono.Xml2.XmlTextReader source;
    private bool entityInsideAttribute;
    private bool insideAttribute;
    private Stack<string> entityNameStack;

    protected XmlTextReader()
    {
    }

    public XmlTextReader(Stream input)
      : this((TextReader) new XmlStreamReader(input))
    {
    }

    public XmlTextReader(string url)
      : this(url, (XmlNameTable) new System.Xml.NameTable())
    {
    }

    public XmlTextReader(TextReader input)
      : this(input, (XmlNameTable) new System.Xml.NameTable())
    {
    }

    protected XmlTextReader(XmlNameTable nt)
      : this(string.Empty, XmlNodeType.Element, (XmlParserContext) null)
    {
    }

    public XmlTextReader(Stream input, XmlNameTable nt)
      : this((TextReader) new XmlStreamReader(input), nt)
    {
    }

    public XmlTextReader(string url, Stream input)
      : this(url, (TextReader) new XmlStreamReader(input))
    {
    }

    public XmlTextReader(string url, TextReader input)
      : this(url, input, (XmlNameTable) new System.Xml.NameTable())
    {
    }

    public XmlTextReader(string url, XmlNameTable nt) => this.source = new Mono.Xml2.XmlTextReader(url, nt);

    public XmlTextReader(TextReader input, XmlNameTable nt)
      : this(string.Empty, input, nt)
    {
    }

    public XmlTextReader(Stream xmlFragment, XmlNodeType fragType, XmlParserContext context) => this.source = new Mono.Xml2.XmlTextReader(xmlFragment, fragType, context);

    public XmlTextReader(string url, Stream input, XmlNameTable nt)
      : this(url, (TextReader) new XmlStreamReader(input), nt)
    {
    }

    public XmlTextReader(string url, TextReader input, XmlNameTable nt) => this.source = new Mono.Xml2.XmlTextReader(url, input, nt);

    public XmlTextReader(string xmlFragment, XmlNodeType fragType, XmlParserContext context) => this.source = new Mono.Xml2.XmlTextReader(xmlFragment, fragType, context);

    internal XmlTextReader(string baseURI, TextReader xmlFragment, XmlNodeType fragType) => this.source = new Mono.Xml2.XmlTextReader(baseURI, xmlFragment, fragType);

    internal XmlTextReader(
      string baseURI,
      TextReader xmlFragment,
      XmlNodeType fragType,
      XmlParserContext context)
    {
      this.source = new Mono.Xml2.XmlTextReader(baseURI, xmlFragment, fragType, context);
    }

    internal XmlTextReader(
      bool dummy,
      XmlResolver resolver,
      string url,
      XmlNodeType fragType,
      XmlParserContext context)
    {
      this.source = new Mono.Xml2.XmlTextReader(dummy, resolver, url, fragType, context);
    }

    private XmlTextReader(Mono.Xml2.XmlTextReader entityContainer, bool insideAttribute)
    {
      this.source = entityContainer;
      this.entityInsideAttribute = insideAttribute;
    }

    XmlParserContext IHasXmlParserContext.ParserContext => this.ParserContext;

    IDictionary<string, string> IXmlNamespaceResolver.GetNamespacesInScope(XmlNamespaceScope scope) => this.GetNamespacesInScope(scope);

    string IXmlNamespaceResolver.LookupPrefix(string ns) => ((IXmlNamespaceResolver) this.Current).LookupPrefix(ns);

    private XmlReader Current => this.entity != null && this.entity.ReadState != ReadState.Initial ? (XmlReader) this.entity : (XmlReader) this.source;

    public override int AttributeCount => this.Current.AttributeCount;

    public override string BaseURI => this.Current.BaseURI;

    public override bool CanReadBinaryContent => true;

    public override bool CanReadValueChunk => true;

    public override bool CanResolveEntity => true;

    public override int Depth => this.entity != null && this.entity.ReadState == ReadState.Interactive ? this.source.Depth + this.entity.Depth + 1 : this.source.Depth;

    public override bool EOF => this.source.EOF;

    public override bool HasValue => this.Current.HasValue;

    public override bool IsDefault => this.Current.IsDefault;

    public override bool IsEmptyElement => this.Current.IsEmptyElement;

    public override string LocalName => this.Current.LocalName;

    public override string Name => this.Current.Name;

    public override string NamespaceURI => this.Current.NamespaceURI;

    public override XmlNameTable NameTable => this.Current.NameTable;

    public override XmlNodeType NodeType
    {
      get
      {
        if (this.entity == null || this.entity.ReadState == ReadState.Initial)
          return this.source.NodeType;
        return this.entity.EOF ? XmlNodeType.EndEntity : this.entity.NodeType;
      }
    }

    internal XmlParserContext ParserContext => ((IHasXmlParserContext) this.Current).ParserContext;

    public override string Prefix => this.Current.Prefix;

    public override char QuoteChar => this.Current.QuoteChar;

    public override ReadState ReadState => this.entity != null ? ReadState.Interactive : this.source.ReadState;

    public override XmlReaderSettings Settings => base.Settings;

    public override string Value => this.Current.Value;

    public override string XmlLang => this.Current.XmlLang;

    public override XmlSpace XmlSpace => this.Current.XmlSpace;

    internal bool CharacterChecking
    {
      get => this.entity != null ? this.entity.CharacterChecking : this.source.CharacterChecking;
      set
      {
        if (this.entity != null)
          this.entity.CharacterChecking = value;
        this.source.CharacterChecking = value;
      }
    }

    internal bool CloseInput
    {
      get => this.entity != null ? this.entity.CloseInput : this.source.CloseInput;
      set
      {
        if (this.entity != null)
          this.entity.CloseInput = value;
        this.source.CloseInput = value;
      }
    }

    internal ConformanceLevel Conformance
    {
      get => this.source.Conformance;
      set
      {
        if (this.entity != null)
          this.entity.Conformance = value;
        this.source.Conformance = value;
      }
    }

    internal XmlResolver Resolver => this.source.Resolver;

    private void CopyProperties(XmlTextReader other)
    {
      this.CharacterChecking = other.CharacterChecking;
      this.CloseInput = other.CloseInput;
      if (other.Settings != null)
        this.Conformance = other.Settings.ConformanceLevel;
      this.XmlResolver = other.Resolver;
    }

    public Encoding Encoding => this.entity != null ? this.entity.Encoding : this.source.Encoding;

    public EntityHandling EntityHandling
    {
      get => this.source.EntityHandling;
      set
      {
        if (this.entity != null)
          this.entity.EntityHandling = value;
        this.source.EntityHandling = value;
      }
    }

    public int LineNumber => this.entity != null ? this.entity.LineNumber : this.source.LineNumber;

    public int LinePosition => this.entity != null ? this.entity.LinePosition : this.source.LinePosition;

    public bool Namespaces
    {
      get => this.source.Namespaces;
      set
      {
        if (this.entity != null)
          this.entity.Namespaces = value;
        this.source.Namespaces = value;
      }
    }

    public bool Normalization
    {
      get => this.source.Normalization;
      set
      {
        if (this.entity != null)
          this.entity.Normalization = value;
        this.source.Normalization = value;
      }
    }

    public bool ProhibitDtd
    {
      get => this.source.ProhibitDtd;
      set
      {
        if (this.entity != null)
          this.entity.ProhibitDtd = value;
        this.source.ProhibitDtd = value;
      }
    }

    public WhitespaceHandling WhitespaceHandling
    {
      get => this.source.WhitespaceHandling;
      set
      {
        if (this.entity != null)
          this.entity.WhitespaceHandling = value;
        this.source.WhitespaceHandling = value;
      }
    }

    public XmlResolver XmlResolver
    {
      set
      {
        if (this.entity != null)
          this.entity.XmlResolver = value;
        this.source.XmlResolver = value;
      }
    }

    internal void AdjustLineInfoOffset(int lineNumberOffset, int linePositionOffset)
    {
      if (this.entity != null)
        this.entity.AdjustLineInfoOffset(lineNumberOffset, linePositionOffset);
      this.source.AdjustLineInfoOffset(lineNumberOffset, linePositionOffset);
    }

    internal void SetNameTable(XmlNameTable nameTable)
    {
      if (this.entity != null)
        this.entity.SetNameTable(nameTable);
      this.source.SetNameTable(nameTable);
    }

    internal void SkipTextDeclaration()
    {
      if (this.entity != null)
        this.entity.SkipTextDeclaration();
      else
        this.source.SkipTextDeclaration();
    }

    public override void Close()
    {
      if (this.entity != null)
        this.entity.Close();
      this.source.Close();
    }

    public override string GetAttribute(int i) => this.Current.GetAttribute(i);

    public override string GetAttribute(string name) => this.Current.GetAttribute(name);

    public override string GetAttribute(string localName, string namespaceURI) => this.Current.GetAttribute(localName, namespaceURI);

    public IDictionary<string, string> GetNamespacesInScope(XmlNamespaceScope scope) => ((IXmlNamespaceResolver) this.Current).GetNamespacesInScope(scope);

    public override string LookupNamespace(string prefix) => this.Current.LookupNamespace(prefix);

    public override void MoveToAttribute(int i)
    {
      if (this.entity != null && this.entityInsideAttribute)
        this.CloseEntity();
      this.Current.MoveToAttribute(i);
      this.insideAttribute = true;
    }

    public override bool MoveToAttribute(string name)
    {
      if (this.entity != null && !this.entityInsideAttribute)
        return this.entity.MoveToAttribute(name);
      if (!this.source.MoveToAttribute(name))
        return false;
      if (this.entity != null && this.entityInsideAttribute)
        this.CloseEntity();
      this.insideAttribute = true;
      return true;
    }

    public override bool MoveToAttribute(string localName, string namespaceName)
    {
      if (this.entity != null && !this.entityInsideAttribute)
        return this.entity.MoveToAttribute(localName, namespaceName);
      if (!this.source.MoveToAttribute(localName, namespaceName))
        return false;
      if (this.entity != null && this.entityInsideAttribute)
        this.CloseEntity();
      this.insideAttribute = true;
      return true;
    }

    public override bool MoveToElement()
    {
      if (this.entity != null && this.entityInsideAttribute)
        this.CloseEntity();
      if (!this.Current.MoveToElement())
        return false;
      this.insideAttribute = false;
      return true;
    }

    public override bool MoveToFirstAttribute()
    {
      if (this.entity != null && !this.entityInsideAttribute)
        return this.entity.MoveToFirstAttribute();
      if (!this.source.MoveToFirstAttribute())
        return false;
      if (this.entity != null && this.entityInsideAttribute)
        this.CloseEntity();
      this.insideAttribute = true;
      return true;
    }

    public override bool MoveToNextAttribute()
    {
      if (this.entity != null && !this.entityInsideAttribute)
        return this.entity.MoveToNextAttribute();
      if (!this.source.MoveToNextAttribute())
        return false;
      if (this.entity != null && this.entityInsideAttribute)
        this.CloseEntity();
      this.insideAttribute = true;
      return true;
    }

    public override bool Read()
    {
      this.insideAttribute = false;
      if (this.entity != null && (this.entityInsideAttribute || this.entity.EOF))
        this.CloseEntity();
      if (this.entity != null)
      {
        if (this.entity.Read() || this.EntityHandling != EntityHandling.ExpandEntities)
          return true;
        this.CloseEntity();
        return this.Read();
      }
      if (!this.source.Read())
        return false;
      if (this.EntityHandling != EntityHandling.ExpandEntities || this.source.NodeType != XmlNodeType.EntityReference)
        return true;
      this.ResolveEntity();
      return this.Read();
    }

    public override bool ReadAttributeValue()
    {
      if (this.entity != null && this.entityInsideAttribute)
      {
        if (this.entity.EOF)
        {
          this.CloseEntity();
        }
        else
        {
          this.entity.Read();
          return true;
        }
      }
      return this.Current.ReadAttributeValue();
    }

    public override string ReadString() => base.ReadString();

    public void ResetState()
    {
      if (this.entity != null)
        this.CloseEntity();
      this.source.ResetState();
    }

    public override void ResolveEntity()
    {
      if (this.entity != null)
      {
        this.entity.ResolveEntity();
      }
      else
      {
        if (this.source.NodeType != XmlNodeType.EntityReference)
          throw new InvalidOperationException("The current node is not an Entity Reference");
        Mono.Xml2.XmlTextReader entityContainer = (Mono.Xml2.XmlTextReader) null;
        if (this.ParserContext.Dtd != null)
          entityContainer = this.ParserContext.Dtd.GenerateEntityContentReader(this.source.Name, this.ParserContext);
        if (entityContainer == null)
          throw new XmlException((IXmlLineInfo) this, this.BaseURI, string.Format("Reference to undeclared entity '{0}'.", (object) this.source.Name));
        if (this.entityNameStack == null)
          this.entityNameStack = new Stack<string>();
        else if (this.entityNameStack.Contains(this.Name))
          throw new XmlException(string.Format("General entity '{0}' has an invalid recursive reference to itself.", (object) this.Name));
        this.entityNameStack.Push(this.Name);
        this.entity = new XmlTextReader(entityContainer, this.insideAttribute);
        this.entity.entityNameStack = this.entityNameStack;
        this.entity.CopyProperties(this);
      }
    }

    private void CloseEntity()
    {
      this.entity.Close();
      this.entity = (XmlTextReader) null;
      this.entityNameStack.Pop();
    }

    public override void Skip() => base.Skip();

    [MonoTODO]
    public TextReader GetRemainder()
    {
      if (this.entity != null)
      {
        this.entity.Close();
        this.entity = (XmlTextReader) null;
        this.entityNameStack.Pop();
      }
      return this.source.GetRemainder();
    }

    public bool HasLineInfo() => true;

    [MonoTODO]
    public int ReadBase64(byte[] buffer, int offset, int length) => this.entity != null ? this.entity.ReadBase64(buffer, offset, length) : this.source.ReadBase64(buffer, offset, length);

    [MonoTODO]
    public int ReadBinHex(byte[] buffer, int offset, int length) => this.entity != null ? this.entity.ReadBinHex(buffer, offset, length) : this.source.ReadBinHex(buffer, offset, length);

    [MonoTODO]
    public int ReadChars(char[] buffer, int offset, int length) => this.entity != null ? this.entity.ReadChars(buffer, offset, length) : this.source.ReadChars(buffer, offset, length);

    [MonoTODO]
    public override int ReadContentAsBase64(byte[] buffer, int offset, int length) => this.entity != null ? this.entity.ReadContentAsBase64(buffer, offset, length) : this.source.ReadContentAsBase64(buffer, offset, length);

    [MonoTODO]
    public override int ReadContentAsBinHex(byte[] buffer, int offset, int length) => this.entity != null ? this.entity.ReadContentAsBinHex(buffer, offset, length) : this.source.ReadContentAsBinHex(buffer, offset, length);

    [MonoTODO]
    public override int ReadElementContentAsBase64(byte[] buffer, int offset, int length) => this.entity != null ? this.entity.ReadElementContentAsBase64(buffer, offset, length) : this.source.ReadElementContentAsBase64(buffer, offset, length);

    [MonoTODO]
    public override int ReadElementContentAsBinHex(byte[] buffer, int offset, int length) => this.entity != null ? this.entity.ReadElementContentAsBinHex(buffer, offset, length) : this.source.ReadElementContentAsBinHex(buffer, offset, length);
  }
}
