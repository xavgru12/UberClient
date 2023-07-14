// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.XPathFunctionNot
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal class XPathFunctionNot : XPathBooleanFunction
  {
    private Expression arg0;

    public XPathFunctionNot(FunctionArguments args)
      : base(args)
    {
      this.arg0 = args != null && args.Tail == null ? args.Arg : throw new XPathException("not takes one arg");
    }

    internal override bool Peer => this.arg0.Peer;

    public override object Evaluate(BaseIterator iter) => (object) !this.arg0.EvaluateBoolean(iter);

    public override string ToString() => "not(" + this.arg0.ToString() + ")";
  }
}
