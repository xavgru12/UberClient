// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.XPathFunctionLocalName
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal class XPathFunctionLocalName : XPathFunction
  {
    private Expression arg0;

    public XPathFunctionLocalName(FunctionArguments args)
      : base(args)
    {
      if (args == null)
        return;
      this.arg0 = args.Arg;
      if (args.Tail != null)
        throw new XPathException("local-name takes 1 or zero args");
    }

    public override XPathResultType ReturnType => XPathResultType.String;

    internal override bool Peer => this.arg0 == null || this.arg0.Peer;

    public override object Evaluate(BaseIterator iter)
    {
      if (this.arg0 == null)
        return (object) iter.Current.LocalName;
      BaseIterator nodeSet = this.arg0.EvaluateNodeSet(iter);
      return nodeSet == null || !nodeSet.MoveNext() ? (object) string.Empty : (object) nodeSet.Current.LocalName;
    }

    public override string ToString() => "local-name(" + this.arg0.ToString() + ")";
  }
}
