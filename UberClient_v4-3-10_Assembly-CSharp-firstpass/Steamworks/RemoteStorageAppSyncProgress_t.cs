// Decompiled with JetBrains decompiler
// Type: Steamworks.RemoteStorageAppSyncProgress_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(1303)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct RemoteStorageAppSyncProgress_t
  {
    public const int k_iCallback = 1303;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
    public string m_rgchCurrentFile;
    public AppId_t m_nAppID;
    public uint m_uBytesTransferredThisChunk;
    public double m_dAppPercentComplete;
    [MarshalAs(UnmanagedType.I1)]
    public bool m_bUploading;
  }
}
