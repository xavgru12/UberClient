// Decompiled with JetBrains decompiler
// Type: AvatarAnimationController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AvatarAnimationController
{
  private Animation _animation;
  private Dictionary<int, AnimationInfo> _animations;
  private Dictionary<int, Transform> _bones;

  public AvatarAnimationController(Animation animation)
  {
    this._bones = new Dictionary<int, Transform>();
    this._animations = new Dictionary<int, AnimationInfo>();
    this._animation = animation;
    this.InitBones(animation.transform);
    this.InitAnimations();
  }

  public void UpdateAnimation()
  {
    foreach (AnimationInfo animation in (IEnumerable<AnimationInfo>) this.Animations)
    {
      if ((double) animation.EndTime >= (double) Time.time)
      {
        if ((double) animation.State.weight == 0.0)
          animation.State.time = 0.0f;
        animation.State.speed = animation.Speed;
        animation.CurrentTimePlayed = animation.State.normalizedTime;
        this.Animation.Blend(animation.Name, 1f, 0.0f);
      }
      else
      {
        animation.State.speed = 0.0f;
        animation.CurrentTimePlayed = animation.State.normalizedTime;
        this.Animation.Blend(animation.Name, 0.0f, 0.3f);
      }
    }
  }

  public void PlayAnimation(AnimationIndex id) => this.PlayAnimation(id, 1f);

  public bool PlayAnimation(AnimationIndex id, float speed) => this.PlayAnimation(id, speed, Time.deltaTime);

  public bool PlayAnimation(AnimationIndex id, float speed, float runtime)
  {
    AnimationInfo animationInfo;
    if (this._animations.TryGetValue((int) id, out animationInfo))
    {
      animationInfo.Speed = speed;
      animationInfo.EndTime = Time.time + runtime;
    }
    return animationInfo != null;
  }

  public void TriggerAnimation(AnimationIndex id) => this.TriggerAnimation(id, 1f, false);

  public void TriggerAnimation(AnimationIndex id, bool stopAll) => this.TriggerAnimation(id, 1f, stopAll);

  public void TriggerAnimation(AnimationIndex id, float speed, bool stopAll)
  {
    AnimationInfo animationInfo;
    if (!this._animations.TryGetValue((int) id, out animationInfo))
      return;
    if (stopAll)
      this.ResetAllAnimations();
    animationInfo.State.time = 0.0f;
    this.PlayAnimation(id, speed, animationInfo.State.length / speed);
  }

  public void RewindAnimation(AnimationIndex id)
  {
    AnimationInfo animationInfo;
    if (!this._animations.TryGetValue((int) id, out animationInfo))
      return;
    animationInfo.State.speed = 0.0f;
    animationInfo.State.time = 0.0f;
    animationInfo.EndTime = 0.0f;
  }

  public void ResetAllAnimations()
  {
    foreach (AnimationIndex key in this._animations.Keys)
      this.RewindAnimation(key);
  }

  public string GetDebugInfo()
  {
    StringBuilder stringBuilder = new StringBuilder();
    foreach (AnimationInfo animationInfo in this._animations.Values)
    {
      UnityEngine.AnimationState state = animationInfo.State;
      stringBuilder.Append(state.name);
      stringBuilder.Append(", weight: ");
      stringBuilder.Append(state.weight.ToString("N2"));
      stringBuilder.Append(", time/runtime: ");
      stringBuilder.Append(state.time.ToString("N2"));
      stringBuilder.Append("/");
      stringBuilder.Append(animationInfo.EndTime.ToString("N2"));
      stringBuilder.Append(", length ");
      stringBuilder.Append(state.length.ToString("N2"));
      stringBuilder.Append("\n");
    }
    return stringBuilder.ToString();
  }

  public bool IsPlaying(AnimationIndex idx)
  {
    AnimationInfo animationInfo;
    return this._animations.TryGetValue((int) idx, out animationInfo) && (double) animationInfo.State.weight > 0.0;
  }

  public void SetAnimationTimeNormalized(AnimationIndex idx, float time)
  {
    AnimationInfo animationInfo;
    if (!this._animations.TryGetValue((int) idx, out animationInfo))
      return;
    animationInfo.State.normalizedTime = Mathf.Lerp(animationInfo.State.normalizedTime, Mathf.Clamp01(time), Time.deltaTime * 10f);
    this.PlayAnimation(idx, 0.0f);
  }

  public Transform GetBoneTransform(BoneIndex i)
  {
    Transform boneTransform = (Transform) null;
    this._bones.TryGetValue((int) i, out boneTransform);
    return boneTransform;
  }

  public bool TryGetAnimationInfo(AnimationIndex id, out AnimationInfo info)
  {
    this._animations.TryGetValue((int) id, out info);
    return info != null;
  }

  private void InitBones(Transform rigging)
  {
    Transform[] componentsInChildren = rigging.GetComponentsInChildren<Transform>(true);
    Dictionary<string, Transform> dictionary = new Dictionary<string, Transform>(componentsInChildren.Length);
    foreach (Transform transform in componentsInChildren)
      dictionary[transform.name] = transform;
    foreach (int num in Enum.GetValues(typeof (BoneIndex)))
    {
      BoneIndex key = (BoneIndex) num;
      string name = Enum.GetName(typeof (BoneIndex), (object) key);
      Transform transform;
      if (dictionary.TryGetValue(name, out transform))
        this._bones.Add((int) key, transform);
    }
  }

  private void InitAnimations()
  {
    foreach (int num in Enum.GetValues(typeof (AnimationIndex)))
    {
      AnimationIndex animationIndex = (AnimationIndex) num;
      UnityEngine.AnimationState state = this._animation[Enum.GetName(typeof (AnimationIndex), (object) animationIndex)];
      if ((TrackedReference) state != (TrackedReference) null)
      {
        this._animations.Add((int) animationIndex, new AnimationInfo(animationIndex, state));
        Transform mix;
        if (this._bones.TryGetValue(this.GetMixingTransformBoneIndex(animationIndex), out mix))
          state.AddMixingTransform(mix);
        state.wrapMode = this.GetAnimationWrapMode(animationIndex);
        state.blendMode = this.GetAnimationBlendMode(animationIndex);
        state.layer = this.GetAnimationLayer(animationIndex);
      }
    }
  }

  private WrapMode GetAnimationWrapMode(AnimationIndex id)
  {
    switch (id)
    {
      case AnimationIndex.idle:
      case AnimationIndex.run:
      case AnimationIndex.lightGunBreathe:
      case AnimationIndex.heavyGunBreathe:
      case AnimationIndex.swimLoop:
      case AnimationIndex.walk:
      case AnimationIndex.crouch:
      case AnimationIndex.HomeNoWeaponIdle:
      case AnimationIndex.HomeNoWeaponnLookAround:
      case AnimationIndex.HomeNoWeaponRelaxNeck:
      case AnimationIndex.HomeMeleeIdle:
      case AnimationIndex.HomeMeleeLookAround:
      case AnimationIndex.HomeMeleeRelaxNeck:
      case AnimationIndex.HomeMeleeCheckWeapon:
      case AnimationIndex.HomeSmallGunIdle:
      case AnimationIndex.HomeSmallGunLookAround:
      case AnimationIndex.HomeSmallGunRelaxNeck:
      case AnimationIndex.HomeSmallGunCheckWeapon:
      case AnimationIndex.HomeMediumGunIdle:
      case AnimationIndex.HomeMediumGunLookAround:
      case AnimationIndex.HomeMediumGunRelaxNeck:
      case AnimationIndex.HomeMediumGunCheckWeapon:
      case AnimationIndex.HomeLargeGunIdle:
      case AnimationIndex.HomeLargeGunLookAround:
      case AnimationIndex.HomeLargeGunRelaxNeck:
      case AnimationIndex.HomeLargeGunCheckWeapon:
      case AnimationIndex.HomeLargeGunShakeWeapon:
      case AnimationIndex.ShopMeleeAimIdle:
      case AnimationIndex.ShopSmallGunAimIdle:
      case AnimationIndex.ShopSmallGunShoot:
      case AnimationIndex.ShopLargeGunAimIdle:
      case AnimationIndex.ShopLargeGunShoot:
      case AnimationIndex.ShopNewGloves:
      case AnimationIndex.ShopNewUpperBody:
      case AnimationIndex.ShopNewBoots:
      case AnimationIndex.ShopNewLowerBody:
      case AnimationIndex.ShopNewHead:
      case AnimationIndex.idleWalk:
      case AnimationIndex.TutorialGuideWalk:
      case AnimationIndex.TutorialGuideIdle:
        return WrapMode.Loop;
      case AnimationIndex.jumpUp:
      case AnimationIndex.die1:
      case AnimationIndex.squat:
      case AnimationIndex.lightGunUpDown:
      case AnimationIndex.heavyGunUpDown:
      case AnimationIndex.snipeUpDown:
      case AnimationIndex.ShopMeleeTakeOut:
      case AnimationIndex.ShopSmallGunTakeOut:
      case AnimationIndex.ShopLargeGunTakeOut:
      case AnimationIndex.ShopHideGun:
      case AnimationIndex.ShopHideMelee:
        return WrapMode.ClampForever;
      case AnimationIndex.shootLightGun:
      case AnimationIndex.shootHeavyGun:
      case AnimationIndex.swimStart:
      case AnimationIndex.gotHit:
      case AnimationIndex.meleeSwingRightToLeft:
      case AnimationIndex.meleSwingLeftToRight:
        return WrapMode.Once;
      default:
        return WrapMode.Default;
    }
  }

  private AnimationBlendMode GetAnimationBlendMode(AnimationIndex id)
  {
    switch (id)
    {
      case AnimationIndex.shootLightGun:
      case AnimationIndex.shootHeavyGun:
        return AnimationBlendMode.Additive;
      default:
        return AnimationBlendMode.Blend;
    }
  }

  private int GetAnimationLayer(AnimationIndex id)
  {
    AnimationIndex animationIndex = id;
    switch (animationIndex)
    {
      case AnimationIndex.lightGunUpDown:
      case AnimationIndex.heavyGunUpDown:
      case AnimationIndex.snipeUpDown:
label_3:
        return 1;
      case AnimationIndex.meleeSwingRightToLeft:
      case AnimationIndex.meleSwingLeftToRight:
        return 2;
      default:
        switch (animationIndex - 9)
        {
          case AnimationIndex.idle:
            goto label_3;
          case AnimationIndex.jumpUp:
            return 5;
          default:
            if (animationIndex != AnimationIndex.ShopSmallGunAimIdle)
              return 0;
            goto label_3;
        }
    }
  }

  private int GetMixingTransformBoneIndex(AnimationIndex id)
  {
    AnimationIndex animationIndex = id;
    switch (animationIndex)
    {
      case AnimationIndex.lightGunUpDown:
      case AnimationIndex.heavyGunUpDown:
      case AnimationIndex.snipeUpDown:
      case AnimationIndex.meleeSwingRightToLeft:
      case AnimationIndex.meleSwingLeftToRight:
        return 9;
      default:
        if (animationIndex != AnimationIndex.shootLightGun && animationIndex != AnimationIndex.shootHeavyGun)
          return 0;
        goto case AnimationIndex.lightGunUpDown;
    }
  }

  public ICollection<AnimationInfo> Animations => (ICollection<AnimationInfo>) this._animations.Values;

  public Animation Animation => this._animation;
}
