// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.ExprLiteral
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal class ExprLiteral : Expression
  {
    protected string _value;

    public ExprLiteral(string value) => this._value = value;

    public string Value => this._value;

    public override string ToString() => "'" + this._value + "'";

    public override XPathResultType ReturnType => XPathResultType.String;

    internal override bool Peer => true;

    public override bool HasStaticValue => true;

    public override string StaticValueAsString => this._value;

    public override object Evaluate(BaseIterator iter) => (object) this._value;

    public override string EvaluateString(BaseIterator iter) => this._value;
  }
}
