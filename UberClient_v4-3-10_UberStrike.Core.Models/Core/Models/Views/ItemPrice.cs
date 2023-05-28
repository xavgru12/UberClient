// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Models.Views.ItemPrice
// Assembly: UberStrike.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: E29887F9-C6F9-4A17-AD3C-0A827CA1DCD6
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Models.dll

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
