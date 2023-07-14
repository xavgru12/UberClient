// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.KeyState
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

using System;

namespace UberStrike.Realtime.UnitySdk
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
