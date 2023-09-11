
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
      if (instance != null)
      {
        using (MemoryStream bytes = new MemoryStream())
        {
          Int32Proxy.Serialize((Stream) bytes, instance.ArmorAbsorptionPercent);
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
          Int32Proxy.Serialize((Stream) bytes, instance.LevelLock);
          if (instance.Name != null)
            StringProxy.Serialize((Stream) bytes, instance.Name);
          else
            num |= 4;
          if (instance.Prices != null)
            ListProxy<ItemPrice>.Serialize((Stream) bytes, instance.Prices, new ListProxy<ItemPrice>.Serializer<ItemPrice>(ItemPriceProxy.Serialize));
          else
            num |= 8;
          EnumProxy<ItemShopHighlightType>.Serialize((Stream) bytes, instance.ShopHighlightType);
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static UberStrikeItemGearView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      UberStrikeItemGearView strikeItemGearView = (UberStrikeItemGearView) null;
      if (num != 0)
      {
        strikeItemGearView = new UberStrikeItemGearView();
        strikeItemGearView.ArmorAbsorptionPercent = Int32Proxy.Deserialize(bytes);
        strikeItemGearView.ArmorPoints = Int32Proxy.Deserialize(bytes);
        strikeItemGearView.ArmorWeight = Int32Proxy.Deserialize(bytes);
        if ((num & 1) != 0)
          strikeItemGearView.CustomProperties = DictionaryProxy<string, string>.Deserialize(bytes, new DictionaryProxy<string, string>.Deserializer<string>(StringProxy.Deserialize), new DictionaryProxy<string, string>.Deserializer<string>(StringProxy.Deserialize));
        if ((num & 2) != 0)
          strikeItemGearView.Description = StringProxy.Deserialize(bytes);
        strikeItemGearView.ID = Int32Proxy.Deserialize(bytes);
        strikeItemGearView.IsConsumable = BooleanProxy.Deserialize(bytes);
        strikeItemGearView.ItemClass = EnumProxy<UberstrikeItemClass>.Deserialize(bytes);
        strikeItemGearView.LevelLock = Int32Proxy.Deserialize(bytes);
        if ((num & 4) != 0)
          strikeItemGearView.Name = StringProxy.Deserialize(bytes);
        if ((num & 8) != 0)
          strikeItemGearView.Prices = (ICollection<ItemPrice>) ListProxy<ItemPrice>.Deserialize(bytes, new ListProxy<ItemPrice>.Deserializer<ItemPrice>(ItemPriceProxy.Deserialize));
        strikeItemGearView.ShopHighlightType = EnumProxy<ItemShopHighlightType>.Deserialize(bytes);
      }
      return strikeItemGearView;
    }
  }
}
