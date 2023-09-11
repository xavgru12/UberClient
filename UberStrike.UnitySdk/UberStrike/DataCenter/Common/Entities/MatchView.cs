// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.MatchView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System;
using System.Collections.Generic;
using UberStrike.Core.Types;

namespace UberStrike.DataCenter.Common.Entities
{
  [Serializable]
  public class MatchView
  {
    public List<PlayerStatisticsView> PlayersCompleted { get; set; }

    public List<PlayerStatisticsView> PlayersNonCompleted { get; set; }

    public int MapId { get; set; }

    public GameModeType GameModeId { get; set; }

    public int TimeLimit { get; set; }

    public int PlayersLimit { get; set; }

    public MatchView()
    {
    }

    public MatchView(
      List<PlayerStatisticsView> playersCompleted,
      List<PlayerStatisticsView> playersNonCompleted,
      int mapId,
      GameModeType gameModeId,
      int timeLimit,
      int playersLimit)
    {
      this.PlayersCompleted = playersCompleted;
      this.PlayersNonCompleted = playersNonCompleted;
      this.MapId = mapId;
      this.GameModeId = gameModeId;
      this.TimeLimit = timeLimit;
      this.PlayersLimit = playersLimit;
    }
  }
}
