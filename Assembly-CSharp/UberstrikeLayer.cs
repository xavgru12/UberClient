// Decompiled with JetBrains decompiler
// Type: UberstrikeLayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

public enum UberstrikeLayer
{
  Default = 0,
  TransparentFX = 1,
  IgnoreRaycast = 2,
  Water = 4,
  GloballyLit = 8,
  GloballyLit_Reflect = 9,
  GloballyLit_Refract = 10, // 0x0000000A
  GloballyLit_ReflectRefract = 11, // 0x0000000B
  GloballyLit_DynamicReflectRefract = 12, // 0x0000000C
  LocallyLit = 13, // 0x0000000D
  LocallyLit_Reflect = 14, // 0x0000000E
  LocallyLit_Refract = 15, // 0x0000000F
  LocallyLit_ReflectRefract = 16, // 0x00000010
  LocallyLit_DynamicReflectRefract = 17, // 0x00000011
  LocalPlayer = 18, // 0x00000012
  Weapons = 19, // 0x00000013
  RemotePlayer = 20, // 0x00000014
  Props = 21, // 0x00000015
  Trigger = 22, // 0x00000016
  Teleporter = 23, // 0x00000017
  RemoteProjectile = 24, // 0x00000018
  Controller = 25, // 0x00000019
  LocalProjectile = 26, // 0x0000001A
  MeshGUI = 27, // 0x0000001B
  Raidbot = 28, // 0x0000001C
  Ragdoll = 29, // 0x0000001D
}
