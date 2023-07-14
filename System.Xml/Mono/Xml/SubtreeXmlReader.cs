// Decompiled with JetBrains decompiler
// Type: Mono.Xml.SubtreeXmlReader
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections.Generic;
using System.Xml;

namespace Mono.Xml
{
  internal class SubtreeXmlReader : XmlReader, IXmlLineInfo, IXmlNamespaceResolver
  {
    private int startDepth;
    private bool eof;
    private bool initial;
    private bool read;
    private XmlReader Reader;
    private IXmlLineInfo li;
    private IXmlNamespaceResolver nsResolver;

    public SubtreeXmlReader(XmlReader reader)
    {
      this.Reader = reader;
      this.li = reader as IXmlLineInfo;
      this.nsResolver = reader as IXmlNamespaceResolver;
      this.initial = true;
      this.startDepth = reader.Depth;
      if (reader.ReadState != ReadState.Initial)
        return;
      this.startDepth = -1;
    }

    IDictionary<string, string> IXmlNamespaceResolver.GetNamespacesInScope(XmlNamespaceScope scope) => this.nsResolver != null ? this.nsResolver.GetNamespacesInScope(scope) : (IDictionary<string, string>) new Dictionary<string, string>();

    string IXmlNamespaceResolver.LookupPrefix(string ns) => this.nsResolver != null ? this.nsResolver.LookupPrefix(ns) : string.Empty;

    public override int AttributeCount => this.initial ? 0 : this.Reader.AttributeCount;

    public override bool CanReadBinaryContent => this.Reader.CanReadBinaryContent;

    public override bool CanReadValueChunk => this.Reader.CanReadValueChunk;

    public override int Depth => this.Reader.Depth - this.startDepth;

    public override string BaseURI => this.Reader.BaseURI;

    public override bool EOF => this.eof || this.Reader.EOF;

    public override bool IsEmptyElement => this.Reader.IsEmptyElement;

    public int LineNumber => this.initial || this.li == null ? 0 : this.li.LineNumber;

    public int LinePosition => this.initial || this.li == null ? 0 : this.li.LinePosition;

    public override bool HasValue => !this.initial && !this.eof && this.Reader.HasValue;

    public override string LocalName => this.initial || this.eof ? string.Empty : this.Reader.LocalName;

    public override string Name => this.initial || this.eof ? string.Empty : this.Reader.Name;

    public override XmlNameTable NameTable => this.Reader.NameTable;

    public override string NamespaceURI => this.initial || this.eof ? string.Empty : this.Reader.NamespaceURI;

    public override XmlNodeType NodeType => this.initial || this.eof ? XmlNodeType.None : this.Reader.NodeType;

    public override string Prefix => this.initial || this.eof ? string.Empty : this.Reader.Prefix;

    public override ReadState ReadState
    {
      get
      {
        if (this.initial)
          return ReadState.Initial;
        return this.eof ? ReadState.EndOfFile : this.Reader.ReadState;
      }
    }

    public override XmlReaderSettings Settings => this.Reader.Settings;

    public override string Value => this.initial ? string.Empty : this.Reader.Value;

    public override void Close()
    {
      do
        ;
      while (this.Read());
    }

    public override string GetAttribute(int i) => this.initial ? (string) null : this.Reader.GetAttribute(i);

    public override string GetAttribute(string name) => this.initial ? (string) null : this.Reader.GetAttribute(name);

    public override string GetAttribute(string local, string ns) => this.initial ? (string) null : this.Reader.GetAttribute(local, ns);

    public bool HasLineInfo() => this.li != null && this.li.HasLineInfo();

    public override string LookupNamespace(string prefix) => this.Reader.LookupNamespace(prefix);

    public override bool MoveToFirstAttribute() => !this.initial && this.Reader.MoveToFirstAttribute();

    public override bool MoveToNextAttribute() => !this.initial && this.Reader.MoveToNextAttribute();

    public override void MoveToAttribute(int i)
    {
      if (this.initial)
        return;
      this.Reader.MoveToAttribute(i);
    }

    public override bool MoveToAttribute(string name) => !this.initial && this.Reader.MoveToAttribute(name);

    public override bool MoveToAttribute(string local, string ns) => !this.initial && this.Reader.MoveToAttribute(local, ns);

    public override bool MoveToElement() => !this.initial && this.Reader.MoveToElement();

    public override bool Read()
    {
      if (this.initial)
      {
        this.initial = false;
        return true;
      }
      if (!this.read)
      {
        this.read = true;
        this.Reader.MoveToElement();
        bool flag = !this.Reader.IsEmptyElement && this.Reader.Read();
        if (!flag)
          this.eof = true;
        return flag;
      }
      this.Reader.MoveToElement();
      if (this.Reader.Depth > this.startDepth && this.Reader.Read())
        return true;
      this.eof = true;
      return false;
    }

    public override bool ReadAttributeValue() => !this.initial && !this.eof && this.Reader.ReadAttributeValue();

    public override void ResolveEntity()
    {
      if (this.initial || this.eof)
        return;
      this.Reader.ResolveEntity();
    }
  }
}
