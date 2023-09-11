// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.Common.KeyState
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

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
