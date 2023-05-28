// Decompiled with JetBrains decompiler
// Type: Steamworks.AvatarImageLoaded_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(334)]
  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct AvatarImageLoaded_t
  {
    public const int k_iCallback = 334;
    public CSteamID m_steamID;
    public int m_iImage;
    public int m_iWide;
    public int m_iTall;
  }
}
