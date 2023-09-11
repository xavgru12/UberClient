// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.UberStrikeItemQuickViewProxy
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

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
      if (instance != null)
      {
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
          Int32Proxy.Serialize((Stream) bytes, instance.LevelLock);
          Int32Proxy.Serialize((Stream) bytes, instance.MaxOwnableAmount);
          if (instance.Name != null)
            StringProxy.Serialize((Stream) bytes, instance.Name);
          else
            num |= 4;
          if (instance.Prices != null)
            ListProxy<ItemPrice>.Serialize((Stream) bytes, instance.Prices, new ListProxy<ItemPrice>.Serializer<ItemPrice>(ItemPriceProxy.Serialize));
          else
            num |= 8;
          EnumProxy<ItemShopHighlightType>.Serialize((Stream) bytes, instance.ShopHighlightType);
          Int32Proxy.Serialize((Stream) bytes, instance.UsesPerGame);
          Int32Proxy.Serialize((Stream) bytes, instance.UsesPerLife);
          Int32Proxy.Serialize((Stream) bytes, instance.UsesPerRound);
          Int32Proxy.Serialize((Stream) bytes, instance.WarmUpTime);
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static UberStrikeItemQuickView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      UberStrikeItemQuickView strikeItemQuickView = (UberStrikeItemQuickView) null;
      if (num != 0)
      {
        strikeItemQuickView = new UberStrikeItemQuickView();
        strikeItemQuickView.BehaviourType = EnumProxy<QuickItemLogic>.Deserialize(bytes);
        strikeItemQuickView.CoolDownTime = Int32Proxy.Deserialize(bytes);
        if ((num & 1) != 0)
          strikeItemQuickView.CustomProperties = DictionaryProxy<string, string>.Deserialize(bytes, new DictionaryProxy<string, string>.Deserializer<string>(StringProxy.Deserialize), new DictionaryProxy<string, string>.Deserializer<string>(StringProxy.Deserialize));
        if ((num & 2) != 0)
          strikeItemQuickView.Description = StringProxy.Deserialize(bytes);
        strikeItemQuickView.ID = Int32Proxy.Deserialize(bytes);
        strikeItemQuickView.IsConsumable = BooleanProxy.Deserialize(bytes);
        strikeItemQuickView.ItemClass = EnumProxy<UberstrikeItemClass>.Deserialize(bytes);
        strikeItemQuickView.LevelLock = Int32Proxy.Deserialize(bytes);
        strikeItemQuickView.MaxOwnableAmount = Int32Proxy.Deserialize(bytes);
        if ((num & 4) != 0)
          strikeItemQuickView.Name = StringProxy.Deserialize(bytes);
        if ((num & 8) != 0)
          strikeItemQuickView.Prices = (ICollection<ItemPrice>) ListProxy<ItemPrice>.Deserialize(bytes, new ListProxy<ItemPrice>.Deserializer<ItemPrice>(ItemPriceProxy.Deserialize));
        strikeItemQuickView.ShopHighlightType = EnumProxy<ItemShopHighlightType>.Deserialize(bytes);
        strikeItemQuickView.UsesPerGame = Int32Proxy.Deserialize(bytes);
        strikeItemQuickView.UsesPerLife = Int32Proxy.Deserialize(bytes);
        strikeItemQuickView.UsesPerRound = Int32Proxy.Deserialize(bytes);
        strikeItemQuickView.WarmUpTime = Int32Proxy.Deserialize(bytes);
      }
      return strikeItemQuickView;
    }
  }
}
