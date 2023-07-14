// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.NullIterator
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal class NullIterator : SelfIterator
  {
    public NullIterator(BaseIterator iter)
      : base(iter)
    {
    }

    public NullIterator(XPathNavigator nav)
      : this(nav, (IXmlNamespaceResolver) null)
    {
    }

    public NullIterator(XPathNavigator nav, IXmlNamespaceResolver nsm)
      : base(nav, nsm)
    {
    }

    private NullIterator(NullIterator other)
      : base((SelfIterator) other, true)
    {
    }

    public override XPathNodeIterator Clone() => (XPathNodeIterator) new NullIterator(this);

    public override bool MoveNextCore() => false;

    public override int CurrentPosition => 1;

    public override XPathNavigator Current => this._nav;
  }
}
