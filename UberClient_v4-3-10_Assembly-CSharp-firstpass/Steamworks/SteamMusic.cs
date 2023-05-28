// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamMusic
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Steamworks
{
  public static class SteamMusic
  {
    public static bool BIsEnabled()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamMusic_BIsEnabled();
    }

    public static bool BIsPlaying()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamMusic_BIsPlaying();
    }

    public static AudioPlayback_Status GetPlaybackStatus()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamMusic_GetPlaybackStatus();
    }

    public static void Play()
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamMusic_Play();
    }

    public static void Pause()
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamMusic_Pause();
    }

    public static void PlayPrevious()
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamMusic_PlayPrevious();
    }

    public static void PlayNext()
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamMusic_PlayNext();
    }

    public static void SetVolume(float flVolume)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamMusic_SetVolume(flVolume);
    }

    public static float GetVolume()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamMusic_GetVolume();
    }
  }
}
