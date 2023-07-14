// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.PrecedingSiblingIterator
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal class PrecedingSiblingIterator : SimpleIterator
  {
    private bool finished;
    private bool started;
    private XPathNavigator startPosition;

    public PrecedingSiblingIterator(BaseIterator iter)
      : base(iter)
    {
      this.startPosition = iter.Current.Clone();
    }

    private PrecedingSiblingIterator(PrecedingSiblingIterator other)
      : base((SimpleIterator) other, true)
    {
      this.startPosition = other.startPosition;
      this.started = other.started;
      this.finished = other.finished;
    }

    public override XPathNodeIterator Clone() => (XPathNodeIterator) new PrecedingSiblingIterator(this);

    public override bool MoveNextCore()
    {
      if (this.finished)
        return false;
      if (!this.started)
      {
        this.started = true;
        switch (this._nav.NodeType)
        {
          case XPathNodeType.Attribute:
          case XPathNodeType.Namespace:
            this.finished = true;
            return false;
          default:
            this._nav.MoveToFirst();
            if (!this._nav.IsSamePosition(this.startPosition))
              return true;
            break;
        }
      }
      else if (!this._nav.MoveToNext())
      {
        this.finished = true;
        return false;
      }
      if (this._nav.ComparePosition(this.startPosition) == XmlNodeOrder.Before)
        return true;
      this.finished = true;
      return false;
    }

    public override bool ReverseAxis => true;
  }
}
