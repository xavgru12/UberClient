// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.ReverseComparer`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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
