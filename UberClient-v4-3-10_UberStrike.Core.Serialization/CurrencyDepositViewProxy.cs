// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.CurrencyDepositViewProxy
// Assembly: UberStrike.Core.Serialization, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: 950E20E9-3609-4E9B-B4D8-B32B07AB805E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Serialization.dll

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
        using (MemoryStream bytes = new MemoryStream())
        {
          Int32Proxy.Serialize((Stream) bytes, instance.ApplicationId);
          if (instance.BundleId.HasValue)
            Int32Proxy.Serialize((Stream) bytes, instance.BundleId ?? 0);
          else
            num |= 1;
          if (instance.BundleName != null)
            StringProxy.Serialize((Stream) bytes, instance.BundleName);
          else
            num |= 2;
          DecimalProxy.Serialize((Stream) bytes, instance.Cash);
          EnumProxy<ChannelType>.Serialize((Stream) bytes, instance.ChannelId);
          Int32Proxy.Serialize((Stream) bytes, instance.Cmid);
          Int32Proxy.Serialize((Stream) bytes, instance.Credits);
          Int32Proxy.Serialize((Stream) bytes, instance.CreditsDepositId);
          if (instance.CurrencyLabel != null)
            StringProxy.Serialize((Stream) bytes, instance.CurrencyLabel);
          else
            num |= 4;
          DateTimeProxy.Serialize((Stream) bytes, instance.DepositDate);
          BooleanProxy.Serialize((Stream) bytes, instance.IsAdminAction);
          EnumProxy<PaymentProviderType>.Serialize((Stream) bytes, instance.PaymentProviderId);
          Int32Proxy.Serialize((Stream) bytes, instance.Points);
          if (instance.TransactionKey != null)
            StringProxy.Serialize((Stream) bytes, instance.TransactionKey);
          else
            num |= 8;
          DecimalProxy.Serialize((Stream) bytes, instance.UsdAmount);
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
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
