// Decompiled with JetBrains decompiler
// Type: Steamworks.HHTMLBrowser
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct HHTMLBrowser : IEquatable<HHTMLBrowser>, IComparable<HHTMLBrowser>
  {
    public static readonly HHTMLBrowser Invalid = new HHTMLBrowser(0U);
    public uint m_HHTMLBrowser;

    public HHTMLBrowser(uint value) => this.m_HHTMLBrowser = value;

    public override string ToString() => this.m_HHTMLBrowser.ToString();

    public override bool Equals(object other) => other is HHTMLBrowser hhtmlBrowser && this == hhtmlBrowser;

    public override int GetHashCode() => this.m_HHTMLBrowser.GetHashCode();

    public bool Equals(HHTMLBrowser other) => (int) this.m_HHTMLBrowser == (int) other.m_HHTMLBrowser;

    public int CompareTo(HHTMLBrowser other) => this.m_HHTMLBrowser.CompareTo(other.m_HHTMLBrowser);

    public static bool operator ==(HHTMLBrowser x, HHTMLBrowser y) => (int) x.m_HHTMLBrowser == (int) y.m_HHTMLBrowser;

    public static bool operator !=(HHTMLBrowser x, HHTMLBrowser y) => !(x == y);

    public static explicit operator HHTMLBrowser(uint value) => new HHTMLBrowser(value);

    public static explicit operator uint(HHTMLBrowser that) => that.m_HHTMLBrowser;
  }
}
