// Decompiled with JetBrains decompiler
// Type: Steamworks.HSteamPipe
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct HSteamPipe : IEquatable<HSteamPipe>, IComparable<HSteamPipe>
  {
    public int m_HSteamPipe;

    public HSteamPipe(int value) => this.m_HSteamPipe = value;

    public override string ToString() => this.m_HSteamPipe.ToString();

    public override bool Equals(object other) => other is HSteamPipe hsteamPipe && this == hsteamPipe;

    public override int GetHashCode() => this.m_HSteamPipe.GetHashCode();

    public bool Equals(HSteamPipe other) => this.m_HSteamPipe == other.m_HSteamPipe;

    public int CompareTo(HSteamPipe other) => this.m_HSteamPipe.CompareTo(other.m_HSteamPipe);

    public static bool operator ==(HSteamPipe x, HSteamPipe y) => x.m_HSteamPipe == y.m_HSteamPipe;

    public static bool operator !=(HSteamPipe x, HSteamPipe y) => !(x == y);

    public static explicit operator HSteamPipe(int value) => new HSteamPipe(value);

    public static explicit operator int(HSteamPipe that) => that.m_HSteamPipe;
  }
}
