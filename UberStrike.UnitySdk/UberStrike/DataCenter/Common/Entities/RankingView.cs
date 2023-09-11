// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.RankingView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

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
