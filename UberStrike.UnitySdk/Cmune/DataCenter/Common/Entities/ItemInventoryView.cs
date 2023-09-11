// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.ItemInventoryView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System;

namespace Cmune.DataCenter.Common.Entities
{
  [Serializable]
  public class ItemInventoryView
  {
    public int Cmid { get; set; }

    public int ItemId { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public int AmountRemaining { get; set; }

    public ItemInventoryView()
    {
    }

    public ItemInventoryView(int itemId, DateTime? expirationDate, int amountRemaining)
    {
      this.ItemId = itemId;
      this.ExpirationDate = expirationDate;
      this.AmountRemaining = amountRemaining;
    }

    public ItemInventoryView(int itemId, DateTime? expirationDate, int amountRemaining, int cmid)
      : this(itemId, expirationDate, amountRemaining)
    {
      this.Cmid = cmid;
    }

    public override string ToString() => "[LiveInventoryView: " + "[Item Id: " + (object) this.ItemId + "]" + "[Expiration date: " + (object) this.ExpirationDate + "]" + "[Amount remaining:" + (object) this.AmountRemaining + "]" + "]";
  }
}
