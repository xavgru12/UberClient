// Decompiled with JetBrains decompiler
// Type: Mono.Xml.XPath.UnionPattern
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.XPath
{
  internal class UnionPattern : Pattern
  {
    public readonly Pattern p0;
    public readonly Pattern p1;

    public UnionPattern(Pattern p0, Pattern p1)
    {
      this.p0 = p0;
      this.p1 = p1;
    }

    public override XPathNodeType EvaluatedNodeType => this.p0.EvaluatedNodeType == this.p1.EvaluatedNodeType ? this.p0.EvaluatedNodeType : XPathNodeType.All;

    public override bool Matches(XPathNavigator node, XsltContext ctx) => this.p0.Matches(node, ctx) || this.p1.Matches(node, ctx);

    public override string ToString() => this.p0.ToString() + " | " + this.p1.ToString();
  }
}
