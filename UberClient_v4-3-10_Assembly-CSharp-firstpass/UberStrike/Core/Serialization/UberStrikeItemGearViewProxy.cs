// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.UberStrikeItemGearViewProxy
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
  public static class UberStrikeItemGearViewProxy
  {
    public static void Serialize(Stream stream, UberStrikeItemGearView instance)
    {
      int num = 0;
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, instance.ArmorPoints);
        Int32Proxy.Serialize((Stream) bytes, instance.ArmorWeight);
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

    public static UberStrikeItemGearView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      UberStrikeItemGearView strikeItemGearView = new UberStrikeItemGearView();
      strikeItemGearView.ArmorPoints = Int32Proxy.Deserialize(bytes);
      strikeItemGearView.ArmorWeight = Int32Proxy.Deserialize(bytes);
      if ((num & 1) != 0)
        strikeItemGearView.CustomProperties = DictionaryProxy<string, string>.Deserialize(bytes, new DictionaryProxy<string, string>.Deserializer<string>(StringProxy.Deserialize), new DictionaryProxy<string, string>.Deserializer<string>(StringProxy.Deserialize));
      if ((num & 2) != 0)
        strikeItemGearView.Description = StringProxy.Deserialize(bytes);
      strikeItemGearView.ID = Int32Proxy.Deserialize(bytes);
      strikeItemGearView.IsConsumable = BooleanProxy.Deserialize(bytes);
      strikeItemGearView.ItemClass = EnumProxy<UberstrikeItemClass>.Deserialize(bytes);
      if ((num & 4) != 0)
        strikeItemGearView.ItemProperties = DictionaryProxy<ItemPropertyType, int>.Deserialize(bytes, new DictionaryProxy<ItemPropertyType, int>.Deserializer<ItemPropertyType>(EnumProxy<ItemPropertyType>.Deserialize), new DictionaryProxy<ItemPropertyType, int>.Deserializer<int>(Int32Proxy.Deserialize));
      strikeItemGearView.LevelLock = Int32Proxy.Deserialize(bytes);
      strikeItemGearView.MaxDurationDays = Int32Proxy.Deserialize(bytes);
      if ((num & 8) != 0)
        strikeItemGearView.Name = StringProxy.Deserialize(bytes);
      if ((num & 16) != 0)
        strikeItemGearView.PrefabName = StringProxy.Deserialize(bytes);
      if ((num & 32) != 0)
        strikeItemGearView.Prices = (ICollection<ItemPrice>) ListProxy<ItemPrice>.Deserialize(bytes, new ListProxy<ItemPrice>.Deserializer<ItemPrice>(ItemPriceProxy.Deserialize));
      strikeItemGearView.ShopHighlightType = EnumProxy<ItemShopHighlightType>.Deserialize(bytes);
      return strikeItemGearView;
    }
  }
}
