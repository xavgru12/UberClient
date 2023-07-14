// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.SelfIterator
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal class SelfIterator : SimpleIterator
  {
    public SelfIterator(BaseIterator iter)
      : base(iter)
    {
    }

    public SelfIterator(XPathNavigator nav, IXmlNamespaceResolver nsm)
      : base(nav, nsm)
    {
    }

    protected SelfIterator(SelfIterator other, bool clone)
      : base((SimpleIterator) other, true)
    {
    }

    public override XPathNodeIterator Clone() => (XPathNodeIterator) new SelfIterator(this, true);

    public override bool MoveNextCore() => this.CurrentPosition == 0;

    public override XPathNavigator Current => this.CurrentPosition == 0 ? (XPathNavigator) null : this._nav;
  }
}
