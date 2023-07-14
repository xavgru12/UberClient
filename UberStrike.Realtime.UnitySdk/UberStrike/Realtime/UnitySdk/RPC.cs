// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.RPC
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

namespace UberStrike.Realtime.UnitySdk
{
  [ExtendableEnumBounds(1, 20)]
  public class RPC : ExtendableEnum<byte>
  {
    public const byte Join = 1;
    public const byte Leave = 2;
    public const byte PlayerUpdate = 3;
    public const byte FullPlayerListUpdate = 4;
    public const byte DeltaPlayerListUpdate = 5;
    public const byte ResetPlayer = 6;
  }
}
