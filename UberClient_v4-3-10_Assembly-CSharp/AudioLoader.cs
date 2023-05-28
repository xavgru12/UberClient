// Decompiled with JetBrains decompiler
// Type: AudioLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class AudioLoader : Singleton<AudioLoader>
{
  private Dictionary<string, AudioClip> cachedAudioClips;

  private AudioLoader() => this.cachedAudioClips = new Dictionary<string, AudioClip>();

  public IEnumerable<KeyValuePair<string, AudioClip>> AllClips => (IEnumerable<KeyValuePair<string, AudioClip>>) this.cachedAudioClips;

  private void CreateStreamedAudioClip(string name)
  {
    StreamedAudioClip streamedAudioClip = new StreamedAudioClip(name);
    this.cachedAudioClips[name] = streamedAudioClip.Clip;
  }

  public AudioClip Get(string name)
  {
    if (!this.cachedAudioClips.ContainsKey(name))
      Debug.LogWarning((object) ("AudioClip was not found : " + name));
    return this.cachedAudioClips[name];
  }
}
