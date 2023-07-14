// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlReader
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml;
using Mono.Xml.Schema;
using System.IO;
using System.Text;
using System.Xml.Schema;

namespace System.Xml
{
  public abstract class XmlReader : IDisposable
  {
    private StringBuilder readStringBuffer;
    private XmlReaderBinarySupport binary;
    private XmlReaderSettings settings;

    void IDisposable.Dispose() => this.Dispose(false);

    public abstract int AttributeCount { get; }

    public abstract string BaseURI { get; }

    internal XmlReaderBinarySupport Binary => this.binary;

    internal XmlReaderBinarySupport.CharGetter BinaryCharGetter
    {
      get => this.binary != null ? this.binary.Getter : (XmlReaderBinarySupport.CharGetter) null;
      set
      {
        if (this.binary == null)
          this.binary = new XmlReaderBinarySupport(this);
        this.binary.Getter = value;
      }
    }

    public virtual bool CanReadBinaryContent => false;

    public virtual bool CanReadValueChunk => false;

    public virtual bool CanResolveEntity => false;

    public abstract int Depth { get; }

    public abstract bool EOF { get; }

    public virtual bool HasAttributes => this.AttributeCount > 0;

    public abstract bool HasValue { get; }

    public abstract bool IsEmptyElement { get; }

    public virtual bool IsDefault => false;

    public virtual string this[int i] => this.GetAttribute(i);

    public virtual string this[string name] => this.GetAttribute(name);

    public virtual string this[string name, string namespaceURI] => this.GetAttribute(name, namespaceURI);

    public abstract string LocalName { get; }

    public virtual string Name => this.Prefix.Length > 0 ? this.Prefix + ":" + this.LocalName : this.LocalName;

    public abstract string NamespaceURI { get; }

    public abstract XmlNameTable NameTable { get; }

    public abstract XmlNodeType NodeType { get; }

    public abstract string Prefix { get; }

    public virtual char QuoteChar => '"';

    public abstract ReadState ReadState { get; }

    public virtual IXmlSchemaInfo SchemaInfo => (IXmlSchemaInfo) null;

    public virtual XmlReaderSettings Settings => this.settings;

    public abstract string Value { get; }

    public virtual string XmlLang => string.Empty;

    public virtual XmlSpace XmlSpace => XmlSpace.None;

    public abstract void Close();

    private static XmlNameTable PopulateNameTable(XmlReaderSettings settings) => settings.NameTable ?? (XmlNameTable) new System.Xml.NameTable();

    private static XmlParserContext PopulateParserContext(
      XmlReaderSettings settings,
      string baseUri)
    {
      XmlNameTable xmlNameTable = XmlReader.PopulateNameTable(settings);
      return new XmlParserContext(xmlNameTable, new XmlNamespaceManager(xmlNameTable), (string) null, (string) null, (string) null, (string) null, baseUri, (string) null, XmlSpace.None, (Encoding) null);
    }

    private static XmlNodeType GetNodeType(XmlReaderSettings settings) => (settings == null ? 0 : (int) settings.ConformanceLevel) == 1 ? XmlNodeType.Element : XmlNodeType.Document;

    public static XmlReader Create(Stream stream) => XmlReader.Create(stream, (XmlReaderSettings) null);

    public static XmlReader Create(string url) => XmlReader.Create(url, (XmlReaderSettings) null);

    public static XmlReader Create(TextReader reader) => XmlReader.Create(reader, (XmlReaderSettings) null);

    public static XmlReader Create(string url, XmlReaderSettings settings) => XmlReader.Create(url, settings, (XmlParserContext) null);

    public static XmlReader Create(Stream stream, XmlReaderSettings settings) => XmlReader.Create(stream, settings, string.Empty);

    public static XmlReader Create(TextReader reader, XmlReaderSettings settings) => XmlReader.Create(reader, settings, string.Empty);

    private static XmlReaderSettings PopulateSettings(XmlReaderSettings src) => src == null ? new XmlReaderSettings() : src.Clone();

    public static XmlReader Create(Stream stream, XmlReaderSettings settings, string baseUri)
    {
      settings = XmlReader.PopulateSettings(settings);
      return XmlReader.Create(stream, settings, XmlReader.PopulateParserContext(settings, baseUri));
    }

    public static XmlReader Create(TextReader reader, XmlReaderSettings settings, string baseUri)
    {
      settings = XmlReader.PopulateSettings(settings);
      return XmlReader.Create(reader, settings, XmlReader.PopulateParserContext(settings, baseUri));
    }

    public static XmlReader Create(XmlReader reader, XmlReaderSettings settings)
    {
      settings = XmlReader.PopulateSettings(settings);
      XmlReader filteredXmlReader = XmlReader.CreateFilteredXmlReader(reader, settings);
      filteredXmlReader.settings = settings;
      return filteredXmlReader;
    }

    public static XmlReader Create(
      string url,
      XmlReaderSettings settings,
      XmlParserContext context)
    {
      settings = XmlReader.PopulateSettings(settings);
      bool closeInput = settings.CloseInput;
      try
      {
        settings.CloseInput = true;
        if (context == null)
          context = XmlReader.PopulateParserContext(settings, url);
        return XmlReader.CreateCustomizedTextReader(new XmlTextReader(false, settings.XmlResolver, url, XmlReader.GetNodeType(settings), context), settings);
      }
      finally
      {
        settings.CloseInput = closeInput;
      }
    }

    public static XmlReader Create(
      Stream stream,
      XmlReaderSettings settings,
      XmlParserContext context)
    {
      settings = XmlReader.PopulateSettings(settings);
      if (context == null)
        context = XmlReader.PopulateParserContext(settings, string.Empty);
      return XmlReader.CreateCustomizedTextReader(new XmlTextReader(stream, XmlReader.GetNodeType(settings), context), settings);
    }

    public static XmlReader Create(
      TextReader reader,
      XmlReaderSettings settings,
      XmlParserContext context)
    {
      settings = XmlReader.PopulateSettings(settings);
      if (context == null)
        context = XmlReader.PopulateParserContext(settings, string.Empty);
      return XmlReader.CreateCustomizedTextReader(new XmlTextReader(context.BaseURI, reader, XmlReader.GetNodeType(settings), context), settings);
    }

    private static XmlReader CreateCustomizedTextReader(
      XmlTextReader reader,
      XmlReaderSettings settings)
    {
      reader.XmlResolver = settings.XmlResolver;
      reader.Normalization = true;
      reader.EntityHandling = EntityHandling.ExpandEntities;
      if (settings.ProhibitDtd)
        reader.ProhibitDtd = true;
      if (!settings.CheckCharacters)
        reader.CharacterChecking = false;
      reader.CloseInput = settings.CloseInput;
      reader.Conformance = settings.ConformanceLevel;
      reader.AdjustLineInfoOffset(settings.LineNumberOffset, settings.LinePositionOffset);
      if (settings.NameTable != null)
        reader.SetNameTable(settings.NameTable);
      XmlReader filteredXmlReader = XmlReader.CreateFilteredXmlReader((XmlReader) reader, settings);
      filteredXmlReader.settings = settings;
      return filteredXmlReader;
    }

    private static XmlReader CreateFilteredXmlReader(XmlReader reader, XmlReaderSettings settings)
    {
      ConformanceLevel conformanceLevel = !(reader is XmlTextReader) ? (reader.Settings == null ? settings.ConformanceLevel : reader.Settings.ConformanceLevel) : ((XmlTextReader) reader).Conformance;
      if (settings.ConformanceLevel != ConformanceLevel.Auto && conformanceLevel != settings.ConformanceLevel)
        throw new InvalidOperationException(string.Format("ConformanceLevel cannot be overwritten by a wrapping XmlReader. The source reader has {0}, while {1} is specified.", (object) conformanceLevel, (object) settings.ConformanceLevel));
      settings.ConformanceLevel = conformanceLevel;
      reader = XmlReader.CreateValidatingXmlReader(reader, settings);
      if (settings.IgnoreComments || settings.IgnoreProcessingInstructions || settings.IgnoreWhitespace)
        return (XmlReader) new XmlFilterReader(reader, settings);
      reader.settings = settings;
      return reader;
    }

    private static XmlReader CreateValidatingXmlReader(XmlReader reader, XmlReaderSettings settings)
    {
      switch (settings.ValidationType)
      {
        case ValidationType.DTD:
          XmlValidatingReader validatingReader = new XmlValidatingReader(reader);
          validatingReader.XmlResolver = settings.XmlResolver;
          validatingReader.ValidationType = ValidationType.DTD;
          if ((settings.ValidationFlags & XmlSchemaValidationFlags.ProcessIdentityConstraints) == XmlSchemaValidationFlags.None)
            throw new NotImplementedException();
          return (XmlReader) validatingReader ?? reader;
        case ValidationType.Schema:
          return (XmlReader) new XmlSchemaValidatingReader(reader, settings);
        default:
          return reader;
      }
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.ReadState == ReadState.Closed)
        return;
      this.Close();
    }

    public abstract string GetAttribute(int i);

    public abstract string GetAttribute(string name);

    public abstract string GetAttribute(string localName, string namespaceName);

    public static bool IsName(string s) => s != null && XmlChar.IsName(s);

    public static bool IsNameToken(string s) => s != null && XmlChar.IsNmToken(s);

    public virtual bool IsStartElement() => this.MoveToContent() == XmlNodeType.Element;

    public virtual bool IsStartElement(string name) => this.IsStartElement() && this.Name == name;

    public virtual bool IsStartElement(string localName, string namespaceName) => this.IsStartElement() && this.LocalName == localName && this.NamespaceURI == namespaceName;

    public abstract string LookupNamespace(string prefix);

    public virtual void MoveToAttribute(int i)
    {
      if (i >= this.AttributeCount)
        throw new ArgumentOutOfRangeException();
      this.MoveToFirstAttribute();
      for (int index = 0; index < i; ++index)
        this.MoveToNextAttribute();
    }

    public abstract bool MoveToAttribute(string name);

    public abstract bool MoveToAttribute(string localName, string namespaceName);

    private bool IsContent(XmlNodeType nodeType)
    {
      XmlNodeType xmlNodeType = nodeType;
      switch (xmlNodeType)
      {
        case XmlNodeType.Element:
          return true;
        case XmlNodeType.Text:
          return true;
        case XmlNodeType.CDATA:
          return true;
        case XmlNodeType.EntityReference:
          return true;
        default:
          return xmlNodeType == XmlNodeType.EndElement || xmlNodeType == XmlNodeType.EndEntity;
      }
    }

    public virtual XmlNodeType MoveToContent()
    {
      switch (this.ReadState)
      {
        case ReadState.Initial:
        case ReadState.Interactive:
          if (this.NodeType == XmlNodeType.Attribute)
            this.MoveToElement();
          while (!this.IsContent(this.NodeType))
          {
            this.Read();
            if (this.EOF)
              return XmlNodeType.None;
          }
          return this.NodeType;
        default:
          return this.NodeType;
      }
    }

    public abstract bool MoveToElement();

    public abstract bool MoveToFirstAttribute();

    public abstract bool MoveToNextAttribute();

    public abstract bool Read();

    public abstract bool ReadAttributeValue();

    public virtual string ReadElementString()
    {
      if (this.MoveToContent() != XmlNodeType.Element)
        throw this.XmlError(string.Format("'{0}' is an invalid node type.", (object) this.NodeType.ToString()));
      string str = string.Empty;
      if (!this.IsEmptyElement)
      {
        this.Read();
        str = this.ReadString();
        if (this.NodeType != XmlNodeType.EndElement)
          throw this.XmlError(string.Format("'{0}' is an invalid node type.", (object) this.NodeType.ToString()));
      }
      this.Read();
      return str;
    }

    public virtual string ReadElementString(string name)
    {
      if (this.MoveToContent() != XmlNodeType.Element)
        throw this.XmlError(string.Format("'{0}' is an invalid node type.", (object) this.NodeType.ToString()));
      if (name != this.Name)
        throw this.XmlError(string.Format("The {0} tag from namespace {1} is expected.", (object) this.Name, (object) this.NamespaceURI));
      string str = string.Empty;
      if (!this.IsEmptyElement)
      {
        this.Read();
        str = this.ReadString();
        if (this.NodeType != XmlNodeType.EndElement)
          throw this.XmlError(string.Format("'{0}' is an invalid node type.", (object) this.NodeType.ToString()));
      }
      this.Read();
      return str;
    }

    public virtual string ReadElementString(string localName, string namespaceName)
    {
      if (this.MoveToContent() != XmlNodeType.Element)
        throw this.XmlError(string.Format("'{0}' is an invalid node type.", (object) this.NodeType.ToString()));
      if (localName != this.LocalName || this.NamespaceURI != namespaceName)
        throw this.XmlError(string.Format("The {0} tag from namespace {1} is expected.", (object) this.LocalName, (object) this.NamespaceURI));
      string str = string.Empty;
      if (!this.IsEmptyElement)
      {
        this.Read();
        str = this.ReadString();
        if (this.NodeType != XmlNodeType.EndElement)
          throw this.XmlError(string.Format("'{0}' is an invalid node type.", (object) this.NodeType.ToString()));
      }
      this.Read();
      return str;
    }

    public virtual void ReadEndElement()
    {
      if (this.MoveToContent() != XmlNodeType.EndElement)
        throw this.XmlError(string.Format("'{0}' is an invalid node type.", (object) this.NodeType.ToString()));
      this.Read();
    }

    public virtual string ReadInnerXml()
    {
      if (this.ReadState != ReadState.Interactive || this.NodeType == XmlNodeType.EndElement)
        return string.Empty;
      if (this.IsEmptyElement)
      {
        this.Read();
        return string.Empty;
      }
      StringWriter writer = new StringWriter();
      XmlTextWriter xmlTextWriter = new XmlTextWriter((TextWriter) writer);
      if (this.NodeType == XmlNodeType.Element)
      {
        int depth = this.Depth;
        this.Read();
        while (depth < this.Depth)
        {
          if (this.ReadState != ReadState.Interactive)
            throw this.XmlError("Unexpected end of the XML reader.");
          xmlTextWriter.WriteNode(this, false);
        }
        this.Read();
      }
      else
        xmlTextWriter.WriteNode(this, false);
      return writer.ToString();
    }

    public virtual string ReadOuterXml()
    {
      if (this.ReadState != ReadState.Interactive || this.NodeType == XmlNodeType.EndElement)
        return string.Empty;
      switch (this.NodeType)
      {
        case XmlNodeType.Element:
        case XmlNodeType.Attribute:
          StringWriter writer = new StringWriter();
          new XmlTextWriter((TextWriter) writer).WriteNode(this, false);
          return writer.ToString();
        default:
          this.Skip();
          return string.Empty;
      }
    }

    public virtual void ReadStartElement()
    {
      if (this.MoveToContent() != XmlNodeType.Element)
        throw this.XmlError(string.Format("'{0}' is an invalid node type.", (object) this.NodeType.ToString()));
      this.Read();
    }

    public virtual void ReadStartElement(string name)
    {
      if (this.MoveToContent() != XmlNodeType.Element)
        throw this.XmlError(string.Format("'{0}' is an invalid node type.", (object) this.NodeType.ToString()));
      if (name != this.Name)
        throw this.XmlError(string.Format("The {0} tag from namespace {1} is expected.", (object) this.Name, (object) this.NamespaceURI));
      this.Read();
    }

    public virtual void ReadStartElement(string localName, string namespaceName)
    {
      if (this.MoveToContent() != XmlNodeType.Element)
        throw this.XmlError(string.Format("'{0}' is an invalid node type.", (object) this.NodeType.ToString()));
      if (localName != this.LocalName || this.NamespaceURI != namespaceName)
        throw this.XmlError(string.Format("Expecting {0} tag from namespace {1}, got {2} and {3} instead", (object) localName, (object) namespaceName, (object) this.LocalName, (object) this.NamespaceURI));
      this.Read();
    }

    public virtual string ReadString()
    {
      if (this.readStringBuffer == null)
        this.readStringBuffer = new StringBuilder();
      this.readStringBuffer.Length = 0;
      this.MoveToElement();
      XmlNodeType nodeType = this.NodeType;
      switch (nodeType)
      {
        case XmlNodeType.Element:
          if (this.IsEmptyElement)
            return string.Empty;
          while (true)
          {
            this.Read();
            switch (this.NodeType)
            {
              case XmlNodeType.Text:
              case XmlNodeType.CDATA:
              case XmlNodeType.Whitespace:
              case XmlNodeType.SignificantWhitespace:
                this.readStringBuffer.Append(this.Value);
                continue;
              default:
                goto label_11;
            }
          }
        case XmlNodeType.Text:
        case XmlNodeType.CDATA:
          while (true)
          {
            switch (this.NodeType)
            {
              case XmlNodeType.Text:
              case XmlNodeType.CDATA:
              case XmlNodeType.Whitespace:
              case XmlNodeType.SignificantWhitespace:
                this.readStringBuffer.Append(this.Value);
                this.Read();
                continue;
              default:
                goto label_11;
            }
          }
        default:
          if (nodeType != XmlNodeType.Whitespace && nodeType != XmlNodeType.SignificantWhitespace)
            return string.Empty;
          goto case XmlNodeType.Text;
      }
label_11:
      string str = this.readStringBuffer.ToString();
      this.readStringBuffer.Length = 0;
      return str;
    }

    public virtual Type ValueType => typeof (string);

    public virtual bool ReadToDescendant(string name)
    {
      if (this.ReadState == ReadState.Initial)
      {
        int content = (int) this.MoveToContent();
        if (this.IsStartElement(name))
          return true;
      }
      if (this.NodeType != XmlNodeType.Element || this.IsEmptyElement)
        return false;
      int depth = this.Depth;
      this.Read();
      while (depth < this.Depth)
      {
        if (this.NodeType == XmlNodeType.Element && name == this.Name)
          return true;
        this.Read();
      }
      return false;
    }

    public virtual bool ReadToDescendant(string localName, string namespaceURI)
    {
      if (this.ReadState == ReadState.Initial)
      {
        int content = (int) this.MoveToContent();
        if (this.IsStartElement(localName, namespaceURI))
          return true;
      }
      if (this.NodeType != XmlNodeType.Element || this.IsEmptyElement)
        return false;
      int depth = this.Depth;
      this.Read();
      while (depth < this.Depth)
      {
        if (this.NodeType == XmlNodeType.Element && localName == this.LocalName && namespaceURI == this.NamespaceURI)
          return true;
        this.Read();
      }
      return false;
    }

    public virtual bool ReadToFollowing(string name)
    {
      while (this.Read())
      {
        if (this.NodeType == XmlNodeType.Element && name == this.Name)
          return true;
      }
      return false;
    }

    public virtual bool ReadToFollowing(string localName, string namespaceURI)
    {
      while (this.Read())
      {
        if (this.NodeType == XmlNodeType.Element && localName == this.LocalName && namespaceURI == this.NamespaceURI)
          return true;
      }
      return false;
    }

    public virtual bool ReadToNextSibling(string name)
    {
      if (this.ReadState != ReadState.Interactive)
        return false;
      int depth = this.Depth;
      this.Skip();
      while (!this.EOF && depth <= this.Depth)
      {
        if (this.NodeType == XmlNodeType.Element && name == this.Name)
          return true;
        this.Skip();
      }
      return false;
    }

    public virtual bool ReadToNextSibling(string localName, string namespaceURI)
    {
      if (this.ReadState != ReadState.Interactive)
        return false;
      int depth = this.Depth;
      this.Skip();
      while (!this.EOF && depth <= this.Depth)
      {
        if (this.NodeType == XmlNodeType.Element && localName == this.LocalName && namespaceURI == this.NamespaceURI)
          return true;
        this.Skip();
      }
      return false;
    }

    public virtual XmlReader ReadSubtree()
    {
      if (this.NodeType != XmlNodeType.Element)
        throw new InvalidOperationException(string.Format("ReadSubtree() can be invoked only when the reader is positioned on an element. Current node is {0}. {1}", (object) this.NodeType, (object) this.GetLocation()));
      return (XmlReader) new SubtreeXmlReader(this);
    }

    private string ReadContentString() => this.NodeType == XmlNodeType.Attribute || this.NodeType != XmlNodeType.Element && this.HasAttributes ? this.Value : this.ReadContentString(true);

    private string ReadContentString(bool isText)
    {
      if (isText)
      {
        XmlNodeType nodeType = this.NodeType;
        switch (nodeType)
        {
          case XmlNodeType.Element:
            throw new InvalidOperationException(string.Format("Node type {0} is not supported in this operation.{1}", (object) this.NodeType, (object) this.GetLocation()));
          case XmlNodeType.Text:
          case XmlNodeType.CDATA:
            break;
          default:
            if (nodeType != XmlNodeType.Whitespace && nodeType != XmlNodeType.SignificantWhitespace)
              return string.Empty;
            break;
        }
      }
      string empty = string.Empty;
      do
      {
        XmlNodeType nodeType = this.NodeType;
        switch (nodeType)
        {
          case XmlNodeType.Element:
            if (isText)
              return empty;
            throw this.XmlError("Child element is not expected in this operation.");
          case XmlNodeType.Text:
          case XmlNodeType.CDATA:
label_12:
            empty += this.Value;
            break;
          default:
            switch (nodeType - 13)
            {
              case XmlNodeType.None:
              case XmlNodeType.Element:
                goto label_12;
              case XmlNodeType.Attribute:
                return empty;
            }
            break;
        }
      }
      while (this.Read());
      throw this.XmlError("Unexpected end of document.");
    }

    private string GetLocation() => this is IXmlLineInfo xmlLineInfo && xmlLineInfo.HasLineInfo() ? string.Format(" {0} (line {1}, column {2})", (object) this.BaseURI, (object) xmlLineInfo.LineNumber, (object) xmlLineInfo.LinePosition) : string.Empty;

    [MonoTODO]
    public virtual object ReadElementContentAsObject() => this.ReadElementContentAs(this.ValueType, (IXmlNamespaceResolver) null);

    [MonoTODO]
    public virtual object ReadElementContentAsObject(string localName, string namespaceURI) => this.ReadElementContentAs(this.ValueType, (IXmlNamespaceResolver) null, localName, namespaceURI);

    [MonoTODO]
    public virtual object ReadContentAsObject() => this.ReadContentAs(this.ValueType, (IXmlNamespaceResolver) null);

    public virtual object ReadElementContentAs(Type type, IXmlNamespaceResolver resolver)
    {
      bool isEmptyElement = this.IsEmptyElement;
      this.ReadStartElement();
      object obj = this.ValueAs(!isEmptyElement ? this.ReadContentString(false) : string.Empty, type, resolver);
      if (!isEmptyElement)
        this.ReadEndElement();
      return obj;
    }

    public virtual object ReadElementContentAs(
      Type type,
      IXmlNamespaceResolver resolver,
      string localName,
      string namespaceURI)
    {
      this.ReadStartElement(localName, namespaceURI);
      object obj = this.ReadContentAs(type, resolver);
      this.ReadEndElement();
      return obj;
    }

    public virtual object ReadContentAs(Type type, IXmlNamespaceResolver resolver) => this.ValueAs(this.ReadContentString(), type, resolver);

    private object ValueAs(string text, Type type, IXmlNamespaceResolver resolver)
    {
      try
      {
        if (type == typeof (object))
          return (object) text;
        if (type == typeof (XmlQualifiedName))
          return resolver != null ? (object) XmlQualifiedName.Parse(text, resolver) : (object) XmlQualifiedName.Parse(text, this);
        if (type == typeof (DateTimeOffset))
          return (object) XmlConvert.ToDateTimeOffset(text);
        switch (Type.GetTypeCode(type))
        {
          case TypeCode.Boolean:
            return (object) XQueryConvert.StringToBoolean(text);
          case TypeCode.Int32:
            return (object) XQueryConvert.StringToInt(text);
          case TypeCode.Int64:
            return (object) XQueryConvert.StringToInteger(text);
          case TypeCode.Single:
            return (object) XQueryConvert.StringToFloat(text);
          case TypeCode.Double:
            return (object) XQueryConvert.StringToDouble(text);
          case TypeCode.Decimal:
            return (object) XQueryConvert.StringToDecimal(text);
          case TypeCode.DateTime:
            return (object) XQueryConvert.StringToDateTime(text);
          case TypeCode.String:
            return (object) text;
        }
      }
      catch (Exception ex)
      {
        throw this.XmlError(string.Format("Current text value '{0}' is not acceptable for specified type '{1}'. {2}", (object) text, (object) type, ex == null ? (object) string.Empty : (object) ex.Message), ex);
      }
      throw new ArgumentException(string.Format("Specified type '{0}' is not supported.", (object) type));
    }

    public virtual bool ReadElementContentAsBoolean()
    {
      try
      {
        return XQueryConvert.StringToBoolean(this.ReadElementContentAsString());
      }
      catch (FormatException ex)
      {
        throw this.XmlError("Typed value is invalid.", (Exception) ex);
      }
    }

    public virtual DateTime ReadElementContentAsDateTime()
    {
      try
      {
        return XQueryConvert.StringToDateTime(this.ReadElementContentAsString());
      }
      catch (FormatException ex)
      {
        throw this.XmlError("Typed value is invalid.", (Exception) ex);
      }
    }

    public virtual Decimal ReadElementContentAsDecimal()
    {
      try
      {
        return XQueryConvert.StringToDecimal(this.ReadElementContentAsString());
      }
      catch (FormatException ex)
      {
        throw this.XmlError("Typed value is invalid.", (Exception) ex);
      }
    }

    public virtual double ReadElementContentAsDouble()
    {
      try
      {
        return XQueryConvert.StringToDouble(this.ReadElementContentAsString());
      }
      catch (FormatException ex)
      {
        throw this.XmlError("Typed value is invalid.", (Exception) ex);
      }
    }

    public virtual float ReadElementContentAsFloat()
    {
      try
      {
        return XQueryConvert.StringToFloat(this.ReadElementContentAsString());
      }
      catch (FormatException ex)
      {
        throw this.XmlError("Typed value is invalid.", (Exception) ex);
      }
    }

    public virtual int ReadElementContentAsInt()
    {
      try
      {
        return XQueryConvert.StringToInt(this.ReadElementContentAsString());
      }
      catch (FormatException ex)
      {
        throw this.XmlError("Typed value is invalid.", (Exception) ex);
      }
    }

    public virtual long ReadElementContentAsLong()
    {
      try
      {
        return XQueryConvert.StringToInteger(this.ReadElementContentAsString());
      }
      catch (FormatException ex)
      {
        throw this.XmlError("Typed value is invalid.", (Exception) ex);
      }
    }

    public virtual string ReadElementContentAsString()
    {
      bool isEmptyElement = this.IsEmptyElement;
      if (this.NodeType != XmlNodeType.Element)
        throw new InvalidOperationException(string.Format("'{0}' is an element node.", (object) this.NodeType));
      this.ReadStartElement();
      if (isEmptyElement)
        return string.Empty;
      string str = this.ReadContentString(false);
      this.ReadEndElement();
      return str;
    }

    public virtual bool ReadElementContentAsBoolean(string localName, string namespaceURI)
    {
      try
      {
        return XQueryConvert.StringToBoolean(this.ReadElementContentAsString(localName, namespaceURI));
      }
      catch (FormatException ex)
      {
        throw this.XmlError("Typed value is invalid.", (Exception) ex);
      }
    }

    public virtual DateTime ReadElementContentAsDateTime(string localName, string namespaceURI)
    {
      try
      {
        return XQueryConvert.StringToDateTime(this.ReadElementContentAsString(localName, namespaceURI));
      }
      catch (FormatException ex)
      {
        throw this.XmlError("Typed value is invalid.", (Exception) ex);
      }
    }

    public virtual Decimal ReadElementContentAsDecimal(string localName, string namespaceURI)
    {
      try
      {
        return XQueryConvert.StringToDecimal(this.ReadElementContentAsString(localName, namespaceURI));
      }
      catch (FormatException ex)
      {
        throw this.XmlError("Typed value is invalid.", (Exception) ex);
      }
    }

    public virtual double ReadElementContentAsDouble(string localName, string namespaceURI)
    {
      try
      {
        return XQueryConvert.StringToDouble(this.ReadElementContentAsString(localName, namespaceURI));
      }
      catch (FormatException ex)
      {
        throw this.XmlError("Typed value is invalid.", (Exception) ex);
      }
    }

    public virtual float ReadElementContentAsFloat(string localName, string namespaceURI)
    {
      try
      {
        return XQueryConvert.StringToFloat(this.ReadElementContentAsString(localName, namespaceURI));
      }
      catch (FormatException ex)
      {
        throw this.XmlError("Typed value is invalid.", (Exception) ex);
      }
    }

    public virtual int ReadElementContentAsInt(string localName, string namespaceURI)
    {
      try
      {
        return XQueryConvert.StringToInt(this.ReadElementContentAsString(localName, namespaceURI));
      }
      catch (FormatException ex)
      {
        throw this.XmlError("Typed value is invalid.", (Exception) ex);
      }
    }

    public virtual long ReadElementContentAsLong(string localName, string namespaceURI)
    {
      try
      {
        return XQueryConvert.StringToInteger(this.ReadElementContentAsString(localName, namespaceURI));
      }
      catch (FormatException ex)
      {
        throw this.XmlError("Typed value is invalid.", (Exception) ex);
      }
    }

    public virtual string ReadElementContentAsString(string localName, string namespaceURI)
    {
      bool isEmptyElement = this.IsEmptyElement;
      if (this.NodeType != XmlNodeType.Element)
        throw new InvalidOperationException(string.Format("'{0}' is an element node.", (object) this.NodeType));
      this.ReadStartElement(localName, namespaceURI);
      if (isEmptyElement)
        return string.Empty;
      string str = this.ReadContentString(false);
      this.ReadEndElement();
      return str;
    }

    public virtual bool ReadContentAsBoolean()
    {
      try
      {
        return XQueryConvert.StringToBoolean(this.ReadContentString());
      }
      catch (FormatException ex)
      {
        throw this.XmlError("Typed value is invalid.", (Exception) ex);
      }
    }

    public virtual DateTime ReadContentAsDateTime()
    {
      try
      {
        return XQueryConvert.StringToDateTime(this.ReadContentString());
      }
      catch (FormatException ex)
      {
        throw this.XmlError("Typed value is invalid.", (Exception) ex);
      }
    }

    public virtual Decimal ReadContentAsDecimal()
    {
      try
      {
        return XQueryConvert.StringToDecimal(this.ReadContentString());
      }
      catch (FormatException ex)
      {
        throw this.XmlError("Typed value is invalid.", (Exception) ex);
      }
    }

    public virtual double ReadContentAsDouble()
    {
      try
      {
        return XQueryConvert.StringToDouble(this.ReadContentString());
      }
      catch (FormatException ex)
      {
        throw this.XmlError("Typed value is invalid.", (Exception) ex);
      }
    }

    public virtual float ReadContentAsFloat()
    {
      try
      {
        return XQueryConvert.StringToFloat(this.ReadContentString());
      }
      catch (FormatException ex)
      {
        throw this.XmlError("Typed value is invalid.", (Exception) ex);
      }
    }

    public virtual int ReadContentAsInt()
    {
      try
      {
        return XQueryConvert.StringToInt(this.ReadContentString());
      }
      catch (FormatException ex)
      {
        throw this.XmlError("Typed value is invalid.", (Exception) ex);
      }
    }

    public virtual long ReadContentAsLong()
    {
      try
      {
        return XQueryConvert.StringToInteger(this.ReadContentString());
      }
      catch (FormatException ex)
      {
        throw this.XmlError("Typed value is invalid.", (Exception) ex);
      }
    }

    public virtual string ReadContentAsString() => this.ReadContentString();

    public virtual int ReadContentAsBase64(byte[] buffer, int offset, int length)
    {
      this.CheckSupport();
      return this.binary.ReadContentAsBase64(buffer, offset, length);
    }

    public virtual int ReadContentAsBinHex(byte[] buffer, int offset, int length)
    {
      this.CheckSupport();
      return this.binary.ReadContentAsBinHex(buffer, offset, length);
    }

    public virtual int ReadElementContentAsBase64(byte[] buffer, int offset, int length)
    {
      this.CheckSupport();
      return this.binary.ReadElementContentAsBase64(buffer, offset, length);
    }

    public virtual int ReadElementContentAsBinHex(byte[] buffer, int offset, int length)
    {
      this.CheckSupport();
      return this.binary.ReadElementContentAsBinHex(buffer, offset, length);
    }

    private void CheckSupport()
    {
      if (!this.CanReadBinaryContent || !this.CanReadValueChunk)
        throw new NotSupportedException();
      if (this.binary != null)
        return;
      this.binary = new XmlReaderBinarySupport(this);
    }

    public virtual int ReadValueChunk(char[] buffer, int offset, int length)
    {
      if (!this.CanReadValueChunk)
        throw new NotSupportedException();
      if (this.binary == null)
        this.binary = new XmlReaderBinarySupport(this);
      return this.binary.ReadValueChunk(buffer, offset, length);
    }

    public abstract void ResolveEntity();

    public virtual void Skip()
    {
      if (this.ReadState != ReadState.Interactive)
        return;
      this.MoveToElement();
      if (this.NodeType != XmlNodeType.Element || this.IsEmptyElement)
      {
        this.Read();
      }
      else
      {
        int depth = this.Depth;
        do
          ;
        while (this.Read() && depth < this.Depth);
        if (this.NodeType != XmlNodeType.EndElement)
          return;
        this.Read();
      }
    }

    private XmlException XmlError(string message) => new XmlException(this as IXmlLineInfo, this.BaseURI, message);

    private XmlException XmlError(string message, Exception innerException) => new XmlException(this as IXmlLineInfo, this.BaseURI, message);
  }
}
