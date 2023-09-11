
using System;

namespace UberStrike.DataCenter.Common.Entities
{
  public class RankingView
  {
    public int Cmid { get; set; }

    public int Rank { get; set; }

    public string ClanTag { get; set; }

    public string Name { get; set; }

    public int Xp { get; set; }

    public int Level { get; set; }

    public int Kills { get; set; }

    public int Deaths { get; set; }

    public Decimal Kdr => this.Deaths != 0 ? (Decimal) this.Kills / (Decimal) this.Deaths : 0M;

    public string DebugInformation { get; set; }
  }
}
