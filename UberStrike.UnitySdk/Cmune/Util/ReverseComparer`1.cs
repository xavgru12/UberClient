// Decompiled with JetBrains decompiler
// Type: Cmune.Util.ReverseComparer`1
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System.Collections.Generic;

namespace Cmune.Util
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
