
using System;

namespace UberStrike.Realtime.Common
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
