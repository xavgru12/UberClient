// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.CmuneOperationCodes
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

namespace UberStrike.Realtime.UnitySdk
{
  [ExtendableEnumBounds(0, 255)]
  public class CmuneOperationCodes : ExtendableEnum<byte>
  {
    public const byte MessageToApplication = 66;
    public const byte MessageToPlayer = 80;
    public const byte MessageToAll = 81;
    public const byte MessageToServer = 82;
    public const byte MessageToOthers = 83;
    public const byte PhotonGameJoin = 88;
    public const byte PhotonGameLeave = 89;
    public const byte GameListUpdate = 90;
    public const byte GameListRemoval = 91;
    public const byte RegisterGameServer = 92;
  }
}
