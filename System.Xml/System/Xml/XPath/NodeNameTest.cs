// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.NodeNameTest
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml.Xsl;

namespace System.Xml.XPath
{
  internal class NodeNameTest : NodeTest
  {
    protected XmlQualifiedName _name;
    protected readonly bool resolvedName;

    public NodeNameTest(Axes axis, XmlQualifiedName name, IStaticXsltContext ctx)
      : base(axis)
    {
      if (ctx != null)
      {
        name = ctx.LookupQName(name.ToString());
        this.resolvedName = true;
      }
      this._name = name;
    }

    public NodeNameTest(Axes axis, XmlQualifiedName name, bool resolvedName)
      : base(axis)
    {
      this._name = name;
      this.resolvedName = resolvedName;
    }

    internal NodeNameTest(NodeNameTest source, Axes axis)
      : base(axis)
    {
      this._name = source._name;
      this.resolvedName = source.resolvedName;
    }

    public override string ToString() => this._axis.ToString() + "::" + this._name.ToString();

    public XmlQualifiedName Name => this._name;

    public override bool Match(IXmlNamespaceResolver nsm, XPathNavigator nav)
    {
      if (nav.NodeType != this._axis.NodeType || this._name.Name != string.Empty && this._name.Name != nav.LocalName)
        return false;
      string str = string.Empty;
      if (this._name.Namespace != string.Empty)
      {
        if (this.resolvedName)
          str = this._name.Namespace;
        else if (nsm != null)
          str = nsm.LookupNamespace(this._name.Namespace);
        if (str == null)
          throw new XPathException("Invalid namespace prefix: " + this._name.Namespace);
      }
      return str == nav.NamespaceURI;
    }

    public override void GetInfo(
      out string name,
      out string ns,
      out XPathNodeType nodetype,
      IXmlNamespaceResolver nsm)
    {
      nodetype = this._axis.NodeType;
      name = !(this._name.Name != string.Empty) ? (string) null : this._name.Name;
      ns = string.Empty;
      if (nsm == null || !(this._name.Namespace != string.Empty))
        return;
      ns = !this.resolvedName ? nsm.LookupNamespace(this._name.Namespace) : this._name.Namespace;
      if (ns == null)
        throw new XPathException("Invalid namespace prefix: " + this._name.Namespace);
    }
  }
}
