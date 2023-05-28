// Decompiled with JetBrains decompiler
// Type: WeaponTailSound
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class WeaponTailSound : BaseWeaponEffect
{
  [SerializeField]
  private AudioClip _tailSound;
  [SerializeField]
  private WeaponHeadAnimation _headAnimation;
  private AudioSource _tailAudioSource;
  private float _tailSoundLength;
  private float _tailSoundMaxLength;

  private void Awake()
  {
    if ((bool) (Object) this._tailSound)
    {
      this._tailAudioSource = this.gameObject.AddComponent<AudioSource>();
      if ((bool) (Object) this._tailAudioSource)
      {
        this._tailAudioSource.clip = this._tailSound;
        this._tailAudioSource.playOnAwake = false;
      }
      this._tailSoundMaxLength = this._tailSound.length * 0.8f;
    }
    else
      Debug.LogError((object) "There is no audio clip signed for WeaponTailSound!");
  }

  private void Update()
  {
    if ((double) this._tailSoundLength <= 0.0)
      return;
    if ((bool) (Object) this._headAnimation)
      this._headAnimation.OnShoot();
    this._tailSoundLength -= Time.deltaTime;
  }

  public override void OnShoot()
  {
    if ((bool) (Object) this._tailAudioSource)
      this._tailAudioSource.Stop();
    this._tailSoundLength = this._tailSoundMaxLength;
  }

  public override void OnPostShoot()
  {
    if (!(bool) (Object) this._tailAudioSource)
      return;
    this._tailAudioSource.Stop();
    this._tailAudioSource.Play();
  }

  public override void Hide()
  {
    if (!(bool) (Object) this._tailAudioSource)
      return;
    this._tailAudioSource.Stop();
  }
}
