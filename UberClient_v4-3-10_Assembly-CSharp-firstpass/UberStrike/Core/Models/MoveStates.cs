// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Models.MoveStates
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace UberStrike.Core.Models
{
  [Flags]
  public enum MoveStates : byte
  {
    None = 0,
    Grounded = 1,
    Jumping = 2,
    Flying = 4,
    Ducked = 8,
    Wading = 16, // 0x10
    Swimming = 32, // 0x20
    Diving = 64, // 0x40
    Landed = 128, // 0x80
  }
}
