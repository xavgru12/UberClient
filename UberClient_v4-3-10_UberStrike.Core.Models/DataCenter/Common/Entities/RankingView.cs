// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.RankingView
// Assembly: UberStrike.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: E29887F9-C6F9-4A17-AD3C-0A827CA1DCD6
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Models.dll

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
