// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamLeaderboardEntries_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct SteamLeaderboardEntries_t : 
    IEquatable<SteamLeaderboardEntries_t>,
    IComparable<SteamLeaderboardEntries_t>
  {
    public ulong m_SteamLeaderboardEntries;

    public SteamLeaderboardEntries_t(ulong value) => this.m_SteamLeaderboardEntries = value;

    public override string ToString() => this.m_SteamLeaderboardEntries.ToString();

    public override bool Equals(object other) => other is SteamLeaderboardEntries_t leaderboardEntriesT && this == leaderboardEntriesT;

    public override int GetHashCode() => this.m_SteamLeaderboardEntries.GetHashCode();

    public bool Equals(SteamLeaderboardEntries_t other) => (long) this.m_SteamLeaderboardEntries == (long) other.m_SteamLeaderboardEntries;

    public int CompareTo(SteamLeaderboardEntries_t other) => this.m_SteamLeaderboardEntries.CompareTo(other.m_SteamLeaderboardEntries);

    public static bool operator ==(SteamLeaderboardEntries_t x, SteamLeaderboardEntries_t y) => (long) x.m_SteamLeaderboardEntries == (long) y.m_SteamLeaderboardEntries;

    public static bool operator !=(SteamLeaderboardEntries_t x, SteamLeaderboardEntries_t y) => !(x == y);

    public static explicit operator SteamLeaderboardEntries_t(ulong value) => new SteamLeaderboardEntries_t(value);

    public static explicit operator ulong(SteamLeaderboardEntries_t that) => that.m_SteamLeaderboardEntries;
  }
}
