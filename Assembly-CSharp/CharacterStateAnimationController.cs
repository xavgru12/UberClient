// Decompiled with JetBrains decompiler
// Type: CharacterStateAnimationController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Core.Types;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class CharacterStateAnimationController
{
  private AnimationIndex _aimingMode = AnimationIndex.heavyGunUpDown;
  private bool _isJumping;

  public void Update(UberStrike.Realtime.UnitySdk.CharacterInfo state, AvatarAnimationController animation)
  {
    if (animation == null || state == null)
      return;
    this.RunPlayerConditions(state, animation);
    animation.UpdateAnimation();
  }

  private void RunPlayerConditions(UberStrike.Realtime.UnitySdk.CharacterInfo state, AvatarAnimationController animation)
  {
    if (this.IsCinematic || animation == null)
      return;
    if (state.Is(PlayerStates.PAUSED))
    {
      if (state.Is(PlayerStates.DUCKED))
        animation.PlayAnimation(AnimationIndex.squat);
      else if (state.CurrentWeaponID == 0)
        animation.PlayAnimation(AnimationIndex.idle);
      else if (state.CurrentWeaponSlot == (byte) 0)
        animation.PlayAnimation(AnimationIndex.ShopSmallGunAimIdle);
      else
        animation.PlayAnimation(AnimationIndex.heavyGunBreathe);
    }
    else
    {
      float speed = 1f;
      if (state.Is(PlayerStates.DIVING))
        animation.PlayAnimation(AnimationIndex.swimLoop, speed * 0.5f);
      else if (state.Is(PlayerStates.SWIMMING))
      {
        if (state.Is(PlayerStates.GROUNDED))
        {
          if (state.Keys == KeyState.Still)
          {
            if (state.CurrentWeaponSlot == (byte) 0)
              animation.PlayAnimation(AnimationIndex.ShopSmallGunAimIdle);
            else
              animation.PlayAnimation(AnimationIndex.heavyGunBreathe);
          }
          else
            animation.PlayAnimation(AnimationIndex.walk, speed);
        }
        else
          animation.PlayAnimation(AnimationIndex.swimLoop, speed);
        this._isJumping = false;
      }
      else
      {
        if ((double) state.Distance > 0.05 && state.Is(PlayerStates.GROUNDED))
        {
          if (state.Is(PlayerStates.DUCKED))
            animation.PlayAnimation(AnimationIndex.crouch, speed);
          else
            animation.PlayAnimation(AnimationIndex.run, speed);
        }
        else if (state.Is(PlayerStates.JUMPING))
        {
          animation.PlayAnimation(AnimationIndex.jumpUp);
          this._isJumping = true;
        }
        else if (state.Is(PlayerStates.DUCKED))
        {
          animation.PlayAnimation(AnimationIndex.squat);
          this._isJumping = false;
        }
        else
        {
          if (this._isJumping)
            animation.TriggerAnimation(AnimationIndex.jumpLand);
          if (state.CurrentWeaponSlot == (byte) 0)
            animation.PlayAnimation(AnimationIndex.ShopSmallGunAimIdle);
          else
            animation.PlayAnimation(AnimationIndex.heavyGunBreathe);
          this._isJumping = false;
        }
        if (state.IsFiring && state.CurrentWeaponCategory == UberstrikeItemClass.WeaponMelee && !this.IsPlayingMelee(animation))
        {
          if (Random.Range(0, 2) == 0)
            animation.TriggerAnimation(AnimationIndex.meleeSwingRightToLeft, 1.5f, false);
          else
            animation.TriggerAnimation(AnimationIndex.meleSwingLeftToRight, 1.5f, false);
        }
      }
      this.UpdateAimingMode(state, animation);
    }
  }

  private bool IsPlayingMelee(AvatarAnimationController animation) => animation.IsPlaying(AnimationIndex.meleeSwingRightToLeft) || animation.IsPlaying(AnimationIndex.meleSwingLeftToRight);

  private void UpdateAimingMode(UberStrike.Realtime.UnitySdk.CharacterInfo state, AvatarAnimationController animation)
  {
    if (state.CurrentFiringMode == FireMode.Secondary)
      this._aimingMode = AnimationIndex.snipeUpDown;
    else if (state.CurrentFiringMode == FireMode.Primary)
      this._aimingMode = AnimationIndex.heavyGunUpDown;
    animation.SetAnimationTimeNormalized(this._aimingMode, state.VerticalRotation);
  }

  public bool IsCinematic { get; set; }
}
