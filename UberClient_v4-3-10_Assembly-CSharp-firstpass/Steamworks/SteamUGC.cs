﻿// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamUGC
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Steamworks
{
  public static class SteamUGC
  {
    public static UGCQueryHandle_t CreateQueryUserUGCRequest(
      AccountID_t unAccountID,
      EUserUGCList eListType,
      EUGCMatchingUGCType eMatchingUGCType,
      EUserUGCListSortOrder eSortOrder,
      AppId_t nCreatorAppID,
      AppId_t nConsumerAppID,
      uint unPage)
    {
      InteropHelp.TestIfAvailableClient();
      return (UGCQueryHandle_t) NativeMethods.ISteamUGC_CreateQueryUserUGCRequest(unAccountID, eListType, eMatchingUGCType, eSortOrder, nCreatorAppID, nConsumerAppID, unPage);
    }

    public static UGCQueryHandle_t CreateQueryAllUGCRequest(
      EUGCQuery eQueryType,
      EUGCMatchingUGCType eMatchingeMatchingUGCTypeFileType,
      AppId_t nCreatorAppID,
      AppId_t nConsumerAppID,
      uint unPage)
    {
      InteropHelp.TestIfAvailableClient();
      return (UGCQueryHandle_t) NativeMethods.ISteamUGC_CreateQueryAllUGCRequest(eQueryType, eMatchingeMatchingUGCTypeFileType, nCreatorAppID, nConsumerAppID, unPage);
    }

    public static SteamAPICall_t SendQueryUGCRequest(UGCQueryHandle_t handle)
    {
      InteropHelp.TestIfAvailableClient();
      return (SteamAPICall_t) NativeMethods.ISteamUGC_SendQueryUGCRequest(handle);
    }

    public static bool GetQueryUGCResult(
      UGCQueryHandle_t handle,
      uint index,
      out SteamUGCDetails_t pDetails)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUGC_GetQueryUGCResult(handle, index, out pDetails);
    }

    public static bool ReleaseQueryUGCRequest(UGCQueryHandle_t handle)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUGC_ReleaseQueryUGCRequest(handle);
    }

    public static bool AddRequiredTag(UGCQueryHandle_t handle, string pTagName)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUGC_AddRequiredTag(handle, pTagName);
    }

    public static bool AddExcludedTag(UGCQueryHandle_t handle, string pTagName)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUGC_AddExcludedTag(handle, pTagName);
    }

    public static bool SetReturnLongDescription(
      UGCQueryHandle_t handle,
      bool bReturnLongDescription)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUGC_SetReturnLongDescription(handle, bReturnLongDescription);
    }

    public static bool SetReturnTotalOnly(UGCQueryHandle_t handle, bool bReturnTotalOnly)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUGC_SetReturnTotalOnly(handle, bReturnTotalOnly);
    }

    public static bool SetAllowCachedResponse(UGCQueryHandle_t handle, uint unMaxAgeSeconds)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUGC_SetAllowCachedResponse(handle, unMaxAgeSeconds);
    }

    public static bool SetCloudFileNameFilter(UGCQueryHandle_t handle, string pMatchCloudFileName)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUGC_SetCloudFileNameFilter(handle, pMatchCloudFileName);
    }

    public static bool SetMatchAnyTag(UGCQueryHandle_t handle, bool bMatchAnyTag)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUGC_SetMatchAnyTag(handle, bMatchAnyTag);
    }

    public static bool SetSearchText(UGCQueryHandle_t handle, string pSearchText)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUGC_SetSearchText(handle, pSearchText);
    }

    public static bool SetRankedByTrendDays(UGCQueryHandle_t handle, uint unDays)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUGC_SetRankedByTrendDays(handle, unDays);
    }

    public static SteamAPICall_t RequestUGCDetails(
      PublishedFileId_t nPublishedFileID,
      uint unMaxAgeSeconds)
    {
      InteropHelp.TestIfAvailableClient();
      return (SteamAPICall_t) NativeMethods.ISteamUGC_RequestUGCDetails(nPublishedFileID, unMaxAgeSeconds);
    }

    public static SteamAPICall_t CreateItem(AppId_t nConsumerAppId, EWorkshopFileType eFileType)
    {
      InteropHelp.TestIfAvailableClient();
      return (SteamAPICall_t) NativeMethods.ISteamUGC_CreateItem(nConsumerAppId, eFileType);
    }

    public static UGCUpdateHandle_t StartItemUpdate(
      AppId_t nConsumerAppId,
      PublishedFileId_t nPublishedFileID)
    {
      InteropHelp.TestIfAvailableClient();
      return (UGCUpdateHandle_t) NativeMethods.ISteamUGC_StartItemUpdate(nConsumerAppId, nPublishedFileID);
    }

    public static bool SetItemTitle(UGCUpdateHandle_t handle, string pchTitle)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUGC_SetItemTitle(handle, pchTitle);
    }

    public static bool SetItemDescription(UGCUpdateHandle_t handle, string pchDescription)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUGC_SetItemDescription(handle, pchDescription);
    }

    public static bool SetItemVisibility(
      UGCUpdateHandle_t handle,
      ERemoteStoragePublishedFileVisibility eVisibility)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUGC_SetItemVisibility(handle, eVisibility);
    }

    public static bool SetItemTags(UGCUpdateHandle_t updateHandle, IList<string> pTags)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUGC_SetItemTags(updateHandle, (IntPtr) new InteropHelp.SteamParamStringArray(pTags));
    }

    public static bool SetItemContent(UGCUpdateHandle_t handle, string pszContentFolder)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUGC_SetItemContent(handle, pszContentFolder);
    }

    public static bool SetItemPreview(UGCUpdateHandle_t handle, string pszPreviewFile)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUGC_SetItemPreview(handle, pszPreviewFile);
    }

    public static SteamAPICall_t SubmitItemUpdate(UGCUpdateHandle_t handle, string pchChangeNote)
    {
      InteropHelp.TestIfAvailableClient();
      return (SteamAPICall_t) NativeMethods.ISteamUGC_SubmitItemUpdate(handle, pchChangeNote);
    }

    public static EItemUpdateStatus GetItemUpdateProgress(
      UGCUpdateHandle_t handle,
      out ulong punBytesProcessed,
      out ulong punBytesTotal)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUGC_GetItemUpdateProgress(handle, out punBytesProcessed, out punBytesTotal);
    }

    public static SteamAPICall_t SubscribeItem(PublishedFileId_t nPublishedFileID)
    {
      InteropHelp.TestIfAvailableClient();
      return (SteamAPICall_t) NativeMethods.ISteamUGC_SubscribeItem(nPublishedFileID);
    }

    public static SteamAPICall_t UnsubscribeItem(PublishedFileId_t nPublishedFileID)
    {
      InteropHelp.TestIfAvailableClient();
      return (SteamAPICall_t) NativeMethods.ISteamUGC_UnsubscribeItem(nPublishedFileID);
    }

    public static uint GetNumSubscribedItems()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUGC_GetNumSubscribedItems();
    }

    public static uint GetSubscribedItems(PublishedFileId_t[] pvecPublishedFileID, uint cMaxEntries)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUGC_GetSubscribedItems(pvecPublishedFileID, cMaxEntries);
    }

    public static bool GetItemInstallInfo(
      PublishedFileId_t nPublishedFileID,
      out ulong punSizeOnDisk,
      out string pchFolder,
      uint cchFolderSize,
      out bool pbLegacyItem)
    {
      InteropHelp.TestIfAvailableClient();
      IntPtr num = Marshal.AllocHGlobal((int) cchFolderSize);
      bool itemInstallInfo = NativeMethods.ISteamUGC_GetItemInstallInfo(nPublishedFileID, out punSizeOnDisk, num, cchFolderSize, out pbLegacyItem);
      pchFolder = !itemInstallInfo ? (string) null : InteropHelp.PtrToStringUTF8(num);
      Marshal.FreeHGlobal(num);
      return itemInstallInfo;
    }

    public static bool GetItemUpdateInfo(
      PublishedFileId_t nPublishedFileID,
      out bool pbNeedsUpdate,
      out bool pbIsDownloading,
      out ulong punBytesDownloaded,
      out ulong punBytesTotal)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUGC_GetItemUpdateInfo(nPublishedFileID, out pbNeedsUpdate, out pbIsDownloading, out punBytesDownloaded, out punBytesTotal);
    }
  }
}
