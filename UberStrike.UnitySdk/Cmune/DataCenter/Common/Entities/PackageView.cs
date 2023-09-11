// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.PackageView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

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
