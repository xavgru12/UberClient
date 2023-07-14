// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.ExprParens
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal class ExprParens : Expression
  {
    protected Expression _expr;

    public ExprParens(Expression expr) => this._expr = expr;

    public override Expression Optimize()
    {
      this._expr.Optimize();
      return (Expression) this;
    }

    public override bool HasStaticValue => this._expr.HasStaticValue;

    public override object StaticValue => this._expr.StaticValue;

    public override string StaticValueAsString => this._expr.StaticValueAsString;

    public override double StaticValueAsNumber => this._expr.StaticValueAsNumber;

    public override bool StaticValueAsBoolean => this._expr.StaticValueAsBoolean;

    public override string ToString() => "(" + this._expr.ToString() + ")";

    public override XPathResultType ReturnType => this._expr.ReturnType;

    public override object Evaluate(BaseIterator iter)
    {
      object obj = this._expr.Evaluate(iter);
      XPathNodeIterator iter1 = obj as XPathNodeIterator;
      switch (iter1)
      {
        case BaseIterator iter2:
        case null:
          return iter2 != null ? (object) new ParensIterator(iter2) : obj;
        default:
          iter2 = (BaseIterator) new WrapperIterator(iter1, iter.NamespaceManager);
          goto case null;
      }
    }

    internal override XPathNodeType EvaluatedNodeType => this._expr.EvaluatedNodeType;

    internal override bool IsPositional => this._expr.IsPositional;

    internal override bool Peer => this._expr.Peer;
  }
}
