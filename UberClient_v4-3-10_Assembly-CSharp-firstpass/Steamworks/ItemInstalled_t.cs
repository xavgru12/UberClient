// Decompiled with JetBrains decompiler
// Type: Steamworks.ItemInstalled_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(3405)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct ItemInstalled_t
  {
    public const int k_iCallback = 3405;
    public AppId_t m_unAppID;
    public PublishedFileId_t m_nPublishedFileId;
  }
}
