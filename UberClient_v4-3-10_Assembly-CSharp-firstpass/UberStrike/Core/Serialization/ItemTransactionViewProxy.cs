// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.ItemTransactionViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class ItemTransactionViewProxy
  {
    public static void Serialize(Stream stream, ItemTransactionView instance)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, instance.Cmid);
        Int32Proxy.Serialize((Stream) bytes, instance.Credits);
        EnumProxy<BuyingDurationType>.Serialize((Stream) bytes, instance.Duration);
        BooleanProxy.Serialize((Stream) bytes, instance.IsAdminAction);
        Int32Proxy.Serialize((Stream) bytes, instance.ItemId);
        Int32Proxy.Serialize((Stream) bytes, instance.Points);
        DateTimeProxy.Serialize((Stream) bytes, instance.WithdrawalDate);
        Int32Proxy.Serialize((Stream) bytes, instance.WithdrawalId);
        bytes.WriteTo(stream);
      }
    }

    public static ItemTransactionView Deserialize(Stream bytes) => new ItemTransactionView()
    {
      Cmid = Int32Proxy.Deserialize(bytes),
      Credits = Int32Proxy.Deserialize(bytes),
      Duration = EnumProxy<BuyingDurationType>.Deserialize(bytes),
      IsAdminAction = BooleanProxy.Deserialize(bytes),
      ItemId = Int32Proxy.Deserialize(bytes),
      Points = Int32Proxy.Deserialize(bytes),
      WithdrawalDate = DateTimeProxy.Deserialize(bytes),
      WithdrawalId = Int32Proxy.Deserialize(bytes)
    };
  }
}
