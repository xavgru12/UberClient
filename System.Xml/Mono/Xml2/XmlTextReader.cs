// Decompiled with JetBrains decompiler
// Type: Mono.Xml2.XmlTextReader
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace Mono.Xml2
{
  internal class XmlTextReader : XmlReader, IHasXmlParserContext, IXmlLineInfo, IXmlNamespaceResolver
  {
    private const int peekCharCapacity = 1024;
    private XmlTextReader.XmlTokenInfo cursorToken;
    private XmlTextReader.XmlTokenInfo currentToken;
    private XmlTextReader.XmlAttributeTokenInfo currentAttributeToken;
    private XmlTextReader.XmlTokenInfo currentAttributeValueToken;
    private XmlTextReader.XmlAttributeTokenInfo[] attributeTokens = new XmlTextReader.XmlAttributeTokenInfo[10];
    private XmlTextReader.XmlTokenInfo[] attributeValueTokens = new XmlTextReader.XmlTokenInfo[10];
    private int currentAttribute;
    private int currentAttributeValue;
    private int attributeCount;
    private XmlParserContext parserContext;
    private XmlNameTable nameTable;
    private XmlNamespaceManager nsmgr;
    private ReadState readState;
    private bool disallowReset;
    private int depth;
    private int elementDepth;
    private bool depthUp;
    private bool popScope;
    private XmlTextReader.TagName[] elementNames;
    private int elementNameStackPos;
    private bool allowMultipleRoot;
    private bool isStandalone;
    private bool returnEntityReference;
    private string entityReferenceName;
    private StringBuilder valueBuffer;
    private TextReader reader;
    private char[] peekChars;
    private int peekCharsIndex;
    private int peekCharsLength;
    private int curNodePeekIndex;
    private bool preserveCurrentTag;
    private int line;
    private int column;
    private int currentLinkedNodeLineNumber;
    private int currentLinkedNodeLinePosition;
    private bool useProceedingLineInfo;
    private XmlNodeType startNodeType;
    private XmlNodeType currentState;
    private int nestLevel;
    private bool readCharsInProgress;
    private XmlReaderBinarySupport.CharGetter binaryCharGetter;
    private bool namespaces = true;
    private WhitespaceHandling whitespaceHandling;
    private XmlResolver resolver = (XmlResolver) new XmlUrlResolver();
    private bool normalization;
    private bool checkCharacters;
    private bool prohibitDtd;
    private bool closeInput = true;
    private EntityHandling entityHandling;
    private System.Xml.NameTable whitespacePool;
    private char[] whitespaceCache;
    private XmlTextReader.DtdInputStateStack stateStack = new XmlTextReader.DtdInputStateStack();

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
      : this(string.Empty, (TextReader) null, XmlNodeType.None, (XmlParserContext) null)
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

    public XmlTextReader(string url, XmlNameTable nt)
    {
      string absoluteUriString;
      Stream streamFromUrl = this.GetStreamFromUrl(url, out absoluteUriString);
      XmlParserContext context = new XmlParserContext(nt, new XmlNamespaceManager(nt), string.Empty, XmlSpace.None);
      this.InitializeContext(absoluteUriString, context, (TextReader) new XmlStreamReader(streamFromUrl), XmlNodeType.Document);
    }

    public XmlTextReader(TextReader input, XmlNameTable nt)
      : this(string.Empty, input, nt)
    {
    }

    internal XmlTextReader(
      bool dummy,
      XmlResolver resolver,
      string url,
      XmlNodeType fragType,
      XmlParserContext context)
    {
      if (resolver == null)
        resolver = (XmlResolver) new XmlUrlResolver();
      this.XmlResolver = resolver;
      string absoluteUriString;
      Stream streamFromUrl = this.GetStreamFromUrl(url, out absoluteUriString);
      this.InitializeContext(absoluteUriString, context, (TextReader) new XmlStreamReader(streamFromUrl), fragType);
    }

    public XmlTextReader(Stream xmlFragment, XmlNodeType fragType, XmlParserContext context)
      : this(context == null ? string.Empty : context.BaseURI, (TextReader) new XmlStreamReader(xmlFragment), fragType, context)
    {
      this.disallowReset = true;
    }

    internal XmlTextReader(string baseURI, TextReader xmlFragment, XmlNodeType fragType)
      : this(baseURI, xmlFragment, fragType, (XmlParserContext) null)
    {
    }

    public XmlTextReader(string url, Stream input, XmlNameTable nt)
      : this(url, (TextReader) new XmlStreamReader(input), nt)
    {
    }

    public XmlTextReader(string url, TextReader input, XmlNameTable nt)
      : this(url, input, XmlNodeType.Document, (XmlParserContext) null)
    {
    }

    public XmlTextReader(string xmlFragment, XmlNodeType fragType, XmlParserContext context)
      : this(context == null ? string.Empty : context.BaseURI, (TextReader) new StringReader(xmlFragment), fragType, context)
    {
      this.disallowReset = true;
    }

    internal XmlTextReader(
      string url,
      TextReader fragment,
      XmlNodeType fragType,
      XmlParserContext context)
    {
      this.InitializeContext(url, context, fragment, fragType);
    }

    XmlParserContext IHasXmlParserContext.ParserContext => this.parserContext;

    IDictionary<string, string> IXmlNamespaceResolver.GetNamespacesInScope(XmlNamespaceScope scope) => this.GetNamespacesInScope(scope);

    string IXmlNamespaceResolver.LookupPrefix(string ns) => this.LookupPrefix(ns, false);

    private Stream GetStreamFromUrl(string url, out string absoluteUriString)
    {
      switch (url)
      {
        case null:
          throw new ArgumentNullException(nameof (url));
        case "":
          throw new ArgumentException(nameof (url));
        default:
          Uri absoluteUri = this.resolver.ResolveUri((Uri) null, url);
          absoluteUriString = !(absoluteUri != (Uri) null) ? string.Empty : absoluteUri.ToString();
          return this.resolver.GetEntity(absoluteUri, (string) null, typeof (Stream)) as Stream;
      }
    }

    public override int AttributeCount => this.attributeCount;

    public override string BaseURI => this.parserContext.BaseURI;

    public override bool CanReadBinaryContent => true;

    public override bool CanReadValueChunk => true;

    internal bool CharacterChecking
    {
      get => this.checkCharacters;
      set => this.checkCharacters = value;
    }

    internal bool CloseInput
    {
      get => this.closeInput;
      set => this.closeInput = value;
    }

    public override int Depth
    {
      get
      {
        int num = this.currentToken.NodeType != XmlNodeType.Element ? -1 : 0;
        if (this.currentAttributeValue >= 0)
          return num + this.elementDepth + 2;
        return this.currentAttribute >= 0 ? num + this.elementDepth + 1 : this.elementDepth;
      }
    }

    public Encoding Encoding => this.parserContext.Encoding;

    public EntityHandling EntityHandling
    {
      get => this.entityHandling;
      set => this.entityHandling = value;
    }

    public override bool EOF => this.readState == ReadState.EndOfFile;

    public override bool HasValue => this.cursorToken.Value != null;

    public override bool IsDefault => false;

    public override bool IsEmptyElement => this.cursorToken.IsEmptyElement;

    public int LineNumber => this.useProceedingLineInfo ? this.line : this.cursorToken.LineNumber;

    public int LinePosition => this.useProceedingLineInfo ? this.column : this.cursorToken.LinePosition;

    public override string LocalName => this.cursorToken.LocalName;

    public override string Name => this.cursorToken.Name;

    public bool Namespaces
    {
      get => this.namespaces;
      set
      {
        if (this.readState != ReadState.Initial)
          throw new InvalidOperationException("Namespaces have to be set before reading.");
        this.namespaces = value;
      }
    }

    public override string NamespaceURI => this.cursorToken.NamespaceURI;

    public override XmlNameTable NameTable => this.nameTable;

    public override XmlNodeType NodeType => this.cursorToken.NodeType;

    public bool Normalization
    {
      get => this.normalization;
      set => this.normalization = value;
    }

    public override string Prefix => this.cursorToken.Prefix;

    public bool ProhibitDtd
    {
      get => this.prohibitDtd;
      set => this.prohibitDtd = value;
    }

    public override char QuoteChar => this.cursorToken.QuoteChar;

    public override ReadState ReadState => this.readState;

    public override XmlReaderSettings Settings => base.Settings;

    public override string Value => this.cursorToken.Value != null ? this.cursorToken.Value : string.Empty;

    public WhitespaceHandling WhitespaceHandling
    {
      get => this.whitespaceHandling;
      set => this.whitespaceHandling = value;
    }

    public override string XmlLang => this.parserContext.XmlLang;

    public XmlResolver XmlResolver
    {
      set => this.resolver = value;
    }

    public override XmlSpace XmlSpace => this.parserContext.XmlSpace;

    public override void Close()
    {
      this.readState = ReadState.Closed;
      this.cursorToken.Clear();
      this.currentToken.Clear();
      this.attributeCount = 0;
      if (!this.closeInput || this.reader == null)
        return;
      this.reader.Close();
    }

    public override string GetAttribute(int i)
    {
      if (i >= this.attributeCount)
        throw new ArgumentOutOfRangeException("i is smaller than AttributeCount");
      return this.attributeTokens[i].Value;
    }

    public override string GetAttribute(string name)
    {
      for (int index = 0; index < this.attributeCount; ++index)
      {
        if (this.attributeTokens[index].Name == name)
          return this.attributeTokens[index].Value;
      }
      return (string) null;
    }

    private int GetIndexOfQualifiedAttribute(string localName, string namespaceURI)
    {
      for (int qualifiedAttribute = 0; qualifiedAttribute < this.attributeCount; ++qualifiedAttribute)
      {
        XmlTextReader.XmlAttributeTokenInfo attributeToken = this.attributeTokens[qualifiedAttribute];
        if (attributeToken.LocalName == localName && attributeToken.NamespaceURI == namespaceURI)
          return qualifiedAttribute;
      }
      return -1;
    }

    public override string GetAttribute(string localName, string namespaceURI)
    {
      int qualifiedAttribute = this.GetIndexOfQualifiedAttribute(localName, namespaceURI);
      return qualifiedAttribute < 0 ? (string) null : this.attributeTokens[qualifiedAttribute].Value;
    }

    public IDictionary<string, string> GetNamespacesInScope(XmlNamespaceScope scope) => this.nsmgr.GetNamespacesInScope(scope);

    public TextReader GetRemainder() => this.peekCharsLength < 0 ? this.reader : (TextReader) new StringReader(new string(this.peekChars, this.peekCharsIndex, this.peekCharsLength - this.peekCharsIndex) + this.reader.ReadToEnd());

    public bool HasLineInfo() => true;

    public override string LookupNamespace(string prefix) => this.LookupNamespace(prefix, false);

    private string LookupNamespace(string prefix, bool atomizedNames)
    {
      string str = this.nsmgr.LookupNamespace(prefix, atomizedNames);
      return str == string.Empty ? (string) null : str;
    }

    public string LookupPrefix(string ns, bool atomizedName) => this.nsmgr.LookupPrefix(ns, atomizedName);

    public override void MoveToAttribute(int i)
    {
      this.currentAttribute = i < this.attributeCount ? i : throw new ArgumentOutOfRangeException("attribute index out of range.");
      this.currentAttributeValue = -1;
      this.cursorToken = (XmlTextReader.XmlTokenInfo) this.attributeTokens[i];
    }

    public override bool MoveToAttribute(string name)
    {
      for (int i = 0; i < this.attributeCount; ++i)
      {
        if (this.attributeTokens[i].Name == name)
        {
          this.MoveToAttribute(i);
          return true;
        }
      }
      return false;
    }

    public override bool MoveToAttribute(string localName, string namespaceName)
    {
      int qualifiedAttribute = this.GetIndexOfQualifiedAttribute(localName, namespaceName);
      if (qualifiedAttribute < 0)
        return false;
      this.MoveToAttribute(qualifiedAttribute);
      return true;
    }

    public override bool MoveToElement()
    {
      if (this.currentToken == null || this.cursorToken == this.currentToken || this.currentAttribute < 0)
        return false;
      this.currentAttribute = -1;
      this.currentAttributeValue = -1;
      this.cursorToken = this.currentToken;
      return true;
    }

    public override bool MoveToFirstAttribute()
    {
      if (this.attributeCount == 0)
        return false;
      this.MoveToElement();
      return this.MoveToNextAttribute();
    }

    public override bool MoveToNextAttribute()
    {
      if (this.currentAttribute == 0 && this.attributeCount == 0 || this.currentAttribute + 1 >= this.attributeCount)
        return false;
      ++this.currentAttribute;
      this.currentAttributeValue = -1;
      this.cursorToken = (XmlTextReader.XmlTokenInfo) this.attributeTokens[this.currentAttribute];
      return true;
    }

    public override bool Read()
    {
      if (this.readState == ReadState.Closed)
        return false;
      this.curNodePeekIndex = this.peekCharsIndex;
      this.preserveCurrentTag = true;
      this.nestLevel = 0;
      this.ClearValueBuffer();
      if (this.startNodeType == XmlNodeType.Attribute)
      {
        if (this.currentAttribute == 0)
          return false;
        this.SkipTextDeclaration();
        this.ClearAttributes();
        this.IncrementAttributeToken();
        this.ReadAttributeValueTokens(34);
        this.cursorToken = (XmlTextReader.XmlTokenInfo) this.attributeTokens[0];
        this.currentAttributeValue = -1;
        this.readState = ReadState.Interactive;
        return true;
      }
      if (this.readState == ReadState.Initial && this.currentState == XmlNodeType.Element)
        this.SkipTextDeclaration();
      if (this.Binary != null)
        this.Binary.Reset();
      this.readState = ReadState.Interactive;
      this.currentLinkedNodeLineNumber = this.line;
      this.currentLinkedNodeLinePosition = this.column;
      this.useProceedingLineInfo = true;
      this.cursorToken = this.currentToken;
      this.attributeCount = 0;
      this.currentAttribute = this.currentAttributeValue = -1;
      this.currentToken.Clear();
      if (this.depthUp)
      {
        ++this.depth;
        this.depthUp = false;
      }
      if (this.readCharsInProgress)
      {
        this.readCharsInProgress = false;
        return this.ReadUntilEndTag();
      }
      bool flag = this.ReadContent();
      if (!flag && this.startNodeType == XmlNodeType.Document && this.currentState != XmlNodeType.EndElement)
        throw this.NotWFError("Document element did not appear.");
      this.useProceedingLineInfo = false;
      return flag;
    }

    public override bool ReadAttributeValue()
    {
      if (this.readState == ReadState.Initial && this.startNodeType == XmlNodeType.Attribute)
        this.Read();
      if (this.currentAttribute < 0)
        return false;
      XmlTextReader.XmlAttributeTokenInfo attributeToken = this.attributeTokens[this.currentAttribute];
      if (this.currentAttributeValue < 0)
        this.currentAttributeValue = attributeToken.ValueTokenStartIndex - 1;
      if (this.currentAttributeValue >= attributeToken.ValueTokenEndIndex)
        return false;
      ++this.currentAttributeValue;
      this.cursorToken = this.attributeValueTokens[this.currentAttributeValue];
      return true;
    }

    public int ReadBase64(byte[] buffer, int offset, int length)
    {
      this.BinaryCharGetter = this.binaryCharGetter;
      try
      {
        return this.Binary.ReadBase64(buffer, offset, length);
      }
      finally
      {
        this.BinaryCharGetter = (XmlReaderBinarySupport.CharGetter) null;
      }
    }

    public int ReadBinHex(byte[] buffer, int offset, int length)
    {
      this.BinaryCharGetter = this.binaryCharGetter;
      try
      {
        return this.Binary.ReadBinHex(buffer, offset, length);
      }
      finally
      {
        this.BinaryCharGetter = (XmlReaderBinarySupport.CharGetter) null;
      }
    }

    public int ReadChars(char[] buffer, int offset, int length)
    {
      if (offset < 0)
        throw new ArgumentOutOfRangeException("Offset must be non-negative integer.");
      if (length < 0)
        throw new ArgumentOutOfRangeException("Length must be non-negative integer.");
      if (buffer.Length < offset + length)
        throw new ArgumentOutOfRangeException("buffer length is smaller than the sum of offset and length.");
      if (this.IsEmptyElement)
      {
        this.Read();
        return 0;
      }
      if (!this.readCharsInProgress && this.NodeType != XmlNodeType.Element)
        return 0;
      this.preserveCurrentTag = false;
      this.readCharsInProgress = true;
      this.useProceedingLineInfo = true;
      return this.ReadCharsInternal(buffer, offset, length);
    }

    public void ResetState()
    {
      if (this.disallowReset)
        throw new InvalidOperationException("Cannot call ResetState when parsing an XML fragment.");
      this.Clear();
    }

    public override void ResolveEntity() => throw new InvalidOperationException("XmlTextReader cannot resolve external entities.");

    [MonoTODO]
    public override void Skip() => base.Skip();

    internal DTDObjectModel DTD => this.parserContext.Dtd;

    internal XmlResolver Resolver => this.resolver;

    private XmlException NotWFError(string message) => new XmlException((IXmlLineInfo) this, this.BaseURI, message);

    private void Init()
    {
      this.allowMultipleRoot = false;
      this.elementNames = new XmlTextReader.TagName[10];
      this.valueBuffer = new StringBuilder();
      this.binaryCharGetter = new XmlReaderBinarySupport.CharGetter(this.ReadChars);
      this.checkCharacters = true;
      if (this.Settings != null)
        this.checkCharacters = this.Settings.CheckCharacters;
      this.prohibitDtd = false;
      this.closeInput = true;
      this.entityHandling = EntityHandling.ExpandCharEntities;
      this.peekCharsIndex = 0;
      if (this.peekChars == null)
        this.peekChars = new char[1024];
      this.peekCharsLength = -1;
      this.curNodePeekIndex = -1;
      this.line = 1;
      this.column = 1;
      this.currentLinkedNodeLineNumber = this.currentLinkedNodeLinePosition = 0;
      this.Clear();
    }

    private void Clear()
    {
      this.currentToken = new XmlTextReader.XmlTokenInfo(this);
      this.cursorToken = this.currentToken;
      this.currentAttribute = -1;
      this.currentAttributeValue = -1;
      this.attributeCount = 0;
      this.readState = ReadState.Initial;
      this.depth = 0;
      this.elementDepth = 0;
      this.depthUp = false;
      this.popScope = this.allowMultipleRoot = false;
      this.elementNameStackPos = 0;
      this.isStandalone = false;
      this.returnEntityReference = false;
      this.entityReferenceName = string.Empty;
      this.useProceedingLineInfo = false;
      this.currentState = XmlNodeType.None;
      this.readCharsInProgress = false;
    }

    private void InitializeContext(
      string url,
      XmlParserContext context,
      TextReader fragment,
      XmlNodeType fragType)
    {
      this.startNodeType = fragType;
      this.parserContext = context;
      if (context == null)
      {
        XmlNameTable xmlNameTable = (XmlNameTable) new System.Xml.NameTable();
        this.parserContext = new XmlParserContext(xmlNameTable, new XmlNamespaceManager(xmlNameTable), string.Empty, XmlSpace.None);
      }
      this.nameTable = this.parserContext.NameTable;
      this.nameTable = this.nameTable == null ? (XmlNameTable) new System.Xml.NameTable() : this.nameTable;
      this.nsmgr = this.parserContext.NamespaceManager;
      this.nsmgr = this.nsmgr == null ? new XmlNamespaceManager(this.nameTable) : this.nsmgr;
      if (url != null && url.Length > 0)
        this.parserContext.BaseURI = new Uri(url, UriKind.RelativeOrAbsolute).ToString();
      this.Init();
      this.reader = fragment;
      switch (fragType)
      {
        case XmlNodeType.Element:
          this.currentState = XmlNodeType.Element;
          this.allowMultipleRoot = true;
          break;
        case XmlNodeType.Attribute:
          this.reader = (TextReader) new StringReader(fragment.ReadToEnd().Replace("\"", "&quot;"));
          break;
        case XmlNodeType.Document:
          break;
        default:
          throw new XmlException(string.Format("NodeType {0} is not allowed to create XmlTextReader.", (object) fragType));
      }
    }

    internal ConformanceLevel Conformance
    {
      get => this.allowMultipleRoot ? ConformanceLevel.Fragment : ConformanceLevel.Document;
      set
      {
        if (value != ConformanceLevel.Fragment)
          return;
        this.currentState = XmlNodeType.Element;
        this.allowMultipleRoot = true;
      }
    }

    internal void AdjustLineInfoOffset(int lineNumberOffset, int linePositionOffset)
    {
      this.line += lineNumberOffset;
      this.column += linePositionOffset;
    }

    internal void SetNameTable(XmlNameTable nameTable) => this.parserContext.NameTable = nameTable;

    private void SetProperties(
      XmlNodeType nodeType,
      string name,
      string prefix,
      string localName,
      bool isEmptyElement,
      string value,
      bool clearAttributes)
    {
      this.SetTokenProperties(this.currentToken, nodeType, name, prefix, localName, isEmptyElement, value, clearAttributes);
      this.currentToken.LineNumber = this.currentLinkedNodeLineNumber;
      this.currentToken.LinePosition = this.currentLinkedNodeLinePosition;
    }

    private void SetTokenProperties(
      XmlTextReader.XmlTokenInfo token,
      XmlNodeType nodeType,
      string name,
      string prefix,
      string localName,
      bool isEmptyElement,
      string value,
      bool clearAttributes)
    {
      token.NodeType = nodeType;
      token.Name = name;
      token.Prefix = prefix;
      token.LocalName = localName;
      token.IsEmptyElement = isEmptyElement;
      token.Value = value;
      this.elementDepth = this.depth;
      if (!clearAttributes)
        return;
      this.ClearAttributes();
    }

    private void ClearAttributes()
    {
      this.attributeCount = 0;
      this.currentAttribute = -1;
      this.currentAttributeValue = -1;
    }

    private int PeekSurrogate(int c)
    {
      if (this.peekCharsLength <= this.peekCharsIndex + 1 && !this.ReadTextReader(c))
        return c;
      int peekChar1 = (int) this.peekChars[this.peekCharsIndex];
      int peekChar2 = (int) this.peekChars[this.peekCharsIndex + 1];
      return (peekChar1 & 64512) != 55296 || (peekChar2 & 64512) != 56320 ? peekChar1 : 65536 + (peekChar1 - 55296) * 1024 + (peekChar2 - 56320);
    }

    private int PeekChar()
    {
      if (this.peekCharsIndex < this.peekCharsLength)
      {
        int peekChar = (int) this.peekChars[this.peekCharsIndex];
        if (peekChar == 0)
          return -1;
        return peekChar < 55296 || peekChar >= 57343 ? peekChar : this.PeekSurrogate(peekChar);
      }
      return !this.ReadTextReader(-1) ? -1 : this.PeekChar();
    }

    private int ReadChar()
    {
      int num = this.PeekChar();
      ++this.peekCharsIndex;
      if (num >= 65536)
        ++this.peekCharsIndex;
      if (num == 10)
      {
        ++this.line;
        this.column = 1;
      }
      else if (num != -1)
        ++this.column;
      return num;
    }

    private void Advance(int ch)
    {
      ++this.peekCharsIndex;
      if (ch >= 65536)
        ++this.peekCharsIndex;
      if (ch == 10)
      {
        ++this.line;
        this.column = 1;
      }
      else
      {
        if (ch == -1)
          return;
        ++this.column;
      }
    }

    private bool ReadTextReader(int remained)
    {
      if (this.peekCharsLength < 0)
      {
        this.peekCharsLength = this.reader.Read(this.peekChars, 0, this.peekChars.Length);
        return this.peekCharsLength > 0;
      }
      int num1 = remained < 0 ? 0 : 1;
      int length = this.peekCharsLength - this.curNodePeekIndex;
      if (!this.preserveCurrentTag)
      {
        this.curNodePeekIndex = 0;
        this.peekCharsIndex = 0;
      }
      else if (this.peekCharsLength >= this.peekChars.Length)
      {
        if (this.curNodePeekIndex <= this.peekCharsLength >> 1)
        {
          char[] destinationArray = new char[this.peekChars.Length * 2];
          Array.Copy((Array) this.peekChars, this.curNodePeekIndex, (Array) destinationArray, 0, length);
          this.peekChars = destinationArray;
          this.curNodePeekIndex = 0;
          this.peekCharsIndex = length;
        }
        else
        {
          Array.Copy((Array) this.peekChars, this.curNodePeekIndex, (Array) this.peekChars, 0, length);
          this.curNodePeekIndex = 0;
          this.peekCharsIndex = length;
        }
      }
      if (remained >= 0)
        this.peekChars[this.peekCharsIndex] = (char) remained;
      int count = this.peekChars.Length - this.peekCharsIndex - num1;
      if (count > 1024)
        count = 1024;
      int num2 = this.reader.Read(this.peekChars, this.peekCharsIndex + num1, count);
      int num3 = num1 + num2;
      this.peekCharsLength = this.peekCharsIndex + num3;
      return num3 != 0;
    }

    private bool ReadContent()
    {
      if (this.popScope)
      {
        this.nsmgr.PopScope();
        this.parserContext.PopScope();
        this.popScope = false;
      }
      if (this.returnEntityReference)
      {
        this.SetEntityReferenceProperties();
      }
      else
      {
        int ch = this.PeekChar();
        if (ch == -1)
        {
          this.readState = ReadState.EndOfFile;
          this.ClearValueBuffer();
          this.SetProperties(XmlNodeType.None, string.Empty, string.Empty, string.Empty, false, (string) null, true);
          if (this.depth > 0)
            throw this.NotWFError("unexpected end of file. Current depth is " + (object) this.depth);
          return false;
        }
        int num = ch;
        switch (num)
        {
          case 9:
          case 10:
          case 13:
            if (!this.ReadWhitespace())
              return this.ReadContent();
            break;
          default:
            if (num != 32)
            {
              if (num == 60)
              {
                this.Advance(ch);
                switch (this.PeekChar())
                {
                  case 33:
                    this.Advance(33);
                    this.ReadDeclaration();
                    break;
                  case 47:
                    this.Advance(47);
                    this.ReadEndTag();
                    break;
                  case 63:
                    this.Advance(63);
                    this.ReadProcessingInstruction();
                    break;
                  default:
                    this.ReadStartTag();
                    break;
                }
              }
              else
              {
                this.ReadText(true);
                break;
              }
            }
            else
              goto case 9;
            break;
        }
      }
      return this.ReadState != ReadState.EndOfFile;
    }

    private void SetEntityReferenceProperties()
    {
      DTDEntityDeclaration entityDecl = this.DTD == null ? (DTDEntityDeclaration) null : this.DTD.EntityDecls[this.entityReferenceName];
      if (this.isStandalone && (this.DTD == null || entityDecl == null || !entityDecl.IsInternalSubset))
        throw this.NotWFError("Standalone document must not contain any references to an non-internally declared entity.");
      if (entityDecl != null && entityDecl.NotationName != null)
        throw this.NotWFError("Reference to any unparsed entities is not allowed here.");
      this.ClearValueBuffer();
      this.SetProperties(XmlNodeType.EntityReference, this.entityReferenceName, string.Empty, this.entityReferenceName, false, (string) null, true);
      this.returnEntityReference = false;
      this.entityReferenceName = string.Empty;
    }

    private void ReadStartTag()
    {
      this.currentState = this.currentState != XmlNodeType.EndElement ? XmlNodeType.Element : throw this.NotWFError("Multiple document element was detected.");
      this.nsmgr.PushScope();
      this.currentLinkedNodeLineNumber = this.line;
      this.currentLinkedNodeLinePosition = this.column;
      string prefix;
      string localName1;
      string name = this.ReadName(out prefix, out localName1);
      if (this.currentState == XmlNodeType.EndElement)
        throw this.NotWFError("document has terminated, cannot open new element");
      bool isEmptyElement = false;
      this.ClearAttributes();
      this.SkipWhitespace();
      if (XmlChar.IsFirstNameChar(this.PeekChar()))
        this.ReadAttributes(false);
      this.cursorToken = this.currentToken;
      for (int index = 0; index < this.attributeCount; ++index)
        this.attributeTokens[index].FillXmlns();
      for (int index = 0; index < this.attributeCount; ++index)
        this.attributeTokens[index].FillNamespace();
      if (this.namespaces)
      {
        for (int index = 0; index < this.attributeCount; ++index)
        {
          if (this.attributeTokens[index].Prefix == "xmlns" && this.attributeTokens[index].Value == string.Empty)
            throw this.NotWFError("Empty namespace URI cannot be mapped to non-empty prefix.");
        }
      }
      for (int index1 = 0; index1 < this.attributeCount; ++index1)
      {
        for (int index2 = index1 + 1; index2 < this.attributeCount; ++index2)
        {
          if (object.ReferenceEquals((object) this.attributeTokens[index1].Name, (object) this.attributeTokens[index2].Name) || object.ReferenceEquals((object) this.attributeTokens[index1].LocalName, (object) this.attributeTokens[index2].LocalName) && object.ReferenceEquals((object) this.attributeTokens[index1].NamespaceURI, (object) this.attributeTokens[index2].NamespaceURI))
            throw this.NotWFError("Attribute name and qualified name must be identical.");
        }
      }
      if (this.PeekChar() == 47)
      {
        this.Advance(47);
        isEmptyElement = true;
        this.popScope = true;
      }
      else
      {
        this.depthUp = true;
        this.PushElementName(name, localName1, prefix);
      }
      this.parserContext.PushScope();
      this.Expect(62);
      this.SetProperties(XmlNodeType.Element, name, prefix, localName1, isEmptyElement, (string) null, false);
      if (prefix.Length > 0)
        this.currentToken.NamespaceURI = this.LookupNamespace(prefix, true);
      else if (this.namespaces)
        this.currentToken.NamespaceURI = this.nsmgr.DefaultNamespace;
      if (this.namespaces)
      {
        if (this.NamespaceURI == null)
          throw this.NotWFError(string.Format("'{0}' is undeclared namespace.", (object) this.Prefix));
        try
        {
          for (int i = 0; i < this.attributeCount; ++i)
          {
            this.MoveToAttribute(i);
            if (this.NamespaceURI == null)
              throw this.NotWFError(string.Format("'{0}' is undeclared namespace.", (object) this.Prefix));
          }
        }
        finally
        {
          this.MoveToElement();
        }
      }
      for (int index = 0; index < this.attributeCount; ++index)
      {
        if (object.ReferenceEquals((object) this.attributeTokens[index].Prefix, (object) "xml"))
        {
          string localName2 = this.attributeTokens[index].LocalName;
          string relativeUri = this.attributeTokens[index].Value;
          string key1 = localName2;
          if (key1 != null)
          {
            // ISSUE: reference to a compiler-generated field
            if (XmlTextReader.\u003C\u003Ef__switch\u0024map52 == null)
            {
              // ISSUE: reference to a compiler-generated field
              XmlTextReader.\u003C\u003Ef__switch\u0024map52 = new Dictionary<string, int>(3)
              {
                {
                  "base",
                  0
                },
                {
                  "lang",
                  1
                },
                {
                  "space",
                  2
                }
              };
            }
            int num1;
            // ISSUE: reference to a compiler-generated field
            if (XmlTextReader.\u003C\u003Ef__switch\u0024map52.TryGetValue(key1, out num1))
            {
              switch (num1)
              {
                case 0:
                  if (this.resolver != null)
                  {
                    Uri uri = this.resolver.ResolveUri(!(this.BaseURI != string.Empty) ? (Uri) null : new Uri(this.BaseURI), relativeUri);
                    this.parserContext.BaseURI = !(uri != (Uri) null) ? string.Empty : uri.ToString();
                    continue;
                  }
                  this.parserContext.BaseURI = relativeUri;
                  continue;
                case 1:
                  this.parserContext.XmlLang = relativeUri;
                  continue;
                case 2:
                  string key2 = relativeUri;
                  if (key2 != null)
                  {
                    // ISSUE: reference to a compiler-generated field
                    if (XmlTextReader.\u003C\u003Ef__switch\u0024map51 == null)
                    {
                      // ISSUE: reference to a compiler-generated field
                      XmlTextReader.\u003C\u003Ef__switch\u0024map51 = new Dictionary<string, int>(2)
                      {
                        {
                          "preserve",
                          0
                        },
                        {
                          "default",
                          1
                        }
                      };
                    }
                    int num2;
                    // ISSUE: reference to a compiler-generated field
                    if (XmlTextReader.\u003C\u003Ef__switch\u0024map51.TryGetValue(key2, out num2))
                    {
                      if (num2 != 0)
                      {
                        if (num2 == 1)
                        {
                          this.parserContext.XmlSpace = XmlSpace.Default;
                          continue;
                        }
                      }
                      else
                      {
                        this.parserContext.XmlSpace = XmlSpace.Preserve;
                        continue;
                      }
                    }
                  }
                  throw this.NotWFError(string.Format("Invalid xml:space value: {0}", (object) relativeUri));
                default:
                  continue;
              }
            }
          }
        }
      }
      if (!this.IsEmptyElement)
        return;
      this.CheckCurrentStateUpdate();
    }

    private void PushElementName(string name, string local, string prefix)
    {
      if (this.elementNames.Length == this.elementNameStackPos)
      {
        XmlTextReader.TagName[] destinationArray = new XmlTextReader.TagName[this.elementNames.Length * 2];
        Array.Copy((Array) this.elementNames, 0, (Array) destinationArray, 0, this.elementNameStackPos);
        this.elementNames = destinationArray;
      }
      this.elementNames[this.elementNameStackPos++] = new XmlTextReader.TagName(name, local, prefix);
    }

    private void ReadEndTag()
    {
      if (this.currentState != XmlNodeType.Element)
        throw this.NotWFError("End tag cannot appear in this state.");
      this.currentLinkedNodeLineNumber = this.line;
      this.currentLinkedNodeLinePosition = this.column;
      XmlTextReader.TagName tagName = this.elementNameStackPos != 0 ? this.elementNames[--this.elementNameStackPos] : throw this.NotWFError("closing element without matching opening element");
      this.Expect(tagName.Name);
      this.ExpectAfterWhitespace('>');
      --this.depth;
      this.SetProperties(XmlNodeType.EndElement, tagName.Name, tagName.Prefix, tagName.LocalName, false, (string) null, true);
      if (tagName.Prefix.Length > 0)
        this.currentToken.NamespaceURI = this.LookupNamespace(tagName.Prefix, true);
      else if (this.namespaces)
        this.currentToken.NamespaceURI = this.nsmgr.DefaultNamespace;
      this.popScope = true;
      this.CheckCurrentStateUpdate();
    }

    private void CheckCurrentStateUpdate()
    {
      if (this.depth != 0 || this.allowMultipleRoot || !this.IsEmptyElement && this.NodeType != XmlNodeType.EndElement)
        return;
      this.currentState = XmlNodeType.EndElement;
    }

    private void AppendValueChar(int ch)
    {
      if (ch < (int) ushort.MaxValue)
        this.valueBuffer.Append((char) ch);
      else
        this.AppendSurrogatePairValueChar(ch);
    }

    private void AppendSurrogatePairValueChar(int ch)
    {
      this.valueBuffer.Append((char) ((ch - 65536) / 1024 + 55296));
      this.valueBuffer.Append((char) ((ch - 65536) % 1024 + 56320));
    }

    private string CreateValueString()
    {
      switch (this.NodeType)
      {
        case XmlNodeType.Whitespace:
        case XmlNodeType.SignificantWhitespace:
          int length = this.valueBuffer.Length;
          if (this.whitespaceCache == null)
            this.whitespaceCache = new char[32];
          if (length < this.whitespaceCache.Length)
          {
            if (this.whitespacePool == null)
              this.whitespacePool = new System.Xml.NameTable();
            for (int index = 0; index < length; ++index)
              this.whitespaceCache[index] = this.valueBuffer[index];
            return this.whitespacePool.Add(this.whitespaceCache, 0, this.valueBuffer.Length);
          }
          break;
      }
      return this.valueBuffer.Capacity < 100 ? this.valueBuffer.ToString(0, this.valueBuffer.Length) : this.valueBuffer.ToString();
    }

    private void ClearValueBuffer() => this.valueBuffer.Length = 0;

    private void ReadText(bool notWhitespace)
    {
      if (this.currentState != XmlNodeType.Element)
        throw this.NotWFError("Text node cannot appear in this state.");
      this.preserveCurrentTag = false;
      if (notWhitespace)
        this.ClearValueBuffer();
      int ch1 = this.PeekChar();
      bool flag = false;
      while (ch1 != 60 && ch1 != -1)
      {
        int ch2;
        if (ch1 == 38)
        {
          this.ReadChar();
          ch2 = this.ReadReference(false);
          if (this.returnEntityReference)
            break;
        }
        else
        {
          if (this.normalization && ch1 == 13)
          {
            this.ReadChar();
            ch1 = this.PeekChar();
            if (ch1 != 10)
            {
              this.AppendValueChar(10);
              continue;
            }
            continue;
          }
          if (this.CharacterChecking && XmlChar.IsInvalid(ch1))
            throw this.NotWFError("Not allowed character was found.");
          ch2 = this.ReadChar();
        }
        if (ch2 < (int) ushort.MaxValue)
          this.valueBuffer.Append((char) ch2);
        else
          this.AppendSurrogatePairValueChar(ch2);
        if (ch2 == 93)
          flag = !flag || this.PeekChar() != 62 ? true : throw this.NotWFError("Inside text content, character sequence ']]>' is not allowed.");
        else if (flag)
          flag = false;
        ch1 = this.PeekChar();
        notWhitespace = true;
      }
      if (this.returnEntityReference && this.valueBuffer.Length == 0)
        this.SetEntityReferenceProperties();
      else
        this.SetProperties(!notWhitespace ? (this.XmlSpace != XmlSpace.Preserve ? XmlNodeType.Whitespace : XmlNodeType.SignificantWhitespace) : XmlNodeType.Text, string.Empty, string.Empty, string.Empty, false, (string) null, true);
    }

    private int ReadReference(bool ignoreEntityReferences)
    {
      if (this.PeekChar() != 35)
        return this.ReadEntityReference(ignoreEntityReferences);
      this.Advance(35);
      return this.ReadCharacterReference();
    }

    private int ReadCharacterReference()
    {
      int ch1 = 0;
      if (this.PeekChar() == 120)
      {
        this.Advance(120);
        int ch2;
        while ((ch2 = this.PeekChar()) != 59 && ch2 != -1)
        {
          this.Advance(ch2);
          if (ch2 >= 48 && ch2 <= 57)
            ch1 = (ch1 << 4) + ch2 - 48;
          else if (ch2 >= 65 && ch2 <= 70)
            ch1 = (ch1 << 4) + ch2 - 65 + 10;
          else if (ch2 >= 97 && ch2 <= 102)
            ch1 = (ch1 << 4) + ch2 - 97 + 10;
          else
            throw this.NotWFError(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "invalid hexadecimal digit: {0} (#x{1:X})", (object) (char) ch2, (object) ch2));
        }
      }
      else
      {
        int ch3;
        while ((ch3 = this.PeekChar()) != 59 && ch3 != -1)
        {
          this.Advance(ch3);
          if (ch3 >= 48 && ch3 <= 57)
            ch1 = ch1 * 10 + ch3 - 48;
          else
            throw this.NotWFError(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "invalid decimal digit: {0} (#x{1:X})", (object) (char) ch3, (object) ch3));
        }
      }
      this.ReadChar();
      if (this.CharacterChecking && this.Normalization && XmlChar.IsInvalid(ch1))
        throw this.NotWFError("Referenced character was not allowed in XML. Normalization is " + (object) this.normalization + ", checkCharacters = " + (object) this.checkCharacters);
      return ch1;
    }

    private int ReadEntityReference(bool ignoreEntityReferences)
    {
      string name = this.ReadName();
      this.Expect(59);
      int predefinedEntity = XmlChar.GetPredefinedEntity(name);
      if (predefinedEntity >= 0)
        return predefinedEntity;
      if (ignoreEntityReferences)
      {
        this.AppendValueChar(38);
        for (int index = 0; index < name.Length; ++index)
          this.AppendValueChar((int) name[index]);
        this.AppendValueChar(59);
      }
      else
      {
        this.returnEntityReference = true;
        this.entityReferenceName = name;
      }
      return -1;
    }

    private void ReadAttributes(bool isXmlDecl)
    {
      bool flag = false;
      this.currentAttribute = -1;
      this.currentAttributeValue = -1;
      while (this.SkipWhitespace() || !flag)
      {
        this.IncrementAttributeToken();
        this.currentAttributeToken.LineNumber = this.line;
        this.currentAttributeToken.LinePosition = this.column;
        string prefix;
        string localName;
        this.currentAttributeToken.Name = this.ReadName(out prefix, out localName);
        this.currentAttributeToken.Prefix = prefix;
        this.currentAttributeToken.LocalName = localName;
        this.ExpectAfterWhitespace('=');
        this.SkipWhitespace();
        this.ReadAttributeValueTokens(-1);
        if (isXmlDecl)
        {
          string str = this.currentAttributeToken.Value;
        }
        ++this.attributeCount;
        if (!this.SkipWhitespace())
          flag = true;
        int num = this.PeekChar();
        if (isXmlDecl)
        {
          if (num == 63)
            goto label_11;
        }
        else if (num == 47 || num == 62)
          goto label_11;
        if (num != -1)
          continue;
label_11:
        this.currentAttribute = -1;
        this.currentAttributeValue = -1;
        return;
      }
      throw this.NotWFError("Unexpected token. Name is required here.");
    }

    private void AddAttributeWithValue(string name, string value)
    {
      this.IncrementAttributeToken();
      XmlTextReader.XmlAttributeTokenInfo attributeToken = this.attributeTokens[this.currentAttribute];
      attributeToken.Name = this.NameTable.Add(name);
      attributeToken.Prefix = string.Empty;
      attributeToken.NamespaceURI = string.Empty;
      this.IncrementAttributeValueToken();
      this.SetTokenProperties(this.attributeValueTokens[this.currentAttributeValue], XmlNodeType.Text, string.Empty, string.Empty, string.Empty, false, value, false);
      attributeToken.Value = value;
      ++this.attributeCount;
    }

    private void IncrementAttributeToken()
    {
      ++this.currentAttribute;
      if (this.attributeTokens.Length == this.currentAttribute)
      {
        XmlTextReader.XmlAttributeTokenInfo[] attributeTokenInfoArray = new XmlTextReader.XmlAttributeTokenInfo[this.attributeTokens.Length * 2];
        this.attributeTokens.CopyTo((Array) attributeTokenInfoArray, 0);
        this.attributeTokens = attributeTokenInfoArray;
      }
      if (this.attributeTokens[this.currentAttribute] == null)
        this.attributeTokens[this.currentAttribute] = new XmlTextReader.XmlAttributeTokenInfo(this);
      this.currentAttributeToken = this.attributeTokens[this.currentAttribute];
      this.currentAttributeToken.Clear();
    }

    private void IncrementAttributeValueToken()
    {
      ++this.currentAttributeValue;
      if (this.attributeValueTokens.Length == this.currentAttributeValue)
      {
        XmlTextReader.XmlTokenInfo[] xmlTokenInfoArray = new XmlTextReader.XmlTokenInfo[this.attributeValueTokens.Length * 2];
        this.attributeValueTokens.CopyTo((Array) xmlTokenInfoArray, 0);
        this.attributeValueTokens = xmlTokenInfoArray;
      }
      if (this.attributeValueTokens[this.currentAttributeValue] == null)
        this.attributeValueTokens[this.currentAttributeValue] = new XmlTextReader.XmlTokenInfo(this);
      this.currentAttributeValueToken = this.attributeValueTokens[this.currentAttributeValue];
      this.currentAttributeValueToken.Clear();
    }

    private void ReadAttributeValueTokens(int dummyQuoteChar)
    {
      int num1 = dummyQuoteChar >= 0 ? dummyQuoteChar : this.ReadChar();
      switch (num1)
      {
        case 34:
        case 39:
          this.currentAttributeToken.QuoteChar = (char) num1;
          this.IncrementAttributeValueToken();
          this.currentAttributeToken.ValueTokenStartIndex = this.currentAttributeValue;
          this.currentAttributeValueToken.LineNumber = this.line;
          this.currentAttributeValueToken.LinePosition = this.column;
          bool flag1 = false;
          bool flag2 = true;
          bool flag3 = true;
          this.currentAttributeValueToken.ValueBufferStart = this.valueBuffer.Length;
          while (flag3)
          {
            int ch = this.ReadChar();
            if (ch != num1)
            {
              if (flag1)
              {
                this.IncrementAttributeValueToken();
                this.currentAttributeValueToken.ValueBufferStart = this.valueBuffer.Length;
                this.currentAttributeValueToken.LineNumber = this.line;
                this.currentAttributeValueToken.LinePosition = this.column;
                flag1 = false;
                flag2 = true;
              }
              int num2 = ch;
              switch (num2)
              {
                case 9:
                case 10:
                  if (this.normalization)
                  {
                    ch = 32;
                    break;
                  }
                  break;
                case 13:
                  if (this.normalization)
                  {
                    if (this.PeekChar() != 10)
                    {
                      if (this.normalization)
                      {
                        ch = 32;
                        break;
                      }
                      break;
                    }
                    continue;
                  }
                  break;
                default:
                  if (num2 != -1)
                  {
                    if (num2 != 38)
                    {
                      if (num2 == 60)
                        throw this.NotWFError("attribute values cannot contain '<'");
                      break;
                    }
                    if (this.PeekChar() == 35)
                    {
                      this.Advance(35);
                      this.AppendValueChar(this.ReadCharacterReference());
                      goto label_39;
                    }
                    else
                    {
                      string str = this.ReadName();
                      this.Expect(59);
                      int predefinedEntity = XmlChar.GetPredefinedEntity(str);
                      if (predefinedEntity < 0)
                      {
                        this.CheckAttributeEntityReferenceWFC(str);
                        if (this.entityHandling == EntityHandling.ExpandEntities)
                        {
                          using (IEnumerator<char> enumerator = ((IEnumerable<char>) this.DTD.GenerateEntityAttributeText(str)).GetEnumerator())
                          {
                            while (enumerator.MoveNext())
                              this.AppendValueChar((int) enumerator.Current);
                            goto label_39;
                          }
                        }
                        else
                        {
                          this.currentAttributeValueToken.ValueBufferEnd = this.valueBuffer.Length;
                          this.currentAttributeValueToken.NodeType = XmlNodeType.Text;
                          if (!flag2)
                            this.IncrementAttributeValueToken();
                          this.currentAttributeValueToken.Name = str;
                          this.currentAttributeValueToken.Value = string.Empty;
                          this.currentAttributeValueToken.NodeType = XmlNodeType.EntityReference;
                          flag1 = true;
                          goto label_39;
                        }
                      }
                      else
                      {
                        this.AppendValueChar(predefinedEntity);
                        goto label_39;
                      }
                    }
                  }
                  else
                  {
                    if (dummyQuoteChar < 0)
                      throw this.NotWFError("unexpected end of file in an attribute value");
                    flag3 = false;
                    goto label_39;
                  }
              }
              if (this.CharacterChecking && XmlChar.IsInvalid(ch))
                throw this.NotWFError("Invalid character was found.");
              if (ch < (int) ushort.MaxValue)
                this.valueBuffer.Append((char) ch);
              else
                this.AppendSurrogatePairValueChar(ch);
label_39:
              flag2 = false;
            }
            else
              break;
          }
          if (!flag1)
          {
            this.currentAttributeValueToken.ValueBufferEnd = this.valueBuffer.Length;
            this.currentAttributeValueToken.NodeType = XmlNodeType.Text;
          }
          this.currentAttributeToken.ValueTokenEndIndex = this.currentAttributeValue;
          break;
        default:
          throw this.NotWFError("an attribute value was not quoted");
      }
    }

    private void CheckAttributeEntityReferenceWFC(string entName)
    {
      DTDEntityDeclaration entityDecl = this.DTD != null ? this.DTD.EntityDecls[entName] : (DTDEntityDeclaration) null;
      if (entityDecl == null)
      {
        if (this.entityHandling == EntityHandling.ExpandEntities || this.DTD != null && this.resolver != null && entityDecl == null)
          throw this.NotWFError(string.Format("Referenced entity '{0}' does not exist.", (object) entName));
      }
      else
      {
        if (entityDecl.HasExternalReference)
          throw this.NotWFError("Reference to external entities is not allowed in the value of an attribute.");
        if (this.isStandalone && !entityDecl.IsInternalSubset)
          throw this.NotWFError("Reference to external entities is not allowed in the internal subset.");
        if (entityDecl.EntityValue.IndexOf('<') >= 0)
          throw this.NotWFError("Attribute must not contain character '<' either directly or indirectly by way of entity references.");
      }
    }

    private void ReadProcessingInstruction()
    {
      string str = this.ReadName();
      if (str != "xml" && str.ToLower(CultureInfo.InvariantCulture) == "xml")
        throw this.NotWFError("Not allowed processing instruction name which starts with 'X', 'M', 'L' was found.");
      if (!this.SkipWhitespace() && this.PeekChar() != 63)
        throw this.NotWFError("Invalid processing instruction name was found.");
      this.ClearValueBuffer();
      int ch;
      while ((ch = this.PeekChar()) != -1)
      {
        this.Advance(ch);
        if (ch == 63 && this.PeekChar() == 62)
        {
          this.Advance(62);
          break;
        }
        if (this.CharacterChecking && XmlChar.IsInvalid(ch))
          throw this.NotWFError("Invalid character was found.");
        this.AppendValueChar(ch);
      }
      if (object.ReferenceEquals((object) str, (object) "xml"))
      {
        this.VerifyXmlDeclaration();
      }
      else
      {
        if (this.currentState == XmlNodeType.None)
          this.currentState = XmlNodeType.XmlDeclaration;
        this.SetProperties(XmlNodeType.ProcessingInstruction, str, string.Empty, str, false, (string) null, true);
      }
    }

    private void VerifyXmlDeclaration()
    {
      this.currentState = this.allowMultipleRoot || this.currentState == XmlNodeType.None ? XmlNodeType.XmlDeclaration : throw this.NotWFError("XML declaration cannot appear in this state.");
      string valueString = this.CreateValueString();
      this.ClearAttributes();
      int idx = 0;
      string str1 = (string) null;
      string str2 = (string) null;
      string name;
      string ianaEncoding;
      this.ParseAttributeFromString(valueString, ref idx, out name, out ianaEncoding);
      name = !(name != "version") && !(ianaEncoding != "1.0") ? string.Empty : throw this.NotWFError("'version' is expected.");
      if (this.SkipWhitespaceInString(valueString, ref idx) && idx < valueString.Length)
        this.ParseAttributeFromString(valueString, ref idx, out name, out ianaEncoding);
      if (name == "encoding")
      {
        if (!XmlChar.IsValidIANAEncoding(ianaEncoding))
          throw this.NotWFError("'encoding' must be a valid IANA encoding name.");
        this.parserContext.Encoding = !(this.reader is XmlStreamReader) ? Encoding.Unicode : ((NonBlockingStreamReader) this.reader).Encoding;
        str1 = ianaEncoding;
        name = string.Empty;
        if (this.SkipWhitespaceInString(valueString, ref idx) && idx < valueString.Length)
          this.ParseAttributeFromString(valueString, ref idx, out name, out ianaEncoding);
      }
      if (name == "standalone")
      {
        this.isStandalone = ianaEncoding == "yes";
        str2 = !(ianaEncoding != "yes") || !(ianaEncoding != "no") ? ianaEncoding : throw this.NotWFError("Only 'yes' or 'no' is allow for 'standalone'");
        this.SkipWhitespaceInString(valueString, ref idx);
      }
      else if (name.Length != 0)
        throw this.NotWFError(string.Format("Unexpected token: '{0}'", (object) name));
      if (idx < valueString.Length)
        throw this.NotWFError("'?' is expected.");
      this.AddAttributeWithValue("version", "1.0");
      if (str1 != null)
        this.AddAttributeWithValue("encoding", str1);
      if (str2 != null)
        this.AddAttributeWithValue("standalone", str2);
      this.currentAttribute = this.currentAttributeValue = -1;
      this.SetProperties(XmlNodeType.XmlDeclaration, "xml", string.Empty, "xml", false, valueString, false);
    }

    private bool SkipWhitespaceInString(string text, ref int idx)
    {
      int num = idx;
      while (idx < text.Length && XmlChar.IsWhitespace((int) text[idx]))
        ++idx;
      return idx - num > 0;
    }

    private void ParseAttributeFromString(
      string src,
      ref int idx,
      out string name,
      out string value)
    {
      while (idx < src.Length && XmlChar.IsWhitespace((int) src[idx]))
        ++idx;
      int startIndex1 = idx;
      while (idx < src.Length && XmlChar.IsNameChar((int) src[idx]))
        ++idx;
      name = src.Substring(startIndex1, idx - startIndex1);
      while (idx < src.Length && XmlChar.IsWhitespace((int) src[idx]))
        ++idx;
      if (idx == src.Length || src[idx] != '=')
        throw this.NotWFError(string.Format("'=' is expected after {0}", (object) name));
      ++idx;
      while (idx < src.Length && XmlChar.IsWhitespace((int) src[idx]))
        ++idx;
      if (idx == src.Length || src[idx] != '"' && src[idx] != '\'')
        throw this.NotWFError("'\"' or ''' is expected.");
      char ch = src[idx];
      ++idx;
      int startIndex2 = idx;
      while (idx < src.Length && (int) src[idx] != (int) ch)
        ++idx;
      ++idx;
      value = src.Substring(startIndex2, idx - startIndex2 - 1);
    }

    internal void SkipTextDeclaration()
    {
      if (this.PeekChar() != 60)
        return;
      this.ReadChar();
      if (this.PeekChar() != 63)
      {
        this.peekCharsIndex = 0;
      }
      else
      {
        this.ReadChar();
        while (this.peekCharsIndex < 6 && this.PeekChar() >= 0)
          this.ReadChar();
        if (new string(this.peekChars, 2, 4) != "xml ")
        {
          if (new string(this.peekChars, 2, 4).ToLower(CultureInfo.InvariantCulture) == "xml ")
            throw this.NotWFError("Processing instruction name must not be character sequence 'X' 'M' 'L' with case insensitivity.");
          this.peekCharsIndex = 0;
        }
        else
        {
          this.SkipWhitespace();
          if (this.PeekChar() == 118)
          {
            this.Expect("version");
            this.ExpectAfterWhitespace('=');
            this.SkipWhitespace();
            int num = this.ReadChar();
            char[] chArray = new char[3];
            int index = 0;
            switch (num)
            {
              case 34:
              case 39:
                while (this.PeekChar() != num)
                {
                  if (this.PeekChar() == -1)
                    throw this.NotWFError("Invalid version declaration inside text declaration.");
                  if (index == 3)
                    throw this.NotWFError("Invalid version number inside text declaration.");
                  chArray[index] = (char) this.ReadChar();
                  ++index;
                  if (index == 3 && new string(chArray) != "1.0")
                    throw this.NotWFError("Invalid version number inside text declaration.");
                }
                this.ReadChar();
                this.SkipWhitespace();
                break;
              default:
                throw this.NotWFError("Invalid version declaration inside text declaration.");
            }
          }
          if (this.PeekChar() == 101)
          {
            this.Expect("encoding");
            this.ExpectAfterWhitespace('=');
            this.SkipWhitespace();
            int num = this.ReadChar();
            switch (num)
            {
              case 34:
              case 39:
                while (this.PeekChar() != num)
                {
                  if (this.ReadChar() == -1)
                    throw this.NotWFError("Invalid encoding declaration inside text declaration.");
                }
                this.ReadChar();
                this.SkipWhitespace();
                break;
              default:
                throw this.NotWFError("Invalid encoding declaration inside text declaration.");
            }
          }
          else if (this.Conformance == ConformanceLevel.Auto)
            throw this.NotWFError("Encoding declaration is mandatory in text declaration.");
          this.Expect("?>");
          this.curNodePeekIndex = this.peekCharsIndex;
        }
      }
    }

    private void ReadDeclaration()
    {
      switch (this.PeekChar())
      {
        case 45:
          this.Expect("--");
          this.ReadComment();
          break;
        case 68:
          this.Expect("DOCTYPE");
          this.ReadDoctypeDecl();
          break;
        case 91:
          this.ReadChar();
          this.Expect("CDATA[");
          this.ReadCDATA();
          break;
        default:
          throw this.NotWFError("Unexpected declaration markup was found.");
      }
    }

    private void ReadComment()
    {
      if (this.currentState == XmlNodeType.None)
        this.currentState = XmlNodeType.XmlDeclaration;
      this.preserveCurrentTag = false;
      this.ClearValueBuffer();
      int ch;
      while ((ch = this.PeekChar()) != -1)
      {
        this.Advance(ch);
        if (ch == 45 && this.PeekChar() == 45)
        {
          this.Advance(45);
          if (this.PeekChar() != 62)
            throw this.NotWFError("comments cannot contain '--'");
          this.Advance(62);
          break;
        }
        if (XmlChar.IsInvalid(ch))
          throw this.NotWFError("Not allowed character was found.");
        this.AppendValueChar(ch);
      }
      this.SetProperties(XmlNodeType.Comment, string.Empty, string.Empty, string.Empty, false, (string) null, true);
    }

    private void ReadCDATA()
    {
      if (this.currentState != XmlNodeType.Element)
        throw this.NotWFError("CDATA section cannot appear in this state.");
      this.preserveCurrentTag = false;
      this.ClearValueBuffer();
      bool flag = false;
      int ch = 0;
      while (this.PeekChar() != -1)
      {
        if (!flag)
          ch = this.ReadChar();
        flag = false;
        if (ch == 93 && this.PeekChar() == 93)
        {
          ch = this.ReadChar();
          if (this.PeekChar() == 62)
          {
            this.ReadChar();
            break;
          }
          flag = true;
        }
        if (this.normalization && ch == 13)
        {
          ch = this.PeekChar();
          if (ch != 10)
            this.AppendValueChar(10);
        }
        else
        {
          if (this.CharacterChecking && XmlChar.IsInvalid(ch))
            throw this.NotWFError("Invalid character was found.");
          if (ch < (int) ushort.MaxValue)
            this.valueBuffer.Append((char) ch);
          else
            this.AppendSurrogatePairValueChar(ch);
        }
      }
      this.SetProperties(XmlNodeType.CDATA, string.Empty, string.Empty, string.Empty, false, (string) null, true);
    }

    private void ReadDoctypeDecl()
    {
      if (this.prohibitDtd)
        throw this.NotWFError("Document Type Declaration (DTD) is prohibited in this XML.");
      switch (this.currentState)
      {
        case XmlNodeType.Element:
        case XmlNodeType.DocumentType:
        case XmlNodeType.EndElement:
          throw this.NotWFError("Document type cannot appear in this state.");
        default:
          this.currentState = XmlNodeType.DocumentType;
          string publicId = (string) null;
          string systemId = (string) null;
          int intSubsetStartLine = 0;
          int intSubsetStartColumn = 0;
          this.SkipWhitespace();
          string str = this.ReadName();
          this.SkipWhitespace();
          switch (this.PeekChar())
          {
            case 80:
              publicId = this.ReadPubidLiteral();
              if (!this.SkipWhitespace())
                throw this.NotWFError("Whitespace is required between PUBLIC id and SYSTEM id.");
              systemId = this.ReadSystemLiteral(false);
              break;
            case 83:
              systemId = this.ReadSystemLiteral(true);
              break;
          }
          this.SkipWhitespace();
          if (this.PeekChar() == 91)
          {
            this.ReadChar();
            intSubsetStartLine = this.LineNumber;
            intSubsetStartColumn = this.LinePosition;
            this.ClearValueBuffer();
            this.ReadInternalSubset();
            this.parserContext.InternalSubset = this.CreateValueString();
          }
          this.ExpectAfterWhitespace('>');
          this.GenerateDTDObjectModel(str, publicId, systemId, this.parserContext.InternalSubset, intSubsetStartLine, intSubsetStartColumn);
          this.SetProperties(XmlNodeType.DocumentType, str, string.Empty, str, false, this.parserContext.InternalSubset, true);
          if (publicId != null)
            this.AddAttributeWithValue("PUBLIC", publicId);
          if (systemId != null)
            this.AddAttributeWithValue("SYSTEM", systemId);
          this.currentAttribute = this.currentAttributeValue = -1;
          break;
      }
    }

    internal DTDObjectModel GenerateDTDObjectModel(
      string name,
      string publicId,
      string systemId,
      string internalSubset)
    {
      return this.GenerateDTDObjectModel(name, publicId, systemId, internalSubset, 0, 0);
    }

    internal DTDObjectModel GenerateDTDObjectModel(
      string name,
      string publicId,
      string systemId,
      string internalSubset,
      int intSubsetStartLine,
      int intSubsetStartColumn)
    {
      this.parserContext.Dtd = new DTDObjectModel(this.NameTable);
      this.DTD.BaseURI = this.BaseURI;
      this.DTD.Name = name;
      this.DTD.PublicId = publicId;
      this.DTD.SystemId = systemId;
      this.DTD.InternalSubset = internalSubset;
      this.DTD.XmlResolver = this.resolver;
      this.DTD.IsStandalone = this.isStandalone;
      this.DTD.LineNumber = this.line;
      this.DTD.LinePosition = this.column;
      return new DTDReader(this.DTD, intSubsetStartLine, intSubsetStartColumn)
      {
        Normalization = this.normalization
      }.GenerateDTDObjectModel();
    }

    private XmlTextReader.DtdInputState State => this.stateStack.Peek();

    private int ReadValueChar()
    {
      int ch = this.ReadChar();
      this.AppendValueChar(ch);
      return ch;
    }

    private void ExpectAndAppend(string s)
    {
      this.Expect(s);
      this.valueBuffer.Append(s);
    }

    private void ReadInternalSubset()
    {
      bool flag = true;
      while (flag)
      {
        int num1 = this.ReadValueChar();
        switch (num1)
        {
          case 34:
            if (this.State == XmlTextReader.DtdInputState.InsideDoubleQuoted)
            {
              int num2 = (int) this.stateStack.Pop();
              continue;
            }
            if (this.State != XmlTextReader.DtdInputState.InsideSingleQuoted && this.State != XmlTextReader.DtdInputState.Comment)
            {
              this.stateStack.Push(XmlTextReader.DtdInputState.InsideDoubleQuoted);
              continue;
            }
            continue;
          case 37:
            if (this.State != XmlTextReader.DtdInputState.Free && this.State != XmlTextReader.DtdInputState.EntityDecl && this.State != XmlTextReader.DtdInputState.Comment && this.State != XmlTextReader.DtdInputState.InsideDoubleQuoted && this.State != XmlTextReader.DtdInputState.InsideSingleQuoted)
              throw this.NotWFError("Parameter Entity Reference cannot appear as a part of markupdecl (see XML spec 2.8).");
            continue;
          case 39:
            if (this.State == XmlTextReader.DtdInputState.InsideSingleQuoted)
            {
              int num3 = (int) this.stateStack.Pop();
              continue;
            }
            if (this.State != XmlTextReader.DtdInputState.InsideDoubleQuoted && this.State != XmlTextReader.DtdInputState.Comment)
            {
              this.stateStack.Push(XmlTextReader.DtdInputState.InsideSingleQuoted);
              continue;
            }
            continue;
          default:
            switch (num1 - 60)
            {
              case 0:
                switch (this.State)
                {
                  case XmlTextReader.DtdInputState.Comment:
                  case XmlTextReader.DtdInputState.InsideSingleQuoted:
                  case XmlTextReader.DtdInputState.InsideDoubleQuoted:
                    continue;
                  default:
                    int num4 = this.ReadValueChar();
                    switch (num4)
                    {
                      case 33:
                        switch (this.ReadValueChar())
                        {
                          case 45:
                            this.ExpectAndAppend("-");
                            this.stateStack.Push(XmlTextReader.DtdInputState.Comment);
                            continue;
                          case 65:
                            this.ExpectAndAppend("TTLIST");
                            this.stateStack.Push(XmlTextReader.DtdInputState.AttlistDecl);
                            continue;
                          case 69:
                            switch (this.ReadValueChar())
                            {
                              case 76:
                                this.ExpectAndAppend("EMENT");
                                this.stateStack.Push(XmlTextReader.DtdInputState.ElementDecl);
                                continue;
                              case 78:
                                this.ExpectAndAppend("TITY");
                                this.stateStack.Push(XmlTextReader.DtdInputState.EntityDecl);
                                continue;
                              default:
                                throw this.NotWFError("unexpected token '<!E'.");
                            }
                          case 78:
                            this.ExpectAndAppend("OTATION");
                            this.stateStack.Push(XmlTextReader.DtdInputState.NotationDecl);
                            continue;
                          default:
                            continue;
                        }
                      case 63:
                        this.stateStack.Push(XmlTextReader.DtdInputState.PI);
                        continue;
                      default:
                        throw this.NotWFError(string.Format("unexpected '<{0}'.", (object) (char) num4));
                    }
                }
              case 2:
                switch (this.State)
                {
                  case XmlTextReader.DtdInputState.ElementDecl:
                  case XmlTextReader.DtdInputState.AttlistDecl:
                  case XmlTextReader.DtdInputState.EntityDecl:
                  case XmlTextReader.DtdInputState.NotationDecl:
                    int num5 = (int) this.stateStack.Pop();
                    continue;
                  case XmlTextReader.DtdInputState.Comment:
                  case XmlTextReader.DtdInputState.InsideSingleQuoted:
                  case XmlTextReader.DtdInputState.InsideDoubleQuoted:
                    continue;
                  default:
                    throw this.NotWFError("unexpected token '>'");
                }
              case 3:
                if (this.State == XmlTextReader.DtdInputState.PI && this.ReadValueChar() == 62)
                {
                  int num6 = (int) this.stateStack.Pop();
                  continue;
                }
                continue;
              default:
                if (num1 == -1)
                  throw this.NotWFError("unexpected end of file at DTD.");
                if (num1 != 45)
                {
                  if (num1 == 93)
                  {
                    switch (this.State)
                    {
                      case XmlTextReader.DtdInputState.Free:
                        this.valueBuffer.Remove(this.valueBuffer.Length - 1, 1);
                        flag = false;
                        continue;
                      case XmlTextReader.DtdInputState.Comment:
                      case XmlTextReader.DtdInputState.InsideSingleQuoted:
                      case XmlTextReader.DtdInputState.InsideDoubleQuoted:
                        continue;
                      default:
                        throw this.NotWFError("unexpected end of file at DTD.");
                    }
                  }
                  else
                    continue;
                }
                else
                {
                  if (this.State == XmlTextReader.DtdInputState.Comment && this.PeekChar() == 45)
                  {
                    this.ReadValueChar();
                    this.ExpectAndAppend(">");
                    int num7 = (int) this.stateStack.Pop();
                    continue;
                  }
                  continue;
                }
            }
        }
      }
    }

    private string ReadSystemLiteral(bool expectSYSTEM)
    {
      if (expectSYSTEM)
      {
        this.Expect("SYSTEM");
        if (!this.SkipWhitespace())
          throw this.NotWFError("Whitespace is required after 'SYSTEM'.");
      }
      else
        this.SkipWhitespace();
      int num = this.ReadChar();
      int ch = 0;
      this.ClearValueBuffer();
      while (ch != num)
      {
        ch = this.ReadChar();
        if (ch < 0)
          throw this.NotWFError("Unexpected end of stream in ExternalID.");
        if (ch != num)
          this.AppendValueChar(ch);
      }
      return this.CreateValueString();
    }

    private string ReadPubidLiteral()
    {
      this.Expect("PUBLIC");
      if (!this.SkipWhitespace())
        throw this.NotWFError("Whitespace is required after 'PUBLIC'.");
      int num = this.ReadChar();
      int ch = 0;
      this.ClearValueBuffer();
      while (ch != num)
      {
        ch = this.ReadChar();
        if (ch < 0)
          throw this.NotWFError("Unexpected end of stream in ExternalID.");
        if (ch != num && !XmlChar.IsPubidChar(ch))
          throw this.NotWFError(string.Format("character '{0}' not allowed for PUBLIC ID", (object) (char) ch));
        if (ch != num)
          this.AppendValueChar(ch);
      }
      return this.CreateValueString();
    }

    private string ReadName() => this.ReadName(out string _, out string _);

    private string ReadName(out string prefix, out string localName)
    {
      bool preserveCurrentTag = this.preserveCurrentTag;
      this.preserveCurrentTag = true;
      int num = this.peekCharsIndex - this.curNodePeekIndex;
      int ch1 = this.PeekChar();
      if (!XmlChar.IsFirstNameChar(ch1))
        throw this.NotWFError(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "a name did not start with a legal character {0} ({1})", (object) ch1, (object) (char) ch1));
      this.Advance(ch1);
      int length1 = 1;
      int length2 = -1;
      int ch2;
      while (XmlChar.IsNameChar(ch2 = this.PeekChar()))
      {
        this.Advance(ch2);
        if (ch2 == 58 && this.namespaces && length2 < 0)
          length2 = length1;
        ++length1;
      }
      int offset = this.curNodePeekIndex + num;
      string str = this.NameTable.Add(this.peekChars, offset, length1);
      if (length2 > 0)
      {
        prefix = this.NameTable.Add(this.peekChars, offset, length2);
        localName = this.NameTable.Add(this.peekChars, offset + length2 + 1, length1 - length2 - 1);
      }
      else
      {
        prefix = string.Empty;
        localName = str;
      }
      this.preserveCurrentTag = preserveCurrentTag;
      return str;
    }

    private void Expect(int expected)
    {
      int num = this.ReadChar();
      if (num != expected)
        throw this.NotWFError(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "expected '{0}' ({1:X}) but found '{2}' ({3:X})", (object) (char) expected, (object) expected, num >= 0 ? (object) (char) num : (object) "EOF", (object) num));
    }

    private void Expect(string expected)
    {
      for (int index = 0; index < expected.Length; ++index)
      {
        if (this.ReadChar() != (int) expected[index])
          throw this.NotWFError(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "'{0}' is expected", (object) expected));
      }
    }

    private void ExpectAfterWhitespace(char c)
    {
      int ch;
      do
      {
        ch = this.ReadChar();
      }
      while (ch < 33 && XmlChar.IsWhitespace(ch));
      if ((int) c != ch)
        throw this.NotWFError(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Expected {0}, but found {1} [{2}]", (object) c, ch >= 0 ? (object) (char) ch : (object) "EOF", (object) ch));
    }

    private bool SkipWhitespace()
    {
      int ch1 = this.PeekChar();
      int num;
      switch (ch1)
      {
        case 9:
        case 10:
        case 32:
          num = 1;
          break;
        default:
          num = ch1 == 13 ? 1 : 0;
          break;
      }
      bool flag = num != 0;
      if (!flag)
        return false;
      this.Advance(ch1);
      int ch2;
      while ((ch2 = this.PeekChar()) == 32 || ch2 == 9 || ch2 == 10 || ch2 == 13)
        this.Advance(ch2);
      return flag;
    }

    private bool ReadWhitespace()
    {
      if (this.currentState == XmlNodeType.None)
        this.currentState = XmlNodeType.XmlDeclaration;
      bool preserveCurrentTag = this.preserveCurrentTag;
      this.preserveCurrentTag = true;
      int num = this.peekCharsIndex - this.curNodePeekIndex;
      int ch = this.PeekChar();
      do
      {
        this.Advance(ch);
        ch = this.PeekChar();
      }
      while (ch == 32 || ch == 9 || ch == 10 || ch == 13);
      bool flag = this.currentState == XmlNodeType.Element && ch != -1 && ch != 60;
      if (!flag && (this.whitespaceHandling == WhitespaceHandling.None || this.whitespaceHandling == WhitespaceHandling.Significant && this.XmlSpace != XmlSpace.Preserve))
        return false;
      this.ClearValueBuffer();
      this.valueBuffer.Append(this.peekChars, this.curNodePeekIndex, this.peekCharsIndex - this.curNodePeekIndex - num);
      this.preserveCurrentTag = preserveCurrentTag;
      if (flag)
        this.ReadText(false);
      else
        this.SetProperties(this.XmlSpace != XmlSpace.Preserve ? XmlNodeType.Whitespace : XmlNodeType.SignificantWhitespace, string.Empty, string.Empty, string.Empty, false, (string) null, true);
      return true;
    }

    private int ReadCharsInternal(char[] buffer, int offset, int length)
    {
      int num1 = offset;
      for (int index1 = 0; index1 < length; ++index1)
      {
        int ch = this.PeekChar();
        switch (ch)
        {
          case -1:
            throw this.NotWFError("Unexpected end of xml.");
          case 60:
            if (index1 + 1 == length)
              return index1;
            this.Advance(ch);
            if (this.PeekChar() != 47)
            {
              ++this.nestLevel;
              buffer[num1++] = '<';
              break;
            }
            if (this.nestLevel-- > 0)
            {
              buffer[num1++] = '<';
              break;
            }
            this.Expect(47);
            if (this.depthUp)
            {
              ++this.depth;
              this.depthUp = false;
            }
            this.ReadEndTag();
            this.readCharsInProgress = false;
            this.Read();
            return index1;
          default:
            this.Advance(ch);
            if (ch < (int) ushort.MaxValue)
            {
              buffer[num1++] = (char) ch;
              break;
            }
            char[] chArray1 = buffer;
            int index2 = num1;
            int num2 = index2 + 1;
            int num3 = (int) (ushort) ((ch - 65536) / 1024 + 55296);
            chArray1[index2] = (char) num3;
            char[] chArray2 = buffer;
            int index3 = num2;
            num1 = index3 + 1;
            int num4 = (int) (ushort) ((ch - 65536) % 1024 + 56320);
            chArray2[index3] = (char) num4;
            break;
        }
      }
      return length;
    }

    private bool ReadUntilEndTag()
    {
      if (this.Depth == 0)
        this.currentState = XmlNodeType.EndElement;
      do
      {
        do
        {
          int num;
          do
          {
            num = this.ReadChar();
            if (num == -1)
              goto label_4;
          }
          while (num != 60);
          goto label_5;
label_4:
          throw this.NotWFError("Unexpected end of xml.");
label_5:
          if (this.PeekChar() != 47)
            ++this.nestLevel;
        }
        while (--this.nestLevel > 0);
        this.ReadChar();
      }
      while (this.ReadName() != this.elementNames[this.elementNameStackPos - 1].Name);
      this.Expect(62);
      --this.depth;
      return this.Read();
    }

    internal class XmlTokenInfo
    {
      private string valueCache;
      protected XmlTextReader Reader;
      public string Name;
      public string LocalName;
      public string Prefix;
      public string NamespaceURI;
      public bool IsEmptyElement;
      public char QuoteChar;
      public int LineNumber;
      public int LinePosition;
      public int ValueBufferStart;
      public int ValueBufferEnd;
      public XmlNodeType NodeType;

      public XmlTokenInfo(XmlTextReader xtr)
      {
        this.Reader = xtr;
        this.Clear();
      }

      public virtual string Value
      {
        get
        {
          if (this.valueCache != null)
            return this.valueCache;
          if (this.ValueBufferStart >= 0)
          {
            this.valueCache = this.Reader.valueBuffer.ToString(this.ValueBufferStart, this.ValueBufferEnd - this.ValueBufferStart);
            return this.valueCache;
          }
          switch (this.NodeType)
          {
            case XmlNodeType.Text:
            case XmlNodeType.CDATA:
            case XmlNodeType.ProcessingInstruction:
            case XmlNodeType.Comment:
            case XmlNodeType.Whitespace:
            case XmlNodeType.SignificantWhitespace:
              this.valueCache = this.Reader.CreateValueString();
              return this.valueCache;
            default:
              return (string) null;
          }
        }
        set => this.valueCache = value;
      }

      public virtual void Clear()
      {
        this.ValueBufferStart = -1;
        this.valueCache = (string) null;
        this.NodeType = XmlNodeType.None;
        this.Name = this.LocalName = this.Prefix = this.NamespaceURI = string.Empty;
        this.IsEmptyElement = false;
        this.QuoteChar = '"';
        this.LineNumber = this.LinePosition = 0;
      }
    }

    internal class XmlAttributeTokenInfo : XmlTextReader.XmlTokenInfo
    {
      public int ValueTokenStartIndex;
      public int ValueTokenEndIndex;
      private string valueCache;
      private StringBuilder tmpBuilder = new StringBuilder();

      public XmlAttributeTokenInfo(XmlTextReader reader)
        : base(reader)
      {
        this.NodeType = XmlNodeType.Attribute;
      }

      public override string Value
      {
        get
        {
          if (this.valueCache != null)
            return this.valueCache;
          if (this.ValueTokenStartIndex == this.ValueTokenEndIndex)
          {
            XmlTextReader.XmlTokenInfo attributeValueToken = this.Reader.attributeValueTokens[this.ValueTokenStartIndex];
            this.valueCache = attributeValueToken.NodeType != XmlNodeType.EntityReference ? attributeValueToken.Value : "&" + attributeValueToken.Name + ";";
            return this.valueCache;
          }
          this.tmpBuilder.Length = 0;
          for (int valueTokenStartIndex = this.ValueTokenStartIndex; valueTokenStartIndex <= this.ValueTokenEndIndex; ++valueTokenStartIndex)
          {
            XmlTextReader.XmlTokenInfo attributeValueToken = this.Reader.attributeValueTokens[valueTokenStartIndex];
            if (attributeValueToken.NodeType == XmlNodeType.Text)
            {
              this.tmpBuilder.Append(attributeValueToken.Value);
            }
            else
            {
              this.tmpBuilder.Append('&');
              this.tmpBuilder.Append(attributeValueToken.Name);
              this.tmpBuilder.Append(';');
            }
          }
          this.valueCache = this.tmpBuilder.ToString(0, this.tmpBuilder.Length);
          return this.valueCache;
        }
        set => this.valueCache = value;
      }

      public override void Clear()
      {
        base.Clear();
        this.valueCache = (string) null;
        this.NodeType = XmlNodeType.Attribute;
        this.ValueTokenStartIndex = this.ValueTokenEndIndex = 0;
      }

      internal void FillXmlns()
      {
        if (object.ReferenceEquals((object) this.Prefix, (object) "xmlns"))
        {
          this.Reader.nsmgr.AddNamespace(this.LocalName, this.Value);
        }
        else
        {
          if (!object.ReferenceEquals((object) this.Name, (object) "xmlns"))
            return;
          this.Reader.nsmgr.AddNamespace(string.Empty, this.Value);
        }
      }

      internal void FillNamespace()
      {
        if (object.ReferenceEquals((object) this.Prefix, (object) "xmlns") || object.ReferenceEquals((object) this.Name, (object) "xmlns"))
          this.NamespaceURI = "http://www.w3.org/2000/xmlns/";
        else if (this.Prefix.Length == 0)
          this.NamespaceURI = string.Empty;
        else
          this.NamespaceURI = this.Reader.LookupNamespace(this.Prefix, true);
      }
    }

    private struct TagName
    {
      public readonly string Name;
      public readonly string LocalName;
      public readonly string Prefix;

      public TagName(string n, string l, string p)
      {
        this.Name = n;
        this.LocalName = l;
        this.Prefix = p;
      }
    }

    private enum DtdInputState
    {
      Free = 1,
      ElementDecl = 2,
      AttlistDecl = 3,
      EntityDecl = 4,
      NotationDecl = 5,
      PI = 6,
      Comment = 7,
      InsideSingleQuoted = 8,
      InsideDoubleQuoted = 9,
    }

    private class DtdInputStateStack
    {
      private Stack intern = new Stack();

      public DtdInputStateStack() => this.Push(XmlTextReader.DtdInputState.Free);

      public XmlTextReader.DtdInputState Peek() => (XmlTextReader.DtdInputState) this.intern.Peek();

      public XmlTextReader.DtdInputState Pop() => (XmlTextReader.DtdInputState) this.intern.Pop();

      public void Push(XmlTextReader.DtdInputState val) => this.intern.Push((object) val);
    }
  }
}
