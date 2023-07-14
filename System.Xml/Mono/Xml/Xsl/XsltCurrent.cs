// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.XsltCurrent
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml;
using System.Xml.XPath;

namespace Mono.Xml.Xsl
{
  internal class XsltCurrent : XPathFunction
  {
    public XsltCurrent(FunctionArguments args)
      : base(args)
    {
      if (args != null)
        throw new XPathException("current takes 0 args");
    }

    public override XPathResultType ReturnType => XPathResultType.NodeSet;

    public override object Evaluate(BaseIterator iter)
    {
      XsltCompiledContext namespaceManager = (XsltCompiledContext) iter.NamespaceManager;
      return (object) new SelfIterator(namespaceManager.Processor.CurrentNode, (IXmlNamespaceResolver) namespaceManager);
    }

    internal override bool Peer => false;

    public override string ToString() => "current()";
  }
}
