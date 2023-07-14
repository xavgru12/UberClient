// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.ReverseComparer`1
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

using System.Collections.Generic;

namespace UberStrike.Realtime.UnitySdk
{
  public sealed class ReverseComparer<T> : IComparer<T>
  {
    private readonly IComparer<T> inner;

    public ReverseComparer()
      : this((IComparer<T>) null)
    {
    }

    public ReverseComparer(IComparer<T> inner) => this.inner = inner ?? (IComparer<T>) Comparer<T>.Default;

    int IComparer<T>.Compare(T x, T y) => this.inner.Compare(y, x);
  }
}
