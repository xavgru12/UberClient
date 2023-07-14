// Decompiled with JetBrains decompiler
// Type: System.Xml.Xsl.XsltContext
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml.XPath;

namespace System.Xml.Xsl
{
  public abstract class XsltContext : XmlNamespaceManager
  {
    protected XsltContext()
      : base((XmlNameTable) new System.Xml.NameTable())
    {
    }

    protected XsltContext(System.Xml.NameTable table)
      : base((XmlNameTable) table)
    {
    }

    public abstract bool Whitespace { get; }

    public abstract bool PreserveWhitespace(XPathNavigator nav);

    public abstract int CompareDocument(string baseUri, string nextbaseUri);

    public abstract IXsltContextFunction ResolveFunction(
      string prefix,
      string name,
      XPathResultType[] argTypes);

    public abstract IXsltContextVariable ResolveVariable(string prefix, string name);

    internal virtual IXsltContextVariable ResolveVariable(XmlQualifiedName name) => this.ResolveVariable(this.LookupPrefix(name.Namespace), name.Name);

    internal virtual IXsltContextFunction ResolveFunction(
      XmlQualifiedName name,
      XPathResultType[] argTypes)
    {
      return this.ResolveFunction(name.Name, name.Namespace, argTypes);
    }
  }
}
