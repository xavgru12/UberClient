// Decompiled with JetBrains decompiler
// Type: Steamworks.P2PSessionState_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct P2PSessionState_t
  {
    public byte m_bConnectionActive;
    public byte m_bConnecting;
    public byte m_eP2PSessionError;
    public byte m_bUsingRelay;
    public int m_nBytesQueuedForSend;
    public int m_nPacketsQueuedForSend;
    public uint m_nRemoteIP;
    public ushort m_nRemotePort;
  }
}
