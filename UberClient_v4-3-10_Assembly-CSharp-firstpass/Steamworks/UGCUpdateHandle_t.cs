// Decompiled with JetBrains decompiler
// Type: Steamworks.UGCUpdateHandle_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct UGCUpdateHandle_t : IEquatable<UGCUpdateHandle_t>, IComparable<UGCUpdateHandle_t>
  {
    public static readonly UGCUpdateHandle_t Invalid = new UGCUpdateHandle_t(ulong.MaxValue);
    public ulong m_UGCQueryHandle;

    public UGCUpdateHandle_t(ulong value) => this.m_UGCQueryHandle = value;

    public override string ToString() => this.m_UGCQueryHandle.ToString();

    public override bool Equals(object other) => other is UGCUpdateHandle_t ugcUpdateHandleT && this == ugcUpdateHandleT;

    public override int GetHashCode() => this.m_UGCQueryHandle.GetHashCode();

    public bool Equals(UGCUpdateHandle_t other) => (long) this.m_UGCQueryHandle == (long) other.m_UGCQueryHandle;

    public int CompareTo(UGCUpdateHandle_t other) => this.m_UGCQueryHandle.CompareTo(other.m_UGCQueryHandle);

    public static bool operator ==(UGCUpdateHandle_t x, UGCUpdateHandle_t y) => (long) x.m_UGCQueryHandle == (long) y.m_UGCQueryHandle;

    public static bool operator !=(UGCUpdateHandle_t x, UGCUpdateHandle_t y) => !(x == y);

    public static explicit operator UGCUpdateHandle_t(ulong value) => new UGCUpdateHandle_t(value);

    public static explicit operator ulong(UGCUpdateHandle_t that) => that.m_UGCQueryHandle;
  }
}
