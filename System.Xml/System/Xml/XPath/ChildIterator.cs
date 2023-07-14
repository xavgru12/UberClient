// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.ChildIterator
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal class ChildIterator : BaseIterator
  {
    private XPathNavigator _nav;

    public ChildIterator(BaseIterator iter)
      : base(iter.NamespaceManager)
    {
      this._nav = iter.CurrentPosition != 0 ? iter.Current : iter.PeekNext();
      if (this._nav != null && this._nav.HasChildren)
        this._nav = this._nav.Clone();
      else
        this._nav = (XPathNavigator) null;
    }

    private ChildIterator(ChildIterator other)
      : base((BaseIterator) other)
    {
      this._nav = other._nav != null ? other._nav.Clone() : (XPathNavigator) null;
    }

    public override XPathNodeIterator Clone() => (XPathNodeIterator) new ChildIterator(this);

    public override bool MoveNextCore()
    {
      if (this._nav == null)
        return false;
      return this.CurrentPosition == 0 ? this._nav.MoveToFirstChild() : this._nav.MoveToNext();
    }

    public override XPathNavigator Current => this.CurrentPosition == 0 ? (XPathNavigator) null : this._nav;
  }
}
