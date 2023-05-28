// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.ItemTransactionView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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
