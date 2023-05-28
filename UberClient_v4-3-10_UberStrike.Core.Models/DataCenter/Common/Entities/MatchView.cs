// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.MatchView
// Assembly: UberStrike.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: E29887F9-C6F9-4A17-AD3C-0A827CA1DCD6
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Models.dll

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
