// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamUGCDetails_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct SteamUGCDetails_t
  {
    public PublishedFileId_t m_nPublishedFileId;
    public EResult m_eResult;
    public EWorkshopFileType m_eFileType;
    public AppId_t m_nCreatorAppID;
    public AppId_t m_nConsumerAppID;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 129)]
    public string m_rgchTitle;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8000)]
    public string m_rgchDescription;
    public ulong m_ulSteamIDOwner;
    public uint m_rtimeCreated;
    public uint m_rtimeUpdated;
    public uint m_rtimeAddedToUserList;
    public ERemoteStoragePublishedFileVisibility m_eVisibility;
    [MarshalAs(UnmanagedType.I1)]
    public bool m_bBanned;
    [MarshalAs(UnmanagedType.I1)]
    public bool m_bAcceptedForUse;
    [MarshalAs(UnmanagedType.I1)]
    public bool m_bTagsTruncated;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1025)]
    public string m_rgchTags;
    public UGCHandle_t m_hFile;
    public UGCHandle_t m_hPreviewFile;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
    public string m_pchFileName;
    public int m_nFileSize;
    public int m_nPreviewFileSize;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string m_rgchURL;
    public uint m_unVotesUp;
    public uint m_unVotesDown;
    public float m_flScore;
    public uint m_unNumChildren;
  }
}
