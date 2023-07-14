// Decompiled with JetBrains decompiler
// Type: Boo.Lang.BooComparer
// Assembly: Boo.Lang, Version=2.0.9.5, Culture=neutral, PublicKeyToken=32c39770e9a21a67
// MVID: D4F47A63-3E02-45E3-BD62-EDD90EF56CC2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Boo.Lang.dll

using System;
using System.Collections;

namespace Boo.Lang
{
  [Serializable]
  public class BooComparer : IComparer
  {
    public static readonly IComparer Default = (IComparer) new BooComparer();

    private BooComparer()
    {
    }

    public int Compare(object lhs, object rhs)
    {
      if (lhs == null)
        return rhs == null ? 0 : -1;
      if (rhs == null)
        return 1;
      if (lhs is IComparable comparable1)
        return comparable1.CompareTo(rhs);
      if (rhs is IComparable comparable2)
        return -1 * comparable2.CompareTo(lhs);
      IEnumerable lhs1 = lhs as IEnumerable;
      IEnumerable rhs1 = rhs as IEnumerable;
      if (lhs1 != null && rhs1 != null)
        return this.CompareEnumerables(lhs1, rhs1);
      return lhs.Equals(rhs) ? 0 : 1;
    }

    private int CompareEnumerables(IEnumerable lhs, IEnumerable rhs)
    {
      IEnumerator enumerator1 = lhs.GetEnumerator();
      IEnumerator enumerator2 = rhs.GetEnumerator();
      while (enumerator1.MoveNext())
      {
        if (!enumerator2.MoveNext())
          return 1;
        int num = this.Compare(enumerator1.Current, enumerator2.Current);
        if (num != 0)
          return num;
      }
      return enumerator2.MoveNext() ? -1 : 0;
    }
  }
}
