// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Types.ModerationTag
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace UberStrike.Core.Types
{
  [Flags]
  public enum ModerationTag
  {
    None = 0,
    Muted = 1,
    Ghosted = 2,
    Banned = 4,
    Speedhacking = 8,
    Spamming = 16, // 0x00000010
    Language = 32, // 0x00000020
    Name = 64, // 0x00000040
  }
}
