// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.XPathFunctionBoolean
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal class XPathFunctionBoolean : XPathBooleanFunction
  {
    private Expression arg0;

    public XPathFunctionBoolean(FunctionArguments args)
      : base(args)
    {
      if (args == null)
        return;
      this.arg0 = args.Arg;
      if (args.Tail != null)
        throw new XPathException("boolean takes 1 or zero args");
    }

    internal override bool Peer => this.arg0 == null || this.arg0.Peer;

    public override object Evaluate(BaseIterator iter) => this.arg0 == null ? (object) XPathFunctions.ToBoolean(iter.Current.Value) : (object) this.arg0.EvaluateBoolean(iter);

    public override string ToString() => "boolean(" + this.arg0.ToString() + ")";
  }
}
