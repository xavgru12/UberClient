// Decompiled with JetBrains decompiler
// Type: Steamworks.HServerQuery
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct HServerQuery : IEquatable<HServerQuery>, IComparable<HServerQuery>
  {
    public static readonly HServerQuery Invalid = new HServerQuery(-1);
    public int m_HServerQuery;

    public HServerQuery(int value) => this.m_HServerQuery = value;

    public override string ToString() => this.m_HServerQuery.ToString();

    public override bool Equals(object other) => other is HServerQuery hserverQuery && this == hserverQuery;

    public override int GetHashCode() => this.m_HServerQuery.GetHashCode();

    public bool Equals(HServerQuery other) => this.m_HServerQuery == other.m_HServerQuery;

    public int CompareTo(HServerQuery other) => this.m_HServerQuery.CompareTo(other.m_HServerQuery);

    public static bool operator ==(HServerQuery x, HServerQuery y) => x.m_HServerQuery == y.m_HServerQuery;

    public static bool operator !=(HServerQuery x, HServerQuery y) => !(x == y);

    public static explicit operator HServerQuery(int value) => new HServerQuery(value);

    public static explicit operator int(HServerQuery that) => that.m_HServerQuery;
  }
}
