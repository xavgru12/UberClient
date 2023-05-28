// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Models.Views.ItemPrice
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System;

namespace UberStrike.Core.Models.Views
{
  [Serializable]
  public class ItemPrice
  {
    public int Price { get; set; }

    public UberStrikeCurrencyType Currency { get; set; }

    public int Discount { get; set; }

    public int Amount { get; set; }

    public PackType PackType { get; set; }

    public BuyingDurationType Duration { get; set; }

    public bool IsConsumable => this.Amount > 0;
  }
}
