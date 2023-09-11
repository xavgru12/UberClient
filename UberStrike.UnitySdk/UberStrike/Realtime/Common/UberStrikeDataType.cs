// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.Common.UberStrikeDataType
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.Core.Types.Attributes;
using Cmune.Realtime.Common;

namespace UberStrike.Realtime.Common
{
  [ExtendableEnumBounds(101, 200)]
  public class UberStrikeDataType : CmuneDataType
  {
    public const byte ActorInfo = 101;
    public const byte ShortVector = 102;
    public const byte GameData = 103;
    public const byte Armor = 104;
    public const byte Weapons = 105;
    public const byte Statistics = 106;
    public const byte DamageEvent = 107;
    public const byte Stats = 108;
    public const byte EndOfMatch = 109;
    public const byte Array_GameData = 111;
  }
}
