// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.CreditPackView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

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
