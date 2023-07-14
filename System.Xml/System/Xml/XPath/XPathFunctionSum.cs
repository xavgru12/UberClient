// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.XPathFunctionSum
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal class XPathFunctionSum : XPathNumericFunction
  {
    private Expression arg0;

    public XPathFunctionSum(FunctionArguments args)
      : base(args)
    {
      this.arg0 = args != null && args.Tail == null ? args.Arg : throw new XPathException("sum takes one arg");
    }

    internal override bool Peer => this.arg0.Peer;

    public override object Evaluate(BaseIterator iter)
    {
      XPathNodeIterator nodeSet = (XPathNodeIterator) this.arg0.EvaluateNodeSet(iter);
      double num = 0.0;
      while (nodeSet.MoveNext())
        num += XPathFunctions.ToNumber(nodeSet.Current.Value);
      return (object) num;
    }

    public override string ToString() => "sum(" + this.arg0.ToString() + ")";
  }
}
