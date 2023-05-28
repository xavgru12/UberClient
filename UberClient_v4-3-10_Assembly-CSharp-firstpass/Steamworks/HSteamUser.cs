// Decompiled with JetBrains decompiler
// Type: Steamworks.HSteamUser
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct HSteamUser : IEquatable<HSteamUser>, IComparable<HSteamUser>
  {
    public int m_HSteamUser;

    public HSteamUser(int value) => this.m_HSteamUser = value;

    public override string ToString() => this.m_HSteamUser.ToString();

    public override bool Equals(object other) => other is HSteamUser hsteamUser && this == hsteamUser;

    public override int GetHashCode() => this.m_HSteamUser.GetHashCode();

    public bool Equals(HSteamUser other) => this.m_HSteamUser == other.m_HSteamUser;

    public int CompareTo(HSteamUser other) => this.m_HSteamUser.CompareTo(other.m_HSteamUser);

    public static bool operator ==(HSteamUser x, HSteamUser y) => x.m_HSteamUser == y.m_HSteamUser;

    public static bool operator !=(HSteamUser x, HSteamUser y) => !(x == y);

    public static explicit operator HSteamUser(int value) => new HSteamUser(value);

    public static explicit operator int(HSteamUser that) => that.m_HSteamUser;
  }
}
