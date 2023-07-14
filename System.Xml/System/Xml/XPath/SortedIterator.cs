// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.SortedIterator
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;

namespace System.Xml.XPath
{
  internal class SortedIterator : BaseIterator
  {
    private ArrayList list;

    public SortedIterator(BaseIterator iter)
      : base(iter.NamespaceManager)
    {
      this.list = new ArrayList();
      while (iter.MoveNext())
        this.list.Add((object) iter.Current.Clone());
      if (this.list.Count == 0)
        return;
      XPathNavigator xpathNavigator = (XPathNavigator) this.list[0];
      this.list.Sort((IComparer) XPathNavigatorComparer.Instance);
      for (int index = 1; index < this.list.Count; ++index)
      {
        XPathNavigator other = (XPathNavigator) this.list[index];
        if (xpathNavigator.IsSamePosition(other))
        {
          this.list.RemoveAt(index);
          --index;
        }
        else
          xpathNavigator = other;
      }
    }

    public SortedIterator(SortedIterator other)
      : base((BaseIterator) other)
    {
      this.list = other.list;
      this.SetPosition(other.CurrentPosition);
    }

    public override XPathNodeIterator Clone() => (XPathNodeIterator) new SortedIterator(this);

    public override bool MoveNextCore() => this.CurrentPosition < this.list.Count;

    public override XPathNavigator Current => this.CurrentPosition == 0 ? (XPathNavigator) null : (XPathNavigator) this.list[this.CurrentPosition - 1];

    public override int Count => this.list.Count;
  }
}
