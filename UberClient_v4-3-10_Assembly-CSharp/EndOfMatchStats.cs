// Decompiled with JetBrains decompiler
// Type: EndOfMatchStats
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;

public class EndOfMatchStats : Singleton<EndOfMatchStats>
{
  private EndOfMatchData _data;

  private EndOfMatchStats() => this.Data = new EndOfMatchData()
  {
    MostValuablePlayers = new List<StatsSummary>(),
    PlayerStatsBestPerLife = new StatsCollection(),
    PlayerStatsTotal = new StatsCollection(),
    PlayerXpEarned = new Dictionary<byte, ushort>()
  };

  public string KillXP { get; private set; }

  public string DamageXP { get; private set; }

  public string NutshotXP { get; private set; }

  public string HeadshotXP { get; private set; }

  public string SmackdownXP { get; private set; }

  public string Suicides { get; private set; }

  public string KDR { get; private set; }

  public string Deaths { get; private set; }

  public string PointsEarned { get; private set; }

  public string XPEarned { get; private set; }

  public EndOfMatchData Data
  {
    get => this._data;
    set
    {
      this._data = value;
      this.OnDataUpdated();
    }
  }

  private void OnDataUpdated()
  {
    this.KillXP = !this.Data.PlayerXpEarned.ContainsKey((byte) 1) ? "0" : this.Data.PlayerXpEarned[(byte) 1].ToString();
    this.HeadshotXP = !this.Data.PlayerXpEarned.ContainsKey((byte) 2) ? "0" : this.Data.PlayerXpEarned[(byte) 2].ToString();
    this.SmackdownXP = !this.Data.PlayerXpEarned.ContainsKey((byte) 4) ? "0" : this.Data.PlayerXpEarned[(byte) 4].ToString();
    this.NutshotXP = !this.Data.PlayerXpEarned.ContainsKey((byte) 3) ? "0" : this.Data.PlayerXpEarned[(byte) 3].ToString();
    this.DamageXP = !this.Data.PlayerXpEarned.ContainsKey((byte) 5) ? "0" : this.Data.PlayerXpEarned[(byte) 5].ToString();
    this.Deaths = this.Data.PlayerStatsTotal.Deaths.ToString();
    this.Suicides = (-this.Data.PlayerStatsTotal.Suicides).ToString();
    this.KDR = this.Data.PlayerStatsTotal.GetKdr().ToString("N1");
    this.PointsEarned = this.Data.PlayerStatsTotal.Points.ToString();
    this.XPEarned = this.Data.PlayerStatsTotal.Xp.ToString();
  }
}
