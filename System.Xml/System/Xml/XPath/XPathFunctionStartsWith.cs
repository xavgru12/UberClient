// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.XPathFunctionStartsWith
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal class XPathFunctionStartsWith : XPathFunction
  {
    private Expression arg0;
    private Expression arg1;

    public XPathFunctionStartsWith(FunctionArguments args)
      : base(args)
    {
      this.arg0 = args != null && args.Tail != null && args.Tail.Tail == null ? args.Arg : throw new XPathException("starts-with takes 2 args");
      this.arg1 = args.Tail.Arg;
    }

    public override XPathResultType ReturnType => XPathResultType.Boolean;

    internal override bool Peer => this.arg0.Peer && this.arg1.Peer;

    public override object Evaluate(BaseIterator iter) => (object) this.arg0.EvaluateString(iter).StartsWith(this.arg1.EvaluateString(iter));

    public override string ToString() => "starts-with(" + this.arg0.ToString() + "," + this.arg1.ToString() + ")";
  }
}
