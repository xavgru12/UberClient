// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.ExprBoolean
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal abstract class ExprBoolean : ExprBinary
  {
    public ExprBoolean(Expression left, Expression right)
      : base(left, right)
    {
    }

    public override Expression Optimize()
    {
      base.Optimize();
      if (!this.HasStaticValue)
        return (Expression) this;
      return this.StaticValueAsBoolean ? (Expression) new XPathFunctionTrue((FunctionArguments) null) : (Expression) new XPathFunctionFalse((FunctionArguments) null);
    }

    public override XPathResultType ReturnType => XPathResultType.Boolean;

    public override object Evaluate(BaseIterator iter) => (object) this.EvaluateBoolean(iter);

    public override double EvaluateNumber(BaseIterator iter) => !this.EvaluateBoolean(iter) ? 0.0 : 1.0;

    public override string EvaluateString(BaseIterator iter) => this.EvaluateBoolean(iter) ? "true" : "false";
  }
}
