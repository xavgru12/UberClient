// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.AttributeIterator
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal class AttributeIterator : SimpleIterator
  {
    public AttributeIterator(BaseIterator iter)
      : base(iter)
    {
    }

    private AttributeIterator(AttributeIterator other)
      : base((SimpleIterator) other, true)
    {
    }

    public override XPathNodeIterator Clone() => (XPathNodeIterator) new AttributeIterator(this);

    public override bool MoveNextCore()
    {
      if (this.CurrentPosition == 0)
      {
        if (this._nav.MoveToFirstAttribute())
          return true;
      }
      else if (this._nav.MoveToNextAttribute())
        return true;
      return false;
    }
  }
}
