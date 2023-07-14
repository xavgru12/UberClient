// Decompiled with JetBrains decompiler
// Type: PlayerSound
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class PlayerSound
{
  private UberStrike.Realtime.UnitySdk.CharacterInfo _character;
  private CharacterConfig _config;

  public PlayerSound(UberStrike.Realtime.UnitySdk.CharacterInfo character) => this._character = character;

  public void SetCharacter(CharacterConfig config) => this._config = config;

  public void Update()
  {
    if ((Object) this._config == (Object) null || (Object) this._config.Decorator == (Object) null)
      return;
    bool flag1 = (this._character.PlayerState & (PlayerStates.GROUNDED | PlayerStates.WADING | PlayerStates.SWIMMING | PlayerStates.DIVING)) != PlayerStates.IDLE;
    bool flag2 = this._character.Is(PlayerStates.DIVING) && this._character.Keys != KeyState.Still || (this._character.Keys & KeyState.Walking) != KeyState.Still;
    if (!flag1 || !flag2 || !this._config.Decorator.CanPlayFootSound)
      return;
    if (this._character.Is(PlayerStates.WADING))
      this._config.Decorator.PlayFootSound(FootStepSoundType.Water, this._config.WalkingSoundSpeed);
    else if (this._character.Is(PlayerStates.SWIMMING))
      this._config.Decorator.PlayFootSound(FootStepSoundType.Swim, this._config.WalkingSoundSpeed);
    else if (this._character.Is(PlayerStates.DIVING))
      this._config.Decorator.PlayFootSound(FootStepSoundType.Dive, this._config.WalkingSoundSpeed);
    else
      this._config.Decorator.PlayFootSound(this._config.WalkingSoundSpeed);
  }
}
