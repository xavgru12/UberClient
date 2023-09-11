// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.ItemTransactionView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

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
