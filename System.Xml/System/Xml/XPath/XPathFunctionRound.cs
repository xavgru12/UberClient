// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.XPathFunctionRound
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal class XPathFunctionRound : XPathNumericFunction
  {
    private Expression arg0;

    public XPathFunctionRound(FunctionArguments args)
      : base(args)
    {
      this.arg0 = args != null && args.Tail == null ? args.Arg : throw new XPathException("round takes one arg");
    }

    public override bool HasStaticValue => this.arg0.HasStaticValue;

    public override double StaticValueAsNumber => this.HasStaticValue ? this.Round(this.arg0.StaticValueAsNumber) : 0.0;

    internal override bool Peer => this.arg0.Peer;

    public override object Evaluate(BaseIterator iter) => (object) this.Round(this.arg0.EvaluateNumber(iter));

    private double Round(double arg) => arg < -0.5 || arg > 0.0 ? Math.Floor(arg + 0.5) : Math.Round(arg);

    public override string ToString() => "round(" + this.arg0.ToString() + ")";
  }
}
