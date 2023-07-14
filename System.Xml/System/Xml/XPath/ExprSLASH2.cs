// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.ExprSLASH2
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal class ExprSLASH2 : NodeSet
  {
    public Expression left;
    public NodeSet right;
    private static NodeTest DescendantOrSelfStar = (NodeTest) new NodeTypeTest(Axes.DescendantOrSelf, XPathNodeType.All);

    public ExprSLASH2(Expression left, NodeSet right)
    {
      this.left = left;
      this.right = right;
    }

    public override Expression Optimize()
    {
      this.left = this.left.Optimize();
      this.right = (NodeSet) this.right.Optimize();
      if (this.right is NodeTest right && right.Axis.Axis == Axes.Child)
      {
        switch (right)
        {
          case NodeNameTest source:
            return (Expression) new ExprSLASH(this.left, (NodeSet) new NodeNameTest(source, Axes.Descendant));
          case NodeTypeTest other:
            return (Expression) new ExprSLASH(this.left, (NodeSet) new NodeTypeTest(other, Axes.Descendant));
        }
      }
      return (Expression) this;
    }

    public override string ToString() => this.left.ToString() + "//" + this.right.ToString();

    public override object Evaluate(BaseIterator iter)
    {
      BaseIterator nodeSet = this.left.EvaluateNodeSet(iter);
      BaseIterator iter1;
      if (this.left.Peer && !this.left.RequireSorting)
      {
        iter1 = (BaseIterator) new SimpleSlashIterator(nodeSet, (NodeSet) ExprSLASH2.DescendantOrSelfStar);
      }
      else
      {
        BaseIterator iter2 = (BaseIterator) new SlashIterator(nodeSet, (NodeSet) ExprSLASH2.DescendantOrSelfStar);
        iter1 = !this.left.RequireSorting ? iter2 : (BaseIterator) new SortedIterator(iter2);
      }
      return (object) new SortedIterator((BaseIterator) new SlashIterator(iter1, this.right));
    }

    public override bool RequireSorting => this.left.RequireSorting || this.right.RequireSorting;

    internal override XPathNodeType EvaluatedNodeType => this.right.EvaluatedNodeType;

    internal override bool IsPositional => this.left.IsPositional || this.right.IsPositional;

    internal override bool Peer => false;

    internal override bool Subtree => this.left is NodeSet left && left.Subtree && this.right.Subtree;
  }
}
