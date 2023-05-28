// Decompiled with JetBrains decompiler
// Type: Steamworks.HServerListRequest
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct HServerListRequest : IEquatable<HServerListRequest>
  {
    public static readonly HServerListRequest Invalid = new HServerListRequest(IntPtr.Zero);
    public IntPtr m_HServerListRequest;

    public HServerListRequest(IntPtr value) => this.m_HServerListRequest = value;

    public override string ToString() => this.m_HServerListRequest.ToString();

    public override bool Equals(object other) => other is HServerListRequest hserverListRequest && this == hserverListRequest;

    public override int GetHashCode() => this.m_HServerListRequest.GetHashCode();

    public bool Equals(HServerListRequest other) => this.m_HServerListRequest == other.m_HServerListRequest;

    public static bool operator ==(HServerListRequest x, HServerListRequest y) => x.m_HServerListRequest == y.m_HServerListRequest;

    public static bool operator !=(HServerListRequest x, HServerListRequest y) => !(x == y);

    public static explicit operator HServerListRequest(IntPtr value) => new HServerListRequest(value);

    public static explicit operator IntPtr(HServerListRequest that) => that.m_HServerListRequest;
  }
}
