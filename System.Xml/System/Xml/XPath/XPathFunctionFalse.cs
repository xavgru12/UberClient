// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.XPathFunctionFalse
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal class XPathFunctionFalse : XPathBooleanFunction
  {
    public XPathFunctionFalse(FunctionArguments args)
      : base(args)
    {
      if (args != null)
        throw new XPathException("false takes 0 args");
    }

    public override bool HasStaticValue => true;

    public override bool StaticValueAsBoolean => false;

    internal override bool Peer => true;

    public override object Evaluate(BaseIterator iter) => (object) false;

    public override string ToString() => "false()";
  }
}
