// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.UberStrikeItemQuickViewProxy
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
  public static class UberStrikeItemQuickViewProxy
  {
    public static void Serialize(Stream stream, UberStrikeItemQuickView instance)
    {
      int num = 0;
      using (MemoryStream bytes = new MemoryStream())
      {
        EnumProxy<QuickItemLogic>.Serialize((Stream) bytes, instance.BehaviourType);
        Int32Proxy.Serialize((Stream) bytes, instance.CoolDownTime);
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
        Int32Proxy.Serialize((Stream) bytes, instance.MaxOwnableAmount);
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
        Int32Proxy.Serialize((Stream) bytes, instance.UsesPerGame);
        Int32Proxy.Serialize((Stream) bytes, instance.UsesPerLife);
        Int32Proxy.Serialize((Stream) bytes, instance.UsesPerRound);
        Int32Proxy.Serialize((Stream) bytes, instance.WarmUpTime);
        Int32Proxy.Serialize(stream, ~num);
        bytes.WriteTo(stream);
      }
    }

    public static UberStrikeItemQuickView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      UberStrikeItemQuickView strikeItemQuickView = new UberStrikeItemQuickView();
      strikeItemQuickView.BehaviourType = EnumProxy<QuickItemLogic>.Deserialize(bytes);
      strikeItemQuickView.CoolDownTime = Int32Proxy.Deserialize(bytes);
      if ((num & 1) != 0)
        strikeItemQuickView.CustomProperties = DictionaryProxy<string, string>.Deserialize(bytes, new DictionaryProxy<string, string>.Deserializer<string>(StringProxy.Deserialize), new DictionaryProxy<string, string>.Deserializer<string>(StringProxy.Deserialize));
      if ((num & 2) != 0)
        strikeItemQuickView.Description = StringProxy.Deserialize(bytes);
      strikeItemQuickView.ID = Int32Proxy.Deserialize(bytes);
      strikeItemQuickView.IsConsumable = BooleanProxy.Deserialize(bytes);
      strikeItemQuickView.ItemClass = EnumProxy<UberstrikeItemClass>.Deserialize(bytes);
      if ((num & 4) != 0)
        strikeItemQuickView.ItemProperties = DictionaryProxy<ItemPropertyType, int>.Deserialize(bytes, new DictionaryProxy<ItemPropertyType, int>.Deserializer<ItemPropertyType>(EnumProxy<ItemPropertyType>.Deserialize), new DictionaryProxy<ItemPropertyType, int>.Deserializer<int>(Int32Proxy.Deserialize));
      strikeItemQuickView.LevelLock = Int32Proxy.Deserialize(bytes);
      strikeItemQuickView.MaxDurationDays = Int32Proxy.Deserialize(bytes);
      strikeItemQuickView.MaxOwnableAmount = Int32Proxy.Deserialize(bytes);
      if ((num & 8) != 0)
        strikeItemQuickView.Name = StringProxy.Deserialize(bytes);
      if ((num & 16) != 0)
        strikeItemQuickView.PrefabName = StringProxy.Deserialize(bytes);
      if ((num & 32) != 0)
        strikeItemQuickView.Prices = (ICollection<ItemPrice>) ListProxy<ItemPrice>.Deserialize(bytes, new ListProxy<ItemPrice>.Deserializer<ItemPrice>(ItemPriceProxy.Deserialize));
      strikeItemQuickView.ShopHighlightType = EnumProxy<ItemShopHighlightType>.Deserialize(bytes);
      strikeItemQuickView.UsesPerGame = Int32Proxy.Deserialize(bytes);
      strikeItemQuickView.UsesPerLife = Int32Proxy.Deserialize(bytes);
      strikeItemQuickView.UsesPerRound = Int32Proxy.Deserialize(bytes);
      strikeItemQuickView.WarmUpTime = Int32Proxy.Deserialize(bytes);
      return strikeItemQuickView;
    }
  }
}
