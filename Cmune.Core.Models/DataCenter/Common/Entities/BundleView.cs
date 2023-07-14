// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.BundleView
// Assembly: Cmune.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: C9B63271-07DC-4C93-BD74-A807803DC1C2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Cmune.Core.Models.dll

using System;
using System.Collections.Generic;

namespace Cmune.DataCenter.Common.Entities
{
  [Serializable]
  public class BundleView
  {
    public int Id { get; set; }

    public int ApplicationId { get; set; }

    public string Name { get; set; }

    public string ImageUrl { get; set; }

    public string IconUrl { get; set; }

    public string Description { get; set; }

    public bool IsOnSale { get; set; }

    public bool IsPromoted { get; set; }

    public Decimal USDPrice { get; set; }

    public Decimal USDPromoPrice { get; set; }

    public int Credits { get; set; }

    public int Points { get; set; }

    public List<BundleItemView> BundleItemViews { get; set; }

    public BundleCategoryType Category { get; set; }

    public List<ChannelType> Availability { get; set; }

    public string PromotionTag { get; set; }

    public string MacAppStoreUniqueId { get; set; }

    public string IosAppStoreUniqueId { get; set; }

    public string AndroidStoreUniqueId { get; set; }

    public bool IsDefault { get; set; }
  }
}
