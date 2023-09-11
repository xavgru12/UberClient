﻿
using System.Collections;

namespace Cmune.Util
{
  public static class Comparison
  {
    public static bool IsEqual(object a, object b)
    {
      if (object.ReferenceEquals(a, b))
        return true;
      if (a == null || b == null)
        return false;
      return a is ICollection && b is ICollection ? Comparison.IsSequenceEqual(a as ICollection, b as ICollection) : a.Equals(b);
    }

    private static bool IsSequenceEqual(ICollection a1, ICollection a2)
    {
      if (a1 == null || a2 == null)
        return false;
      bool flag = true;
      IEnumerator enumerator1 = a1.GetEnumerator();
      IEnumerator enumerator2 = a2.GetEnumerator();
      while (flag && enumerator1.MoveNext() && enumerator2.MoveNext())
        flag = !(enumerator1.Current is ICollection) || !(enumerator2.Current is ICollection) ? enumerator1.Current != null && enumerator1.Current.Equals(enumerator2.Current) : Comparison.IsSequenceEqual(enumerator1.Current as ICollection, enumerator2.Current as ICollection);
      return flag;
    }
  }
}
