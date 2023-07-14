// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.XPathFunctionTranslate
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Text;

namespace System.Xml.XPath
{
  internal class XPathFunctionTranslate : XPathFunction
  {
    private Expression arg0;
    private Expression arg1;
    private Expression arg2;

    public XPathFunctionTranslate(FunctionArguments args)
      : base(args)
    {
      this.arg0 = args != null && args.Tail != null && args.Tail.Tail != null && args.Tail.Tail.Tail == null ? args.Arg : throw new XPathException("translate takes 3 args");
      this.arg1 = args.Tail.Arg;
      this.arg2 = args.Tail.Tail.Arg;
    }

    public override XPathResultType ReturnType => XPathResultType.String;

    internal override bool Peer => this.arg0.Peer && this.arg1.Peer && this.arg2.Peer;

    public override object Evaluate(BaseIterator iter)
    {
      string str1 = this.arg0.EvaluateString(iter);
      string str2 = this.arg1.EvaluateString(iter);
      string str3 = this.arg2.EvaluateString(iter);
      StringBuilder stringBuilder = new StringBuilder(str1.Length);
      int index1 = 0;
      int length1 = str1.Length;
      int length2 = str3.Length;
      for (; index1 < length1; ++index1)
      {
        int index2 = str2.IndexOf(str1[index1]);
        if (index2 != -1)
        {
          if (index2 < length2)
            stringBuilder.Append(str3[index2]);
        }
        else
          stringBuilder.Append(str1[index1]);
      }
      return (object) stringBuilder.ToString();
    }

    public override string ToString() => "string-length(" + this.arg0.ToString() + "," + this.arg1.ToString() + "," + this.arg2.ToString() + ")";
  }
}
