﻿// Decompiled with JetBrains decompiler
// Type: Steamworks.UGCQueryHandle_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct UGCQueryHandle_t : IEquatable<UGCQueryHandle_t>, IComparable<UGCQueryHandle_t>
  {
    public static readonly UGCQueryHandle_t Invalid = new UGCQueryHandle_t(ulong.MaxValue);
    public ulong m_UGCQueryHandle;

    public UGCQueryHandle_t(ulong value) => this.m_UGCQueryHandle = value;

    public override string ToString() => this.m_UGCQueryHandle.ToString();

    public override bool Equals(object other) => other is UGCQueryHandle_t ugcQueryHandleT && this == ugcQueryHandleT;

    public override int GetHashCode() => this.m_UGCQueryHandle.GetHashCode();

    public bool Equals(UGCQueryHandle_t other) => (long) this.m_UGCQueryHandle == (long) other.m_UGCQueryHandle;

    public int CompareTo(UGCQueryHandle_t other) => this.m_UGCQueryHandle.CompareTo(other.m_UGCQueryHandle);

    public static bool operator ==(UGCQueryHandle_t x, UGCQueryHandle_t y) => (long) x.m_UGCQueryHandle == (long) y.m_UGCQueryHandle;

    public static bool operator !=(UGCQueryHandle_t x, UGCQueryHandle_t y) => !(x == y);

    public static explicit operator UGCQueryHandle_t(ulong value) => new UGCQueryHandle_t(value);

    public static explicit operator ulong(UGCQueryHandle_t that) => that.m_UGCQueryHandle;
  }
}
