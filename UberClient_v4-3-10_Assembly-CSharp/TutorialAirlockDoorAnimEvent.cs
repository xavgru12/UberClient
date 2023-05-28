// Decompiled with JetBrains decompiler
// Type: TutorialAirlockDoorAnimEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class TutorialAirlockDoorAnimEvent : MonoBehaviour
{
  private Dictionary<string, AudioSource> _doorAudioSources;
  private Dictionary<string, float> _doorLockTiming;

  private void Awake()
  {
    this._doorLockTiming = new Dictionary<string, float>();
    this._doorAudioSources = new Dictionary<string, AudioSource>();
    this._doorLockTiming.Add("AirlockDoor", 0.0f);
    this._doorLockTiming.Add("Gear4", 0.5f);
    this._doorLockTiming.Add("Gear3", 0.6666667f);
    this._doorLockTiming.Add("Gear2", 0.8333333f);
    this._doorLockTiming.Add("Gear1", 1f);
    this._doorLockTiming.Add("Gear10", 1.16666663f);
    this._doorLockTiming.Add("Gear9", 1.33333337f);
    this._doorLockTiming.Add("Gear8", 1.5f);
    foreach (AudioSource componentsInChild in this.GetComponentsInChildren<AudioSource>(true))
    {
      AnimationEvent evt = new AnimationEvent();
      evt.functionName = "OnDoorUnlock";
      evt.stringParameter = componentsInChild.gameObject.name;
      float num;
      if (this._doorLockTiming.TryGetValue(evt.stringParameter, out num))
      {
        evt.time = num;
        this.animation.clip.AddEvent(evt);
        this._doorAudioSources.Add(evt.stringParameter, componentsInChild);
      }
      else
        Debug.LogError((object) ("Failed to get door lock: " + evt.stringParameter));
    }
  }

  private void OnDoorUnlock(string lockName)
  {
    AudioSource audioSource;
    if (!this._doorAudioSources.TryGetValue(lockName, out audioSource))
      return;
    audioSource.Play();
    this._doorAudioSources.Remove(lockName);
  }
}
