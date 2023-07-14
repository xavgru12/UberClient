// Decompiled with JetBrains decompiler
// Type: AvatarAnimationManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UberStrike.Core.Types;
using UnityEngine;

public class AvatarAnimationManager : AutoMonoBehaviour<AvatarAnimationManager>
{
  private Dictionary<AvatarAnimationManager.AnimationState, AnimationIndex[]> _homeAnimations = new Dictionary<AvatarAnimationManager.AnimationState, AnimationIndex[]>();
  private Dictionary<AvatarAnimationManager.AnimationState, AnimationIndex[]> _shopAnimations = new Dictionary<AvatarAnimationManager.AnimationState, AnimationIndex[]>();
  private float _nextAnimationTime;
  private Dictionary<AvatarAnimationManager.AnimationState, AnimationIndex[]> _currentSet;
  private AvatarAnimationManager.AnimationState _currentState;

  private void Awake()
  {
    AnimationIndex[] animationIndexArray1 = new AnimationIndex[3]
    {
      AnimationIndex.HomeNoWeaponIdle,
      AnimationIndex.HomeNoWeaponnLookAround,
      AnimationIndex.HomeNoWeaponRelaxNeck
    };
    AnimationIndex[] animationIndexArray2 = new AnimationIndex[4]
    {
      AnimationIndex.HomeMeleeIdle,
      AnimationIndex.HomeMeleeCheckWeapon,
      AnimationIndex.HomeMeleeLookAround,
      AnimationIndex.HomeMeleeRelaxNeck
    };
    AnimationIndex[] animationIndexArray3 = new AnimationIndex[4]
    {
      AnimationIndex.HomeSmallGunIdle,
      AnimationIndex.HomeSmallGunCheckWeapon,
      AnimationIndex.HomeSmallGunLookAround,
      AnimationIndex.HomeSmallGunRelaxNeck
    };
    AnimationIndex[] animationIndexArray4 = new AnimationIndex[4]
    {
      AnimationIndex.HomeMediumGunIdle,
      AnimationIndex.HomeMediumGunCheckWeapon,
      AnimationIndex.HomeMediumGunLookAround,
      AnimationIndex.HomeMediumGunRelaxNeck
    };
    AnimationIndex[] animationIndexArray5 = new AnimationIndex[5]
    {
      AnimationIndex.HomeLargeGunIdle,
      AnimationIndex.HomeLargeGunCheckWeapon,
      AnimationIndex.HomeLargeGunLookAround,
      AnimationIndex.HomeLargeGunRelaxNeck,
      AnimationIndex.HomeLargeGunShakeWeapon
    };
    this._homeAnimations.Add(AvatarAnimationManager.AnimationState.Idle, animationIndexArray1);
    this._homeAnimations.Add(AvatarAnimationManager.AnimationState.Melee, animationIndexArray2);
    this._homeAnimations.Add(AvatarAnimationManager.AnimationState.SmallGun, animationIndexArray3);
    this._homeAnimations.Add(AvatarAnimationManager.AnimationState.MediumGun, animationIndexArray4);
    this._homeAnimations.Add(AvatarAnimationManager.AnimationState.HeavyGun, animationIndexArray5);
    this._shopAnimations.Add(AvatarAnimationManager.AnimationState.Idle, animationIndexArray1);
    this._shopAnimations.Add(AvatarAnimationManager.AnimationState.Melee, new AnimationIndex[1]
    {
      AnimationIndex.ShopMeleeAimIdle
    });
    this._shopAnimations.Add(AvatarAnimationManager.AnimationState.SmallGun, new AnimationIndex[2]
    {
      AnimationIndex.ShopSmallGunAimIdle,
      AnimationIndex.ShopSmallGunShoot
    });
    this._shopAnimations.Add(AvatarAnimationManager.AnimationState.MediumGun, new AnimationIndex[2]
    {
      AnimationIndex.ShopLargeGunAimIdle,
      AnimationIndex.ShopLargeGunShoot
    });
    this._shopAnimations.Add(AvatarAnimationManager.AnimationState.HeavyGun, new AnimationIndex[2]
    {
      AnimationIndex.ShopLargeGunAimIdle,
      AnimationIndex.ShopLargeGunShoot
    });
  }

  private void Update()
  {
    if (this._currentState == AvatarAnimationManager.AnimationState.None || GameState.HasCurrentGame)
      return;
    if ((double) this._nextAnimationTime < (double) Time.time)
      this.PlayAnimation(this.GetNextAnimation(), this._currentState);
    if (!((Object) GameState.LocalDecorator != (Object) null) || GameState.LocalDecorator.AnimationController == null)
      return;
    GameState.LocalDecorator.AnimationController.UpdateAnimation();
  }

  private AnimationIndex GetNextAnimation()
  {
    AnimationIndex[] current = this._currentSet[this._currentState];
    return current[Random.Range(0, current.Length)];
  }

  private void PlayAnimation(
    AnimationIndex nextAnimation,
    AvatarAnimationManager.AnimationState state,
    bool resetAnimations = false)
  {
    if ((Object) GameState.LocalDecorator != (Object) null && GameState.LocalDecorator.AnimationController != null)
    {
      AnimationInfo info;
      if (GameState.LocalDecorator.AnimationController.TryGetAnimationInfo(nextAnimation, out info))
      {
        GameState.LocalDecorator.AnimationController.TriggerAnimation(info.Index, resetAnimations || this._currentState != state);
        this._nextAnimationTime = (float) ((double) Time.time + (double) info.State.length - 0.10000000149011612);
      }
      else
        this._nextAnimationTime = Time.time + 1f;
    }
    else
      this._nextAnimationTime = Time.time + 0.01f;
    this._currentState = state;
  }

  public void ResetAnimationState(PageType page) => this.SetAnimationState(page, (UberstrikeItemClass) 0, true);

  public void SetAnimationState(PageType page, UberstrikeItemClass type, bool resetAnimations = false)
  {
    if (page == PageType.Shop)
    {
      this._currentSet = this._shopAnimations;
      switch (type)
      {
        case UberstrikeItemClass.WeaponMelee:
          this.PlayAnimation(AnimationIndex.ShopMeleeTakeOut, AvatarAnimationManager.AnimationState.Melee, resetAnimations);
          break;
        case UberstrikeItemClass.WeaponHandgun:
          this.PlayAnimation(AnimationIndex.ShopSmallGunTakeOut, AvatarAnimationManager.AnimationState.SmallGun, resetAnimations);
          break;
        case UberstrikeItemClass.WeaponMachinegun:
        case UberstrikeItemClass.WeaponShotgun:
        case UberstrikeItemClass.WeaponSniperRifle:
        case UberstrikeItemClass.WeaponCannon:
        case UberstrikeItemClass.WeaponSplattergun:
        case UberstrikeItemClass.WeaponLauncher:
          this.PlayAnimation(AnimationIndex.ShopLargeGunTakeOut, AvatarAnimationManager.AnimationState.HeavyGun, resetAnimations);
          break;
        case UberstrikeItemClass.GearBoots:
          this.PlayAnimation(AnimationIndex.ShopNewBoots, AvatarAnimationManager.AnimationState.Idle, resetAnimations);
          break;
        case UberstrikeItemClass.GearHead:
        case UberstrikeItemClass.GearFace:
          this.PlayAnimation(AnimationIndex.ShopNewHead, AvatarAnimationManager.AnimationState.Idle, resetAnimations);
          break;
        case UberstrikeItemClass.GearUpperBody:
          this.PlayAnimation(AnimationIndex.ShopNewUpperBody, AvatarAnimationManager.AnimationState.Idle, resetAnimations);
          break;
        case UberstrikeItemClass.GearLowerBody:
          this.PlayAnimation(AnimationIndex.ShopNewLowerBody, AvatarAnimationManager.AnimationState.Idle, resetAnimations);
          break;
        case UberstrikeItemClass.GearGloves:
          this.PlayAnimation(AnimationIndex.ShopNewGloves, AvatarAnimationManager.AnimationState.Idle, resetAnimations);
          break;
        case UberstrikeItemClass.GearHolo:
          this.PlayAnimation(AnimationIndex.ShopNewUpperBody, AvatarAnimationManager.AnimationState.Idle, resetAnimations);
          break;
        default:
          if (this._currentState == AvatarAnimationManager.AnimationState.Melee)
            this.PlayAnimation(AnimationIndex.ShopHideMelee, AvatarAnimationManager.AnimationState.Idle, resetAnimations);
          else if (this._currentState == AvatarAnimationManager.AnimationState.SmallGun || this._currentState == AvatarAnimationManager.AnimationState.MediumGun || this._currentState == AvatarAnimationManager.AnimationState.HeavyGun)
            this.PlayAnimation(AnimationIndex.ShopHideGun, AvatarAnimationManager.AnimationState.Idle, resetAnimations);
          else
            this._nextAnimationTime = 0.0f;
          this._currentState = AvatarAnimationManager.AnimationState.Idle;
          break;
      }
    }
    else
    {
      this._currentSet = this._homeAnimations;
      this._nextAnimationTime = 0.0f;
      switch (type)
      {
        case UberstrikeItemClass.WeaponMelee:
          this._currentState = AvatarAnimationManager.AnimationState.Melee;
          break;
        case UberstrikeItemClass.WeaponHandgun:
          this._currentState = AvatarAnimationManager.AnimationState.SmallGun;
          break;
        case UberstrikeItemClass.WeaponMachinegun:
        case UberstrikeItemClass.WeaponShotgun:
        case UberstrikeItemClass.WeaponSniperRifle:
        case UberstrikeItemClass.WeaponCannon:
        case UberstrikeItemClass.WeaponSplattergun:
        case UberstrikeItemClass.WeaponLauncher:
          this._currentState = AvatarAnimationManager.AnimationState.HeavyGun;
          break;
        default:
          this._currentState = AvatarAnimationManager.AnimationState.Idle;
          this.PlayAnimation(AnimationIndex.HomeNoWeaponIdle, AvatarAnimationManager.AnimationState.Idle, resetAnimations);
          break;
      }
    }
  }

  private enum AnimationState
  {
    None,
    Idle,
    Melee,
    SmallGun,
    MediumGun,
    HeavyGun,
  }
}
