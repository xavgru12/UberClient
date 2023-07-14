// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.XsltGenerateId
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace Mono.Xml.Xsl
{
  internal class XsltGenerateId : XPathFunction
  {
    private Expression arg0;

    public XsltGenerateId(FunctionArguments args)
      : base(args)
    {
      if (args == null)
        return;
      this.arg0 = args.Tail == null ? args.Arg : throw new XPathException("generate-id takes 1 or no args");
    }

    public override XPathResultType ReturnType => XPathResultType.String;

    internal override bool Peer => this.arg0.Peer;

    public override object Evaluate(BaseIterator iter)
    {
      XPathNavigator nav;
      if (this.arg0 != null)
      {
        XPathNodeIterator nodeSet = (XPathNodeIterator) this.arg0.EvaluateNodeSet(iter);
        if (!nodeSet.MoveNext())
          return (object) string.Empty;
        nav = nodeSet.Current.Clone();
      }
      else
        nav = iter.Current.Clone();
      StringBuilder stringBuilder = new StringBuilder("Mono");
      stringBuilder.Append(XmlConvert.EncodeLocalName(nav.BaseURI));
      stringBuilder.Replace('_', 'm');
      stringBuilder.Append((object) nav.NodeType);
      stringBuilder.Append('m');
      do
      {
        stringBuilder.Append(this.IndexInParent(nav));
        stringBuilder.Append('m');
      }
      while (nav.MoveToParent());
      return (object) stringBuilder.ToString();
    }

    private int IndexInParent(XPathNavigator nav)
    {
      int num = 0;
      while (nav.MoveToPrevious())
        ++num;
      return num;
    }
  }
}
