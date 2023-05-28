// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.MysteryBoxView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;

namespace Cmune.DataCenter.Common.Entities
{
  public class MysteryBoxView
  {
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int Price { get; set; }

    public UberStrikeCurrencyType UberStrikeCurrencyType { get; set; }

    public string IconUrl { get; set; }

    public BundleCategoryType Category { get; set; }

    public bool IsAvailableInShop { get; set; }

    public int ItemsAttributed { get; set; }

    public string ImageUrl { get; set; }

    public bool ExposeItemsToPlayers { get; set; }

    public int PointsAttributed { get; set; }

    public int PointsAttributedWeight { get; set; }

    public int CreditsAttributed { get; set; }

    public int CreditsAttributedWeight { get; set; }

    public List<MysteryBoxItemView> MysteryBoxItems { get; set; }
  }
}
