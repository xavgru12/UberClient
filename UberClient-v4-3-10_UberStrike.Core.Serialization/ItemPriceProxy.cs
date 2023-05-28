// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.ItemPriceProxy
// Assembly: UberStrike.Core.Serialization, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: 950E20E9-3609-4E9B-B4D8-B32B07AB805E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Serialization.dll

using Cmune.DataCenter.Common.Entities;
using System.IO;
using UberStrike.Core.Models.Views;

namespace UberStrike.Core.Serialization
{
  public static class ItemPriceProxy
  {
    public static void Serialize(Stream stream, ItemPrice instance)
    {
      int num = 0;
      if (instance != null)
      {
        using (MemoryStream bytes = new MemoryStream())
        {
          Int32Proxy.Serialize((Stream) bytes, instance.Amount);
          EnumProxy<UberStrikeCurrencyType>.Serialize((Stream) bytes, instance.Currency);
          Int32Proxy.Serialize((Stream) bytes, instance.Discount);
          EnumProxy<BuyingDurationType>.Serialize((Stream) bytes, instance.Duration);
          EnumProxy<PackType>.Serialize((Stream) bytes, instance.PackType);
          Int32Proxy.Serialize((Stream) bytes, instance.Price);
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static ItemPrice Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      ItemPrice itemPrice = (ItemPrice) null;
      if (num != 0)
      {
        itemPrice = new ItemPrice();
        itemPrice.Amount = Int32Proxy.Deserialize(bytes);
        itemPrice.Currency = EnumProxy<UberStrikeCurrencyType>.Deserialize(bytes);
        itemPrice.Discount = Int32Proxy.Deserialize(bytes);
        itemPrice.Duration = EnumProxy<BuyingDurationType>.Deserialize(bytes);
        itemPrice.PackType = EnumProxy<PackType>.Deserialize(bytes);
        itemPrice.Price = Int32Proxy.Deserialize(bytes);
      }
      return itemPrice;
    }
  }
}
