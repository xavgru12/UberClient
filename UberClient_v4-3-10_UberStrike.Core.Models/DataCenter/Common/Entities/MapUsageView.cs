// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.MapUsageView
// Assembly: UberStrike.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: E29887F9-C6F9-4A17-AD3C-0A827CA1DCD6
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Models.dll

using System;
using UberStrike.Core.Types;

namespace UberStrike.DataCenter.Common.Entities
{
  public class MapUsageView
  {
    public DateTime PlayDate { get; private set; }

    public int MapId { get; private set; }

    public GameModeType GameModeId { get; private set; }

    public int TimeLimit { get; private set; }

    public int PlayerLimit { get; private set; }

    public int PlayersTotal { get; private set; }

    public int PlayersCompleted { get; private set; }

    public MapUsageView(
      DateTime playDate,
      int mapId,
      GameModeType gameModeId,
      int timeLimit,
      int playerLimit,
      int playersTotal,
      int playersCompleted)
    {
      this.PlayDate = playDate;
      this.MapId = mapId;
      this.GameModeId = gameModeId;
      this.TimeLimit = timeLimit;
      this.PlayerLimit = playerLimit;
      this.PlayersTotal = playersTotal;
      this.PlayersCompleted = playersCompleted;
    }
  }
}
