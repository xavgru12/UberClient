// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.EpinBatchView
// Assembly: Cmune.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: C9B63271-07DC-4C93-BD74-A807803DC1C2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Cmune.Core.Models.dll

using System;
using System.Collections.Generic;

namespace Cmune.DataCenter.Common.Entities
{
  public class EpinBatchView
  {
    public int BatchId { get; private set; }

    public int ApplicationId { get; private set; }

    public PaymentProviderType EpinProvider { get; private set; }

    public int Amount { get; private set; }

    public int CreditAmount { get; private set; }

    public DateTime BatchDate { get; private set; }

    public bool IsAdmin { get; private set; }

    public bool IsRetired { get; private set; }

    public List<EpinView> Epins { get; private set; }

    public EpinBatchView(
      int batchId,
      int applicationId,
      PaymentProviderType epinProvider,
      int amount,
      int creditAmount,
      DateTime batchDate,
      bool isAdmin,
      bool isRetired,
      List<EpinView> epins)
    {
      this.BatchId = batchId;
      this.ApplicationId = applicationId;
      this.EpinProvider = epinProvider;
      this.Amount = amount;
      this.CreditAmount = creditAmount;
      this.BatchDate = batchDate;
      this.IsAdmin = isAdmin;
      this.Epins = epins;
      this.IsRetired = isRetired;
    }
  }
}
