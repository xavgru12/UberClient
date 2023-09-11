
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
    DUCKED = 16,     DEAD = 32,     PAUSED = 64,     SPECTATOR = 128,     WADING = 256,     SWIMMING = 512,     DIVING = 1024,   }
}
