// Decompiled with JetBrains decompiler
// Type: HoloGearItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Core.Models.Views;
using UnityEngine;

public class HoloGearItem : BaseUnityItem
{
  [SerializeField]
  private HoloGearItemConfiguration _config;

  public HoloGearItemConfiguration Configuration
  {
    get => this._config;
    set => this._config = value;
  }

  public override BaseUberStrikeItemView ItemView => (BaseUberStrikeItemView) this.Configuration;
}
