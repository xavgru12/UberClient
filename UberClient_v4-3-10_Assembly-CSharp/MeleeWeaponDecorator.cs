// Decompiled with JetBrains decompiler
// Type: MeleeWeaponDecorator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

public sealed class MeleeWeaponDecorator : BaseWeaponDecorator
{
  [SerializeField]
  private Animation _animation;
  [SerializeField]
  private AnimationClip[] _shootAnimClips;
  [SerializeField]
  private AudioClip[] _impactSounds;
  [SerializeField]
  private AudioClip _equipSound;

  protected override void Awake()
  {
    base.Awake();
    this.IsMelee = true;
  }

  public override void ShowShootEffect(RaycastHit[] hits)
  {
    base.ShowShootEffect(hits);
    if (!this.EnableShootAnimation || !(bool) (Object) this._animation || this._shootAnimClips.Length <= 0)
      return;
    this._animation.clip = this._shootAnimClips[Random.Range(0, this._shootAnimClips.Length)];
    this._animation.Rewind();
    this._animation.Play();
  }

  public override void PlayHitSound()
  {
  }

  public override void PlayEquipSound()
  {
    if (!(bool) (Object) this._mainAudioSource || !(bool) (Object) this._equipSound)
      return;
    this._mainAudioSource.PlayOneShot(this._equipSound);
  }

  protected override void EmitImpactSound(string impactType, Vector3 position)
  {
    if (this._impactSounds != null && this._impactSounds.Length > 0)
    {
      AudioClip impactSound = this._impactSounds[Random.Range(0, this._impactSounds.Length)];
      if ((bool) (Object) impactSound)
        SfxManager.Play3dAudioClip(impactSound, position);
      else
        UnityEngine.Debug.LogError((object) "Missing impact sound for melee weapon!");
    }
    else
      UnityEngine.Debug.LogError((object) "Melee impact sound is not signed!");
  }

  protected override void ShowImpactEffects(
    RaycastHit hit,
    Vector3 direction,
    Vector3 muzzlePosition,
    float distance,
    bool playSound)
  {
    this.StartCoroutine(this.StartShowImpactEffects(hit, direction, muzzlePosition, distance, playSound));
  }

  [DebuggerHidden]
  private IEnumerator StartShowImpactEffects(
    RaycastHit hit,
    Vector3 direction,
    Vector3 muzzlePosition,
    float distance,
    bool playSound)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new MeleeWeaponDecorator.\u003CStartShowImpactEffects\u003Ec__Iterator88()
    {
      hit = hit,
      direction = direction,
      muzzlePosition = muzzlePosition,
      distance = distance,
      playSound = playSound,
      \u003C\u0024\u003Ehit = hit,
      \u003C\u0024\u003Edirection = direction,
      \u003C\u0024\u003EmuzzlePosition = muzzlePosition,
      \u003C\u0024\u003Edistance = distance,
      \u003C\u0024\u003EplaySound = playSound,
      \u003C\u003Ef__this = this
    };
  }
}
