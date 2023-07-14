// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.AncestorOrSelfIterator
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;

namespace System.Xml.XPath
{
  internal class AncestorOrSelfIterator : SimpleIterator
  {
    private int currentPosition;
    private ArrayList navigators;
    private XPathNavigator startPosition;

    public AncestorOrSelfIterator(BaseIterator iter)
      : base(iter)
    {
      this.startPosition = iter.Current.Clone();
    }

    private AncestorOrSelfIterator(AncestorOrSelfIterator other)
      : base((SimpleIterator) other, true)
    {
      this.startPosition = other.startPosition;
      if (other.navigators != null)
        this.navigators = (ArrayList) other.navigators.Clone();
      this.currentPosition = other.currentPosition;
    }

    public override XPathNodeIterator Clone() => (XPathNodeIterator) new AncestorOrSelfIterator(this);

    private void CollectResults()
    {
      this.navigators = new ArrayList();
      XPathNavigator xpathNavigator = this.startPosition.Clone();
      if (!xpathNavigator.MoveToParent())
        return;
      while (xpathNavigator.NodeType != XPathNodeType.Root)
      {
        this.navigators.Add((object) xpathNavigator.Clone());
        xpathNavigator.MoveToParent();
      }
      this.currentPosition = this.navigators.Count;
    }

    public override bool MoveNextCore()
    {
      if (this.navigators == null)
      {
        this.CollectResults();
        if (this.startPosition.NodeType != XPathNodeType.Root)
        {
          this._nav.MoveToRoot();
          return true;
        }
      }
      if (this.currentPosition == -1)
        return false;
      if (this.currentPosition-- == 0)
      {
        this._nav.MoveTo(this.startPosition);
        return true;
      }
      this._nav.MoveTo((XPathNavigator) this.navigators[this.currentPosition]);
      return true;
    }

    public override bool ReverseAxis => true;

    public override int Count
    {
      get
      {
        if (this.navigators == null)
          this.CollectResults();
        return this.navigators.Count + 1;
      }
    }
  }
}
