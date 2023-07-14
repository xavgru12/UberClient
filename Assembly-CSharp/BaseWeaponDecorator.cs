// Decompiled with JetBrains decompiler
// Type: BaseWeaponDecorator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (AudioSource))]
public abstract class BaseWeaponDecorator : MonoBehaviour
{
  [SerializeField]
  private Transform _muzzlePosition;
  [SerializeField]
  private AudioClip[] _shootSounds;
  private Vector3 _defaultPosition;
  private Vector3 _ironSightPosition;
  private ParticleConfigurationType _effectType;
  private MoveTrailrendererObject _trailRenderer;
  private Transform _parent;
  private ParticleSystem _particles;
  private bool _isEnabled = true;
  private bool _isShootAnimationEnabled;
  protected AudioSource _mainAudioSource;
  private Dictionary<string, SurfaceEffectType> _effectMap;
  private List<BaseWeaponEffect> _effects = new List<BaseWeaponEffect>();

  public bool IsEnabled
  {
    get => this._isEnabled;
    set
    {
      if (this.gameObject.activeSelf == value)
        return;
      this._isEnabled = value;
      this.gameObject.SetActive(this._isEnabled);
      this.HideAllWeaponEffect();
    }
  }

  public void HideAllWeaponEffect()
  {
    if (this._effects == null)
      return;
    foreach (BaseWeaponEffect effect in this._effects)
      effect.Hide();
  }

  public bool EnableShootAnimation
  {
    get => this._isShootAnimationEnabled;
    set
    {
      this._isShootAnimationEnabled = value;
      if (this._isShootAnimationEnabled)
        return;
      WeaponShootAnimation weaponShootAnimation = this._effects.Find((Predicate<BaseWeaponEffect>) (p => p is WeaponShootAnimation)) as WeaponShootAnimation;
      if (!(bool) (UnityEngine.Object) weaponShootAnimation)
        return;
      this._effects.Remove((BaseWeaponEffect) weaponShootAnimation);
      UnityEngine.Object.Destroy((UnityEngine.Object) weaponShootAnimation);
    }
  }

  public bool HasShootAnimation { get; private set; }

  public Vector3 MuzzlePosition => (bool) (UnityEngine.Object) this._muzzlePosition ? this._muzzlePosition.position : Vector3.zero;

  public Vector3 DefaultPosition
  {
    get => this._defaultPosition;
    set
    {
      this._defaultPosition = value;
      this.transform.localPosition = this._defaultPosition;
    }
  }

  public Vector3 CurrentPosition
  {
    get => this.transform.localPosition;
    set => this.transform.localPosition = value;
  }

  public Quaternion CurrentRotation
  {
    get => this.transform.localRotation;
    set => this.transform.localRotation = value;
  }

  public Vector3 IronSightPosition
  {
    get => this._ironSightPosition;
    set => this._ironSightPosition = value;
  }

  public Vector3 DefaultAngles { get; set; }

  public MoveTrailrendererObject TrailRenderer => this._trailRenderer;

  public bool IsMelee { get; protected set; }

  protected virtual void Awake()
  {
    this._parent = this.transform.parent;
    this._mainAudioSource = this.GetComponent<AudioSource>();
    if ((bool) (UnityEngine.Object) this._mainAudioSource)
      this._mainAudioSource.priority = 0;
    this._effects.AddRange((IEnumerable<BaseWeaponEffect>) this.GetComponentsInChildren<BaseWeaponEffect>(true));
    if ((bool) (UnityEngine.Object) this._muzzlePosition)
      this._particles = this._muzzlePosition.GetComponent<ParticleSystem>();
    this.HasShootAnimation = this._effects.Exists((Predicate<BaseWeaponEffect>) (e => e is WeaponShootAnimation));
    this.InitEffectMap();
  }

  protected virtual void Start() => this.HideAllWeaponEffect();

  public BaseWeaponDecorator Clone() => UnityEngine.Object.Instantiate((UnityEngine.Object) this) as BaseWeaponDecorator;

  public virtual void ShowShootEffect(RaycastHit[] hits)
  {
    if (!this.IsEnabled)
      return;
    if ((bool) (UnityEngine.Object) this._muzzlePosition)
    {
      Vector3 position = this._muzzlePosition.position;
      for (int index = 0; index < hits.Length; ++index)
      {
        Vector3 normalized = (hits[index].point - position).normalized;
        float distance = Vector3.Distance(position, hits[index].point);
        this.ShowImpactEffects(hits[index], normalized, position, distance, index == 0);
      }
    }
    foreach (BaseWeaponEffect effect in this._effects)
      effect.OnShoot();
    if ((bool) (UnityEngine.Object) this._particles)
    {
      this._particles.Stop();
      this._particles.Play(this._isShootAnimationEnabled);
    }
    this.PlayShootSound();
  }

  public virtual void PostShoot()
  {
    if (!this.IsEnabled || this._effects == null)
      return;
    foreach (BaseWeaponEffect effect in this._effects)
      effect.OnPostShoot();
  }

  protected virtual void ShowImpactEffects(
    RaycastHit hit,
    Vector3 direction,
    Vector3 muzzlePosition,
    float distance,
    bool playSound)
  {
    this.EmitImpactParticles(hit, direction, muzzlePosition, distance, playSound);
  }

  private static void Play3dAudioClip(AudioSource audioSource, AudioClip soundEffect) => BaseWeaponDecorator.Play3dAudioClip(audioSource, soundEffect, 0.0f);

  private static void Play3dAudioClip(AudioSource audioSource, AudioClip soundEffect, float delay)
  {
    try
    {
      audioSource.clip = soundEffect;
      ulong delay1 = (ulong) ((double) delay * (double) audioSource.clip.frequency);
      audioSource.Play(delay1);
    }
    catch
    {
      Debug.LogError((object) ("Play3dAudioClip: " + (object) soundEffect + " failed."));
    }
  }

  public virtual void StopSound() => this._mainAudioSource.Stop();

  public void PlayShootSound()
  {
    if (!(bool) (UnityEngine.Object) this._mainAudioSource || this._shootSounds == null || this._shootSounds.Length <= 0)
      return;
    AudioClip shootSound = this._shootSounds[UnityEngine.Random.Range(0, this._shootSounds.Length)];
    if (!(bool) (UnityEngine.Object) shootSound)
      return;
    this._mainAudioSource.volume = ApplicationDataManager.ApplicationOptions.AudioEffectsVolume;
    this._mainAudioSource.PlayOneShot(shootSound);
  }

  private void InitEffectMap()
  {
    this._effectMap = new Dictionary<string, SurfaceEffectType>();
    this._effectMap.Add("Wood", SurfaceEffectType.WoodEffect);
    this._effectMap.Add("SolidWood", SurfaceEffectType.WoodEffect);
    this._effectMap.Add("Stone", SurfaceEffectType.StoneEffect);
    this._effectMap.Add("Metal", SurfaceEffectType.MetalEffect);
    this._effectMap.Add("Sand", SurfaceEffectType.SandEffect);
    this._effectMap.Add("Grass", SurfaceEffectType.GrassEffect);
    this._effectMap.Add("Avatar", SurfaceEffectType.Splat);
    this._effectMap.Add("Water", SurfaceEffectType.WaterEffect);
    this._effectMap.Add("NoTarget", SurfaceEffectType.None);
    this._effectMap.Add("Cement", SurfaceEffectType.StoneEffect);
  }

  public void SetSurfaceEffect(ParticleConfigurationType effect) => this._effectType = effect;

  public virtual void PlayEquipSound() => SfxManager.Play2dAudioClip(GameAudio.WeaponSwitch);

  public virtual void PlayHitSound() => Debug.LogError((object) "Not Implemented: Should play WeaponHit sound!");

  public void PlayOutOfAmmoSound() => BaseWeaponDecorator.Play3dAudioClip(this._mainAudioSource, GameAudio.OutOfAmmoClick);

  public void PlayImpactSoundAt(HitPoint point)
  {
    if (point == null)
      return;
    if (GameState.HasCurrentSpace && GameState.CurrentSpace.HasWaterPlane && ((double) this._muzzlePosition.position.y > (double) GameState.CurrentSpace.WaterPlaneHeight && (double) point.Point.y < (double) GameState.CurrentSpace.WaterPlaneHeight || (double) this._muzzlePosition.position.y < (double) GameState.CurrentSpace.WaterPlaneHeight && (double) point.Point.y > (double) GameState.CurrentSpace.WaterPlaneHeight))
      AutoMonoBehaviour<SfxManager>.Instance.PlayImpactSound("Water", point.Point with
      {
        y = 0.0f
      });
    else
      this.EmitImpactSound(point.Tag, point.Point);
  }

  protected virtual void EmitImpactSound(string impactType, Vector3 position) => AutoMonoBehaviour<SfxManager>.Instance.PlayImpactSound(impactType, position);

  protected void EmitImpactParticles(
    RaycastHit hit,
    Vector3 direction,
    Vector3 muzzlePosition,
    float distance,
    bool playSound)
  {
    string tag = TagUtil.GetTag(hit.collider);
    Vector3 point = hit.point;
    Vector3 hitNormal = hit.normal;
    SurfaceEffectType surface = SurfaceEffectType.Default;
    if (!this._effectMap.TryGetValue(tag, out surface))
      return;
    if (GameState.HasCurrentSpace && GameState.CurrentSpace.HasWaterPlane && ((double) this._muzzlePosition.position.y > (double) GameState.CurrentSpace.WaterPlaneHeight && (double) point.y < (double) GameState.CurrentSpace.WaterPlaneHeight || (double) this._muzzlePosition.position.y < (double) GameState.CurrentSpace.WaterPlaneHeight && (double) point.y > (double) GameState.CurrentSpace.WaterPlaneHeight))
    {
      surface = SurfaceEffectType.WaterEffect;
      hitNormal = Vector3.up;
      point.y = GameState.CurrentSpace.WaterPlaneHeight;
      if (!Mathf.Approximately(direction.y, 0.0f))
      {
        point.x = (GameState.CurrentSpace.WaterPlaneHeight - hit.point.y) / direction.y * direction.x + hit.point.x;
        point.z = (GameState.CurrentSpace.WaterPlaneHeight - hit.point.y) / direction.y * direction.z + hit.point.z;
      }
    }
    ParticleEffectController.ShowHitEffect(this._effectType, surface, direction, point, hitNormal, muzzlePosition, distance, ref this._trailRenderer, this._parent);
  }

  public void SetMuzzlePosition(Transform muzzle) => this._muzzlePosition = muzzle;

  public void SetWeaponSounds(AudioClip[] sounds)
  {
    if (sounds == null)
      return;
    this._shootSounds = new AudioClip[sounds.Length];
    sounds.CopyTo((Array) this._shootSounds, 0);
  }
}
