// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.XPathFunctionSubstringAfter
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal class XPathFunctionSubstringAfter : XPathFunction
  {
    private Expression arg0;
    private Expression arg1;

    public XPathFunctionSubstringAfter(FunctionArguments args)
      : base(args)
    {
      this.arg0 = args != null && args.Tail != null && args.Tail.Tail == null ? args.Arg : throw new XPathException("substring-after takes 2 args");
      this.arg1 = args.Tail.Arg;
    }

    public override XPathResultType ReturnType => XPathResultType.String;

    internal override bool Peer => this.arg0.Peer && this.arg1.Peer;

    public override object Evaluate(BaseIterator iter)
    {
      string str1 = this.arg0.EvaluateString(iter);
      string str2 = this.arg1.EvaluateString(iter);
      int num = str1.IndexOf(str2);
      return num < 0 ? (object) string.Empty : (object) str1.Substring(num + str2.Length);
    }

    public override string ToString() => "substring-after(" + this.arg0.ToString() + "," + this.arg1.ToString() + ")";
  }
}
