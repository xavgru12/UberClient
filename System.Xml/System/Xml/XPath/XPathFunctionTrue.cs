// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.XPathFunctionTrue
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal class XPathFunctionTrue : XPathBooleanFunction
  {
    public XPathFunctionTrue(FunctionArguments args)
      : base(args)
    {
      if (args != null)
        throw new XPathException("true takes 0 args");
    }

    public override bool HasStaticValue => true;

    public override bool StaticValueAsBoolean => true;

    internal override bool Peer => true;

    public override object Evaluate(BaseIterator iter) => (object) true;

    public override string ToString() => "true()";
  }
}
