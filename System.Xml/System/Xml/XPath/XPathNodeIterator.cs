// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.XPathNodeIterator
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Diagnostics;

namespace System.Xml.XPath
{
  public abstract class XPathNodeIterator : IEnumerable, ICloneable
  {
    private int _count = -1;

    object ICloneable.Clone() => (object) this.Clone();

    public virtual int Count
    {
      get
      {
        if (this._count == -1)
        {
          XPathNodeIterator xpathNodeIterator = this.Clone();
          do
            ;
          while (xpathNodeIterator.MoveNext());
          this._count = xpathNodeIterator.CurrentPosition;
        }
        return this._count;
      }
    }

    public abstract XPathNavigator Current { get; }

    public abstract int CurrentPosition { get; }

    public abstract XPathNodeIterator Clone();

    [DebuggerHidden]
    public virtual IEnumerator GetEnumerator() => (IEnumerator) new XPathNodeIterator.\u003CGetEnumerator\u003Ec__Iterator2()
    {
      \u003C\u003Ef__this = this
    };

    public abstract bool MoveNext();
  }
}
