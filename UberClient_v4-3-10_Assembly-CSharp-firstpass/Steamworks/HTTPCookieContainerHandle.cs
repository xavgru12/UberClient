// Decompiled with JetBrains decompiler
// Type: Steamworks.HTTPCookieContainerHandle
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct HTTPCookieContainerHandle : 
    IEquatable<HTTPCookieContainerHandle>,
    IComparable<HTTPCookieContainerHandle>
  {
    public static readonly HTTPCookieContainerHandle Invalid = new HTTPCookieContainerHandle(0U);
    public uint m_HTTPCookieContainerHandle;

    public HTTPCookieContainerHandle(uint value) => this.m_HTTPCookieContainerHandle = value;

    public override string ToString() => this.m_HTTPCookieContainerHandle.ToString();

    public override bool Equals(object other) => other is HTTPCookieContainerHandle cookieContainerHandle && this == cookieContainerHandle;

    public override int GetHashCode() => this.m_HTTPCookieContainerHandle.GetHashCode();

    public bool Equals(HTTPCookieContainerHandle other) => (int) this.m_HTTPCookieContainerHandle == (int) other.m_HTTPCookieContainerHandle;

    public int CompareTo(HTTPCookieContainerHandle other) => this.m_HTTPCookieContainerHandle.CompareTo(other.m_HTTPCookieContainerHandle);

    public static bool operator ==(HTTPCookieContainerHandle x, HTTPCookieContainerHandle y) => (int) x.m_HTTPCookieContainerHandle == (int) y.m_HTTPCookieContainerHandle;

    public static bool operator !=(HTTPCookieContainerHandle x, HTTPCookieContainerHandle y) => !(x == y);

    public static explicit operator HTTPCookieContainerHandle(uint value) => new HTTPCookieContainerHandle(value);

    public static explicit operator uint(HTTPCookieContainerHandle that) => that.m_HTTPCookieContainerHandle;
  }
}
