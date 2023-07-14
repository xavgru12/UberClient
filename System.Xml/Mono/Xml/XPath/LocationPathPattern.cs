// Decompiled with JetBrains decompiler
// Type: Mono.Xml.XPath.LocationPathPattern
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml.Xsl;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.XPath
{
  internal class LocationPathPattern : Pattern
  {
    private LocationPathPattern patternPrevious;
    private bool isAncestor;
    private NodeTest nodeTest;
    private ExprFilter filter;

    public LocationPathPattern(NodeTest nodeTest) => this.nodeTest = nodeTest;

    public LocationPathPattern(ExprFilter filter)
    {
      this.filter = filter;
      while (!(filter.expr is NodeTest))
        filter = (ExprFilter) filter.expr;
      this.nodeTest = (NodeTest) filter.expr;
    }

    internal void SetPreviousPattern(Pattern prev, bool isAncestor)
    {
      LocationPathPattern lastPathPattern = this.LastPathPattern;
      lastPathPattern.patternPrevious = (LocationPathPattern) prev;
      lastPathPattern.isAncestor = isAncestor;
    }

    public override double DefaultPriority
    {
      get
      {
        if (this.patternPrevious != null || this.filter != null)
          return 0.5;
        if (!(this.nodeTest is NodeNameTest nodeTest))
          return -0.5;
        return nodeTest.Name.Name == "*" || nodeTest.Name.Name.Length == 0 ? -0.25 : 0.0;
      }
    }

    public override XPathNodeType EvaluatedNodeType => this.nodeTest.EvaluatedNodeType;

    public override bool Matches(XPathNavigator node, XsltContext ctx)
    {
      if (!this.nodeTest.Match((IXmlNamespaceResolver) ctx, node) || this.nodeTest is NodeTypeTest && ((NodeTypeTest) this.nodeTest).type == XPathNodeType.All && (node.NodeType == XPathNodeType.Root || node.NodeType == XPathNodeType.Attribute))
        return false;
      if (this.filter == null && this.patternPrevious == null)
        return true;
      if (this.patternPrevious != null)
      {
        XPathNavigator navCache = ((XsltCompiledContext) ctx).GetNavCache((Pattern) this, node);
        if (!this.isAncestor)
        {
          navCache.MoveToParent();
          if (!this.patternPrevious.Matches(navCache, ctx))
            return false;
        }
        else
        {
          while (navCache.MoveToParent())
          {
            if (this.patternPrevious.Matches(navCache, ctx))
              goto label_11;
          }
          return false;
        }
      }
label_11:
      if (this.filter == null)
        return true;
      if (!this.filter.IsPositional && !(this.filter.expr is ExprFilter))
        return this.filter.pred.EvaluateBoolean((BaseIterator) new NullIterator(node, (IXmlNamespaceResolver) ctx));
      XPathNavigator navCache1 = ((XsltCompiledContext) ctx).GetNavCache((Pattern) this, node);
      navCache1.MoveToParent();
      BaseIterator nodeSet = this.filter.EvaluateNodeSet((BaseIterator) new NullIterator(navCache1, (IXmlNamespaceResolver) ctx));
      while (nodeSet.MoveNext())
      {
        if (node.IsSamePosition(nodeSet.Current))
          return true;
      }
      return false;
    }

    public override string ToString()
    {
      string str = string.Empty;
      if (this.patternPrevious != null)
        str = this.patternPrevious.ToString() + (!this.isAncestor ? "/" : "//");
      return this.filter == null ? str + this.nodeTest.ToString() : str + this.filter.ToString();
    }

    public LocationPathPattern LastPathPattern
    {
      get
      {
        LocationPathPattern lastPathPattern = this;
        while (lastPathPattern.patternPrevious != null)
          lastPathPattern = lastPathPattern.patternPrevious;
        return lastPathPattern;
      }
    }
  }
}
