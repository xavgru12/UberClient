// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.UnionIterator
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal class UnionIterator : BaseIterator
  {
    private BaseIterator _left;
    private BaseIterator _right;
    private bool keepLeft;
    private bool keepRight;
    private XPathNavigator _current;

    public UnionIterator(BaseIterator iter, BaseIterator left, BaseIterator right)
      : base(iter.NamespaceManager)
    {
      this._left = left;
      this._right = right;
    }

    private UnionIterator(UnionIterator other)
      : base((BaseIterator) other)
    {
      this._left = (BaseIterator) other._left.Clone();
      this._right = (BaseIterator) other._right.Clone();
      this.keepLeft = other.keepLeft;
      this.keepRight = other.keepRight;
      if (other._current == null)
        return;
      this._current = other._current.Clone();
    }

    public override XPathNodeIterator Clone() => (XPathNodeIterator) new UnionIterator(this);

    public override bool MoveNextCore()
    {
      if (!this.keepLeft)
        this.keepLeft = this._left.MoveNext();
      if (!this.keepRight)
        this.keepRight = this._right.MoveNext();
      if (!this.keepLeft && !this.keepRight)
        return false;
      if (!this.keepRight)
      {
        this.keepLeft = false;
        this.SetCurrent((XPathNodeIterator) this._left);
        return true;
      }
      if (!this.keepLeft)
      {
        this.keepRight = false;
        this.SetCurrent((XPathNodeIterator) this._right);
        return true;
      }
      switch (this._left.Current.ComparePosition(this._right.Current))
      {
        case XmlNodeOrder.Before:
        case XmlNodeOrder.Unknown:
          this.keepLeft = false;
          this.SetCurrent((XPathNodeIterator) this._left);
          return true;
        case XmlNodeOrder.After:
          this.keepRight = false;
          this.SetCurrent((XPathNodeIterator) this._right);
          return true;
        case XmlNodeOrder.Same:
          this.keepLeft = this.keepRight = false;
          this.SetCurrent((XPathNodeIterator) this._right);
          return true;
        default:
          throw new InvalidOperationException("Should not happen.");
      }
    }

    private void SetCurrent(XPathNodeIterator iter)
    {
      if (this._current == null)
      {
        this._current = iter.Current.Clone();
      }
      else
      {
        if (this._current.MoveTo(iter.Current))
          return;
        this._current = iter.Current.Clone();
      }
    }

    public override XPathNavigator Current => this.CurrentPosition > 0 ? this._current : (XPathNavigator) null;
  }
}
