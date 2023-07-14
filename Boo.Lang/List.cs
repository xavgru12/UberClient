// Decompiled with JetBrains decompiler
// Type: Boo.Lang.List
// Assembly: Boo.Lang, Version=2.0.9.5, Culture=neutral, PublicKeyToken=32c39770e9a21a67
// MVID: D4F47A63-3E02-45E3-BD62-EDD90EF56CC2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Boo.Lang.dll

using System;
using System.Collections;

namespace Boo.Lang
{
  [Serializable]
  public class List : List<object>
  {
    public List()
    {
    }

    public List(IEnumerable enumerable)
      : base(enumerable)
    {
    }

    public List(int initialCapacity)
      : base(initialCapacity)
    {
    }

    public List(object[] items, bool takeOwnership)
      : base(items, takeOwnership)
    {
    }

    public object Find(Predicate<object> predicate)
    {
      object found;
      return this.Find(predicate, out found) ? found : (object) null;
    }

    protected override List<object> NewConcreteList(object[] items, bool takeOwnership) => (List<object>) new List(items, takeOwnership);

    public Array ToArray(Type targetType)
    {
      Array instance = Array.CreateInstance(targetType, this._count);
      Array.Copy((Array) this._items, 0, instance, 0, this._count);
      return instance;
    }

    public static string operator %(string format, List rhs) => string.Format(format, rhs.ToArray());
  }
}
