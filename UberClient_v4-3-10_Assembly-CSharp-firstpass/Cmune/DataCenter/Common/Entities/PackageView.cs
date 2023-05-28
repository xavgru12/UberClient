// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.PackageView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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
