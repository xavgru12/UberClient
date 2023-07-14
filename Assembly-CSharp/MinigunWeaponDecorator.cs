// Decompiled with JetBrains decompiler
// Type: MinigunWeaponDecorator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class MinigunWeaponDecorator : BaseWeaponDecorator
{
  [SerializeField]
  private AudioClip _duringShootSound;
  [SerializeField]
  private AudioClip _warmUpSound;
  [SerializeField]
  private AudioClip _warmDownSound;
  private AudioSource _warmUpAudioSource;
  private AudioSource _warmDownAudioSource;
  private AudioSource _duringShootAudioSource;
  private WeaponHeadAnimation _headAnim;
  private float _maxWarmUpTime;
  private float _maxWarmDownTime;

  public float MaxWarmUpTime => this._maxWarmUpTime;

  public float MaxWarmDownTime => this._maxWarmDownTime;

  protected override void Awake()
  {
    base.Awake();
    if ((Object) this._warmUpSound == (Object) null)
      throw new CmuneException("MinigunWeaponDecorator - _warmUpSound is NULL", new object[0]);
    if ((Object) this._warmDownSound == (Object) null)
      throw new CmuneException("MinigunWeaponDecorator - _warmDownSound is NULL", new object[0]);
    this.InitAudioSource();
    this._headAnim = this.GetComponentInChildren<WeaponHeadAnimation>();
  }

  private void InitAudioSource()
  {
    if ((bool) (Object) this._duringShootSound)
    {
      this._duringShootAudioSource = this.gameObject.AddComponent<AudioSource>();
      if ((bool) (Object) this._duringShootAudioSource)
      {
        this._duringShootAudioSource.loop = true;
        this._duringShootAudioSource.priority = 0;
        this._duringShootAudioSource.playOnAwake = false;
        this._duringShootAudioSource.clip = this._duringShootSound;
      }
    }
    this._warmUpAudioSource = this.gameObject.AddComponent<AudioSource>();
    if ((bool) (Object) this._warmUpAudioSource)
    {
      this._warmUpAudioSource.priority = 0;
      this._warmUpAudioSource.playOnAwake = false;
      this._maxWarmUpTime = this._warmUpSound.length;
      this._warmUpAudioSource.clip = this._warmUpSound;
    }
    if (!(bool) (Object) this._warmDownSound)
      return;
    this._warmDownAudioSource = this.gameObject.AddComponent<AudioSource>();
    if (!(bool) (Object) this._warmDownAudioSource)
      return;
    this._warmDownAudioSource.priority = 0;
    this._warmDownAudioSource.playOnAwake = false;
    this._maxWarmDownTime = this._warmDownSound.length;
    this._warmDownAudioSource.clip = this._warmDownSound;
  }

  public override void ShowShootEffect(RaycastHit[] hits) => base.ShowShootEffect(hits);

  public void PlayWindUpSound(float time)
  {
    if ((bool) (Object) this._warmDownAudioSource)
      this._warmDownAudioSource.Stop();
    if (!(bool) (Object) this._warmUpAudioSource)
      return;
    this._warmUpAudioSource.time = time;
    this._warmUpAudioSource.Play();
  }

  public void PlayWindDownSound(float time)
  {
    if ((bool) (Object) this._duringShootAudioSource)
      this._duringShootAudioSource.Stop();
    if ((bool) (Object) this._warmUpAudioSource)
      this._warmUpAudioSource.Stop();
    if (!(bool) (Object) this._warmDownAudioSource)
      return;
    this._warmDownAudioSource.time = time;
    this._warmDownAudioSource.Play();
  }

  public void PlayDuringSound()
  {
    if (this._duringShootAudioSource.isPlaying)
      return;
    this._duringShootAudioSource.Play();
  }

  public override void StopSound()
  {
    base.StopSound();
    this._duringShootAudioSource.Stop();
  }

  public void SpinWeaponHead()
  {
    if (!(bool) (Object) this._headAnim)
      return;
    this._headAnim.OnShoot();
  }
}
