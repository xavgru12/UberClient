// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.CurrencyDepositViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class CurrencyDepositViewProxy
  {
    public static void Serialize(Stream stream, CurrencyDepositView instance)
    {
      int num = 0;
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, instance.ApplicationId);
        if (instance.BundleId.HasValue)
        {
          int? bundleId = instance.BundleId;
          Int32Proxy.Serialize((Stream) bytes, bundleId.HasValue ? bundleId.Value : 0);
        }
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

    public static CurrencyDepositView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      CurrencyDepositView currencyDepositView = new CurrencyDepositView();
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
      return currencyDepositView;
    }
  }
}
