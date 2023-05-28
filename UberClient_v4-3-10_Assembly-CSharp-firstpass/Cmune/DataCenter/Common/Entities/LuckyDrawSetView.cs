// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.LuckyDrawSetView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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
