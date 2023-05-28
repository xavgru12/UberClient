// Decompiled with JetBrains decompiler
// Type: Steamworks.RemoteStorageGetPublishedItemVoteDetailsResult_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(1320)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct RemoteStorageGetPublishedItemVoteDetailsResult_t
  {
    public const int k_iCallback = 1320;
    public EResult m_eResult;
    public PublishedFileId_t m_unPublishedFileId;
    public int m_nVotesFor;
    public int m_nVotesAgainst;
    public int m_nReports;
    public float m_fScore;
  }
}
