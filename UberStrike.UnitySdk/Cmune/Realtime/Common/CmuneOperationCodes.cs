// Decompiled with JetBrains decompiler
// Type: Cmune.Realtime.Common.CmuneOperationCodes
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.Core.Types;
using Cmune.Core.Types.Attributes;

namespace Cmune.Realtime.Common
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
