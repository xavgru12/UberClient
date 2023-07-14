// Decompiled with JetBrains decompiler
// Type: AudioTriggerArea
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[RequireComponent(typeof (AudioSource))]
[RequireComponent(typeof (Collider))]
public class AudioTriggerArea : MonoBehaviour
{
  [SerializeField]
  private bool loopClip;
  [SerializeField]
  private float maxVolume;
  private AudioSource audioSource;
  private float wishVolume;

  private void Awake()
  {
    this.audioSource = this.GetComponent<AudioSource>();
    this.audioSource.volume = 0.0f;
  }

  private void Update()
  {
    if (!this.audioSource.isPlaying)
      return;
    if ((double) this.audioSource.volume < (double) this.wishVolume)
      this.audioSource.volume += Time.deltaTime;
    else
      this.audioSource.volume -= Time.deltaTime;
    if ((double) this.audioSource.volume > 0.0)
      return;
    this.audioSource.Stop();
  }

  private void OnTriggerEnter(Collider collider)
  {
    if (!(collider.tag == "Player"))
      return;
    this.audioSource.loop = this.loopClip;
    this.wishVolume = this.maxVolume;
    this.audioSource.Play();
  }

  private void OnTriggerExit(Collider collider)
  {
    if (!(collider.tag == "Player") || !this.audioSource.isPlaying)
      return;
    this.wishVolume = 0.0f;
  }
}
