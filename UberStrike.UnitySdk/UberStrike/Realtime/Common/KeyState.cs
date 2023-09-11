
using System;

namespace UberStrike.Realtime.Common
{
  [Flags]
  public enum KeyState
  {
    Still = 0,
    Forward = 1,
    Backward = 2,
    Left = 4,
    Right = 8,
    Jump = 16, // 0x00000010
    Crouch = 32, // 0x00000020
    Vertical = Backward | Forward, // 0x00000003
    Horizontal = Right | Left, // 0x0000000C
    Walking = Horizontal | Vertical, // 0x0000000F
  }
}
