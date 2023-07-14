// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.ListIterator
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;

namespace System.Xml.XPath
{
  internal class ListIterator : BaseIterator
  {
    private IList _list;

    public ListIterator(BaseIterator iter, IList list)
      : base(iter.NamespaceManager)
    {
      this._list = list;
    }

    public ListIterator(IList list, IXmlNamespaceResolver nsm)
      : base(nsm)
    {
      this._list = list;
    }

    private ListIterator(ListIterator other)
      : base((BaseIterator) other)
    {
      this._list = other._list;
    }

    public override XPathNodeIterator Clone() => (XPathNodeIterator) new ListIterator(this);

    public override bool MoveNextCore() => this.CurrentPosition < this._list.Count;

    public override XPathNavigator Current => this._list.Count == 0 || this.CurrentPosition == 0 ? (XPathNavigator) null : (XPathNavigator) this._list[this.CurrentPosition - 1];

    public override int Count => this._list.Count;
  }
}
