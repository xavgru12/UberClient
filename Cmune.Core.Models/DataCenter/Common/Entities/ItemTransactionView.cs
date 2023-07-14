// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.ItemTransactionView
// Assembly: Cmune.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: C9B63271-07DC-4C93-BD74-A807803DC1C2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Cmune.Core.Models.dll

using System;

namespace Cmune.DataCenter.Common.Entities
{
  [Serializable]
  public class ItemTransactionView
  {
    public int WithdrawalId { get; set; }

    public DateTime WithdrawalDate { get; set; }

    public int Points { get; set; }

    public int Credits { get; set; }

    public int Cmid { get; set; }

    public bool IsAdminAction { get; set; }

    public int ItemId { get; set; }

    public BuyingDurationType Duration { get; set; }

    public ItemTransactionView()
    {
    }

    public ItemTransactionView(
      int withdrawalId,
      DateTime withdrawalDate,
      int points,
      int credits,
      int cmid,
      bool isAdminAction,
      int itemId,
      BuyingDurationType duration)
    {
      this.WithdrawalId = withdrawalId;
      this.WithdrawalDate = withdrawalDate;
      this.Points = points;
      this.Credits = credits;
      this.Cmid = cmid;
      this.IsAdminAction = isAdminAction;
      this.ItemId = itemId;
      this.Duration = duration;
    }
  }
}
