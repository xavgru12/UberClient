// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.RankingView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace UberStrike.DataCenter.Common.Entities
{
  public class RankingView
  {
    public int Cmid { get; set; }

    public long Rank { get; set; }

    public string ClanTag { get; set; }

    public string Name { get; set; }

    public int Xp { get; set; }

    public int Level { get; set; }

    public int Kills { get; set; }

    public int Deaths { get; set; }

    public Decimal Kdr { get; set; }

    public string DebugInformation { get; set; }
  }
}
