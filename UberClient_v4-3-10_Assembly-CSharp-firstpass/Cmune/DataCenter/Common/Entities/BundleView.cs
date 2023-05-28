// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.BundleView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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
