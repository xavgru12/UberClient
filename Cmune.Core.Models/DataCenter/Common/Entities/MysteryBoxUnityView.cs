// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.MysteryBoxUnityView
// Assembly: Cmune.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: C9B63271-07DC-4C93-BD74-A807803DC1C2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Cmune.Core.Models.dll

using System;
using System.Collections.Generic;

namespace Cmune.DataCenter.Common.Entities
{
  [Serializable]
  public class MysteryBoxUnityView
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

    public List<BundleItemView> MysteryBoxItems { get; set; }
  }
}
