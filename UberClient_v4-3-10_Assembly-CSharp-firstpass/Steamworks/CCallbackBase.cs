// Decompiled with JetBrains decompiler
// Type: Steamworks.CCallbackBase
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
  [StructLayout(LayoutKind.Sequential)]
  public class CCallbackBase
  {
    public const byte k_ECallbackFlagsRegistered = 1;
    public const byte k_ECallbackFlagsGameServer = 2;
    public IntPtr m_vfptr;
    public byte m_nCallbackFlags;
    public int m_iCallback;
  }
}
