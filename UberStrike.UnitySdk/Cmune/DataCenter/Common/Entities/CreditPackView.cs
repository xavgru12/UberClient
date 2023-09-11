﻿
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
