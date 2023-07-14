// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.DescendantIterator
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal class DescendantIterator : SimpleIterator
  {
    private int _depth;
    private bool _finished;

    public DescendantIterator(BaseIterator iter)
      : base(iter)
    {
    }

    private DescendantIterator(DescendantIterator other)
      : base((SimpleIterator) other, true)
    {
      this._depth = other._depth;
      this._finished = other._finished;
    }

    public override XPathNodeIterator Clone() => (XPathNodeIterator) new DescendantIterator(this);

    public override bool MoveNextCore()
    {
      if (this._finished)
        return false;
      if (this._nav.MoveToFirstChild())
      {
        ++this._depth;
        return true;
      }
      for (; this._depth != 0; --this._depth)
      {
        if (this._nav.MoveToNext())
          return true;
        if (!this._nav.MoveToParent())
          throw new XPathException("Current node is removed while it should not be, or there are some bugs in the XPathNavigator implementation class: " + (object) this._nav.GetType());
      }
      this._finished = true;
      return false;
    }
  }
}
