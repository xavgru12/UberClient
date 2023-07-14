// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.NodeTest
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal abstract class NodeTest : NodeSet
  {
    protected AxisSpecifier _axis;

    public NodeTest(Axes axis) => this._axis = new AxisSpecifier(axis);

    public abstract bool Match(IXmlNamespaceResolver nsm, XPathNavigator nav);

    public AxisSpecifier Axis => this._axis;

    public override object Evaluate(BaseIterator iter) => (object) new AxisIterator(this._axis.Evaluate(iter), this);

    public abstract void GetInfo(
      out string name,
      out string ns,
      out XPathNodeType nodetype,
      IXmlNamespaceResolver nsm);

    public override bool RequireSorting
    {
      get
      {
        switch (this._axis.Axis)
        {
          case Axes.Ancestor:
          case Axes.AncestorOrSelf:
          case Axes.Attribute:
          case Axes.Namespace:
          case Axes.Preceding:
          case Axes.PrecedingSibling:
            return true;
          default:
            return false;
        }
      }
    }

    internal override bool Peer
    {
      get
      {
        switch (this._axis.Axis)
        {
          case Axes.Ancestor:
          case Axes.AncestorOrSelf:
          case Axes.Descendant:
          case Axes.DescendantOrSelf:
          case Axes.Following:
          case Axes.Preceding:
            return false;
          default:
            return true;
        }
      }
    }

    internal override bool Subtree
    {
      get
      {
        switch (this._axis.Axis)
        {
          case Axes.Ancestor:
          case Axes.AncestorOrSelf:
          case Axes.Following:
          case Axes.FollowingSibling:
          case Axes.Parent:
          case Axes.Preceding:
          case Axes.PrecedingSibling:
            return false;
          default:
            return true;
        }
      }
    }

    internal override XPathNodeType EvaluatedNodeType => this._axis.NodeType;
  }
}
