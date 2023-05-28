// Decompiled with JetBrains decompiler
// Type: StreamedAudioClip
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class StreamedAudioClip
{
  private AudioClip _clip;
  private AudioClip _streamedAudioClip;
  private int _lastPosition;
  private bool _downloadingFinished;
  private float[] _samples;

  public StreamedAudioClip(string name)
  {
    WWW www = new WWW(ApplicationDataManager.BaseAudioURL + name);
    MonoRoutine.Start(this.StartDownloadingAudioClip(www));
    this._streamedAudioClip = www.GetAudioClip(false, true, AudioType.OGGVORBIS);
    this._samples = new float[this._streamedAudioClip.samples * this._streamedAudioClip.channels];
    this._clip = AudioClip.Create(name, this._streamedAudioClip.samples, this._streamedAudioClip.channels, this._streamedAudioClip.frequency, false, true, new AudioClip.PCMReaderCallback(this.OnPCMRead), new AudioClip.PCMSetPositionCallback(this.OnPCMSetPosition));
  }

  public StreamedAudioClip(string name, AudioClip source)
  {
    this._samples = new float[source.samples * source.channels];
    source.GetData(this._samples, 0);
    this._clip = AudioClip.Create(name, source.samples, source.channels, source.frequency, false, true, new AudioClip.PCMReaderCallback(this.OnPCMRead), new AudioClip.PCMSetPositionCallback(this.OnPCMSetPosition));
    this._downloadingFinished = true;
  }

  public AudioClip Clip => this._clip;

  public bool IsDownloaded => this._downloadingFinished;

  private void OnPCMRead(float[] data)
  {
    for (int index1 = 0; this._downloadingFinished && index1 < data.Length; ++index1)
    {
      int index2 = this._lastPosition + index1;
      data[index1] = index2 >= this._samples.Length ? 0.0f : this._samples[index2];
    }
    this._lastPosition += data.Length;
  }

  private void OnPCMSetPosition(int position) => this._lastPosition = position;

  [DebuggerHidden]
  private IEnumerator StartDownloadingAudioClip(WWW www) => (IEnumerator) new StreamedAudioClip.\u003CStartDownloadingAudioClip\u003Ec__Iterator86()
  {
    www = www,
    \u003C\u0024\u003Ewww = www,
    \u003C\u003Ef__this = this
  };
}
