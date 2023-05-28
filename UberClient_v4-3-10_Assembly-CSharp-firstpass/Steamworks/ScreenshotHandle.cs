// Decompiled with JetBrains decompiler
// Type: Steamworks.ScreenshotHandle
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct ScreenshotHandle : IEquatable<ScreenshotHandle>, IComparable<ScreenshotHandle>
  {
    public static readonly ScreenshotHandle Invalid = new ScreenshotHandle(0U);
    public uint m_ScreenshotHandle;

    public ScreenshotHandle(uint value) => this.m_ScreenshotHandle = value;

    public override string ToString() => this.m_ScreenshotHandle.ToString();

    public override bool Equals(object other) => other is ScreenshotHandle screenshotHandle && this == screenshotHandle;

    public override int GetHashCode() => this.m_ScreenshotHandle.GetHashCode();

    public bool Equals(ScreenshotHandle other) => (int) this.m_ScreenshotHandle == (int) other.m_ScreenshotHandle;

    public int CompareTo(ScreenshotHandle other) => this.m_ScreenshotHandle.CompareTo(other.m_ScreenshotHandle);

    public static bool operator ==(ScreenshotHandle x, ScreenshotHandle y) => (int) x.m_ScreenshotHandle == (int) y.m_ScreenshotHandle;

    public static bool operator !=(ScreenshotHandle x, ScreenshotHandle y) => !(x == y);

    public static explicit operator ScreenshotHandle(uint value) => new ScreenshotHandle(value);

    public static explicit operator uint(ScreenshotHandle that) => that.m_ScreenshotHandle;
  }
}
