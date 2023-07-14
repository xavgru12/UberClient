// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.ExprPLUS
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal class ExprPLUS : ExprNumeric
  {
    public ExprPLUS(Expression left, Expression right)
      : base(left, right)
    {
    }

    protected override string Operator => "+";

    public override double StaticValueAsNumber => this.HasStaticValue ? this._left.StaticValueAsNumber + this._right.StaticValueAsNumber : 0.0;

    public override double EvaluateNumber(BaseIterator iter) => this._left.EvaluateNumber(iter) + this._right.EvaluateNumber(iter);
  }
}
