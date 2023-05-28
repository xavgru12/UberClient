// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamController
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Steamworks
{
  public static class SteamController
  {
    public static bool Init(string pchAbsolutePathToControllerConfigVDF)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamController_Init(pchAbsolutePathToControllerConfigVDF);
    }

    public static bool Shutdown()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamController_Shutdown();
    }

    public static void RunFrame()
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamController_RunFrame();
    }

    public static bool GetControllerState(uint unControllerIndex, out SteamControllerState_t pState)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamController_GetControllerState(unControllerIndex, out pState);
    }

    public static void TriggerHapticPulse(
      uint unControllerIndex,
      ESteamControllerPad eTargetPad,
      ushort usDurationMicroSec)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamController_TriggerHapticPulse(unControllerIndex, eTargetPad, usDurationMicroSec);
    }

    public static void SetOverrideMode(string pchMode)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamController_SetOverrideMode(pchMode);
    }
  }
}
