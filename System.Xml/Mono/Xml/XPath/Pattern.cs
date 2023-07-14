// Decompiled with JetBrains decompiler
// Type: Mono.Xml.XPath.Pattern
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml.Xsl;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.XPath
{
  internal abstract class Pattern
  {
    internal static Pattern Compile(string s, Compiler comp) => Pattern.Compile(comp.patternParser.Compile(s));

    internal static Pattern Compile(Expression e)
    {
      switch (e)
      {
        case ExprUNION _:
          return (Pattern) new UnionPattern(Pattern.Compile(((ExprUNION) e).left), Pattern.Compile(((ExprUNION) e).right));
        case ExprRoot _:
          return (Pattern) new LocationPathPattern((NodeTest) new NodeTypeTest(Axes.Self, XPathNodeType.Root));
        case NodeTest _:
          return (Pattern) new LocationPathPattern((NodeTest) e);
        case ExprFilter _:
          return (Pattern) new LocationPathPattern((ExprFilter) e);
        case ExprSLASH _:
          Pattern prev1 = Pattern.Compile(((ExprSLASH) e).left);
          LocationPathPattern locationPathPattern1 = (LocationPathPattern) Pattern.Compile((Expression) ((ExprSLASH) e).right);
          locationPathPattern1.SetPreviousPattern(prev1, false);
          return (Pattern) locationPathPattern1;
        case ExprSLASH2 _:
          if (((ExprSLASH2) e).left is ExprRoot)
            return Pattern.Compile((Expression) ((ExprSLASH2) e).right);
          Pattern prev2 = Pattern.Compile(((ExprSLASH2) e).left);
          LocationPathPattern locationPathPattern2 = (LocationPathPattern) Pattern.Compile((Expression) ((ExprSLASH2) e).right);
          locationPathPattern2.SetPreviousPattern(prev2, true);
          return (Pattern) locationPathPattern2;
        case XPathFunctionId _:
          return (Pattern) new IdPattern((((XPathFunctionId) e).Id as ExprLiteral).Value);
        case XsltKey _:
          return (Pattern) new KeyPattern((XsltKey) e);
        default:
          return (Pattern) null;
      }
    }

    public virtual double DefaultPriority => 0.5;

    public virtual XPathNodeType EvaluatedNodeType => XPathNodeType.All;

    public abstract bool Matches(XPathNavigator node, XsltContext ctx);
  }
}
