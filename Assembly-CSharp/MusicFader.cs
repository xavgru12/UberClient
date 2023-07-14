// Decompiled with JetBrains decompiler
// Type: MusicFader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class MusicFader
{
  private bool _isFading;
  private float _targetVolume;
  private AudioSource _audioSource;

  public MusicFader(AudioSource audio) => this._audioSource = audio;

  public AudioSource Source => this._audioSource;

  public void FadeIn(float volume)
  {
    this._targetVolume = volume;
    if (this._isFading)
      return;
    if (!this._audioSource.isPlaying)
      this._audioSource.Play();
    MonoRoutine.Start(this.StartFading());
  }

  public void FadeOut()
  {
    this._targetVolume = 0.0f;
    if (this._isFading)
      return;
    MonoRoutine.Start(this.StartFading());
  }

  [DebuggerHidden]
  private IEnumerator StartFading() => (IEnumerator) new MusicFader.\u003CStartFading\u003Ec__IteratorA()
  {
    \u003C\u003Ef__this = this
  };
}
