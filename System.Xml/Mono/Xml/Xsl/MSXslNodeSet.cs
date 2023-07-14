// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.MSXslNodeSet
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Collections;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl
{
  internal class MSXslNodeSet : XPathFunction
  {
    private Expression arg0;

    public MSXslNodeSet(FunctionArguments args)
      : base(args)
    {
      this.arg0 = args != null && args.Tail == null ? args.Arg : throw new XPathException("element-available takes 1 arg");
    }

    public override XPathResultType ReturnType => XPathResultType.NodeSet;

    internal override bool Peer => this.arg0.Peer;

    public override object Evaluate(BaseIterator iter)
    {
      XsltCompiledContext namespaceManager = iter.NamespaceManager as XsltCompiledContext;
      XPathNavigator nav = iter.Current == null ? (XPathNavigator) null : iter.Current.Clone();
      if (!(this.arg0.EvaluateAs(iter, XPathResultType.Navigator) is XPathNavigator xpathNavigator))
        return nav != null ? (object) new XsltException("Cannot convert the XPath argument to a result tree fragment.", (Exception) null, nav) : (object) new XsltException("Cannot convert the XPath argument to a result tree fragment.", (Exception) null);
      return (object) new ListIterator((IList) new ArrayList()
      {
        (object) xpathNavigator
      }, (IXmlNamespaceResolver) namespaceManager);
    }
  }
}
