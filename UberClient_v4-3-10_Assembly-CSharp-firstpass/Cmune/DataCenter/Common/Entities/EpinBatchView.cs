// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.EpinBatchView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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
