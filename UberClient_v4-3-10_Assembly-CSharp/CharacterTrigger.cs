// Decompiled with JetBrains decompiler
// Type: CharacterTrigger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[RequireComponent(typeof (Collider))]
public class CharacterTrigger : MonoBehaviour
{
  [SerializeField]
  private AvatarHudInformation _hud;
  [SerializeField]
  private CharacterConfig _config;

  public AvatarHudInformation HudInfo => (Object) this._hud == (Object) null && (Object) this._config != (Object) null && (Object) this._config.Decorator != (Object) null ? this._config.Decorator.HudInformation : this._hud;

  public CharacterConfig Avatar => this._config;
}
