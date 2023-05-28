// Decompiled with JetBrains decompiler
// Type: GearItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Core.Models.Views;
using UnityEngine;

public class GearItem : BaseUnityItem
{
  [SerializeField]
  private GearItemConfiguration _config;

  public GearItemConfiguration Configuration
  {
    get => this._config;
    set => this._config = value;
  }

  public override BaseUberStrikeItemView ItemView => (BaseUberStrikeItemView) this.Configuration;
}
