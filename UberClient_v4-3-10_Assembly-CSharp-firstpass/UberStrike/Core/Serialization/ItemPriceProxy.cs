// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.ItemPriceProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System.IO;
using UberStrike.Core.Models.Views;

namespace UberStrike.Core.Serialization
{
  public static class ItemPriceProxy
  {
    public static void Serialize(Stream stream, ItemPrice instance)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, instance.Amount);
        EnumProxy<UberStrikeCurrencyType>.Serialize((Stream) bytes, instance.Currency);
        Int32Proxy.Serialize((Stream) bytes, instance.Discount);
        EnumProxy<BuyingDurationType>.Serialize((Stream) bytes, instance.Duration);
        EnumProxy<PackType>.Serialize((Stream) bytes, instance.PackType);
        Int32Proxy.Serialize((Stream) bytes, instance.Price);
        bytes.WriteTo(stream);
      }
    }

    public static ItemPrice Deserialize(Stream bytes) => new ItemPrice()
    {
      Amount = Int32Proxy.Deserialize(bytes),
      Currency = EnumProxy<UberStrikeCurrencyType>.Deserialize(bytes),
      Discount = Int32Proxy.Deserialize(bytes),
      Duration = EnumProxy<BuyingDurationType>.Deserialize(bytes),
      PackType = EnumProxy<PackType>.Deserialize(bytes),
      Price = Int32Proxy.Deserialize(bytes)
    };
  }
}
