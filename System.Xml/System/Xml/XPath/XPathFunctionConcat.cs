// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.XPathFunctionConcat
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Globalization;
using System.Text;

namespace System.Xml.XPath
{
  internal class XPathFunctionConcat : XPathFunction
  {
    private ArrayList rgs;

    public XPathFunctionConcat(FunctionArguments args)
      : base(args)
    {
      if (args == null || args.Tail == null)
        throw new XPathException("concat takes 2 or more args");
      args.ToArrayList(this.rgs = new ArrayList());
    }

    public override XPathResultType ReturnType => XPathResultType.String;

    internal override bool Peer
    {
      get
      {
        for (int index = 0; index < this.rgs.Count; ++index)
        {
          if (!((Expression) this.rgs[index]).Peer)
            return false;
        }
        return true;
      }
    }

    public override object Evaluate(BaseIterator iter)
    {
      StringBuilder stringBuilder = new StringBuilder();
      int count = this.rgs.Count;
      for (int index = 0; index < count; ++index)
        stringBuilder.Append(((Expression) this.rgs[index]).EvaluateString(iter));
      return (object) stringBuilder.ToString();
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("concat(");
      for (int index = 0; index < this.rgs.Count - 1; ++index)
      {
        stringBuilder.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, "{0}", (object) this.rgs[index].ToString());
        stringBuilder.Append(',');
      }
      stringBuilder.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, "{0}", (object) this.rgs[this.rgs.Count - 1].ToString());
      stringBuilder.Append(')');
      return stringBuilder.ToString();
    }
  }
}
