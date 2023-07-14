// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaReader
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.Schema
{
  internal class XmlSchemaReader : XmlReader, IXmlLineInfo
  {
    private XmlReader reader;
    private ValidationEventHandler handler;
    private bool hasLineInfo;

    public XmlSchemaReader(XmlReader reader, ValidationEventHandler handler)
    {
      this.reader = reader;
      this.handler = handler;
      if (!(reader is IXmlLineInfo))
        return;
      this.hasLineInfo = ((IXmlLineInfo) reader).HasLineInfo();
    }

    public string FullName => this.NamespaceURI + ":" + this.LocalName;

    public XmlReader Reader => this.reader;

    public void RaiseInvalidElementError()
    {
      string message = "Element " + this.FullName + " is invalid in this context.\n";
      if (this.hasLineInfo)
        message = message + "The error occured on (" + (object) ((IXmlLineInfo) this.reader).LineNumber + "," + (object) ((IXmlLineInfo) this.reader).LinePosition + ")";
      XmlSchemaObject.error(this.handler, message, (Exception) null);
      this.SkipToEnd();
    }

    public bool ReadNextElement()
    {
      this.MoveToElement();
      while (this.Read())
      {
        if (this.NodeType == XmlNodeType.Element || this.NodeType == XmlNodeType.EndElement)
        {
          if (!(this.reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema"))
            return true;
          this.RaiseInvalidElementError();
        }
      }
      return false;
    }

    public void SkipToEnd()
    {
      this.MoveToElement();
      if (this.IsEmptyElement || this.NodeType != XmlNodeType.Element || this.NodeType != XmlNodeType.Element)
        return;
      int depth = this.Depth;
      do
        ;
      while (this.Read() && this.Depth != depth);
    }

    public bool HasLineInfo() => this.hasLineInfo;

    public int LineNumber => this.hasLineInfo ? ((IXmlLineInfo) this.reader).LineNumber : 0;

    public int LinePosition => this.hasLineInfo ? ((IXmlLineInfo) this.reader).LinePosition : 0;

    public override int AttributeCount => this.reader.AttributeCount;

    public override string BaseURI => this.reader.BaseURI;

    public override bool CanResolveEntity => this.reader.CanResolveEntity;

    public override int Depth => this.reader.Depth;

    public override bool EOF => this.reader.EOF;

    public override bool HasAttributes => this.reader.HasAttributes;

    public override bool HasValue => this.reader.HasValue;

    public override bool IsDefault => this.reader.IsDefault;

    public override bool IsEmptyElement => this.reader.IsEmptyElement;

    public override string this[int i] => this.reader[i];

    public override string this[string name] => this.reader[name];

    public override string this[string name, string namespaceURI] => this.reader[name, namespaceURI];

    public override string LocalName => this.reader.LocalName;

    public override string Name => this.reader.Name;

    public override string NamespaceURI => this.reader.NamespaceURI;

    public override XmlNameTable NameTable => this.reader.NameTable;

    public override XmlNodeType NodeType => this.reader.NodeType;

    public override string Prefix => this.reader.Prefix;

    public override char QuoteChar => this.reader.QuoteChar;

    public override ReadState ReadState => this.reader.ReadState;

    public override string Value => this.reader.Value;

    public override string XmlLang => this.reader.XmlLang;

    public override XmlSpace XmlSpace => this.reader.XmlSpace;

    public override void Close() => this.reader.Close();

    public override bool Equals(object obj) => this.reader.Equals(obj);

    public override string GetAttribute(int i) => this.reader.GetAttribute(i);

    public override string GetAttribute(string name) => this.reader.GetAttribute(name);

    public override string GetAttribute(string name, string namespaceURI) => this.reader.GetAttribute(name, namespaceURI);

    public override int GetHashCode() => this.reader.GetHashCode();

    public override bool IsStartElement() => this.reader.IsStartElement();

    public override bool IsStartElement(string localname, string ns) => this.reader.IsStartElement(localname, ns);

    public override bool IsStartElement(string name) => this.reader.IsStartElement(name);

    public override string LookupNamespace(string prefix) => this.reader.LookupNamespace(prefix);

    public override void MoveToAttribute(int i) => this.reader.MoveToAttribute(i);

    public override bool MoveToAttribute(string name) => this.reader.MoveToAttribute(name);

    public override bool MoveToAttribute(string name, string ns) => this.reader.MoveToAttribute(name, ns);

    public override XmlNodeType MoveToContent() => this.reader.MoveToContent();

    public override bool MoveToElement() => this.reader.MoveToElement();

    public override bool MoveToFirstAttribute() => this.reader.MoveToFirstAttribute();

    public override bool MoveToNextAttribute() => this.reader.MoveToNextAttribute();

    public override bool Read() => this.reader.Read();

    public override bool ReadAttributeValue() => this.reader.ReadAttributeValue();

    public override string ReadElementString() => this.reader.ReadElementString();

    public override string ReadElementString(string localname, string ns) => this.reader.ReadElementString(localname, ns);

    public override string ReadElementString(string name) => this.reader.ReadElementString(name);

    public override void ReadEndElement() => this.reader.ReadEndElement();

    public override string ReadInnerXml() => this.reader.ReadInnerXml();

    public override string ReadOuterXml() => this.reader.ReadOuterXml();

    public override void ReadStartElement() => this.reader.ReadStartElement();

    public override void ReadStartElement(string localname, string ns) => this.reader.ReadStartElement(localname, ns);

    public override void ReadStartElement(string name) => this.reader.ReadStartElement(name);

    public override string ReadString() => this.reader.ReadString();

    public override void ResolveEntity() => this.reader.ResolveEntity();

    public override void Skip() => this.reader.Skip();

    public override string ToString() => this.reader.ToString();
  }
}
