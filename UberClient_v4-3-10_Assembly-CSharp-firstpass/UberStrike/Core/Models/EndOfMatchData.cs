// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Models.EndOfMatchData
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

namespace UberStrike.Core.Models
{
  [Serializable]
  public class EndOfMatchData
  {
    public List<StatsSummary> MostValuablePlayers { get; set; }

    public int MostEffecientWeaponId { get; set; }

    public StatsCollection PlayerStatsTotal { get; set; }

    public StatsCollection PlayerStatsBestPerLife { get; set; }

    public Dictionary<byte, ushort> PlayerXpEarned { get; set; }

    public int TimeInGameMinutes { get; set; }

    public bool HasWonMatch { get; set; }

    public string MatchGuid { get; set; }
  }
}
