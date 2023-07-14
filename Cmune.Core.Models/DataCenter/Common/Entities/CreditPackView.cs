// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.CreditPackView
// Assembly: Cmune.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: C9B63271-07DC-4C93-BD74-A807803DC1C2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Cmune.Core.Models.dll

using System;
using System.Collections.Generic;

namespace Cmune.DataCenter.Common.Entities
{
  public class CreditPackView
  {
    public int Id { get; set; }

    public string Name { get; set; }

    public string ImageUrl { get; set; }

    public string Description { get; set; }

    public int Bonus { get; set; }

    public int CmuneCredits { get; set; }

    public int TotalCCredits => this.Bonus + this.CmuneCredits;

    public Decimal USDPrice { get; set; }

    public bool OnSale { get; set; }

    public List<CreditPackItemView> CreditPackItemViews { get; set; }
  }
}
