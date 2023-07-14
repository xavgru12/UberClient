// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.ExprBinary
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal abstract class ExprBinary : Expression
  {
    protected Expression _left;
    protected Expression _right;

    public ExprBinary(Expression left, Expression right)
    {
      this._left = left;
      this._right = right;
    }

    public override Expression Optimize()
    {
      this._left = this._left.Optimize();
      this._right = this._right.Optimize();
      return (Expression) this;
    }

    public override bool HasStaticValue => this._left.HasStaticValue && this._right.HasStaticValue;

    public override string ToString() => this._left.ToString() + (object) ' ' + this.Operator + (object) ' ' + this._right.ToString();

    protected abstract string Operator { get; }

    internal override XPathNodeType EvaluatedNodeType => this._left.EvaluatedNodeType == this._right.EvaluatedNodeType ? this._left.EvaluatedNodeType : XPathNodeType.All;

    internal override bool IsPositional => this._left.IsPositional || this._right.IsPositional;

    internal override bool Peer => this._left.Peer && this._right.Peer;
  }
}
