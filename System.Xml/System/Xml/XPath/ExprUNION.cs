// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.ExprUNION
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal class ExprUNION : NodeSet
  {
    internal Expression left;
    internal Expression right;

    public ExprUNION(Expression left, Expression right)
    {
      this.left = left;
      this.right = right;
    }

    public override Expression Optimize()
    {
      this.left = this.left.Optimize();
      this.right = this.right.Optimize();
      return (Expression) this;
    }

    public override string ToString() => this.left.ToString() + " | " + this.right.ToString();

    public override object Evaluate(BaseIterator iter)
    {
      BaseIterator nodeSet1 = this.left.EvaluateNodeSet(iter);
      BaseIterator nodeSet2 = this.right.EvaluateNodeSet(iter);
      return (object) new UnionIterator(iter, nodeSet1, nodeSet2);
    }

    internal override XPathNodeType EvaluatedNodeType => this.left.EvaluatedNodeType == this.right.EvaluatedNodeType ? this.left.EvaluatedNodeType : XPathNodeType.All;

    internal override bool IsPositional => this.left.IsPositional || this.right.IsPositional;

    internal override bool Peer => this.left.Peer && this.right.Peer;

    internal override bool Subtree
    {
      get
      {
        NodeSet left = this.left as NodeSet;
        NodeSet right = this.right as NodeSet;
        return left != null && right != null && left.Subtree && right.Subtree;
      }
    }
  }
}
