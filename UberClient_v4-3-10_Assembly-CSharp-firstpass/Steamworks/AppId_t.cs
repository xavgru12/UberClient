// Decompiled with JetBrains decompiler
// Type: Steamworks.AppId_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct AppId_t : IEquatable<AppId_t>, IComparable<AppId_t>
  {
    public static readonly AppId_t Invalid = new AppId_t(0U);
    public uint m_AppId;

    public AppId_t(uint value) => this.m_AppId = value;

    public override string ToString() => this.m_AppId.ToString();

    public override bool Equals(object other) => other is AppId_t appIdT && this == appIdT;

    public override int GetHashCode() => this.m_AppId.GetHashCode();

    public bool Equals(AppId_t other) => (int) this.m_AppId == (int) other.m_AppId;

    public int CompareTo(AppId_t other) => this.m_AppId.CompareTo(other.m_AppId);

    public static bool operator ==(AppId_t x, AppId_t y) => (int) x.m_AppId == (int) y.m_AppId;

    public static bool operator !=(AppId_t x, AppId_t y) => !(x == y);

    public static explicit operator AppId_t(uint value) => new AppId_t(value);

    public static explicit operator uint(AppId_t that) => that.m_AppId;
  }
}
