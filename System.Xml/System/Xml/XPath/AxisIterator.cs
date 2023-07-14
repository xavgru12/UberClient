// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.AxisIterator
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal class AxisIterator : BaseIterator
  {
    private BaseIterator _iter;
    private NodeTest _test;

    public AxisIterator(BaseIterator iter, NodeTest test)
      : base(iter.NamespaceManager)
    {
      this._iter = iter;
      this._test = test;
    }

    private AxisIterator(AxisIterator other)
      : base((BaseIterator) other)
    {
      this._iter = (BaseIterator) other._iter.Clone();
      this._test = other._test;
    }

    public override XPathNodeIterator Clone() => (XPathNodeIterator) new AxisIterator(this);

    public override bool MoveNextCore()
    {
      while (this._iter.MoveNext())
      {
        if (this._test.Match(this.NamespaceManager, this._iter.Current))
          return true;
      }
      return false;
    }

    public override XPathNavigator Current => this.CurrentPosition == 0 ? (XPathNavigator) null : this._iter.Current;

    public override bool ReverseAxis => this._iter.ReverseAxis;
  }
}
