// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.UberStrikeItemFunctionalViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
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
        if (instance.ItemProperties != null)
          DictionaryProxy<ItemPropertyType, int>.Serialize((Stream) bytes, instance.ItemProperties, new DictionaryProxy<ItemPropertyType, int>.Serializer<ItemPropertyType>(EnumProxy<ItemPropertyType>.Serialize), new DictionaryProxy<ItemPropertyType, int>.Serializer<int>(Int32Proxy.Serialize));
        else
          num |= 4;
        Int32Proxy.Serialize((Stream) bytes, instance.LevelLock);
        Int32Proxy.Serialize((Stream) bytes, instance.MaxDurationDays);
        if (instance.Name != null)
          StringProxy.Serialize((Stream) bytes, instance.Name);
        else
          num |= 8;
        if (instance.PrefabName != null)
          StringProxy.Serialize((Stream) bytes, instance.PrefabName);
        else
          num |= 16;
        if (instance.Prices != null)
          ListProxy<ItemPrice>.Serialize((Stream) bytes, instance.Prices, new ListProxy<ItemPrice>.Serializer<ItemPrice>(ItemPriceProxy.Serialize));
        else
          num |= 32;
        EnumProxy<ItemShopHighlightType>.Serialize((Stream) bytes, instance.ShopHighlightType);
        Int32Proxy.Serialize(stream, ~num);
        bytes.WriteTo(stream);
      }
    }

    public static UberStrikeItemFunctionalView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      UberStrikeItemFunctionalView itemFunctionalView = new UberStrikeItemFunctionalView();
      if ((num & 1) != 0)
        itemFunctionalView.CustomProperties = DictionaryProxy<string, string>.Deserialize(bytes, new DictionaryProxy<string, string>.Deserializer<string>(StringProxy.Deserialize), new DictionaryProxy<string, string>.Deserializer<string>(StringProxy.Deserialize));
      if ((num & 2) != 0)
        itemFunctionalView.Description = StringProxy.Deserialize(bytes);
      itemFunctionalView.ID = Int32Proxy.Deserialize(bytes);
      itemFunctionalView.IsConsumable = BooleanProxy.Deserialize(bytes);
      itemFunctionalView.ItemClass = EnumProxy<UberstrikeItemClass>.Deserialize(bytes);
      if ((num & 4) != 0)
        itemFunctionalView.ItemProperties = DictionaryProxy<ItemPropertyType, int>.Deserialize(bytes, new DictionaryProxy<ItemPropertyType, int>.Deserializer<ItemPropertyType>(EnumProxy<ItemPropertyType>.Deserialize), new DictionaryProxy<ItemPropertyType, int>.Deserializer<int>(Int32Proxy.Deserialize));
      itemFunctionalView.LevelLock = Int32Proxy.Deserialize(bytes);
      itemFunctionalView.MaxDurationDays = Int32Proxy.Deserialize(bytes);
      if ((num & 8) != 0)
        itemFunctionalView.Name = StringProxy.Deserialize(bytes);
      if ((num & 16) != 0)
        itemFunctionalView.PrefabName = StringProxy.Deserialize(bytes);
      if ((num & 32) != 0)
        itemFunctionalView.Prices = (ICollection<ItemPrice>) ListProxy<ItemPrice>.Deserialize(bytes, new ListProxy<ItemPrice>.Deserializer<ItemPrice>(ItemPriceProxy.Deserialize));
      itemFunctionalView.ShopHighlightType = EnumProxy<ItemShopHighlightType>.Deserialize(bytes);
      return itemFunctionalView;
    }
  }
}
