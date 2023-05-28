// Decompiled with JetBrains decompiler
// Type: StreamedAudioPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class StreamedAudioPlayer : AutoMonoBehaviour<StreamedAudioPlayer>
{
  private static int _playCounter;

  public void PlayMusic(AudioSource source, string clipName)
  {
    if (!string.IsNullOrEmpty(clipName))
      this.StartCoroutine(this.PlayMusic(source, Singleton<AudioLoader>.Instance.Get(clipName)));
    else
      this.StopMusic(source);
  }

  public void StopMusic(AudioSource source)
  {
    ++StreamedAudioPlayer._playCounter;
    source.Stop();
  }

  [DebuggerHidden]
  private IEnumerator PlayMusic(AudioSource source, AudioClip clip) => (IEnumerator) new StreamedAudioPlayer.\u003CPlayMusic\u003Ec__IteratorB()
  {
    clip = clip,
    source = source,
    \u003C\u0024\u003Eclip = clip,
    \u003C\u0024\u003Esource = source
  };
}
