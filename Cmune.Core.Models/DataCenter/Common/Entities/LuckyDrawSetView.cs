// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.LuckyDrawSetView
// Assembly: Cmune.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: C9B63271-07DC-4C93-BD74-A807803DC1C2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Cmune.Core.Models.dll

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
