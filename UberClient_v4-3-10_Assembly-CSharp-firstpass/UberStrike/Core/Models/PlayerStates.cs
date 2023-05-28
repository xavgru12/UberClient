// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Models.PlayerStates
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace UberStrike.Core.Models
{
  [Flags]
  public enum PlayerStates : byte
  {
    None = 0,
    Spectator = 1,
    Dead = 2,
    Paused = 4,
    Sniping = 8,
    Shooting = 16, // 0x10
    Ready = 32, // 0x20
    Offline = 64, // 0x40
  }
}
