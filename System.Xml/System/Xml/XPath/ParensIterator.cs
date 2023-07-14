// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.ParensIterator
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal class ParensIterator : BaseIterator
  {
    private BaseIterator _iter;

    public ParensIterator(BaseIterator iter)
      : base(iter.NamespaceManager)
    {
      this._iter = iter;
    }

    private ParensIterator(ParensIterator other)
      : base((BaseIterator) other)
    {
      this._iter = (BaseIterator) other._iter.Clone();
    }

    public override XPathNodeIterator Clone() => (XPathNodeIterator) new ParensIterator(this);

    public override bool MoveNextCore() => this._iter.MoveNext();

    public override XPathNavigator Current => this._iter.Current;

    public override int Count => this._iter.Count;
  }
}
