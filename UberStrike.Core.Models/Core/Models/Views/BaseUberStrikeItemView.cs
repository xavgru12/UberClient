// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Models.Views.BaseUberStrikeItemView
// Assembly: UberStrike.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: E29887F9-C6F9-4A17-AD3C-0A827CA1DCD6
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Models.dll

using System.Collections.Generic;
using UberStrike.Core.Types;
using UnityEngine;

namespace UberStrike.Core.Models.Views
{
  public abstract class BaseUberStrikeItemView
  {
    [SerializeField]
    private UberstrikeItemClass _itemClass;

    public abstract UberstrikeItemType ItemType { get; }

    public UberstrikeItemClass ItemClass
    {
      get => this._itemClass;
      set => this._itemClass = value;
    }

    public int ID { get; set; }

    public string Name { get; set; }

    public string PrefabName { get; set; }

    public string Description { get; set; }

    public int LevelLock { get; set; }

    public bool IsConsumable { get; set; }

    public ICollection<ItemPrice> Prices { get; set; }

    public bool IsForSale => this.Prices != null && this.Prices.Count > 0;

    public ItemShopHighlightType ShopHighlightType { get; set; }

    public Dictionary<string, string> CustomProperties { get; set; }
  }
}
