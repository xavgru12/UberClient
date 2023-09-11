// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.LuckyDrawSetView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System.Collections.Generic;

namespace Cmune.DataCenter.Common.Entities
{
  public class LuckyDrawSetView
  {
    public int Id { get; set; }

    public int SetWeight { get; set; }

    public int CreditsAttributed { get; set; }

    public int PointsAttributed { get; set; }

    public string ImageUrl { get; set; }

    public bool ExposeItemsToPlayers { get; set; }

    public int LuckyDrawId { get; set; }

    public List<LuckyDrawSetItemView> LuckyDrawSetItems { get; set; }
  }
}
