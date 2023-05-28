// Decompiled with JetBrains decompiler
// Type: Steamworks.ClientGameServerDeny_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(113)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct ClientGameServerDeny_t
  {
    public const int k_iCallback = 113;
    public uint m_uAppID;
    public uint m_unGameServerIP;
    public ushort m_usGameServerPort;
    public ushort m_bSecure;
    public uint m_uReason;
  }
}
