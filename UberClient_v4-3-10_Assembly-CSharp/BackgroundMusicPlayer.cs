// Decompiled with JetBrains decompiler
// Type: BackgroundMusicPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class BackgroundMusicPlayer : AutoMonoBehaviour<BackgroundMusicPlayer>
{
  private MusicFader musicFaderA;
  private MusicFader musicFaderB;
  private bool toggle;

  private void Awake()
  {
    AudioSource audio1 = this.gameObject.AddComponent<AudioSource>();
    audio1.volume = 0.0f;
    audio1.loop = true;
    AudioSource audio2 = this.gameObject.AddComponent<AudioSource>();
    audio2.volume = 0.0f;
    audio2.loop = true;
    this.musicFaderA = new MusicFader(audio1);
    this.musicFaderB = new MusicFader(audio2);
  }

  public float Volume
  {
    set => this.Current.Source.volume = value;
  }

  public void Play(AudioClip clip)
  {
    if ((Object) this.Current.Source.clip != (Object) clip)
    {
      this.Current.FadeOut();
      this.toggle = !this.toggle;
      this.Current.Source.clip = clip;
      this.Current.FadeIn(SfxManager.MusicAudioVolume);
    }
    else
      this.Current.FadeIn(SfxManager.MusicAudioVolume);
  }

  public void Stop() => this.Current.FadeOut();

  private MusicFader Current => this.toggle ? this.musicFaderA : this.musicFaderB;
}
