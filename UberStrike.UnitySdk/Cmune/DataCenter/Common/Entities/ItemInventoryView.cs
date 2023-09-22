
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
