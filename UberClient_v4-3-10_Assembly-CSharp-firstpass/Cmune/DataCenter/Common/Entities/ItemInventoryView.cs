// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.ItemInventoryView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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

    public override string ToString() => "[LiveInventoryView: " + "[Item Id: " + this.ItemId.ToString() + "]" + "[Expiration date: " + this.ExpirationDate.ToString() + "]" + "[Amount remaining:" + this.AmountRemaining.ToString() + "]" + "]";
  }
}
