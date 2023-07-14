// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.XPathFunctionSubstring
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal class XPathFunctionSubstring : XPathFunction
  {
    private Expression arg0;
    private Expression arg1;
    private Expression arg2;

    public XPathFunctionSubstring(FunctionArguments args)
      : base(args)
    {
      this.arg0 = args != null && args.Tail != null && (args.Tail.Tail == null || args.Tail.Tail.Tail == null) ? args.Arg : throw new XPathException("substring takes 2 or 3 args");
      this.arg1 = args.Tail.Arg;
      if (args.Tail.Tail == null)
        return;
      this.arg2 = args.Tail.Tail.Arg;
    }

    public override XPathResultType ReturnType => XPathResultType.String;

    internal override bool Peer
    {
      get
      {
        if (!this.arg0.Peer || !this.arg1.Peer)
          return false;
        return this.arg2 == null || this.arg2.Peer;
      }
    }

    public override object Evaluate(BaseIterator iter)
    {
      string str = this.arg0.EvaluateString(iter);
      double num1 = Math.Round(this.arg1.EvaluateNumber(iter)) - 1.0;
      if (double.IsNaN(num1) || double.IsNegativeInfinity(num1) || num1 >= (double) str.Length)
        return (object) string.Empty;
      if (this.arg2 == null)
      {
        if (num1 < 0.0)
          num1 = 0.0;
        return (object) str.Substring((int) num1);
      }
      double num2 = Math.Round(this.arg2.EvaluateNumber(iter));
      if (double.IsNaN(num2))
        return (object) string.Empty;
      if (num1 < 0.0 || num2 < 0.0)
      {
        num2 = num1 + num2;
        if (num2 <= 0.0)
          return (object) string.Empty;
        num1 = 0.0;
      }
      double num3 = (double) str.Length - num1;
      if (num2 > num3)
        num2 = num3;
      return (object) str.Substring((int) num1, (int) num2);
    }

    public override string ToString() => "substring(" + this.arg0.ToString() + "," + this.arg1.ToString() + "," + this.arg2.ToString() + ")";
  }
}
