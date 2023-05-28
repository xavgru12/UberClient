// Decompiled with JetBrains decompiler
// Type: Steamworks.AccountID_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct AccountID_t : IEquatable<AccountID_t>, IComparable<AccountID_t>
  {
    public uint m_AccountID;

    public AccountID_t(uint value) => this.m_AccountID = value;

    public override string ToString() => this.m_AccountID.ToString();

    public override bool Equals(object other) => other is AccountID_t accountIdT && this == accountIdT;

    public override int GetHashCode() => this.m_AccountID.GetHashCode();

    public bool Equals(AccountID_t other) => (int) this.m_AccountID == (int) other.m_AccountID;

    public int CompareTo(AccountID_t other) => this.m_AccountID.CompareTo(other.m_AccountID);

    public static bool operator ==(AccountID_t x, AccountID_t y) => (int) x.m_AccountID == (int) y.m_AccountID;

    public static bool operator !=(AccountID_t x, AccountID_t y) => !(x == y);

    public static explicit operator AccountID_t(uint value) => new AccountID_t(value);

    public static explicit operator uint(AccountID_t that) => that.m_AccountID;
  }
}
