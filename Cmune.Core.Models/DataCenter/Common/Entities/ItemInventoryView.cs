// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.ItemInventoryView
// Assembly: Cmune.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: C9B63271-07DC-4C93-BD74-A807803DC1C2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Cmune.Core.Models.dll

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
