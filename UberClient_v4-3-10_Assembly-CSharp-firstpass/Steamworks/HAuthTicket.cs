// Decompiled with JetBrains decompiler
// Type: Steamworks.HAuthTicket
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct HAuthTicket : IEquatable<HAuthTicket>, IComparable<HAuthTicket>
  {
    public static readonly HAuthTicket Invalid = new HAuthTicket(0U);
    public uint m_HAuthTicket;

    public HAuthTicket(uint value) => this.m_HAuthTicket = value;

    public override string ToString() => this.m_HAuthTicket.ToString();

    public override bool Equals(object other) => other is HAuthTicket hauthTicket && this == hauthTicket;

    public override int GetHashCode() => this.m_HAuthTicket.GetHashCode();

    public bool Equals(HAuthTicket other) => (int) this.m_HAuthTicket == (int) other.m_HAuthTicket;

    public int CompareTo(HAuthTicket other) => this.m_HAuthTicket.CompareTo(other.m_HAuthTicket);

    public static bool operator ==(HAuthTicket x, HAuthTicket y) => (int) x.m_HAuthTicket == (int) y.m_HAuthTicket;

    public static bool operator !=(HAuthTicket x, HAuthTicket y) => !(x == y);

    public static explicit operator HAuthTicket(uint value) => new HAuthTicket(value);

    public static explicit operator uint(HAuthTicket that) => that.m_HAuthTicket;
  }
}
