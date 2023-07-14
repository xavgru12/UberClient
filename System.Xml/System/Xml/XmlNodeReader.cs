// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlNodeReader
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml;
using System.Collections.Generic;
using System.Xml.Schema;

namespace System.Xml
{
  public class XmlNodeReader : XmlReader, IHasXmlParserContext, IXmlNamespaceResolver
  {
    private XmlReader entity;
    private XmlNodeReaderImpl source;
    private bool entityInsideAttribute;
    private bool insideAttribute;

    public XmlNodeReader(XmlNode node) => this.source = new XmlNodeReaderImpl(node);

    private XmlNodeReader(XmlNodeReaderImpl entityContainer, bool insideAttribute)
    {
      this.source = new XmlNodeReaderImpl(entityContainer);
      this.entityInsideAttribute = insideAttribute;
    }

    XmlParserContext IHasXmlParserContext.ParserContext => ((IHasXmlParserContext) this.Current).ParserContext;

    IDictionary<string, string> IXmlNamespaceResolver.GetNamespacesInScope(XmlNamespaceScope scope) => ((IXmlNamespaceResolver) this.Current).GetNamespacesInScope(scope);

    string IXmlNamespaceResolver.LookupPrefix(string ns) => ((IXmlNamespaceResolver) this.Current).LookupPrefix(ns);

    private XmlReader Current => this.entity != null && this.entity.ReadState != ReadState.Initial ? this.entity : (XmlReader) this.source;

    public override int AttributeCount => this.Current.AttributeCount;

    public override string BaseURI => this.Current.BaseURI;

    public override bool CanReadBinaryContent => true;

    public override bool CanResolveEntity => true;

    public override int Depth => this.entity != null && this.entity.ReadState == ReadState.Interactive ? this.source.Depth + this.entity.Depth + 1 : this.source.Depth;

    public override bool EOF => this.source.EOF;

    public override bool HasAttributes => this.Current.HasAttributes;

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

    public override string Prefix => this.Current.Prefix;

    public override ReadState ReadState => this.entity != null ? ReadState.Interactive : this.source.ReadState;

    public override IXmlSchemaInfo SchemaInfo => this.entity != null ? this.entity.SchemaInfo : this.source.SchemaInfo;

    public override string Value => this.Current.Value;

    public override string XmlLang => this.Current.XmlLang;

    public override XmlSpace XmlSpace => this.Current.XmlSpace;

    public override void Close()
    {
      if (this.entity != null)
        this.entity.Close();
      this.source.Close();
    }

    public override string GetAttribute(int attributeIndex) => this.Current.GetAttribute(attributeIndex);

    public override string GetAttribute(string name) => this.Current.GetAttribute(name);

    public override string GetAttribute(string name, string namespaceURI) => this.Current.GetAttribute(name, namespaceURI);

    public override string LookupNamespace(string prefix) => this.Current.LookupNamespace(prefix);

    public override void MoveToAttribute(int i)
    {
      if (this.entity != null && this.entityInsideAttribute)
      {
        this.entity.Close();
        this.entity = (XmlReader) null;
      }
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
      {
        this.entity.Close();
        this.entity = (XmlReader) null;
      }
      this.insideAttribute = true;
      return true;
    }

    public override bool MoveToAttribute(string localName, string namespaceURI)
    {
      if (this.entity != null && !this.entityInsideAttribute)
        return this.entity.MoveToAttribute(localName, namespaceURI);
      if (!this.source.MoveToAttribute(localName, namespaceURI))
        return false;
      if (this.entity != null && this.entityInsideAttribute)
      {
        this.entity.Close();
        this.entity = (XmlReader) null;
      }
      this.insideAttribute = true;
      return true;
    }

    public override bool MoveToElement()
    {
      if (this.entity != null && this.entityInsideAttribute)
        this.entity = (XmlReader) null;
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
      {
        this.entity.Close();
        this.entity = (XmlReader) null;
      }
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
      {
        this.entity.Close();
        this.entity = (XmlReader) null;
      }
      this.insideAttribute = true;
      return true;
    }

    public override bool Read()
    {
      this.insideAttribute = false;
      if (this.entity != null && (this.entityInsideAttribute || this.entity.EOF))
        this.entity = (XmlReader) null;
      if (this.entity == null)
        return this.source.Read();
      this.entity.Read();
      return true;
    }

    public override bool ReadAttributeValue()
    {
      if (this.entity != null && this.entityInsideAttribute)
      {
        if (this.entity.EOF)
        {
          this.entity = (XmlReader) null;
        }
        else
        {
          this.entity.Read();
          return true;
        }
      }
      return this.Current.ReadAttributeValue();
    }

    public override int ReadContentAsBase64(byte[] buffer, int offset, int length) => this.entity != null ? this.entity.ReadContentAsBase64(buffer, offset, length) : this.source.ReadContentAsBase64(buffer, offset, length);

    public override int ReadContentAsBinHex(byte[] buffer, int offset, int length) => this.entity != null ? this.entity.ReadContentAsBinHex(buffer, offset, length) : this.source.ReadContentAsBinHex(buffer, offset, length);

    public override int ReadElementContentAsBase64(byte[] buffer, int offset, int length) => this.entity != null ? this.entity.ReadElementContentAsBase64(buffer, offset, length) : this.source.ReadElementContentAsBase64(buffer, offset, length);

    public override int ReadElementContentAsBinHex(byte[] buffer, int offset, int length) => this.entity != null ? this.entity.ReadElementContentAsBinHex(buffer, offset, length) : this.source.ReadElementContentAsBinHex(buffer, offset, length);

    public override string ReadString() => base.ReadString();

    public override void ResolveEntity()
    {
      if (this.entity != null)
        this.entity.ResolveEntity();
      else
        this.entity = this.source.NodeType == XmlNodeType.EntityReference ? (XmlReader) new XmlNodeReader(this.source, this.insideAttribute) : throw new InvalidOperationException("The current node is not an Entity Reference");
    }

    public override void Skip()
    {
      if (this.entity != null && this.entityInsideAttribute)
        this.entity = (XmlReader) null;
      this.Current.Skip();
    }
  }
}
