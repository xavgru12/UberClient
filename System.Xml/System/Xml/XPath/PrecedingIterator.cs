// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.PrecedingIterator
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal class PrecedingIterator : SimpleIterator
  {
    private bool finished;
    private bool started;
    private XPathNavigator startPosition;

    public PrecedingIterator(BaseIterator iter)
      : base(iter)
    {
      this.startPosition = iter.Current.Clone();
    }

    private PrecedingIterator(PrecedingIterator other)
      : base((SimpleIterator) other, true)
    {
      this.startPosition = other.startPosition;
      this.started = other.started;
      this.finished = other.finished;
    }

    public override XPathNodeIterator Clone() => (XPathNodeIterator) new PrecedingIterator(this);

    public override bool MoveNextCore()
    {
      if (this.finished)
        return false;
      if (!this.started)
      {
        this.started = true;
        this._nav.MoveToRoot();
      }
      bool flag = true;
      while (flag)
      {
        if (!this._nav.MoveToFirstChild())
        {
          while (!this._nav.MoveToNext())
          {
            if (!this._nav.MoveToParent())
            {
              this.finished = true;
              return false;
            }
          }
        }
        if (!this._nav.IsDescendant(this.startPosition))
          break;
      }
      if (this._nav.ComparePosition(this.startPosition) == XmlNodeOrder.Before)
        return true;
      this.finished = true;
      return false;
    }

    public override bool ReverseAxis => true;
  }
}
