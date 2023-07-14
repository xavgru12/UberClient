// Decompiled with JetBrains decompiler
// Type: Mono.Xml.XPath.KeyPattern
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml.Xsl;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.XPath
{
  internal class KeyPattern : LocationPathPattern
  {
    private XsltKey key;

    public KeyPattern(XsltKey key)
      : base((NodeTest) null)
    {
      this.key = key;
    }

    public override bool Matches(XPathNavigator node, XsltContext ctx) => this.key.PatternMatches(node, ctx);

    public override double DefaultPriority => 0.5;
  }
}
