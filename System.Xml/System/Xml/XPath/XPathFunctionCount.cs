// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.XPathFunctionCount
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal class XPathFunctionCount : XPathFunction
  {
    private Expression arg0;

    public XPathFunctionCount(FunctionArguments args)
      : base(args)
    {
      this.arg0 = args != null && args.Tail == null ? args.Arg : throw new XPathException("count takes 1 arg");
    }

    public override XPathResultType ReturnType => XPathResultType.Number;

    internal override bool Peer => this.arg0.Peer;

    public override object Evaluate(BaseIterator iter) => (object) (double) this.arg0.EvaluateNodeSet(iter).Count;

    public override bool EvaluateBoolean(BaseIterator iter) => this.arg0.GetReturnType(iter) == XPathResultType.NodeSet ? this.arg0.EvaluateBoolean(iter) : this.arg0.EvaluateNodeSet(iter).MoveNext();

    public override string ToString() => "count(" + this.arg0.ToString() + ")";
  }
}
