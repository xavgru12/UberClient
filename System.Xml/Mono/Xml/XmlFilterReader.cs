// Decompiled with JetBrains decompiler
// Type: Mono.Xml.XmlFilterReader
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml;

namespace Mono.Xml
{
  internal class XmlFilterReader : XmlReader, IXmlLineInfo
  {
    private XmlReader reader;
    private XmlReaderSettings settings;
    private IXmlLineInfo lineInfo;

    public XmlFilterReader(XmlReader reader, XmlReaderSettings settings)
    {
      this.reader = reader;
      this.settings = settings.Clone();
      this.lineInfo = reader as IXmlLineInfo;
    }

    public override bool CanReadBinaryContent => this.reader.CanReadBinaryContent;

    public override bool CanReadValueChunk => this.reader.CanReadValueChunk;

    public XmlReader Reader => this.reader;

    public int LineNumber => this.lineInfo != null ? this.lineInfo.LineNumber : 0;

    public int LinePosition => this.lineInfo != null ? this.lineInfo.LinePosition : 0;

    public override XmlNodeType NodeType => this.reader.NodeType;

    public override string Name => this.reader.Name;

    public override string LocalName => this.reader.LocalName;

    public override string NamespaceURI => this.reader.NamespaceURI;

    public override string Prefix => this.reader.Prefix;

    public override bool HasValue => this.reader.HasValue;

    public override int Depth => this.reader.Depth;

    public override string Value => this.reader.Value;

    public override string BaseURI => this.reader.BaseURI;

    public override bool IsEmptyElement => this.reader.IsEmptyElement;

    public override bool IsDefault => this.reader.IsDefault;

    public override char QuoteChar => this.reader.QuoteChar;

    public override string XmlLang => this.reader.XmlLang;

    public override XmlSpace XmlSpace => this.reader.XmlSpace;

    public override int AttributeCount => this.reader.AttributeCount;

    public override string this[int i] => this.reader[i];

    public override string this[string name] => this.reader[name];

    public override string this[string localName, string namespaceURI] => this.reader[localName, namespaceURI];

    public override bool EOF => this.reader.EOF;

    public override ReadState ReadState => this.reader.ReadState;

    public override XmlNameTable NameTable => this.reader.NameTable;

    public override XmlReaderSettings Settings => this.settings;

    public override string GetAttribute(string name) => this.reader.GetAttribute(name);

    public override string GetAttribute(string localName, string namespaceURI) => this.reader.GetAttribute(localName, namespaceURI);

    public override string GetAttribute(int i) => this.reader.GetAttribute(i);

    public bool HasLineInfo() => this.lineInfo != null && this.lineInfo.HasLineInfo();

    public override bool MoveToAttribute(string name) => this.reader.MoveToAttribute(name);

    public override bool MoveToAttribute(string localName, string namespaceURI) => this.reader.MoveToAttribute(localName, namespaceURI);

    public override void MoveToAttribute(int i) => this.reader.MoveToAttribute(i);

    public override bool MoveToFirstAttribute() => this.reader.MoveToFirstAttribute();

    public override bool MoveToNextAttribute() => this.reader.MoveToNextAttribute();

    public override bool MoveToElement() => this.reader.MoveToElement();

    public override void Close()
    {
      if (!this.settings.CloseInput)
        return;
      this.reader.Close();
    }

    public override bool Read()
    {
      if (!this.reader.Read())
        return false;
      if (this.reader.NodeType == XmlNodeType.DocumentType && this.settings.ProhibitDtd)
        throw new XmlException("Document Type Definition (DTD) is prohibited in this XML reader.");
      return (this.reader.NodeType != XmlNodeType.Whitespace || !this.settings.IgnoreWhitespace) && (this.reader.NodeType != XmlNodeType.ProcessingInstruction || !this.settings.IgnoreProcessingInstructions) && (this.reader.NodeType != XmlNodeType.Comment || !this.settings.IgnoreComments) || this.Read();
    }

    public override string ReadString() => this.reader.ReadString();

    public override string LookupNamespace(string prefix) => this.reader.LookupNamespace(prefix);

    public override void ResolveEntity() => this.reader.ResolveEntity();

    public override bool ReadAttributeValue() => this.reader.ReadAttributeValue();
  }
}
