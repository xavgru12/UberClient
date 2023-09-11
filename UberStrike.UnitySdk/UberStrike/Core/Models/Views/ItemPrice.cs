// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Models.Views.ItemPrice
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

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
