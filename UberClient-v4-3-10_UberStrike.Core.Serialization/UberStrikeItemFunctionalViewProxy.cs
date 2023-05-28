// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.UberStrikeItemFunctionalViewProxy
// Assembly: UberStrike.Core.Serialization, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: 950E20E9-3609-4E9B-B4D8-B32B07AB805E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Serialization.dll

using System.Collections.Generic;
using System.IO;
using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;

namespace UberStrike.Core.Serialization
{
  public static class UberStrikeItemFunctionalViewProxy
  {
    public static void Serialize(Stream stream, UberStrikeItemFunctionalView instance)
    {
      int num = 0;
      if (instance != null)
      {
        using (MemoryStream bytes = new MemoryStream())
        {
          if (instance.CustomProperties != null)
            DictionaryProxy<string, string>.Serialize((Stream) bytes, instance.CustomProperties, new DictionaryProxy<string, string>.Serializer<string>(StringProxy.Serialize), new DictionaryProxy<string, string>.Serializer<string>(StringProxy.Serialize));
          else
            num |= 1;
          if (instance.Description != null)
            StringProxy.Serialize((Stream) bytes, instance.Description);
          else
            num |= 2;
          Int32Proxy.Serialize((Stream) bytes, instance.ID);
          BooleanProxy.Serialize((Stream) bytes, instance.IsConsumable);
          EnumProxy<UberstrikeItemClass>.Serialize((Stream) bytes, instance.ItemClass);
          Int32Proxy.Serialize((Stream) bytes, instance.LevelLock);
          if (instance.Name != null)
            StringProxy.Serialize((Stream) bytes, instance.Name);
          else
            num |= 4;
          if (instance.PrefabName != null)
            StringProxy.Serialize((Stream) bytes, instance.PrefabName);
          else
            num |= 8;
          if (instance.Prices != null)
            ListProxy<ItemPrice>.Serialize((Stream) bytes, instance.Prices, new ListProxy<ItemPrice>.Serializer<ItemPrice>(ItemPriceProxy.Serialize));
          else
            num |= 16;
          EnumProxy<ItemShopHighlightType>.Serialize((Stream) bytes, instance.ShopHighlightType);
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static UberStrikeItemFunctionalView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      UberStrikeItemFunctionalView itemFunctionalView = (UberStrikeItemFunctionalView) null;
      if (num != 0)
      {
        itemFunctionalView = new UberStrikeItemFunctionalView();
        if ((num & 1) != 0)
          itemFunctionalView.CustomProperties = DictionaryProxy<string, string>.Deserialize(bytes, new DictionaryProxy<string, string>.Deserializer<string>(StringProxy.Deserialize), new DictionaryProxy<string, string>.Deserializer<string>(StringProxy.Deserialize));
        if ((num & 2) != 0)
          itemFunctionalView.Description = StringProxy.Deserialize(bytes);
        itemFunctionalView.ID = Int32Proxy.Deserialize(bytes);
        itemFunctionalView.IsConsumable = BooleanProxy.Deserialize(bytes);
        itemFunctionalView.ItemClass = EnumProxy<UberstrikeItemClass>.Deserialize(bytes);
        itemFunctionalView.LevelLock = Int32Proxy.Deserialize(bytes);
        if ((num & 4) != 0)
          itemFunctionalView.Name = StringProxy.Deserialize(bytes);
        if ((num & 8) != 0)
          itemFunctionalView.PrefabName = StringProxy.Deserialize(bytes);
        if ((num & 16) != 0)
          itemFunctionalView.Prices = (ICollection<ItemPrice>) ListProxy<ItemPrice>.Deserialize(bytes, new ListProxy<ItemPrice>.Deserializer<ItemPrice>(ItemPriceProxy.Deserialize));
        itemFunctionalView.ShopHighlightType = EnumProxy<ItemShopHighlightType>.Deserialize(bytes);
      }
      return itemFunctionalView;
    }
  }
}
