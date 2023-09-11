
using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class ItemTransactionViewProxy
  {
    public static void Serialize(Stream stream, ItemTransactionView instance)
    {
      int num = 0;
      if (instance != null)
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
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static ItemTransactionView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      ItemTransactionView itemTransactionView = (ItemTransactionView) null;
      if (num != 0)
      {
        itemTransactionView = new ItemTransactionView();
        itemTransactionView.Cmid = Int32Proxy.Deserialize(bytes);
        itemTransactionView.Credits = Int32Proxy.Deserialize(bytes);
        itemTransactionView.Duration = EnumProxy<BuyingDurationType>.Deserialize(bytes);
        itemTransactionView.IsAdminAction = BooleanProxy.Deserialize(bytes);
        itemTransactionView.ItemId = Int32Proxy.Deserialize(bytes);
        itemTransactionView.Points = Int32Proxy.Deserialize(bytes);
        itemTransactionView.WithdrawalDate = DateTimeProxy.Deserialize(bytes);
        itemTransactionView.WithdrawalId = Int32Proxy.Deserialize(bytes);
      }
      return itemTransactionView;
    }
  }
}
