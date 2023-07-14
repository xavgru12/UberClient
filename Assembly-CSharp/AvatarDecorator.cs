// Decompiled with JetBrains decompiler
// Type: AvatarDecorator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class AvatarDecorator : MonoBehaviour
{
  public const float DiveFootstep = 3.5f;
  public const float SwimFootstep = 1.7f;
  public const float WaterFootstep = 1.4f;
  private Transform _transform;
  private Animation _animation;
  [SerializeField]
  private CharacterHitArea[] _hitAreas;
  [SerializeField]
  private Transform _weaponAttachPoint;
  private FootStepSoundType _footStep;
  private LoadoutSlotType _currentWeaponSlot;
  private UberstrikeLayer _layer;
  private float _nextFootStepTime;
  private BaseWeaponDecorator _meleeWeapon;
  private BaseWeaponDecorator _primaryWeapon;
  private BaseWeaponDecorator _secondaryWeapon;
  private BaseWeaponDecorator _tertiaryWeapon;
  private BaseWeaponDecorator _pickupWeapon;
  private AvatarDecoratorConfig _configuration;

  public AvatarDecoratorConfig CurrentRagdoll { get; private set; }

  public AvatarDecoratorConfig Configuration
  {
    get
    {
      if ((Object) this._configuration == (Object) null)
        this._configuration = this.GetComponent<AvatarDecoratorConfig>();
      return this._configuration;
    }
  }

  public CharacterHitArea[] HitAreas
  {
    get => this._hitAreas;
    set => this._hitAreas = value;
  }

  public Renderer MeshRenderer => this.renderer;

  public Animation Animation => this._animation;

  public Transform WeaponAttachPoint
  {
    get => this._weaponAttachPoint;
    set => this._weaponAttachPoint = value;
  }

  public LoadoutSlotType CurrentWeaponSlot => this._currentWeaponSlot;

  public BaseWeaponDecorator MeleeWeapon
  {
    get => this._meleeWeapon;
    private set
    {
      if ((bool) (Object) this._meleeWeapon)
      {
        Object.Destroy((Object) this._meleeWeapon.gameObject);
        Object.Destroy((Object) this._meleeWeapon);
      }
      this._meleeWeapon = value;
    }
  }

  public BaseWeaponDecorator PrimaryWeapon
  {
    get => this._primaryWeapon;
    private set
    {
      if ((bool) (Object) this._primaryWeapon)
      {
        Object.Destroy((Object) this._primaryWeapon.gameObject);
        Object.Destroy((Object) this._primaryWeapon);
      }
      this._primaryWeapon = value;
    }
  }

  public BaseWeaponDecorator SecondaryWeapon
  {
    get => this._secondaryWeapon;
    private set
    {
      if ((bool) (Object) this._secondaryWeapon)
      {
        Object.Destroy((Object) this._secondaryWeapon.gameObject);
        Object.Destroy((Object) this._secondaryWeapon);
      }
      this._secondaryWeapon = value;
    }
  }

  public BaseWeaponDecorator TertiaryWeapon
  {
    get => this._tertiaryWeapon;
    private set
    {
      if ((bool) (Object) this._tertiaryWeapon)
      {
        Object.Destroy((Object) this._tertiaryWeapon.gameObject);
        Object.Destroy((Object) this._tertiaryWeapon);
      }
      this._tertiaryWeapon = value;
    }
  }

  public BaseWeaponDecorator PickupWeapon
  {
    get => this._pickupWeapon;
    private set
    {
      if ((bool) (Object) this._pickupWeapon)
      {
        Object.Destroy((Object) this._pickupWeapon.gameObject);
        Object.Destroy((Object) this._pickupWeapon);
      }
      this._pickupWeapon = value;
    }
  }

  public AvatarHudInformation HudInformation { get; private set; }

  public AvatarAnimationController AnimationController { get; private set; }

  public int[] MyGear { get; set; }

  public GearLoadout Gear { get; set; }

  private void Awake()
  {
    this.Gear = new GearLoadout();
    this._currentWeaponSlot = LoadoutSlotType.WeaponPrimary;
    this.HudInformation = this.GetComponentInChildren<AvatarHudInformation>();
    if ((bool) (Object) this.HudInformation)
      this.HudInformation.Target = this.GetBone(BoneIndex.HeadTop);
    this._transform = this.transform;
    this._animation = this.GetComponent<Animation>();
    if (!(bool) (Object) this._animation)
      return;
    this.AnimationController = new AvatarAnimationController(this._animation);
  }

  public void SetSkinColor(Color color) => this.Configuration.SkinColor = color;

  public void EnableOutline(bool showOutline)
  {
    if (!OutlineEffectController.Exists)
      return;
    if (showOutline)
      OutlineEffectController.Instance.AddOutlineObject(this.gameObject, this.Configuration.MaterialGroup, ColorScheme.TeamOutline);
    else
      OutlineEffectController.Instance.RemoveOutlineObject(this.gameObject);
  }

  public void SetShotFeedback(BodyPart bodyPart)
  {
    if (!(bool) (Object) this.HudInformation)
      return;
    switch (bodyPart)
    {
      case BodyPart.Head:
        this.HudInformation.SetInGameFeedback(InGameEventFeedbackType.HeadShot);
        break;
      case BodyPart.Nuts:
        this.HudInformation.SetInGameFeedback(InGameEventFeedbackType.NutShot);
        break;
    }
  }

  public void DisableRagdoll()
  {
    this.DestroyCurrentRagdoll();
    this._transform.gameObject.SetActive(true);
    this.ShowWeapon(this._currentWeaponSlot);
  }

  public AvatarDecoratorConfig SpawnDeadRagdoll(DamageInfo shot)
  {
    this.DestroyCurrentRagdoll();
    AvatarDecoratorConfig avatarDecoratorConfig = this.InstantiateRagdoll();
    foreach (ArrowProjectile componentsInChild in this.GetComponentsInChildren<ArrowProjectile>(true))
    {
      Vector3 localPosition = componentsInChild.transform.localPosition;
      Quaternion localRotation = componentsInChild.transform.localRotation;
      componentsInChild.transform.parent = avatarDecoratorConfig.GetBone(BoneIndex.Hips);
      componentsInChild.transform.localPosition = localPosition;
      componentsInChild.transform.localRotation = localRotation;
    }
    this._transform.gameObject.SetActive(false);
    Vector3 force = shot == null ? Vector3.zero : shot.Force.normalized;
    foreach (AvatarBone bone in avatarDecoratorConfig.Bones)
    {
      if ((bool) (Object) bone.Rigidbody)
      {
        bone.Rigidbody.isKinematic = false;
        if (bone.Bone == BoneIndex.Hips)
          bone.Rigidbody.AddForce(force * 3f);
        else
          bone.Rigidbody.AddForce(force, ForceMode.VelocityChange);
        if (GameState.IsRagdollShootable)
          bone.Transform.gameObject.layer = 21;
      }
    }
    this.CurrentRagdoll = avatarDecoratorConfig;
    return avatarDecoratorConfig;
  }

  private AvatarDecoratorConfig InstantiateRagdoll()
  {
    AvatarDecoratorConfig ragdoll = Singleton<RagdollBuilder>.Instance.CreateRagdoll(this.Gear);
    if ((bool) (Object) ragdoll)
    {
      ragdoll.transform.localPosition = this._transform.position;
      ragdoll.transform.localRotation = this._transform.rotation;
      AvatarDecoratorConfig.CopyBones(this.Configuration, ragdoll);
      SkinnedMeshRenderer component = ragdoll.GetComponent<SkinnedMeshRenderer>();
      if ((bool) (Object) component)
        component.updateWhenOffscreen = true;
      ragdoll.SkinColor = this.Configuration.SkinColor;
    }
    return ragdoll;
  }

  public void AssignWeapon(LoadoutSlotType slot, BaseWeaponDecorator decorator)
  {
    if ((Object) decorator != (Object) null)
    {
      switch (slot)
      {
        case LoadoutSlotType.WeaponMelee:
          if ((Object) this.MeleeWeapon != (Object) decorator)
          {
            this.MeleeWeapon = decorator;
            break;
          }
          break;
        case LoadoutSlotType.WeaponPrimary:
          if ((Object) this.PrimaryWeapon != (Object) decorator)
          {
            this.PrimaryWeapon = decorator;
            break;
          }
          break;
        case LoadoutSlotType.WeaponSecondary:
          if ((Object) this.SecondaryWeapon != (Object) decorator)
          {
            this.SecondaryWeapon = decorator;
            break;
          }
          break;
        case LoadoutSlotType.WeaponTertiary:
          if ((Object) this.TertiaryWeapon != (Object) decorator)
          {
            this.TertiaryWeapon = decorator;
            break;
          }
          break;
        case LoadoutSlotType.WeaponPickup:
          if ((Object) this.PickupWeapon != (Object) decorator)
          {
            this.PickupWeapon = decorator;
            break;
          }
          break;
      }
      decorator.transform.parent = this._weaponAttachPoint;
      LayerUtil.SetLayerRecursively(decorator.gameObject.transform, this._layer);
      decorator.transform.localPosition = Vector3.zero;
      decorator.transform.localRotation = Quaternion.identity;
    }
    else
    {
      switch (slot)
      {
        case LoadoutSlotType.WeaponMelee:
          this.MeleeWeapon = decorator;
          break;
        case LoadoutSlotType.WeaponPrimary:
          this.PrimaryWeapon = decorator;
          break;
        case LoadoutSlotType.WeaponSecondary:
          this.SecondaryWeapon = decorator;
          break;
        case LoadoutSlotType.WeaponTertiary:
          this.TertiaryWeapon = decorator;
          break;
        case LoadoutSlotType.WeaponPickup:
          this.PickupWeapon = decorator;
          break;
        default:
          Debug.LogError((object) "Couldn't assign Weapon Slot because Slot is NULL");
          break;
      }
    }
  }

  public void SetActiveWeaponSlot(LoadoutSlotType slot) => this._currentWeaponSlot = slot;

  public void ShowWeapon(LoadoutSlotType slot)
  {
    this._currentWeaponSlot = slot;
    if ((Object) this.MeleeWeapon != (Object) null)
      this.MeleeWeapon.IsEnabled = slot == LoadoutSlotType.WeaponMelee;
    if ((Object) this.PrimaryWeapon != (Object) null)
      this.PrimaryWeapon.IsEnabled = slot == LoadoutSlotType.WeaponPrimary;
    if ((Object) this.SecondaryWeapon != (Object) null)
      this.SecondaryWeapon.IsEnabled = slot == LoadoutSlotType.WeaponSecondary;
    if ((Object) this.TertiaryWeapon != (Object) null)
      this.TertiaryWeapon.IsEnabled = slot == LoadoutSlotType.WeaponTertiary;
    if (!((Object) this.PickupWeapon != (Object) null))
      return;
    this.PickupWeapon.IsEnabled = slot == LoadoutSlotType.WeaponPickup;
  }

  public void HideWeapons()
  {
    if ((Object) this.MeleeWeapon != (Object) null)
      this.MeleeWeapon.IsEnabled = false;
    if ((Object) this.PrimaryWeapon != (Object) null)
      this.PrimaryWeapon.IsEnabled = false;
    if ((Object) this.SecondaryWeapon != (Object) null)
      this.SecondaryWeapon.IsEnabled = false;
    if ((Object) this.TertiaryWeapon != (Object) null)
      this.TertiaryWeapon.IsEnabled = false;
    if (!((Object) this.PickupWeapon != (Object) null))
      return;
    this.PickupWeapon.IsEnabled = false;
  }

  public void SetLayers(UberstrikeLayer layer)
  {
    this._layer = layer;
    this.UpdateLayers();
  }

  public void UpdateLayers() => LayerUtil.SetLayerRecursively(this.transform, this._layer);

  public Transform GetBone(BoneIndex bone) => this.Configuration.GetBone(bone);

  public void SetPosition(Vector3 position, Quaternion rotation)
  {
    this.transform.localPosition = position;
    this.transform.localRotation = rotation;
  }

  public void SetFootStep(FootStepSoundType sound) => this._footStep = sound;

  public bool CanPlayFootSound => (double) this._nextFootStepTime < (double) Time.time;

  public void PlayFootSound(float length)
  {
    if (!this.CanPlayFootSound)
      return;
    this.PlayFootSound(this._footStep, length);
  }

  public void PlayFootSound(FootStepSoundType sound, float length)
  {
    switch (sound)
    {
      case FootStepSoundType.Water:
        length *= 1.4f;
        break;
      case FootStepSoundType.Swim:
        length *= 1.7f;
        break;
      case FootStepSoundType.Dive:
        length *= 3.5f;
        break;
    }
    this._nextFootStepTime = Time.time + length;
    AutoMonoBehaviour<SfxManager>.Instance.PlayFootStepAudioClip(sound, this._transform.position);
  }

  public void PlayDieSound()
  {
    int num = Random.Range(0, 3);
    AudioClip audioClip = GameAudio.NormalKill1;
    switch (num)
    {
      case 0:
        audioClip = GameAudio.NormalKill1;
        break;
      case 1:
        audioClip = GameAudio.NormalKill2;
        break;
      case 3:
        audioClip = GameAudio.NormalKill3;
        break;
    }
    SfxManager.Play3dAudioClip(audioClip, this._transform.position);
  }

  public void DestroyCurrentRagdoll()
  {
    if (!(bool) (Object) this.CurrentRagdoll)
      return;
    AvatarBuilder.Destroy(this.CurrentRagdoll.gameObject);
    this.CurrentRagdoll = (AvatarDecoratorConfig) null;
  }
}
