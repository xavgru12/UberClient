// Decompiled with JetBrains decompiler
// Type: Steamworks.ClientUnifiedMessageHandle
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct ClientUnifiedMessageHandle : 
    IEquatable<ClientUnifiedMessageHandle>,
    IComparable<ClientUnifiedMessageHandle>
  {
    public static readonly ClientUnifiedMessageHandle Invalid = new ClientUnifiedMessageHandle(0UL);
    public ulong m_ClientUnifiedMessageHandle;

    public ClientUnifiedMessageHandle(ulong value) => this.m_ClientUnifiedMessageHandle = value;

    public override string ToString() => this.m_ClientUnifiedMessageHandle.ToString();

    public override bool Equals(object other) => other is ClientUnifiedMessageHandle unifiedMessageHandle && this == unifiedMessageHandle;

    public override int GetHashCode() => this.m_ClientUnifiedMessageHandle.GetHashCode();

    public bool Equals(ClientUnifiedMessageHandle other) => (long) this.m_ClientUnifiedMessageHandle == (long) other.m_ClientUnifiedMessageHandle;

    public int CompareTo(ClientUnifiedMessageHandle other) => this.m_ClientUnifiedMessageHandle.CompareTo(other.m_ClientUnifiedMessageHandle);

    public static bool operator ==(ClientUnifiedMessageHandle x, ClientUnifiedMessageHandle y) => (long) x.m_ClientUnifiedMessageHandle == (long) y.m_ClientUnifiedMessageHandle;

    public static bool operator !=(ClientUnifiedMessageHandle x, ClientUnifiedMessageHandle y) => !(x == y);

    public static explicit operator ClientUnifiedMessageHandle(ulong value) => new ClientUnifiedMessageHandle(value);

    public static explicit operator ulong(ClientUnifiedMessageHandle that) => that.m_ClientUnifiedMessageHandle;
  }
}
