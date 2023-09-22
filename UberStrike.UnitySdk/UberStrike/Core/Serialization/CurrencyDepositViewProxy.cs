
using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class CurrencyDepositViewProxy
  {
    public static void Serialize(Stream stream, CurrencyDepositView instance)
    {
      int num = 0;
      if (instance != null)
      {
        using (MemoryStream bytes1 = new MemoryStream())
        {
          Int32Proxy.Serialize((Stream) bytes1, instance.ApplicationId);
          int? bundleId = instance.BundleId;
          if (bundleId.HasValue)
          {
            MemoryStream bytes2 = bytes1;
            bundleId = instance.BundleId;
            int instance1 = bundleId ?? 0;
            Int32Proxy.Serialize((Stream) bytes2, instance1);
          }
          else
            num |= 1;
          if (instance.BundleName != null)
            StringProxy.Serialize((Stream) bytes1, instance.BundleName);
          else
            num |= 2;
          DecimalProxy.Serialize((Stream) bytes1, instance.Cash);
          EnumProxy<ChannelType>.Serialize((Stream) bytes1, instance.ChannelId);
          Int32Proxy.Serialize((Stream) bytes1, instance.Cmid);
          Int32Proxy.Serialize((Stream) bytes1, instance.Credits);
          Int32Proxy.Serialize((Stream) bytes1, instance.CreditsDepositId);
          if (instance.CurrencyLabel != null)
            StringProxy.Serialize((Stream) bytes1, instance.CurrencyLabel);
          else
            num |= 4;
          DateTimeProxy.Serialize((Stream) bytes1, instance.DepositDate);
          BooleanProxy.Serialize((Stream) bytes1, instance.IsAdminAction);
          EnumProxy<PaymentProviderType>.Serialize((Stream) bytes1, instance.PaymentProviderId);
          Int32Proxy.Serialize((Stream) bytes1, instance.Points);
          if (instance.TransactionKey != null)
            StringProxy.Serialize((Stream) bytes1, instance.TransactionKey);
          else
            num |= 8;
          DecimalProxy.Serialize((Stream) bytes1, instance.UsdAmount);
          Int32Proxy.Serialize(stream, ~num);
          bytes1.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static CurrencyDepositView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      CurrencyDepositView currencyDepositView = (CurrencyDepositView) null;
      if (num != 0)
      {
        currencyDepositView = new CurrencyDepositView();
        currencyDepositView.ApplicationId = Int32Proxy.Deserialize(bytes);
        if ((num & 1) != 0)
          currencyDepositView.BundleId = new int?(Int32Proxy.Deserialize(bytes));
        if ((num & 2) != 0)
          currencyDepositView.BundleName = StringProxy.Deserialize(bytes);
        currencyDepositView.Cash = DecimalProxy.Deserialize(bytes);
        currencyDepositView.ChannelId = EnumProxy<ChannelType>.Deserialize(bytes);
        currencyDepositView.Cmid = Int32Proxy.Deserialize(bytes);
        currencyDepositView.Credits = Int32Proxy.Deserialize(bytes);
        currencyDepositView.CreditsDepositId = Int32Proxy.Deserialize(bytes);
        if ((num & 4) != 0)
          currencyDepositView.CurrencyLabel = StringProxy.Deserialize(bytes);
        currencyDepositView.DepositDate = DateTimeProxy.Deserialize(bytes);
        currencyDepositView.IsAdminAction = BooleanProxy.Deserialize(bytes);
        currencyDepositView.PaymentProviderId = EnumProxy<PaymentProviderType>.Deserialize(bytes);
        currencyDepositView.Points = Int32Proxy.Deserialize(bytes);
        if ((num & 8) != 0)
          currencyDepositView.TransactionKey = StringProxy.Deserialize(bytes);
        currencyDepositView.UsdAmount = DecimalProxy.Deserialize(bytes);
      }
      return currencyDepositView;
    }
  }
}
