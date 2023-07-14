// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.SlashIterator
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;

namespace System.Xml.XPath
{
  internal class SlashIterator : BaseIterator
  {
    private BaseIterator _iterLeft;
    private BaseIterator _iterRight;
    private NodeSet _expr;
    private SortedList _iterList;
    private bool _finished;
    private BaseIterator _nextIterRight;

    public SlashIterator(BaseIterator iter, NodeSet expr)
      : base(iter.NamespaceManager)
    {
      this._iterLeft = iter;
      this._expr = expr;
    }

    private SlashIterator(SlashIterator other)
      : base((BaseIterator) other)
    {
      this._iterLeft = (BaseIterator) other._iterLeft.Clone();
      if (other._iterRight != null)
        this._iterRight = (BaseIterator) other._iterRight.Clone();
      this._expr = other._expr;
      if (other._iterList != null)
        this._iterList = (SortedList) other._iterList.Clone();
      this._finished = other._finished;
      if (other._nextIterRight == null)
        return;
      this._nextIterRight = (BaseIterator) other._nextIterRight.Clone();
    }

    public override XPathNodeIterator Clone() => (XPathNodeIterator) new SlashIterator(this);

    public override bool MoveNextCore()
    {
      if (this._finished)
        return false;
      if (this._iterRight == null)
      {
        if (!this._iterLeft.MoveNext())
          return false;
        this._iterRight = this._expr.EvaluateNodeSet(this._iterLeft);
        this._iterList = new SortedList((IComparer) XPathIteratorComparer.Instance);
      }
      for (; !this._iterRight.MoveNext(); this._iterRight = this._expr.EvaluateNodeSet(this._iterLeft))
      {
        if (this._iterList.Count > 0)
        {
          int index = this._iterList.Count - 1;
          this._iterRight = (BaseIterator) this._iterList.GetByIndex(index);
          this._iterList.RemoveAt(index);
          break;
        }
        if (this._nextIterRight != null)
        {
          this._iterRight = this._nextIterRight;
          this._nextIterRight = (BaseIterator) null;
          break;
        }
        if (!this._iterLeft.MoveNext())
        {
          this._finished = true;
          return false;
        }
      }
      bool flag1 = true;
      while (flag1)
      {
        flag1 = false;
        if (this._nextIterRight == null)
        {
          bool flag2 = false;
          for (; this._nextIterRight == null || !this._nextIterRight.MoveNext(); this._nextIterRight = this._expr.EvaluateNodeSet(this._iterLeft))
          {
            if (!this._iterLeft.MoveNext())
            {
              flag2 = true;
              break;
            }
          }
          if (flag2)
            this._nextIterRight = (BaseIterator) null;
        }
        if (this._nextIterRight != null)
        {
          switch (this._iterRight.Current.ComparePosition(this._nextIterRight.Current))
          {
            case XmlNodeOrder.After:
              this._iterList[(object) this._iterRight] = (object) this._iterRight;
              this._iterRight = this._nextIterRight;
              this._nextIterRight = (BaseIterator) null;
              flag1 = true;
              continue;
            case XmlNodeOrder.Same:
              if (!this._nextIterRight.MoveNext())
              {
                this._nextIterRight = (BaseIterator) null;
              }
              else
              {
                int count = this._iterList.Count;
                this._iterList[(object) this._nextIterRight] = (object) this._nextIterRight;
                if (count != this._iterList.Count)
                {
                  this._nextIterRight = (BaseIterator) this._iterList.GetByIndex(count);
                  this._iterList.RemoveAt(count);
                }
              }
              flag1 = true;
              continue;
            default:
              continue;
          }
        }
      }
      return true;
    }

    public override XPathNavigator Current => this.CurrentPosition == 0 ? (XPathNavigator) null : this._iterRight.Current;
  }
}
