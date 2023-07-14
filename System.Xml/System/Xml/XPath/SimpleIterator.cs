// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.SimpleIterator
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal abstract class SimpleIterator : BaseIterator
  {
    protected readonly XPathNavigator _nav;
    protected XPathNavigator _current;
    private bool skipfirst;

    public SimpleIterator(BaseIterator iter)
      : base(iter.NamespaceManager)
    {
      if (iter.CurrentPosition == 0)
      {
        this.skipfirst = true;
        iter.MoveNext();
      }
      if (iter.CurrentPosition <= 0)
        return;
      this._nav = iter.Current.Clone();
    }

    protected SimpleIterator(SimpleIterator other, bool clone)
      : base((BaseIterator) other)
    {
      if (other._nav != null)
        this._nav = !clone ? other._nav : other._nav.Clone();
      this.skipfirst = other.skipfirst;
    }

    public SimpleIterator(XPathNavigator nav, IXmlNamespaceResolver nsm)
      : base(nsm)
    {
      this._nav = nav.Clone();
    }

    public override bool MoveNext()
    {
      if (!this.skipfirst)
        return base.MoveNext();
      if (this._nav == null)
        return false;
      this.skipfirst = false;
      this.SetPosition(1);
      return true;
    }

    public override XPathNavigator Current
    {
      get
      {
        if (this.CurrentPosition == 0)
          return (XPathNavigator) null;
        this._current = this._nav;
        return this._current;
      }
    }
  }
}
