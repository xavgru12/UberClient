// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.XPathFunctionId
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;

namespace System.Xml.XPath
{
  internal class XPathFunctionId : XPathFunction
  {
    private Expression arg0;
    private static char[] rgchWhitespace = new char[4]
    {
      ' ',
      '\t',
      '\r',
      '\n'
    };

    public XPathFunctionId(FunctionArguments args)
      : base(args)
    {
      this.arg0 = args != null && args.Tail == null ? args.Arg : throw new XPathException("id takes 1 arg");
    }

    public Expression Id => this.arg0;

    public override XPathResultType ReturnType => XPathResultType.NodeSet;

    internal override bool Peer => this.arg0.Peer;

    public override object Evaluate(BaseIterator iter)
    {
      object obj = this.arg0.Evaluate(iter);
      string str;
      if (obj is XPathNodeIterator xpathNodeIterator)
      {
        str = string.Empty;
        while (xpathNodeIterator.MoveNext())
          str = str + xpathNodeIterator.Current.Value + " ";
      }
      else
        str = XPathFunctions.ToString(obj);
      XPathNavigator xpathNavigator = iter.Current.Clone();
      ArrayList arrayList = new ArrayList();
      foreach (string id in str.Split(XPathFunctionId.rgchWhitespace))
      {
        if (xpathNavigator.MoveToId(id))
          arrayList.Add((object) xpathNavigator.Clone());
      }
      arrayList.Sort((IComparer) XPathNavigatorComparer.Instance);
      return (object) new ListIterator(iter, (IList) arrayList);
    }

    public override string ToString() => "id(" + this.arg0.ToString() + ")";
  }
}
