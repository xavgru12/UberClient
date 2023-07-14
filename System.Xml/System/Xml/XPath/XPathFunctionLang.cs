// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.XPathFunctionLang
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Globalization;

namespace System.Xml.XPath
{
  internal class XPathFunctionLang : XPathFunction
  {
    private Expression arg0;

    public XPathFunctionLang(FunctionArguments args)
      : base(args)
    {
      this.arg0 = args != null && args.Tail == null ? args.Arg : throw new XPathException("lang takes one arg");
    }

    public override XPathResultType ReturnType => XPathResultType.Boolean;

    internal override bool Peer => this.arg0.Peer;

    public override object Evaluate(BaseIterator iter) => (object) this.EvaluateBoolean(iter);

    public override bool EvaluateBoolean(BaseIterator iter)
    {
      string lower1 = this.arg0.EvaluateString(iter).ToLower(CultureInfo.InvariantCulture);
      string lower2 = iter.Current.XmlLang.ToLower(CultureInfo.InvariantCulture);
      if (lower1 == lower2)
        return true;
      return lower1 == lower2.Split('-')[0];
    }

    public override string ToString() => "lang(" + this.arg0.ToString() + ")";
  }
}
