// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.SimpleSlashIterator
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal class SimpleSlashIterator : BaseIterator
  {
    private NodeSet _expr;
    private BaseIterator _left;
    private BaseIterator _right;
    private XPathNavigator _current;

    public SimpleSlashIterator(BaseIterator left, NodeSet expr)
      : base(left.NamespaceManager)
    {
      this._left = left;
      this._expr = expr;
    }

    private SimpleSlashIterator(SimpleSlashIterator other)
      : base((BaseIterator) other)
    {
      this._expr = other._expr;
      this._left = (BaseIterator) other._left.Clone();
      if (other._right == null)
        return;
      this._right = (BaseIterator) other._right.Clone();
    }

    public override XPathNodeIterator Clone() => (XPathNodeIterator) new SimpleSlashIterator(this);

    public override bool MoveNextCore()
    {
      for (; this._right == null || !this._right.MoveNext(); this._right = this._expr.EvaluateNodeSet(this._left))
      {
        if (!this._left.MoveNext())
          return false;
      }
      if (this._current == null)
        this._current = this._right.Current.Clone();
      else if (!this._current.MoveTo(this._right.Current))
        this._current = this._right.Current.Clone();
      return true;
    }

    public override XPathNavigator Current => this._current;
  }
}
