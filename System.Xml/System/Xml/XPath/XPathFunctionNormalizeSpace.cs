// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.XPathFunctionNormalizeSpace
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Text;

namespace System.Xml.XPath
{
  internal class XPathFunctionNormalizeSpace : XPathFunction
  {
    private Expression arg0;

    public XPathFunctionNormalizeSpace(FunctionArguments args)
      : base(args)
    {
      if (args == null)
        return;
      this.arg0 = args.Arg;
      if (args.Tail != null)
        throw new XPathException("normalize-space takes 1 or zero args");
    }

    public override XPathResultType ReturnType => XPathResultType.String;

    internal override bool Peer => this.arg0 == null || this.arg0.Peer;

    public override object Evaluate(BaseIterator iter)
    {
      string str = this.arg0 == null ? iter.Current.Value : this.arg0.EvaluateString(iter);
      StringBuilder stringBuilder = new StringBuilder();
      bool flag = false;
      for (int index = 0; index < str.Length; ++index)
      {
        char ch = str[index];
        switch (ch)
        {
          case '\t':
          case '\n':
          case '\r':
          case ' ':
            flag = true;
            break;
          default:
            if (flag)
            {
              flag = false;
              if (stringBuilder.Length > 0)
                stringBuilder.Append(' ');
            }
            stringBuilder.Append(ch);
            break;
        }
      }
      return (object) stringBuilder.ToString();
    }

    public override string ToString() => "normalize-space(" + (this.arg0 == null ? string.Empty : this.arg0.ToString()) + ")";
  }
}
