// Decompiled with JetBrains decompiler
// Type: Steamworks.ERemoteStoragePlatform
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  [Flags]
  public enum ERemoteStoragePlatform
  {
    k_ERemoteStoragePlatformNone = 0,
    k_ERemoteStoragePlatformWindows = 1,
    k_ERemoteStoragePlatformOSX = 2,
    k_ERemoteStoragePlatformPS3 = 4,
    k_ERemoteStoragePlatformLinux = 8,
    k_ERemoteStoragePlatformReserved2 = 16, // 0x00000010
    k_ERemoteStoragePlatformAll = -1, // 0xFFFFFFFF
  }
}
