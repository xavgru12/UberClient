// Decompiled with JetBrains decompiler
// Type: Steamworks.EChatSteamIDInstanceFlags
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  [Flags]
  public enum EChatSteamIDInstanceFlags
  {
    k_EChatAccountInstanceMask = 4095, // 0x00000FFF
    k_EChatInstanceFlagClan = 524288, // 0x00080000
    k_EChatInstanceFlagLobby = 262144, // 0x00040000
    k_EChatInstanceFlagMMSLobby = 131072, // 0x00020000
  }
}
