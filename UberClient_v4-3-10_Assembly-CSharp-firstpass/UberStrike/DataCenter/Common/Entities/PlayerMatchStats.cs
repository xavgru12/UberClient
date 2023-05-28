// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.PlayerMatchStats
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace UberStrike.DataCenter.Common.Entities
{
  [Serializable]
  public class PlayerMatchStats
  {
    public int Cmid { get; set; }

    public int Kills { get; set; }

    public int Death { get; set; }

    public long Shots { get; set; }

    public long Hits { get; set; }

    public int TimeSpentInGame { get; set; }

    public int Headshots { get; set; }

    public int Nutshots { get; set; }

    public int Smackdowns { get; set; }

    public bool HasFinishedMatch { get; set; }

    public bool HasWonMatch { get; set; }

    public PlayerPersonalRecordStatisticsView PersonalRecord { get; set; }

    public PlayerWeaponStatisticsView WeaponStatistics { get; set; }
  }
}
