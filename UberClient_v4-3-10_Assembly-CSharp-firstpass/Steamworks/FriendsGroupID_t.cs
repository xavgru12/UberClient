// Decompiled with JetBrains decompiler
// Type: Steamworks.FriendsGroupID_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct FriendsGroupID_t : IEquatable<FriendsGroupID_t>, IComparable<FriendsGroupID_t>
  {
    public static readonly FriendsGroupID_t Invalid = new FriendsGroupID_t((short) -1);
    public short m_FriendsGroupID_t;

    public FriendsGroupID_t(short value) => this.m_FriendsGroupID_t = value;

    public override string ToString() => this.m_FriendsGroupID_t.ToString();

    public override bool Equals(object other) => other is FriendsGroupID_t friendsGroupIdT && this == friendsGroupIdT;

    public override int GetHashCode() => this.m_FriendsGroupID_t.GetHashCode();

    public bool Equals(FriendsGroupID_t other) => (int) this.m_FriendsGroupID_t == (int) other.m_FriendsGroupID_t;

    public int CompareTo(FriendsGroupID_t other) => this.m_FriendsGroupID_t.CompareTo(other.m_FriendsGroupID_t);

    public static bool operator ==(FriendsGroupID_t x, FriendsGroupID_t y) => (int) x.m_FriendsGroupID_t == (int) y.m_FriendsGroupID_t;

    public static bool operator !=(FriendsGroupID_t x, FriendsGroupID_t y) => !(x == y);

    public static explicit operator FriendsGroupID_t(short value) => new FriendsGroupID_t(value);

    public static explicit operator short(FriendsGroupID_t that) => that.m_FriendsGroupID_t;
  }
}
