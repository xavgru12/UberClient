// Decompiled with JetBrains decompiler
// Type: Steamworks.PublishedFileId_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct PublishedFileId_t : IEquatable<PublishedFileId_t>, IComparable<PublishedFileId_t>
  {
    public static readonly PublishedFileId_t Invalid = new PublishedFileId_t(0UL);
    public ulong m_PublishedFileId;

    public PublishedFileId_t(ulong value) => this.m_PublishedFileId = value;

    public override string ToString() => this.m_PublishedFileId.ToString();

    public override bool Equals(object other) => other is PublishedFileId_t publishedFileIdT && this == publishedFileIdT;

    public override int GetHashCode() => this.m_PublishedFileId.GetHashCode();

    public bool Equals(PublishedFileId_t other) => (long) this.m_PublishedFileId == (long) other.m_PublishedFileId;

    public int CompareTo(PublishedFileId_t other) => this.m_PublishedFileId.CompareTo(other.m_PublishedFileId);

    public static bool operator ==(PublishedFileId_t x, PublishedFileId_t y) => (long) x.m_PublishedFileId == (long) y.m_PublishedFileId;

    public static bool operator !=(PublishedFileId_t x, PublishedFileId_t y) => !(x == y);

    public static explicit operator PublishedFileId_t(ulong value) => new PublishedFileId_t(value);

    public static explicit operator ulong(PublishedFileId_t that) => that.m_PublishedFileId;
  }
}
