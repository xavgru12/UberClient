// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.WrapperIterator
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal class WrapperIterator : BaseIterator
  {
    private XPathNodeIterator iter;

    public WrapperIterator(XPathNodeIterator iter, IXmlNamespaceResolver nsm)
      : base(nsm)
    {
      this.iter = iter;
    }

    private WrapperIterator(WrapperIterator other)
      : base((BaseIterator) other)
    {
      this.iter = other.iter.Clone();
    }

    public override XPathNodeIterator Clone() => (XPathNodeIterator) new WrapperIterator(this);

    public override bool MoveNextCore() => this.iter.MoveNext();

    public override XPathNavigator Current => this.iter.Current;
  }
}
