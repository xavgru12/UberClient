// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.ExprAND
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal class ExprAND : ExprBoolean
  {
    public ExprAND(Expression left, Expression right)
      : base(left, right)
    {
    }

    protected override string Operator => "and";

    public override bool StaticValueAsBoolean => this.HasStaticValue && this._left.StaticValueAsBoolean && this._right.StaticValueAsBoolean;

    public override bool EvaluateBoolean(BaseIterator iter) => this._left.EvaluateBoolean(iter) && this._right.EvaluateBoolean(iter);
  }
}
