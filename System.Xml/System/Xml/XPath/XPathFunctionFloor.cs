// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.XPathFunctionFloor
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal class XPathFunctionFloor : XPathNumericFunction
  {
    private Expression arg0;

    public XPathFunctionFloor(FunctionArguments args)
      : base(args)
    {
      this.arg0 = args != null && args.Tail == null ? args.Arg : throw new XPathException("floor takes one arg");
    }

    public override bool HasStaticValue => this.arg0.HasStaticValue;

    public override double StaticValueAsNumber => this.HasStaticValue ? Math.Floor(this.arg0.StaticValueAsNumber) : 0.0;

    internal override bool Peer => this.arg0.Peer;

    public override object Evaluate(BaseIterator iter) => (object) Math.Floor(this.arg0.EvaluateNumber(iter));

    public override string ToString() => "floor(" + this.arg0.ToString() + ")";
  }
}
