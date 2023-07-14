// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.ExprVariable
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml.Xsl;

namespace System.Xml.XPath
{
  internal class ExprVariable : Expression
  {
    protected XmlQualifiedName _name;
    protected bool resolvedName;

    public ExprVariable(XmlQualifiedName name, IStaticXsltContext ctx)
    {
      if (ctx != null)
      {
        name = ctx.LookupQName(name.ToString());
        this.resolvedName = true;
      }
      this._name = name;
    }

    public override string ToString() => "$" + this._name.ToString();

    public override XPathResultType ReturnType => XPathResultType.Any;

    public override XPathResultType GetReturnType(BaseIterator iter) => XPathResultType.Any;

    public override object Evaluate(BaseIterator iter)
    {
      if (!(iter.NamespaceManager is XsltContext namespaceManager))
        throw new XPathException(string.Format("XSLT context is required to resolve variable. Current namespace manager in current node-set '{0}' is '{1}'", (object) iter.GetType(), iter.NamespaceManager == null ? (object) (Type) null : (object) iter.NamespaceManager.GetType()));
      object obj = ((!this.resolvedName ? namespaceManager.ResolveVariable(new XmlQualifiedName(this._name.Name, this._name.Namespace)) : namespaceManager.ResolveVariable(this._name)) ?? throw new XPathException("variable " + this._name.ToString() + " not found")).Evaluate(namespaceManager);
      if (!(obj is XPathNodeIterator iter1))
        return obj;
      return iter1 is BaseIterator ? (object) iter1 : (object) new WrapperIterator(iter1, iter.NamespaceManager);
    }

    internal override bool Peer => false;
  }
}
