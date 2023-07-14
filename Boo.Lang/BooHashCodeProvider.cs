// Decompiled with JetBrains decompiler
// Type: Boo.Lang.BooHashCodeProvider
// Assembly: Boo.Lang, Version=2.0.9.5, Culture=neutral, PublicKeyToken=32c39770e9a21a67
// MVID: D4F47A63-3E02-45E3-BD62-EDD90EF56CC2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Boo.Lang.dll

using System;
using System.Collections;

namespace Boo.Lang
{
  [Serializable]
  public class BooHashCodeProvider : IEqualityComparer
  {
    public static readonly IEqualityComparer Default = (IEqualityComparer) new BooHashCodeProvider();

    private BooHashCodeProvider()
    {
    }

    public int GetHashCode(object o)
    {
      if (o == null)
        return 0;
      return o is Array array ? this.GetArrayHashCode(array) : o.GetHashCode();
    }

    public bool Equals(object lhs, object rhs) => BooComparer.Default.Compare(lhs, rhs) == 0;

    public int GetArrayHashCode(Array array)
    {
      int arrayHashCode = 1;
      int num = 0;
      foreach (object o in array)
        arrayHashCode ^= this.GetHashCode(o) * ++num;
      return arrayHashCode;
    }
  }
}
