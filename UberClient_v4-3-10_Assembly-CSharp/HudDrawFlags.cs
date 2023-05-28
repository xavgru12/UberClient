// Decompiled with JetBrains decompiler
// Type: HudDrawFlags
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;

[Flags]
public enum HudDrawFlags
{
  None = 0,
  Score = 1,
  HealthArmor = 2,
  Ammo = 4,
  Weapons = 8,
  Reticle = 16, // 0x00000010
  RoundTime = 32, // 0x00000020
  XpPoints = 64, // 0x00000040
  InGameHelp = 128, // 0x00000080
  EventStream = 256, // 0x00000100
  RemainingKill = 512, // 0x00000200
  InGameChat = 1024, // 0x00000400
  StateMsg = 2048, // 0x00000800
}
