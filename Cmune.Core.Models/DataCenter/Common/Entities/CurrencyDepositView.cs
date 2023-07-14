// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.CurrencyDepositView
// Assembly: Cmune.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: C9B63271-07DC-4C93-BD74-A807803DC1C2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Cmune.Core.Models.dll

using System;

namespace Cmune.DataCenter.Common.Entities
{
  [Serializable]
  public class CurrencyDepositView
  {
    public int CreditsDepositId { get; set; }

    public DateTime DepositDate { get; set; }

    public int Credits { get; set; }

    public int Points { get; set; }

    public Decimal Cash { get; set; }

    public string CurrencyLabel { get; set; }

    public int Cmid { get; set; }

    public bool IsAdminAction { get; set; }

    public PaymentProviderType PaymentProviderId { get; set; }

    public string TransactionKey { get; set; }

    public int ApplicationId { get; set; }

    public ChannelType ChannelId { get; set; }

    public Decimal UsdAmount { get; set; }

    public int? BundleId { get; set; }

    public string BundleName { get; set; }

    public CurrencyDepositView()
    {
    }

    public CurrencyDepositView(
      int creditsDepositId,
      DateTime depositDate,
      int credits,
      int points,
      Decimal cash,
      string currencyLabel,
      int cmid,
      bool isAdminAction,
      PaymentProviderType paymentProviderId,
      string transactionKey,
      int applicationId,
      ChannelType channelId,
      Decimal usdAmount,
      int? bundleId,
      string bundleName)
    {
      this.CreditsDepositId = creditsDepositId;
      this.DepositDate = depositDate;
      this.Credits = credits;
      this.Points = points;
      this.Cash = cash;
      this.CurrencyLabel = currencyLabel;
      this.Cmid = cmid;
      this.IsAdminAction = isAdminAction;
      this.PaymentProviderId = paymentProviderId;
      this.TransactionKey = transactionKey;
      this.ApplicationId = applicationId;
      this.ChannelId = channelId;
      this.UsdAmount = usdAmount;
      this.BundleId = bundleId;
      this.BundleName = bundleName;
    }
  }
}
