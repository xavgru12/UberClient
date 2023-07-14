// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.PackageView
// Assembly: Cmune.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: C9B63271-07DC-4C93-BD74-A807803DC1C2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Cmune.Core.Models.dll

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
