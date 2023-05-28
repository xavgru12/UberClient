// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.UberStrikeItemShopClientViewProxy
// Assembly: UberStrike.Core.Serialization, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: 950E20E9-3609-4E9B-B4D8-B32B07AB805E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Serialization.dll

using System.Collections.Generic;
using System.IO;
using UberStrike.Core.Models.Views;

namespace UberStrike.Core.Serialization
{
  public static class UberStrikeItemShopClientViewProxy
  {
    public static void Serialize(Stream stream, UberStrikeItemShopClientView instance)
    {
      int num = 0;
      if (instance != null)
      {
        using (MemoryStream bytes = new MemoryStream())
        {
          if (instance.FunctionalItems != null)
            ListProxy<UberStrikeItemFunctionalView>.Serialize((Stream) bytes, (ICollection<UberStrikeItemFunctionalView>) instance.FunctionalItems, new ListProxy<UberStrikeItemFunctionalView>.Serializer<UberStrikeItemFunctionalView>(UberStrikeItemFunctionalViewProxy.Serialize));
          else
            num |= 1;
          if (instance.GearItems != null)
            ListProxy<UberStrikeItemGearView>.Serialize((Stream) bytes, (ICollection<UberStrikeItemGearView>) instance.GearItems, new ListProxy<UberStrikeItemGearView>.Serializer<UberStrikeItemGearView>(UberStrikeItemGearViewProxy.Serialize));
          else
            num |= 2;
          if (instance.ItemsRecommendationPerMap != null)
            DictionaryProxy<int, int>.Serialize((Stream) bytes, instance.ItemsRecommendationPerMap, new DictionaryProxy<int, int>.Serializer<int>(Int32Proxy.Serialize), new DictionaryProxy<int, int>.Serializer<int>(Int32Proxy.Serialize));
          else
            num |= 4;
          if (instance.QuickItems != null)
            ListProxy<UberStrikeItemQuickView>.Serialize((Stream) bytes, (ICollection<UberStrikeItemQuickView>) instance.QuickItems, new ListProxy<UberStrikeItemQuickView>.Serializer<UberStrikeItemQuickView>(UberStrikeItemQuickViewProxy.Serialize));
          else
            num |= 8;
          if (instance.WeaponItems != null)
            ListProxy<UberStrikeItemWeaponView>.Serialize((Stream) bytes, (ICollection<UberStrikeItemWeaponView>) instance.WeaponItems, new ListProxy<UberStrikeItemWeaponView>.Serializer<UberStrikeItemWeaponView>(UberStrikeItemWeaponViewProxy.Serialize));
          else
            num |= 16;
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static UberStrikeItemShopClientView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      UberStrikeItemShopClientView itemShopClientView = (UberStrikeItemShopClientView) null;
      if (num != 0)
      {
        itemShopClientView = new UberStrikeItemShopClientView();
        if ((num & 1) != 0)
          itemShopClientView.FunctionalItems = ListProxy<UberStrikeItemFunctionalView>.Deserialize(bytes, new ListProxy<UberStrikeItemFunctionalView>.Deserializer<UberStrikeItemFunctionalView>(UberStrikeItemFunctionalViewProxy.Deserialize));
        if ((num & 2) != 0)
          itemShopClientView.GearItems = ListProxy<UberStrikeItemGearView>.Deserialize(bytes, new ListProxy<UberStrikeItemGearView>.Deserializer<UberStrikeItemGearView>(UberStrikeItemGearViewProxy.Deserialize));
        if ((num & 4) != 0)
          itemShopClientView.ItemsRecommendationPerMap = DictionaryProxy<int, int>.Deserialize(bytes, new DictionaryProxy<int, int>.Deserializer<int>(Int32Proxy.Deserialize), new DictionaryProxy<int, int>.Deserializer<int>(Int32Proxy.Deserialize));
        if ((num & 8) != 0)
          itemShopClientView.QuickItems = ListProxy<UberStrikeItemQuickView>.Deserialize(bytes, new ListProxy<UberStrikeItemQuickView>.Deserializer<UberStrikeItemQuickView>(UberStrikeItemQuickViewProxy.Deserialize));
        if ((num & 16) != 0)
          itemShopClientView.WeaponItems = ListProxy<UberStrikeItemWeaponView>.Deserialize(bytes, new ListProxy<UberStrikeItemWeaponView>.Deserializer<UberStrikeItemWeaponView>(UberStrikeItemWeaponViewProxy.Deserialize));
      }
      return itemShopClientView;
    }
  }
}
