// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.XsltKey
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl
{
  internal class XsltKey : XPathFunction
  {
    private Expression arg0;
    private Expression arg1;
    private IStaticXsltContext staticContext;

    public XsltKey(FunctionArguments args, IStaticXsltContext ctx)
      : base(args)
    {
      this.staticContext = ctx;
      this.arg0 = args != null && args.Tail != null ? args.Arg : throw new XPathException("key takes 2 args");
      this.arg1 = args.Tail.Arg;
    }

    public Expression KeyName => this.arg0;

    public Expression Field => this.arg1;

    public override XPathResultType ReturnType => XPathResultType.NodeSet;

    internal override bool Peer => this.arg0.Peer && this.arg1.Peer;

    public bool PatternMatches(XPathNavigator nav, XsltContext nsmgr) => (nsmgr as XsltCompiledContext).MatchesKey(nav, this.staticContext, this.arg0.StaticValueAsString, this.arg1.StaticValueAsString);

    public override object Evaluate(BaseIterator iter) => (iter.NamespaceManager as XsltCompiledContext).EvaluateKey(this.staticContext, iter, this.arg0, this.arg1);
  }
}
