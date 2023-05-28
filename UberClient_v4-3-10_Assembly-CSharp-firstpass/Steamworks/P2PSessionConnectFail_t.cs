// Decompiled with JetBrains decompiler
// Type: Steamworks.P2PSessionConnectFail_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(1203)]
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct P2PSessionConnectFail_t
  {
    public const int k_iCallback = 1203;
    public CSteamID m_steamIDRemote;
    public byte m_eP2PSessionError;
  }
}
