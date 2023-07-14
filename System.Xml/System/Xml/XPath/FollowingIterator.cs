// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.FollowingIterator
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal class FollowingIterator : SimpleIterator
  {
    private bool _finished;

    public FollowingIterator(BaseIterator iter)
      : base(iter)
    {
    }

    private FollowingIterator(FollowingIterator other)
      : base((SimpleIterator) other, true)
    {
    }

    public override XPathNodeIterator Clone() => (XPathNodeIterator) new FollowingIterator(this);

    public override bool MoveNextCore()
    {
      if (this._finished)
        return false;
      bool flag = true;
      if (this.CurrentPosition == 0)
      {
        flag = false;
        switch (this._nav.NodeType)
        {
          case XPathNodeType.Attribute:
          case XPathNodeType.Namespace:
            this._nav.MoveToParent();
            flag = true;
            break;
          default:
            if (this._nav.MoveToNext())
              return true;
            while (this._nav.MoveToParent())
            {
              if (this._nav.MoveToNext())
                return true;
            }
            break;
        }
      }
      if (flag)
      {
        if (this._nav.MoveToFirstChild())
          return true;
        while (!this._nav.MoveToNext())
        {
          if (!this._nav.MoveToParent())
            goto label_16;
        }
        return true;
      }
label_16:
      this._finished = true;
      return false;
    }
  }
}
