
using System;
using System.Text;

namespace UberStrike.DataCenter.Common.Entities
{
  [Serializable]
  public class PlayerStatisticsView
  {
    public int Cmid { get; set; }

    public int Splats { get; set; }

    public int Splatted { get; set; }

    public long Shots { get; set; }

    public long Hits { get; set; }

    public int Headshots { get; set; }

    public int Nutshots { get; set; }

    public int Xp { get; set; }

    public int Level { get; set; }

    public int TimeSpentInGame { get; set; }

    public PlayerPersonalRecordStatisticsView PersonalRecord { get; set; }

    public PlayerWeaponStatisticsView WeaponStatistics { get; set; }

    public int Points { get; set; }

    public PlayerStatisticsView()
    {
      this.PersonalRecord = new PlayerPersonalRecordStatisticsView();
      this.WeaponStatistics = new PlayerWeaponStatisticsView();
    }

    public PlayerStatisticsView(
      int cmid,
      int splats,
      int splatted,
      long shots,
      long hits,
      int headshots,
      int nutshots,
      PlayerPersonalRecordStatisticsView personalRecord,
      PlayerWeaponStatisticsView weaponStatistics,
      int points)
    {
      this.Cmid = cmid;
      this.Hits = hits;
      this.Level = 0;
      this.Shots = shots;
      this.Splats = splats;
      this.Splatted = splatted;
      this.Headshots = headshots;
      this.Nutshots = nutshots;
      this.Xp = 0;
      this.PersonalRecord = personalRecord;
      this.WeaponStatistics = weaponStatistics;
      this.Points = points;
    }

    public PlayerStatisticsView(
      int cmid,
      int splats,
      int splatted,
      long shots,
      long hits,
      int headshots,
      int nutshots,
      int xp,
      int level,
      PlayerPersonalRecordStatisticsView personalRecord,
      PlayerWeaponStatisticsView weaponStatistics,
      int points)
    {
      this.Cmid = cmid;
      this.Hits = hits;
      this.Level = level;
      this.Shots = shots;
      this.Splats = splats;
      this.Splatted = splatted;
      this.Headshots = headshots;
      this.Nutshots = nutshots;
      this.Xp = xp;
      this.PersonalRecord = personalRecord;
      this.WeaponStatistics = weaponStatistics;
      this.Points = points;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("[PlayerStatisticsView: ");
      stringBuilder.Append("[Cmid: ");
      stringBuilder.Append(this.Cmid);
      stringBuilder.Append("][Hits: ");
      stringBuilder.Append(this.Hits);
      stringBuilder.Append("][Level: ");
      stringBuilder.Append(this.Level);
      stringBuilder.Append("][Shots: ");
      stringBuilder.Append(this.Shots);
      stringBuilder.Append("][Splats: ");
      stringBuilder.Append(this.Splats);
      stringBuilder.Append("][Splatted: ");
      stringBuilder.Append(this.Splatted);
      stringBuilder.Append("][Headshots: ");
      stringBuilder.Append(this.Headshots);
      stringBuilder.Append("][Nutshots: ");
      stringBuilder.Append(this.Nutshots);
      stringBuilder.Append("][Xp: ");
      stringBuilder.Append(this.Xp);
      stringBuilder.Append("][Points: ");
      stringBuilder.Append(this.Points);
      stringBuilder.Append("]");
      stringBuilder.Append((object) this.PersonalRecord);
      stringBuilder.Append((object) this.WeaponStatistics);
      stringBuilder.Append("]");
      return stringBuilder.ToString();
    }
  }
}
