// Decompiled with JetBrains decompiler
// Type: AudioFadeArea
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class AudioFadeArea : MonoBehaviour
{
  private static AudioFadeArea Current;
  [SerializeField]
  private AudioFadeArea.AudioArea outdoorAudio;
  [SerializeField]
  private AudioFadeArea.AudioArea indoorAudio;

  private void Awake() => this.collider.isTrigger = true;

  private void Update()
  {
    if (!((UnityEngine.Object) AudioFadeArea.Current == (UnityEngine.Object) this))
      return;
    this.outdoorAudio.Update();
    this.indoorAudio.Update();
  }

  private void OnTriggerEnter(Collider collider)
  {
    if (!(collider.tag == "Player"))
      return;
    AudioFadeArea.Current = this;
    if (this.outdoorAudio != null)
      this.outdoorAudio.Enabled = false;
    if (this.indoorAudio == null)
      return;
    this.indoorAudio.Enabled = true;
  }

  private void OnTriggerExit(Collider collider)
  {
    if (!(collider.tag == "Player"))
      return;
    if (this.outdoorAudio != null)
      this.outdoorAudio.Enabled = (UnityEngine.Object) AudioFadeArea.Current == (UnityEngine.Object) this;
    if (this.indoorAudio == null)
      return;
    this.indoorAudio.Enabled = false;
  }

  [Serializable]
  private class AudioArea
  {
    [SerializeField]
    private AudioSource[] sources;
    [SerializeField]
    private float maxVolume = 1f;
    [SerializeField]
    private float currentVolume = 1f;
    [SerializeField]
    private float fadeSpeed = 3f;
    [SerializeField]
    private bool enabled;

    public AudioArea() => this.currentVolume = !this.enabled ? 0.0f : this.maxVolume;

    public bool Enabled
    {
      get => this.enabled;
      set => this.enabled = value;
    }

    public bool Update()
    {
      if (this.enabled && (double) this.currentVolume < (double) this.maxVolume)
      {
        this.currentVolume = Mathf.Lerp(this.currentVolume, this.maxVolume, Time.deltaTime * this.fadeSpeed);
        if ((double) Mathf.Abs(this.currentVolume - this.maxVolume) < 0.0099999997764825821)
          this.currentVolume = this.maxVolume;
        Array.ForEach<AudioSource>(this.sources, (Action<AudioSource>) (s => s.volume = this.currentVolume));
        return true;
      }
      if (this.enabled || (double) this.currentVolume <= 0.0)
        return false;
      this.currentVolume = Mathf.Lerp(this.currentVolume, 0.0f, Time.deltaTime * this.fadeSpeed);
      if ((double) this.currentVolume < 0.0099999997764825821)
        this.currentVolume = 0.0f;
      Array.ForEach<AudioSource>(this.sources, (Action<AudioSource>) (s => s.volume = this.currentVolume));
      return true;
    }
  }
}
