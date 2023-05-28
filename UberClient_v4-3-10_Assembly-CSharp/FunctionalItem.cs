// Decompiled with JetBrains decompiler
// Type: FunctionalItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;
using UnityEngine;

public class FunctionalItem : IUnityItem
{
  private Texture2D _icon;

  public Texture2D Icon
  {
    get => this._icon;
    set => this._icon = value;
  }

  public int ItemId
  {
    get => this.ItemView.ID;
    set => this.ItemView.ID = value;
  }

  public string Name
  {
    get => this.ItemView.Name;
    set => this.ItemView.Name = value;
  }

  public UberstrikeItemType ItemType => this.ItemView.ItemType;

  public UberstrikeItemClass ItemClass => this.ItemView.ItemClass;

  public string PrefabName => string.Empty;

  public MonoBehaviour Prefab => (MonoBehaviour) null;

  public FunctionalItemConfiguration Configuration { get; set; }

  public BaseUberStrikeItemView ItemView => (BaseUberStrikeItemView) this.Configuration;
}
