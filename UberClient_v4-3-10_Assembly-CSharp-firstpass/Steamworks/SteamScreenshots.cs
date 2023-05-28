// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamScreenshots
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Steamworks
{
  public static class SteamScreenshots
  {
    public static ScreenshotHandle WriteScreenshot(
      byte[] pubRGB,
      uint cubRGB,
      int nWidth,
      int nHeight)
    {
      InteropHelp.TestIfAvailableClient();
      return (ScreenshotHandle) NativeMethods.ISteamScreenshots_WriteScreenshot(pubRGB, cubRGB, nWidth, nHeight);
    }

    public static ScreenshotHandle AddScreenshotToLibrary(
      string pchFilename,
      string pchThumbnailFilename,
      int nWidth,
      int nHeight)
    {
      InteropHelp.TestIfAvailableClient();
      return (ScreenshotHandle) NativeMethods.ISteamScreenshots_AddScreenshotToLibrary(pchFilename, pchThumbnailFilename, nWidth, nHeight);
    }

    public static void TriggerScreenshot()
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamScreenshots_TriggerScreenshot();
    }

    public static void HookScreenshots(bool bHook)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamScreenshots_HookScreenshots(bHook);
    }

    public static bool SetLocation(ScreenshotHandle hScreenshot, string pchLocation)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamScreenshots_SetLocation(hScreenshot, pchLocation);
    }

    public static bool TagUser(ScreenshotHandle hScreenshot, CSteamID steamID)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamScreenshots_TagUser(hScreenshot, steamID);
    }

    public static bool TagPublishedFile(
      ScreenshotHandle hScreenshot,
      PublishedFileId_t unPublishedFileID)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamScreenshots_TagPublishedFile(hScreenshot, unPublishedFileID);
    }
  }
}
