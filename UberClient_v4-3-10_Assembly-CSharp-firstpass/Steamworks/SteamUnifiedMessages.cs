// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamUnifiedMessages
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Steamworks
{
  public static class SteamUnifiedMessages
  {
    public static ClientUnifiedMessageHandle SendMethod(
      string pchServiceMethod,
      byte[] pRequestBuffer,
      uint unRequestBufferSize,
      ulong unContext)
    {
      InteropHelp.TestIfAvailableClient();
      return (ClientUnifiedMessageHandle) NativeMethods.ISteamUnifiedMessages_SendMethod(pchServiceMethod, pRequestBuffer, unRequestBufferSize, unContext);
    }

    public static bool GetMethodResponseInfo(
      ClientUnifiedMessageHandle hHandle,
      out uint punResponseSize,
      out EResult peResult)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUnifiedMessages_GetMethodResponseInfo(hHandle, out punResponseSize, out peResult);
    }

    public static bool GetMethodResponseData(
      ClientUnifiedMessageHandle hHandle,
      byte[] pResponseBuffer,
      uint unResponseBufferSize,
      bool bAutoRelease)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUnifiedMessages_GetMethodResponseData(hHandle, pResponseBuffer, unResponseBufferSize, bAutoRelease);
    }

    public static bool ReleaseMethod(ClientUnifiedMessageHandle hHandle)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUnifiedMessages_ReleaseMethod(hHandle);
    }

    public static bool SendNotification(
      string pchServiceNotification,
      byte[] pNotificationBuffer,
      uint unNotificationBufferSize)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUnifiedMessages_SendNotification(pchServiceNotification, pNotificationBuffer, unNotificationBufferSize);
    }
  }
}
