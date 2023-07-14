// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.XPathFunctionNumber
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal class XPathFunctionNumber : XPathNumericFunction
  {
    private Expression arg0;

    public XPathFunctionNumber(FunctionArguments args)
      : base(args)
    {
      if (args == null)
        return;
      this.arg0 = args.Arg;
      if (args.Tail != null)
        throw new XPathException("number takes 1 or zero args");
    }

    public override Expression Optimize()
    {
      if (this.arg0 == null)
        return (Expression) this;
      this.arg0 = this.arg0.Optimize();
      return !this.arg0.HasStaticValue ? (Expression) this : (Expression) new ExprNumber(this.StaticValueAsNumber);
    }

    public override bool HasStaticValue => this.arg0 != null && this.arg0.HasStaticValue;

    public override double StaticValueAsNumber => this.arg0 != null ? this.arg0.StaticValueAsNumber : 0.0;

    internal override bool Peer => this.arg0 == null || this.arg0.Peer;

    public override object Evaluate(BaseIterator iter) => this.arg0 == null ? (object) XPathFunctions.ToNumber(iter.Current.Value) : (object) this.arg0.EvaluateNumber(iter);

    public override string ToString() => "number(" + this.arg0.ToString() + ")";
  }
}
