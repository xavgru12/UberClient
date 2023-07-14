// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.ExprNumber
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal class ExprNumber : Expression
  {
    protected double _value;

    public ExprNumber(double value) => this._value = value;

    public override string ToString() => this._value.ToString();

    public override XPathResultType ReturnType => XPathResultType.Number;

    internal override bool Peer => true;

    public override bool HasStaticValue => true;

    public override double StaticValueAsNumber => XPathFunctions.ToNumber((object) this._value);

    public override object Evaluate(BaseIterator iter) => (object) this._value;

    public override double EvaluateNumber(BaseIterator iter) => this._value;

    internal override bool IsPositional => false;
  }
}
