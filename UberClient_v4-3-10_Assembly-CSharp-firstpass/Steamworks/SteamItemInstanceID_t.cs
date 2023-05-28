// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamItemInstanceID_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct SteamItemInstanceID_t : 
    IEquatable<SteamItemInstanceID_t>,
    IComparable<SteamItemInstanceID_t>
  {
    public static readonly SteamItemInstanceID_t Invalid = new SteamItemInstanceID_t(ulong.MaxValue);
    public ulong m_SteamItemInstanceID_t;

    public SteamItemInstanceID_t(ulong value) => this.m_SteamItemInstanceID_t = value;

    public override string ToString() => this.m_SteamItemInstanceID_t.ToString();

    public override bool Equals(object other) => other is SteamItemInstanceID_t steamItemInstanceIdT && this == steamItemInstanceIdT;

    public override int GetHashCode() => this.m_SteamItemInstanceID_t.GetHashCode();

    public bool Equals(SteamItemInstanceID_t other) => (long) this.m_SteamItemInstanceID_t == (long) other.m_SteamItemInstanceID_t;

    public int CompareTo(SteamItemInstanceID_t other) => this.m_SteamItemInstanceID_t.CompareTo(other.m_SteamItemInstanceID_t);

    public static bool operator ==(SteamItemInstanceID_t x, SteamItemInstanceID_t y) => (long) x.m_SteamItemInstanceID_t == (long) y.m_SteamItemInstanceID_t;

    public static bool operator !=(SteamItemInstanceID_t x, SteamItemInstanceID_t y) => !(x == y);

    public static explicit operator SteamItemInstanceID_t(ulong value) => new SteamItemInstanceID_t(value);

    public static explicit operator ulong(SteamItemInstanceID_t that) => that.m_SteamItemInstanceID_t;
  }
}
