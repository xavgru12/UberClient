﻿// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.ParentIterator
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal class ParentIterator : SimpleIterator
  {
    private bool canMove;

    public ParentIterator(BaseIterator iter)
      : base(iter)
    {
      this.canMove = this._nav.MoveToParent();
    }

    private ParentIterator(ParentIterator other, bool dummy)
      : base((SimpleIterator) other, true)
    {
      this.canMove = other.canMove;
    }

    public ParentIterator(XPathNavigator nav, IXmlNamespaceResolver nsm)
      : base(nav, nsm)
    {
    }

    public override XPathNodeIterator Clone() => (XPathNodeIterator) new ParentIterator(this, true);

    public override bool MoveNextCore()
    {
      if (!this.canMove)
        return false;
      this.canMove = false;
      return true;
    }
  }
}
