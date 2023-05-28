// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.BundleViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System.Collections.Generic;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class BundleViewProxy
  {
    public static void Serialize(Stream stream, BundleView instance)
    {
      int num = 0;
      using (MemoryStream bytes = new MemoryStream())
      {
        if (instance.AndroidStoreUniqueId != null)
          StringProxy.Serialize((Stream) bytes, instance.AndroidStoreUniqueId);
        else
          num |= 1;
        Int32Proxy.Serialize((Stream) bytes, instance.ApplicationId);
        if (instance.Availability != null)
          ListProxy<ChannelType>.Serialize((Stream) bytes, (ICollection<ChannelType>) instance.Availability, new ListProxy<ChannelType>.Serializer<ChannelType>(EnumProxy<ChannelType>.Serialize));
        else
          num |= 2;
        if (instance.BundleItemViews != null)
          ListProxy<BundleItemView>.Serialize((Stream) bytes, (ICollection<BundleItemView>) instance.BundleItemViews, new ListProxy<BundleItemView>.Serializer<BundleItemView>(BundleItemViewProxy.Serialize));
        else
          num |= 4;
        EnumProxy<BundleCategoryType>.Serialize((Stream) bytes, instance.Category);
        Int32Proxy.Serialize((Stream) bytes, instance.Credits);
        if (instance.Description != null)
          StringProxy.Serialize((Stream) bytes, instance.Description);
        else
          num |= 8;
        if (instance.IconUrl != null)
          StringProxy.Serialize((Stream) bytes, instance.IconUrl);
        else
          num |= 16;
        Int32Proxy.Serialize((Stream) bytes, instance.Id);
        if (instance.ImageUrl != null)
          StringProxy.Serialize((Stream) bytes, instance.ImageUrl);
        else
          num |= 32;
        if (instance.IosAppStoreUniqueId != null)
          StringProxy.Serialize((Stream) bytes, instance.IosAppStoreUniqueId);
        else
          num |= 64;
        BooleanProxy.Serialize((Stream) bytes, instance.IsDefault);
        BooleanProxy.Serialize((Stream) bytes, instance.IsOnSale);
        BooleanProxy.Serialize((Stream) bytes, instance.IsPromoted);
        if (instance.MacAppStoreUniqueId != null)
          StringProxy.Serialize((Stream) bytes, instance.MacAppStoreUniqueId);
        else
          num |= 128;
        if (instance.Name != null)
          StringProxy.Serialize((Stream) bytes, instance.Name);
        else
          num |= 256;
        Int32Proxy.Serialize((Stream) bytes, instance.Points);
        if (instance.PromotionTag != null)
          StringProxy.Serialize((Stream) bytes, instance.PromotionTag);
        else
          num |= 512;
        DecimalProxy.Serialize((Stream) bytes, instance.USDPrice);
        DecimalProxy.Serialize((Stream) bytes, instance.USDPromoPrice);
        Int32Proxy.Serialize(stream, ~num);
        bytes.WriteTo(stream);
      }
    }

    public static BundleView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      BundleView bundleView = new BundleView();
      if ((num & 1) != 0)
        bundleView.AndroidStoreUniqueId = StringProxy.Deserialize(bytes);
      bundleView.ApplicationId = Int32Proxy.Deserialize(bytes);
      if ((num & 2) != 0)
        bundleView.Availability = ListProxy<ChannelType>.Deserialize(bytes, new ListProxy<ChannelType>.Deserializer<ChannelType>(EnumProxy<ChannelType>.Deserialize));
      if ((num & 4) != 0)
        bundleView.BundleItemViews = ListProxy<BundleItemView>.Deserialize(bytes, new ListProxy<BundleItemView>.Deserializer<BundleItemView>(BundleItemViewProxy.Deserialize));
      bundleView.Category = EnumProxy<BundleCategoryType>.Deserialize(bytes);
      bundleView.Credits = Int32Proxy.Deserialize(bytes);
      if ((num & 8) != 0)
        bundleView.Description = StringProxy.Deserialize(bytes);
      if ((num & 16) != 0)
        bundleView.IconUrl = StringProxy.Deserialize(bytes);
      bundleView.Id = Int32Proxy.Deserialize(bytes);
      if ((num & 32) != 0)
        bundleView.ImageUrl = StringProxy.Deserialize(bytes);
      if ((num & 64) != 0)
        bundleView.IosAppStoreUniqueId = StringProxy.Deserialize(bytes);
      bundleView.IsDefault = BooleanProxy.Deserialize(bytes);
      bundleView.IsOnSale = BooleanProxy.Deserialize(bytes);
      bundleView.IsPromoted = BooleanProxy.Deserialize(bytes);
      if ((num & 128) != 0)
        bundleView.MacAppStoreUniqueId = StringProxy.Deserialize(bytes);
      if ((num & 256) != 0)
        bundleView.Name = StringProxy.Deserialize(bytes);
      bundleView.Points = Int32Proxy.Deserialize(bytes);
      if ((num & 512) != 0)
        bundleView.PromotionTag = StringProxy.Deserialize(bytes);
      bundleView.USDPrice = DecimalProxy.Deserialize(bytes);
      bundleView.USDPromoPrice = DecimalProxy.Deserialize(bytes);
      return bundleView;
    }
  }
}
