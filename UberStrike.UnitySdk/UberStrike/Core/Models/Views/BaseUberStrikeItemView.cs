// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Models.Views.BaseUberStrikeItemView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System.Collections.Generic;
using UberStrike.Core.Types;

namespace UberStrike.Core.Models.Views
{
  public abstract class BaseUberStrikeItemView
  {
    public abstract UberstrikeItemType ItemType { get; }

    public UberstrikeItemClass ItemClass { get; set; }

    public int ID { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int LevelLock { get; set; }

    public bool IsConsumable { get; set; }

    public ICollection<ItemPrice> Prices { get; set; }

    public bool IsForSale => this.Prices != null && this.Prices.Count > 0;

    public ItemShopHighlightType ShopHighlightType { get; set; }

    public Dictionary<string, string> CustomProperties { get; set; }
  }
}
