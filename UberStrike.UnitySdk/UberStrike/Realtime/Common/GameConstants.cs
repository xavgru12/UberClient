// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.Common.GameConstants
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

namespace UberStrike.Realtime.Common
{
  public static class GameConstants
  {
    private const int Minute = 60;
    public static readonly GameConstants.Limits DefaultDeathMatchLimit = new GameConstants.Limits(new GameConstants.Bounds(60, 900, 300), new GameConstants.Bounds(1, 200, 20), new GameConstants.Bounds(2, 16, 8));
    public static readonly GameConstants.Limits DefaultTeamDeathMatchLimit = new GameConstants.Limits(new GameConstants.Bounds(60, 900, 480), new GameConstants.Bounds(10, 200, 40), new GameConstants.Bounds(2, 16, 12));
    public static readonly GameConstants.Limits DefaultTeamEliminationLimit = new GameConstants.Limits(new GameConstants.Bounds(60, 300, 120), new GameConstants.Bounds(1, 20, 5), new GameConstants.Bounds(2, 16, 12));
    public static readonly GameConstants.Limits SpaceStationAlphaLimit = new GameConstants.Limits(new GameConstants.Bounds(60, 900, 300), new GameConstants.Bounds(1, 200, 20), new GameConstants.Bounds(2, 12, 8));

    public static void CheckGameData(short mode, GameMetaData data)
    {
      if (data.MapID == 10)
      {
        data.RoundTime = GameConstants.CheckValueLimit(data.RoundTime, GameConstants.SpaceStationAlphaLimit.Time);
        data.SplatLimit = GameConstants.CheckValueLimit(data.SplatLimit, GameConstants.SpaceStationAlphaLimit.Kills);
        data.MaxPlayers = GameConstants.CheckValueLimit(data.MaxPlayers, GameConstants.SpaceStationAlphaLimit.Players);
      }
      else
      {
        switch (mode)
        {
          case 100:
            data.RoundTime = GameConstants.CheckValueLimit(data.RoundTime, GameConstants.DefaultTeamDeathMatchLimit.Time);
            data.SplatLimit = GameConstants.CheckValueLimit(data.SplatLimit, GameConstants.DefaultTeamDeathMatchLimit.Kills);
            data.MaxPlayers = GameConstants.CheckValueLimit(data.MaxPlayers, GameConstants.DefaultTeamDeathMatchLimit.Players);
            break;
          case 101:
            data.RoundTime = GameConstants.CheckValueLimit(data.RoundTime, GameConstants.DefaultDeathMatchLimit.Time);
            data.SplatLimit = GameConstants.CheckValueLimit(data.SplatLimit, GameConstants.DefaultDeathMatchLimit.Kills);
            data.MaxPlayers = GameConstants.CheckValueLimit(data.MaxPlayers, GameConstants.DefaultDeathMatchLimit.Players);
            break;
          case 106:
            data.RoundTime = GameConstants.CheckValueLimit(data.RoundTime, GameConstants.DefaultTeamEliminationLimit.Time);
            data.SplatLimit = GameConstants.CheckValueLimit(data.SplatLimit, GameConstants.DefaultTeamEliminationLimit.Kills);
            data.MaxPlayers = GameConstants.CheckValueLimit(data.MaxPlayers, GameConstants.DefaultTeamEliminationLimit.Players);
            break;
        }
      }
    }

    private static int CheckValueLimit(int value, int fallback, int min, int max) => value < min || value > max ? fallback : value;

    private static int CheckValueLimit(int value, GameConstants.Bounds bounds) => value < bounds.Min || value > bounds.Max ? bounds.Default : value;

    public class Limits
    {
      public GameConstants.Bounds Time { get; private set; }

      public GameConstants.Bounds Kills { get; private set; }

      public GameConstants.Bounds Players { get; private set; }

      public Limits(
        GameConstants.Bounds time,
        GameConstants.Bounds splats,
        GameConstants.Bounds playerCount)
      {
        this.Time = time;
        this.Kills = splats;
        this.Players = playerCount;
      }
    }

    public class Bounds
    {
      public int Min { get; private set; }

      public int Max { get; private set; }

      public int Default { get; private set; }

      public Bounds(int min, int max, int def)
      {
        this.Min = min;
        this.Max = max;
        this.Default = def;
      }
    }
  }
}
