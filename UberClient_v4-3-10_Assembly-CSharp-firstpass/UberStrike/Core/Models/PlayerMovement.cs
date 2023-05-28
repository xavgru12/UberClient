// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Models.PlayerMovement
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace UberStrike.Core.Models
{
  [Serializable]
  public class PlayerMovement
  {
    public byte Number { get; set; }

    public ShortVector3 Position { get; set; }

    public ShortVector3 Velocity { get; set; }

    public byte HorizontalRotation { get; set; }

    public byte VerticalRotation { get; set; }

    public byte KeyState { get; set; }

    public byte MovementState { get; set; }
  }
}
