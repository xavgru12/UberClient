// Decompiled with JetBrains decompiler
// Type: CombatRangeCategory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;

[Flags]
public enum CombatRangeCategory
{
  Close = 1,
  Medium = 2,
  Far = 4,
  CloseMedium = Medium | Close, // 0x00000003
  MediumFar = Far | Medium, // 0x00000006
  CloseMediumFar = MediumFar | Close, // 0x00000007
  CloseFar = Far | Close, // 0x00000005
}
