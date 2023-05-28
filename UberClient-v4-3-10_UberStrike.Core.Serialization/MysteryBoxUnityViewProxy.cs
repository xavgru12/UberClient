// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.MysteryBoxUnityViewProxy
// Assembly: UberStrike.Core.Serialization, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: 950E20E9-3609-4E9B-B4D8-B32B07AB805E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Serialization.dll

using Cmune.DataCenter.Common.Entities;
using System.Collections.Generic;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class MysteryBoxUnityViewProxy
  {
    public static void Serialize(Stream stream, MysteryBoxUnityView instance)
    {
      int num = 0;
      if (instance != null)
      {
        using (MemoryStream bytes = new MemoryStream())
        {
          EnumProxy<BundleCategoryType>.Serialize((Stream) bytes, instance.Category);
          Int32Proxy.Serialize((Stream) bytes, instance.CreditsAttributed);
          Int32Proxy.Serialize((Stream) bytes, instance.CreditsAttributedWeight);
          if (instance.Description != null)
            StringProxy.Serialize((Stream) bytes, instance.Description);
          else
            num |= 1;
          BooleanProxy.Serialize((Stream) bytes, instance.ExposeItemsToPlayers);
          if (instance.IconUrl != null)
            StringProxy.Serialize((Stream) bytes, instance.IconUrl);
          else
            num |= 2;
          Int32Proxy.Serialize((Stream) bytes, instance.Id);
          if (instance.ImageUrl != null)
            StringProxy.Serialize((Stream) bytes, instance.ImageUrl);
          else
            num |= 4;
          BooleanProxy.Serialize((Stream) bytes, instance.IsAvailableInShop);
          Int32Proxy.Serialize((Stream) bytes, instance.ItemsAttributed);
          if (instance.MysteryBoxItems != null)
            ListProxy<BundleItemView>.Serialize((Stream) bytes, (ICollection<BundleItemView>) instance.MysteryBoxItems, new ListProxy<BundleItemView>.Serializer<BundleItemView>(BundleItemViewProxy.Serialize));
          else
            num |= 8;
          if (instance.Name != null)
            StringProxy.Serialize((Stream) bytes, instance.Name);
          else
            num |= 16;
          Int32Proxy.Serialize((Stream) bytes, instance.PointsAttributed);
          Int32Proxy.Serialize((Stream) bytes, instance.PointsAttributedWeight);
          Int32Proxy.Serialize((Stream) bytes, instance.Price);
          EnumProxy<UberStrikeCurrencyType>.Serialize((Stream) bytes, instance.UberStrikeCurrencyType);
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static MysteryBoxUnityView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      MysteryBoxUnityView mysteryBoxUnityView = (MysteryBoxUnityView) null;
      if (num != 0)
      {
        mysteryBoxUnityView = new MysteryBoxUnityView();
        mysteryBoxUnityView.Category = EnumProxy<BundleCategoryType>.Deserialize(bytes);
        mysteryBoxUnityView.CreditsAttributed = Int32Proxy.Deserialize(bytes);
        mysteryBoxUnityView.CreditsAttributedWeight = Int32Proxy.Deserialize(bytes);
        if ((num & 1) != 0)
          mysteryBoxUnityView.Description = StringProxy.Deserialize(bytes);
        mysteryBoxUnityView.ExposeItemsToPlayers = BooleanProxy.Deserialize(bytes);
        if ((num & 2) != 0)
          mysteryBoxUnityView.IconUrl = StringProxy.Deserialize(bytes);
        mysteryBoxUnityView.Id = Int32Proxy.Deserialize(bytes);
        if ((num & 4) != 0)
          mysteryBoxUnityView.ImageUrl = StringProxy.Deserialize(bytes);
        mysteryBoxUnityView.IsAvailableInShop = BooleanProxy.Deserialize(bytes);
        mysteryBoxUnityView.ItemsAttributed = Int32Proxy.Deserialize(bytes);
        if ((num & 8) != 0)
          mysteryBoxUnityView.MysteryBoxItems = ListProxy<BundleItemView>.Deserialize(bytes, new ListProxy<BundleItemView>.Deserializer<BundleItemView>(BundleItemViewProxy.Deserialize));
        if ((num & 16) != 0)
          mysteryBoxUnityView.Name = StringProxy.Deserialize(bytes);
        mysteryBoxUnityView.PointsAttributed = Int32Proxy.Deserialize(bytes);
        mysteryBoxUnityView.PointsAttributedWeight = Int32Proxy.Deserialize(bytes);
        mysteryBoxUnityView.Price = Int32Proxy.Deserialize(bytes);
        mysteryBoxUnityView.UberStrikeCurrencyType = EnumProxy<UberStrikeCurrencyType>.Deserialize(bytes);
      }
      return mysteryBoxUnityView;
    }
  }
}
