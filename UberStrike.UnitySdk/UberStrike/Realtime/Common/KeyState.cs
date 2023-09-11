
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
    Jump = 16,     Crouch = 32,     Vertical = Backward | Forward,     Horizontal = Right | Left,     Walking = Horizontal | Vertical,   }
}
