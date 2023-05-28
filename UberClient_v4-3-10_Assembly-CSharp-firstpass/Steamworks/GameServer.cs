// Decompiled with JetBrains decompiler
// Type: Steamworks.GameServer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Steamworks
{
  public static class GameServer
  {
    public static bool InitSafe(
      uint unIP,
      ushort usSteamPort,
      ushort usGamePort,
      ushort usQueryPort,
      EServerMode eServerMode,
      string pchVersionString)
    {
      InteropHelp.TestIfPlatformSupported();
      return NativeMethods.SteamGameServer_InitSafe(unIP, usSteamPort, usGamePort, usQueryPort, eServerMode, pchVersionString);
    }

    public static bool Init(
      uint unIP,
      ushort usSteamPort,
      ushort usGamePort,
      ushort usQueryPort,
      EServerMode eServerMode,
      string pchVersionString)
    {
      InteropHelp.TestIfPlatformSupported();
      return NativeMethods.SteamGameServer_InitSafe(unIP, usSteamPort, usGamePort, usQueryPort, eServerMode, pchVersionString);
    }

    public static void Shutdown()
    {
      InteropHelp.TestIfPlatformSupported();
      NativeMethods.SteamGameServer_Shutdown();
    }

    public static void RunCallbacks()
    {
      InteropHelp.TestIfPlatformSupported();
      NativeMethods.SteamGameServer_RunCallbacks();
    }

    public static bool BSecure()
    {
      InteropHelp.TestIfPlatformSupported();
      return NativeMethods.SteamGameServer_BSecure();
    }

    public static CSteamID GetSteamID()
    {
      InteropHelp.TestIfPlatformSupported();
      return (CSteamID) NativeMethods.SteamGameServer_GetSteamID();
    }

    public static HSteamPipe GetHSteamPipe()
    {
      InteropHelp.TestIfPlatformSupported();
      return (HSteamPipe) NativeMethods.SteamGameServer_GetHSteamPipe();
    }

    public static HSteamUser GetHSteamUser()
    {
      InteropHelp.TestIfPlatformSupported();
      return (HSteamUser) NativeMethods.SteamGameServer_GetHSteamUser();
    }
  }
}
