// Decompiled with JetBrains decompiler
// Type: Mono.Xml.EntityResolvingXmlReader
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Collections.Generic;
using System.Xml;

namespace Mono.Xml
{
  internal class EntityResolvingXmlReader : 
    XmlReader,
    IHasXmlParserContext,
    IXmlLineInfo,
    IXmlNamespaceResolver
  {
    private EntityResolvingXmlReader entity;
    private XmlReader source;
    private XmlParserContext context;
    private XmlResolver resolver;
    private EntityHandling entity_handling;
    private bool entity_inside_attr;
    private bool inside_attr;
    private bool do_resolve;

    public EntityResolvingXmlReader(XmlReader source)
    {
      this.source = source;
      if (source is IHasXmlParserContext xmlParserContext)
        this.context = xmlParserContext.ParserContext;
      else
        this.context = new XmlParserContext(source.NameTable, new XmlNamespaceManager(source.NameTable), (string) null, XmlSpace.None);
    }

    private EntityResolvingXmlReader(XmlReader entityContainer, bool inside_attr)
    {
      this.source = entityContainer;
      this.entity_inside_attr = inside_attr;
    }

    XmlParserContext IHasXmlParserContext.ParserContext => this.context;

    IDictionary<string, string> IXmlNamespaceResolver.GetNamespacesInScope(XmlNamespaceScope scope) => this.GetNamespacesInScope(scope);

    string IXmlNamespaceResolver.LookupPrefix(string ns) => ((IXmlNamespaceResolver) this.Current).LookupPrefix(ns);

    private XmlReader Current => this.entity != null && this.entity.ReadState != ReadState.Initial ? (XmlReader) this.entity : this.source;

    public override int AttributeCount => this.Current.AttributeCount;

    public override string BaseURI => this.Current.BaseURI;

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

    internal XmlParserContext ParserContext => this.context;

    public override string Prefix => this.Current.Prefix;

    public override char QuoteChar => this.Current.QuoteChar;

    public override ReadState ReadState => this.entity != null ? ReadState.Interactive : this.source.ReadState;

    public override string Value => this.Current.Value;

    public override string XmlLang => this.Current.XmlLang;

    public override XmlSpace XmlSpace => this.Current.XmlSpace;

    private void CopyProperties(EntityResolvingXmlReader other)
    {
      this.context = other.context;
      this.resolver = other.resolver;
      this.entity_handling = other.entity_handling;
    }

    public EntityHandling EntityHandling
    {
      get => this.entity_handling;
      set
      {
        if (this.entity != null)
          this.entity.EntityHandling = value;
        this.entity_handling = value;
      }
    }

    public int LineNumber => !(this.Current is IXmlLineInfo current) ? 0 : current.LineNumber;

    public int LinePosition => !(this.Current is IXmlLineInfo current) ? 0 : current.LinePosition;

    public XmlResolver XmlResolver
    {
      set
      {
        if (this.entity != null)
          this.entity.XmlResolver = value;
        this.resolver = value;
      }
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
      if (this.entity != null && this.entity_inside_attr)
      {
        this.entity.Close();
        this.entity = (EntityResolvingXmlReader) null;
      }
      this.Current.MoveToAttribute(i);
      this.inside_attr = true;
    }

    public override bool MoveToAttribute(string name)
    {
      if (this.entity != null && !this.entity_inside_attr)
        return this.entity.MoveToAttribute(name);
      if (!this.source.MoveToAttribute(name))
        return false;
      if (this.entity != null && this.entity_inside_attr)
      {
        this.entity.Close();
        this.entity = (EntityResolvingXmlReader) null;
      }
      this.inside_attr = true;
      return true;
    }

    public override bool MoveToAttribute(string localName, string namespaceName)
    {
      if (this.entity != null && !this.entity_inside_attr)
        return this.entity.MoveToAttribute(localName, namespaceName);
      if (!this.source.MoveToAttribute(localName, namespaceName))
        return false;
      if (this.entity != null && this.entity_inside_attr)
      {
        this.entity.Close();
        this.entity = (EntityResolvingXmlReader) null;
      }
      this.inside_attr = true;
      return true;
    }

    public override bool MoveToElement()
    {
      if (this.entity != null && this.entity_inside_attr)
      {
        this.entity.Close();
        this.entity = (EntityResolvingXmlReader) null;
      }
      if (!this.Current.MoveToElement())
        return false;
      this.inside_attr = false;
      return true;
    }

    public override bool MoveToFirstAttribute()
    {
      if (this.entity != null && !this.entity_inside_attr)
        return this.entity.MoveToFirstAttribute();
      if (!this.source.MoveToFirstAttribute())
        return false;
      if (this.entity != null && this.entity_inside_attr)
      {
        this.entity.Close();
        this.entity = (EntityResolvingXmlReader) null;
      }
      this.inside_attr = true;
      return true;
    }

    public override bool MoveToNextAttribute()
    {
      if (this.entity != null && !this.entity_inside_attr)
        return this.entity.MoveToNextAttribute();
      if (!this.source.MoveToNextAttribute())
        return false;
      if (this.entity != null && this.entity_inside_attr)
      {
        this.entity.Close();
        this.entity = (EntityResolvingXmlReader) null;
      }
      this.inside_attr = true;
      return true;
    }

    public override bool Read()
    {
      if (this.do_resolve)
      {
        this.DoResolveEntity();
        this.do_resolve = false;
      }
      this.inside_attr = false;
      if (this.entity != null && (this.entity_inside_attr || this.entity.EOF))
      {
        this.entity.Close();
        this.entity = (EntityResolvingXmlReader) null;
      }
      if (this.entity != null)
      {
        if (this.entity.Read() || this.EntityHandling != EntityHandling.ExpandEntities)
          return true;
        this.entity.Close();
        this.entity = (EntityResolvingXmlReader) null;
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
      if (this.entity != null && this.entity_inside_attr)
      {
        if (this.entity.EOF)
        {
          this.entity.Close();
          this.entity = (EntityResolvingXmlReader) null;
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

    public override void ResolveEntity() => this.DoResolveEntity();

    private void DoResolveEntity()
    {
      if (this.entity != null)
      {
        this.entity.ResolveEntity();
      }
      else
      {
        if (this.source.NodeType != XmlNodeType.EntityReference)
          throw new InvalidOperationException("The current node is not an Entity Reference");
        if (this.ParserContext.Dtd == null)
          throw new XmlException((IXmlLineInfo) this, this.BaseURI, string.Format("Cannot resolve entity without DTD: '{0}'", (object) this.source.Name));
        this.entity = new EntityResolvingXmlReader((XmlReader) this.ParserContext.Dtd.GenerateEntityContentReader(this.source.Name, this.ParserContext) ?? throw new XmlException((IXmlLineInfo) this, this.BaseURI, string.Format("Reference to undeclared entity '{0}'.", (object) this.source.Name)), this.inside_attr);
        this.entity.CopyProperties(this);
      }
    }

    public override void Skip() => base.Skip();

    public bool HasLineInfo() => this.Current is IXmlLineInfo current && current.HasLineInfo();
  }
}
