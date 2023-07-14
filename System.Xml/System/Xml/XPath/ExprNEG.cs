// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.ExprNEG
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal class ExprNEG : Expression
  {
    private Expression _expr;

    public ExprNEG(Expression expr) => this._expr = expr;

    public override string ToString() => "- " + this._expr.ToString();

    public override XPathResultType ReturnType => XPathResultType.Number;

    public override Expression Optimize()
    {
      this._expr = this._expr.Optimize();
      return !this.HasStaticValue ? (Expression) this : (Expression) new ExprNumber(this.StaticValueAsNumber);
    }

    internal override bool Peer => this._expr.Peer;

    public override bool HasStaticValue => this._expr.HasStaticValue;

    public override double StaticValueAsNumber => this._expr.HasStaticValue ? -1.0 * this._expr.StaticValueAsNumber : 0.0;

    public override object Evaluate(BaseIterator iter) => (object) -this._expr.EvaluateNumber(iter);

    public override double EvaluateNumber(BaseIterator iter) => -this._expr.EvaluateNumber(iter);

    internal override bool IsPositional => this._expr.IsPositional;
  }
}
