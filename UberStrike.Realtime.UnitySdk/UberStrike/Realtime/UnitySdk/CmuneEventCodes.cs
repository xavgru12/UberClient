// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.CmuneEventCodes
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

namespace UberStrike.Realtime.UnitySdk
{
  [ExtendableEnumBounds(0, 255)]
  public class CmuneEventCodes : ExtendableEnum<byte>
  {
    public const byte Standard = 0;
    public const byte JoinEvent = 1;
    public const byte LeaveEvent = 2;
    public const byte GameListInit = 3;
    public const byte GameListUpdate = 4;
    public const byte GameListRemoval = 5;
    public const byte OldMessage = 100;
  }
}
