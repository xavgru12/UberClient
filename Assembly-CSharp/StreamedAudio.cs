// Decompiled with JetBrains decompiler
// Type: StreamedAudio
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[RequireComponent(typeof (AudioSource))]
internal class StreamedAudio : MonoBehaviour
{
  [SerializeField]
  private string _clipName;

  private void OnEnable() => AutoMonoBehaviour<StreamedAudioPlayer>.Instance.PlayMusic(this.audio, this._clipName);

  private void OnDisable() => AutoMonoBehaviour<StreamedAudioPlayer>.Instance.StopMusic(this.audio);
}
