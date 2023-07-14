// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.PlayerStates
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

using System;

namespace UberStrike.Realtime.UnitySdk
{
  [Flags]
  public enum PlayerStates
  {
    IDLE = 0,
    GROUNDED = 1,
    JUMPING = 2,
    FLYING = 8,
    DUCKED = 16, // 0x00000010
    DEAD = 32, // 0x00000020
    PAUSED = 64, // 0x00000040
    SPECTATOR = 128, // 0x00000080
    WADING = 256, // 0x00000100
    SWIMMING = 512, // 0x00000200
    DIVING = 1024, // 0x00000400
  }
}
