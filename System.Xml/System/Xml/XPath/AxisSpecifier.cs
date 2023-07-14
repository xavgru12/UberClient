// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.AxisSpecifier
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal class AxisSpecifier
  {
    protected Axes _axis;

    public AxisSpecifier(Axes axis) => this._axis = axis;

    public XPathNodeType NodeType
    {
      get
      {
        switch (this._axis)
        {
          case Axes.Attribute:
            return XPathNodeType.Attribute;
          case Axes.Namespace:
            return XPathNodeType.Namespace;
          default:
            return XPathNodeType.Element;
        }
      }
    }

    public override string ToString()
    {
      switch (this._axis)
      {
        case Axes.Ancestor:
          return "ancestor";
        case Axes.AncestorOrSelf:
          return "ancestor-or-self";
        case Axes.Attribute:
          return "attribute";
        case Axes.Child:
          return "child";
        case Axes.Descendant:
          return "descendant";
        case Axes.DescendantOrSelf:
          return "descendant-or-self";
        case Axes.Following:
          return "following";
        case Axes.FollowingSibling:
          return "following-sibling";
        case Axes.Namespace:
          return "namespace";
        case Axes.Parent:
          return "parent";
        case Axes.Preceding:
          return "preceding";
        case Axes.PrecedingSibling:
          return "preceding-sibling";
        case Axes.Self:
          return "self";
        default:
          throw new IndexOutOfRangeException();
      }
    }

    public Axes Axis => this._axis;

    public BaseIterator Evaluate(BaseIterator iter)
    {
      switch (this._axis)
      {
        case Axes.Ancestor:
          return (BaseIterator) new AncestorIterator(iter);
        case Axes.AncestorOrSelf:
          return (BaseIterator) new AncestorOrSelfIterator(iter);
        case Axes.Attribute:
          return (BaseIterator) new AttributeIterator(iter);
        case Axes.Child:
          return (BaseIterator) new ChildIterator(iter);
        case Axes.Descendant:
          return (BaseIterator) new DescendantIterator(iter);
        case Axes.DescendantOrSelf:
          return (BaseIterator) new DescendantOrSelfIterator(iter);
        case Axes.Following:
          return (BaseIterator) new FollowingIterator(iter);
        case Axes.FollowingSibling:
          return (BaseIterator) new FollowingSiblingIterator(iter);
        case Axes.Namespace:
          return (BaseIterator) new NamespaceIterator(iter);
        case Axes.Parent:
          return (BaseIterator) new ParentIterator(iter);
        case Axes.Preceding:
          return (BaseIterator) new PrecedingIterator(iter);
        case Axes.PrecedingSibling:
          return (BaseIterator) new PrecedingSiblingIterator(iter);
        case Axes.Self:
          return (BaseIterator) new SelfIterator(iter);
        default:
          throw new IndexOutOfRangeException();
      }
    }
  }
}
