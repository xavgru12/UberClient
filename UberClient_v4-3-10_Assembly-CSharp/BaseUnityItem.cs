// Decompiled with JetBrains decompiler
// Type: BaseUnityItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;
using UnityEngine;

public abstract class BaseUnityItem : MonoBehaviour, IUnityItem
{
  [SerializeField]
  private Texture2D _icon;

  public abstract BaseUberStrikeItemView ItemView { get; }

  public Texture2D Icon
  {
    get => this._icon;
    set => this._icon = value;
  }

  public int ItemId => this.ItemView.ID;

  public string Name => this.ItemView.Name;

  public UberstrikeItemType ItemType => this.ItemView.ItemType;

  public UberstrikeItemClass ItemClass => this.ItemView.ItemClass;

  public string PrefabName => this.ItemView.PrefabName;

  public MonoBehaviour Prefab => (MonoBehaviour) this;
}
