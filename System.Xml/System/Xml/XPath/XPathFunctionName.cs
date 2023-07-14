// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.XPathFunctionName
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal class XPathFunctionName : XPathFunction
  {
    private Expression arg0;

    public XPathFunctionName(FunctionArguments args)
      : base(args)
    {
      if (args == null)
        return;
      this.arg0 = args.Arg;
      if (args.Tail != null)
        throw new XPathException("name takes 1 or zero args");
    }

    public override XPathResultType ReturnType => XPathResultType.String;

    internal override bool Peer => this.arg0 == null || this.arg0.Peer;

    public override object Evaluate(BaseIterator iter)
    {
      if (this.arg0 == null)
        return (object) iter.Current.Name;
      BaseIterator nodeSet = this.arg0.EvaluateNodeSet(iter);
      return nodeSet == null || !nodeSet.MoveNext() ? (object) string.Empty : (object) nodeSet.Current.Name;
    }

    public override string ToString() => "name(" + (this.arg0 == null ? string.Empty : this.arg0.ToString()) + ")";
  }
}
