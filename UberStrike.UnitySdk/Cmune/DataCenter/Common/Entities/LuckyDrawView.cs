// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.LuckyDrawView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System.Collections.Generic;

namespace Cmune.DataCenter.Common.Entities
{
  public class LuckyDrawView
  {
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int Price { get; set; }

    public UberStrikeCurrencyType UberStrikeCurrencyType { get; set; }

    public string IconUrl { get; set; }

    public BundleCategoryType Category { get; set; }

    public bool IsAvailableInShop { get; set; }

    public List<LuckyDrawSetView> LuckyDrawSets { get; set; }
  }
}
