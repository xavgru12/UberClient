// Decompiled with JetBrains decompiler
// Type: Mono.Xml.XPath.IdPattern
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml.Xsl;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.XPath
{
  internal class IdPattern : LocationPathPattern
  {
    private string[] ids;

    public IdPattern(string arg0)
      : base((NodeTest) null)
    {
      this.ids = arg0.Split(XmlChar.WhitespaceChars);
    }

    public override XPathNodeType EvaluatedNodeType => XPathNodeType.Element;

    public override bool Matches(XPathNavigator node, XsltContext ctx)
    {
      XPathNavigator navCache = ((XsltCompiledContext) ctx).GetNavCache((Pattern) this, node);
      for (int index = 0; index < this.ids.Length; ++index)
      {
        if (navCache.MoveToId(this.ids[index]) && navCache.IsSamePosition(node))
          return true;
      }
      return false;
    }

    public override double DefaultPriority => 0.5;
  }
}
