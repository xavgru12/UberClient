// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.ExprFilter
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal class ExprFilter : NodeSet
  {
    internal Expression expr;
    internal Expression pred;

    public ExprFilter(Expression expr, Expression pred)
    {
      this.expr = expr;
      this.pred = pred;
    }

    public override Expression Optimize()
    {
      this.expr = this.expr.Optimize();
      this.pred = this.pred.Optimize();
      return (Expression) this;
    }

    internal Expression LeftHandSide => this.expr;

    public override string ToString() => "(" + this.expr.ToString() + ")[" + this.pred.ToString() + "]";

    public override object Evaluate(BaseIterator iter) => (object) new PredicateIterator(this.expr.EvaluateNodeSet(iter), this.pred);

    internal override XPathNodeType EvaluatedNodeType => this.expr.EvaluatedNodeType;

    internal override bool IsPositional => this.pred.ReturnType == XPathResultType.Number || this.expr.IsPositional || this.pred.IsPositional;

    internal override bool Peer => this.expr.Peer && this.pred.Peer;

    internal override bool Subtree => this.expr is NodeSet expr && expr.Subtree;
  }
}
