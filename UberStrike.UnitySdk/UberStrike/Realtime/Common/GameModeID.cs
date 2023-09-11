// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.Common.GameModeID
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.Core.Types.Attributes;

namespace UberStrike.Realtime.Common
{
  [ExtendableEnumBounds(100, 150)]
  public class GameModeID : UberstrikeClassID
  {
    public const short TeamDeathMatch = 100;
    public const short DeathMatch = 101;
    public const short FunMode = 102;
    public const short CooperationMode = 103;
    public const short CaptureTheFlag = 104;
    public const short LastManStanding = 105;
    public const short EliminationMode = 106;
    public const short ModerationMode = 107;
  }
}
