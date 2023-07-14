// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.OrderedIterator
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;

namespace System.Xml.XPath
{
  internal class OrderedIterator : BaseIterator
  {
    private BaseIterator iter;
    private ArrayList list;
    private int index = -1;

    public OrderedIterator(BaseIterator iter)
      : base(iter.NamespaceManager)
    {
      this.list = new ArrayList();
      while (iter.MoveNext())
        this.list.Add((object) iter.Current);
      this.list.Sort((IComparer) XPathNavigatorComparer.Instance);
    }

    private OrderedIterator(OrderedIterator other, bool dummy)
      : base((BaseIterator) other)
    {
      if (other.iter != null)
        this.iter = (BaseIterator) other.iter.Clone();
      this.list = other.list;
      this.index = other.index;
    }

    public override XPathNodeIterator Clone() => (XPathNodeIterator) new OrderedIterator((BaseIterator) this);

    public override bool MoveNextCore()
    {
      if (this.iter != null)
        return this.iter.MoveNext();
      if (this.index++ < this.list.Count)
        return true;
      --this.index;
      return false;
    }

    public override XPathNavigator Current
    {
      get
      {
        if (this.iter != null)
          return this.iter.Current;
        return this.index < 0 ? (XPathNavigator) null : (XPathNavigator) this.list[this.index];
      }
    }
  }
}
