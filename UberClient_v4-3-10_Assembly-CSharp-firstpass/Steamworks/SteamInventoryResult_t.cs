// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamInventoryResult_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct SteamInventoryResult_t : 
    IEquatable<SteamInventoryResult_t>,
    IComparable<SteamInventoryResult_t>
  {
    public static readonly SteamInventoryResult_t Invalid = new SteamInventoryResult_t(-1);
    public int m_SteamInventoryResult_t;

    public SteamInventoryResult_t(int value) => this.m_SteamInventoryResult_t = value;

    public override string ToString() => this.m_SteamInventoryResult_t.ToString();

    public override bool Equals(object other) => other is SteamInventoryResult_t inventoryResultT && this == inventoryResultT;

    public override int GetHashCode() => this.m_SteamInventoryResult_t.GetHashCode();

    public bool Equals(SteamInventoryResult_t other) => this.m_SteamInventoryResult_t == other.m_SteamInventoryResult_t;

    public int CompareTo(SteamInventoryResult_t other) => this.m_SteamInventoryResult_t.CompareTo(other.m_SteamInventoryResult_t);

    public static bool operator ==(SteamInventoryResult_t x, SteamInventoryResult_t y) => x.m_SteamInventoryResult_t == y.m_SteamInventoryResult_t;

    public static bool operator !=(SteamInventoryResult_t x, SteamInventoryResult_t y) => !(x == y);

    public static explicit operator SteamInventoryResult_t(int value) => new SteamInventoryResult_t(value);

    public static explicit operator int(SteamInventoryResult_t that) => that.m_SteamInventoryResult_t;
  }
}
