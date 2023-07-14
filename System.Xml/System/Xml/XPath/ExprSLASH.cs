// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.ExprSLASH
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal class ExprSLASH : NodeSet
  {
    public Expression left;
    public NodeSet right;

    public ExprSLASH(Expression left, NodeSet right)
    {
      this.left = left;
      this.right = right;
    }

    public override Expression Optimize()
    {
      this.left = this.left.Optimize();
      this.right = (NodeSet) this.right.Optimize();
      return (Expression) this;
    }

    public override string ToString() => this.left.ToString() + "/" + this.right.ToString();

    public override object Evaluate(BaseIterator iter)
    {
      BaseIterator nodeSet = this.left.EvaluateNodeSet(iter);
      return this.left.Peer && this.right.Subtree ? (object) new SimpleSlashIterator(nodeSet, this.right) : (object) new SortedIterator((BaseIterator) new SlashIterator(nodeSet, this.right));
    }

    public override bool RequireSorting => this.left.RequireSorting || this.right.RequireSorting;

    internal override XPathNodeType EvaluatedNodeType => this.right.EvaluatedNodeType;

    internal override bool IsPositional => this.left.IsPositional || this.right.IsPositional;

    internal override bool Peer => this.left.Peer && this.right.Peer;

    internal override bool Subtree => this.left is NodeSet left && left.Subtree && this.right.Subtree;
  }
}
