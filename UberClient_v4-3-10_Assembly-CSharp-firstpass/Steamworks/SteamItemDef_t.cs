// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamItemDef_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct SteamItemDef_t : IEquatable<SteamItemDef_t>, IComparable<SteamItemDef_t>
  {
    public int m_SteamItemDef_t;

    public SteamItemDef_t(int value) => this.m_SteamItemDef_t = value;

    public override string ToString() => this.m_SteamItemDef_t.ToString();

    public override bool Equals(object other) => other is SteamItemDef_t steamItemDefT && this == steamItemDefT;

    public override int GetHashCode() => this.m_SteamItemDef_t.GetHashCode();

    public bool Equals(SteamItemDef_t other) => this.m_SteamItemDef_t == other.m_SteamItemDef_t;

    public int CompareTo(SteamItemDef_t other) => this.m_SteamItemDef_t.CompareTo(other.m_SteamItemDef_t);

    public static bool operator ==(SteamItemDef_t x, SteamItemDef_t y) => x.m_SteamItemDef_t == y.m_SteamItemDef_t;

    public static bool operator !=(SteamItemDef_t x, SteamItemDef_t y) => !(x == y);

    public static explicit operator SteamItemDef_t(int value) => new SteamItemDef_t(value);

    public static explicit operator int(SteamItemDef_t that) => that.m_SteamItemDef_t;
  }
}
