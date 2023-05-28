// Decompiled with JetBrains decompiler
// Type: WeaponItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Core.Models.Views;
using UnityEngine;

public class WeaponItem : BaseUnityItem
{
  [SerializeField]
  private WeaponItemConfiguration _config;

  public WeaponItemConfiguration Configuration
  {
    get => this._config;
    set => this._config = value;
  }

  public override BaseUberStrikeItemView ItemView => (BaseUberStrikeItemView) this.Configuration;
}
