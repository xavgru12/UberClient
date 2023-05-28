// Decompiled with JetBrains decompiler
// Type: Steamworks.SocketStatusCallback_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(1201)]
  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct SocketStatusCallback_t
  {
    public const int k_iCallback = 1201;
    public SNetSocket_t m_hSocket;
    public SNetListenSocket_t m_hListenSocket;
    public CSteamID m_steamIDRemote;
    public int m_eSNetSocketState;
  }
}
