// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Models.Views.BaseUberStrikeItemView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
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

    public int MaxDurationDays { get; set; }

    public bool IsConsumable { get; set; }

    public ICollection<ItemPrice> Prices { get; set; }

    public bool IsForSale => this.Prices != null && this.Prices.Count > 0;

    public ItemShopHighlightType ShopHighlightType { get; set; }

    public Dictionary<string, string> CustomProperties { get; set; }

    public Dictionary<ItemPropertyType, int> ItemProperties { get; set; }
  }
}
