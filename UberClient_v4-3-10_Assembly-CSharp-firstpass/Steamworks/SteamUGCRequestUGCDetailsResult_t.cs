// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamUGCRequestUGCDetailsResult_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(3402)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct SteamUGCRequestUGCDetailsResult_t
  {
    public const int k_iCallback = 3402;
    public SteamUGCDetails_t m_details;
    [MarshalAs(UnmanagedType.I1)]
    public bool m_bCachedData;
  }
}
