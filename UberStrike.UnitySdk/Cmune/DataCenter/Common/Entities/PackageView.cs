
using System;
using System.Collections.Generic;

namespace Cmune.DataCenter.Common.Entities
{
  [Serializable]
  public class PackageView
  {
    public int Bonus { get; set; }

    public Decimal Price { get; set; }

    public List<int> Items { get; set; }

    public string Name { get; set; }

    public PackageView()
    {
      this.Bonus = 0;
      this.Price = 0M;
      this.Items = new List<int>();
      this.Name = string.Empty;
    }

    public PackageView(int bonus, Decimal price, List<int> items, string name)
    {
      this.Bonus = bonus;
      this.Price = price;
      this.Items = items;
      this.Name = name;
    }
  }
}
