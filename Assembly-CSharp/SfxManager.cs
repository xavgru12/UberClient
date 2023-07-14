// Decompiled with JetBrains decompiler
// Type: SfxManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class SfxManager : AutoMonoBehaviour<SfxManager>
{
  private AudioSource uiAudioSource;
  private AudioSource musicAudioSource;
  private AudioClip _lastFootStep;
  private AudioClip[] _footStepDirt;
  private AudioClip[] _footStepGrass;
  private AudioClip[] _footStepMetal;
  private AudioClip[] _footStepHeavyMetal;
  private AudioClip[] _footStepRock;
  private AudioClip[] _footStepSand;
  private AudioClip[] _footStepWater;
  private AudioClip[] _footStepWood;
  private AudioClip[] _swimAboveWater;
  private AudioClip[] _swimUnderWater;
  private AudioClip[] _footStepSnow;
  private AudioClip[] _footStepGlass;
  private Dictionary<string, AudioClip[]> _surfaceImpactSoundMap;

  public static float EffectsAudioVolume => ApplicationDataManager.ApplicationOptions.AudioEffectsVolume;

  public static float MusicAudioVolume => ApplicationDataManager.ApplicationOptions.AudioMusicVolume;

  public static float MasterAudioVolume => ApplicationDataManager.ApplicationOptions.AudioMasterVolume;

  private void Awake()
  {
    this.uiAudioSource = this.gameObject.AddComponent<AudioSource>();
    this.uiAudioSource.playOnAwake = false;
    this.uiAudioSource.rolloffMode = AudioRolloffMode.Linear;
    this.musicAudioSource = this.gameObject.AddComponent<AudioSource>();
    this.musicAudioSource.loop = true;
    this.musicAudioSource.playOnAwake = false;
    this._footStepDirt = new AudioClip[4]
    {
      GameAudio.FootStepDirt1,
      GameAudio.FootStepDirt2,
      GameAudio.FootStepDirt3,
      GameAudio.FootStepDirt4
    };
    this._footStepGrass = new AudioClip[4]
    {
      GameAudio.FootStepGrass1,
      GameAudio.FootStepGrass2,
      GameAudio.FootStepGrass3,
      GameAudio.FootStepGrass4
    };
    this._footStepMetal = new AudioClip[4]
    {
      GameAudio.FootStepMetal1,
      GameAudio.FootStepMetal2,
      GameAudio.FootStepMetal3,
      GameAudio.FootStepMetal4
    };
    this._footStepHeavyMetal = new AudioClip[4]
    {
      GameAudio.FootStepHeavyMetal1,
      GameAudio.FootStepHeavyMetal2,
      GameAudio.FootStepHeavyMetal3,
      GameAudio.FootStepHeavyMetal4
    };
    this._footStepRock = new AudioClip[4]
    {
      GameAudio.FootStepRock1,
      GameAudio.FootStepRock2,
      GameAudio.FootStepRock3,
      GameAudio.FootStepRock4
    };
    this._footStepSand = new AudioClip[4]
    {
      GameAudio.FootStepSand1,
      GameAudio.FootStepSand2,
      GameAudio.FootStepSand3,
      GameAudio.FootStepSand4
    };
    this._footStepWater = new AudioClip[3]
    {
      GameAudio.FootStepWater1,
      GameAudio.FootStepWater2,
      GameAudio.FootStepWater3
    };
    this._footStepWood = new AudioClip[4]
    {
      GameAudio.FootStepWood1,
      GameAudio.FootStepWood2,
      GameAudio.FootStepWood3,
      GameAudio.FootStepWood4
    };
    this._swimAboveWater = new AudioClip[4]
    {
      GameAudio.SwimAboveWater1,
      GameAudio.SwimAboveWater2,
      GameAudio.SwimAboveWater3,
      GameAudio.SwimAboveWater4
    };
    this._swimUnderWater = new AudioClip[1]
    {
      GameAudio.SwimUnderWater
    };
    this._footStepSnow = new AudioClip[4]
    {
      GameAudio.FootStepSnow1,
      GameAudio.FootStepSnow2,
      GameAudio.FootStepSnow3,
      GameAudio.FootStepSnow4
    };
    this._footStepGlass = new AudioClip[4]
    {
      GameAudio.FootStepGlass1,
      GameAudio.FootStepGlass2,
      GameAudio.FootStepGlass3,
      GameAudio.FootStepGlass4
    };
    AudioClip[] audioClipArray1 = new AudioClip[4]
    {
      GameAudio.ImpactCement1,
      GameAudio.ImpactCement2,
      GameAudio.ImpactCement3,
      GameAudio.ImpactCement4
    };
    AudioClip[] audioClipArray2 = new AudioClip[5]
    {
      GameAudio.ImpactGlass1,
      GameAudio.ImpactGlass2,
      GameAudio.ImpactGlass3,
      GameAudio.ImpactGlass4,
      GameAudio.ImpactGlass5
    };
    AudioClip[] audioClipArray3 = new AudioClip[4]
    {
      GameAudio.ImpactGrass1,
      GameAudio.ImpactGrass2,
      GameAudio.ImpactGrass3,
      GameAudio.ImpactGrass4
    };
    AudioClip[] audioClipArray4 = new AudioClip[5]
    {
      GameAudio.ImpactMetal1,
      GameAudio.ImpactMetal2,
      GameAudio.ImpactMetal3,
      GameAudio.ImpactMetal4,
      GameAudio.ImpactMetal5
    };
    AudioClip[] audioClipArray5 = new AudioClip[5]
    {
      GameAudio.ImpactSand1,
      GameAudio.ImpactSand2,
      GameAudio.ImpactSand3,
      GameAudio.ImpactSand4,
      GameAudio.ImpactSand5
    };
    AudioClip[] audioClipArray6 = new AudioClip[5]
    {
      GameAudio.ImpactStone1,
      GameAudio.ImpactStone2,
      GameAudio.ImpactStone3,
      GameAudio.ImpactStone4,
      GameAudio.ImpactStone5
    };
    AudioClip[] audioClipArray7 = new AudioClip[5]
    {
      GameAudio.ImpactWater1,
      GameAudio.ImpactWater2,
      GameAudio.ImpactWater3,
      GameAudio.ImpactWater4,
      GameAudio.ImpactWater5
    };
    AudioClip[] audioClipArray8 = new AudioClip[5]
    {
      GameAudio.ImpactWood1,
      GameAudio.ImpactWood2,
      GameAudio.ImpactWood3,
      GameAudio.ImpactWood4,
      GameAudio.ImpactWood5
    };
    this._surfaceImpactSoundMap = new Dictionary<string, AudioClip[]>()
    {
      {
        "Wood",
        audioClipArray8
      },
      {
        "SolidWood",
        audioClipArray8
      },
      {
        "Stone",
        audioClipArray6
      },
      {
        "Metal",
        audioClipArray4
      },
      {
        "Sand",
        audioClipArray5
      },
      {
        "Grass",
        audioClipArray3
      },
      {
        "Glass",
        audioClipArray2
      },
      {
        "Cement",
        audioClipArray1
      },
      {
        "Water",
        audioClipArray7
      }
    };
  }

  public static void StopAll2dAudio() => AutoMonoBehaviour<SfxManager>.Instance.uiAudioSource.Stop();

  public static void Play2dAudioClip(AudioClip soundEffect, float delay) => MonoRoutine.Start(SfxManager.Play2dAudioClipInSeconds(soundEffect, delay));

  [DebuggerHidden]
  private static IEnumerator Play2dAudioClipInSeconds(AudioClip soundEffect, float delay) => (IEnumerator) new SfxManager.\u003CPlay2dAudioClipInSeconds\u003Ec__Iterator79()
  {
    delay = delay,
    soundEffect = soundEffect,
    \u003C\u0024\u003Edelay = delay,
    \u003C\u0024\u003EsoundEffect = soundEffect
  };

  public static void Play2dAudioClip(AudioClip audioClip)
  {
    try
    {
      AutoMonoBehaviour<SfxManager>.Instance.uiAudioSource.PlayOneShot(audioClip);
    }
    catch
    {
      UnityEngine.Debug.LogError((object) "Play2dAudioClip: failed.");
    }
  }

  public static void Play3dAudioClip(AudioClip audioClip, Vector3 position)
  {
    try
    {
      AudioSource.PlayClipAtPoint(audioClip, position, AutoMonoBehaviour<SfxManager>.Instance.uiAudioSource.volume);
    }
    catch
    {
      UnityEngine.Debug.LogError((object) "Play3dAudioClip: failed.");
    }
  }

  public static void Play3dAudioClip(
    AudioClip soundEffect,
    float volume,
    float minDistance,
    float maxDistance,
    AudioRolloffMode rolloffMode,
    Vector3 position)
  {
    if ((double) minDistance <= 0.0)
      return;
    GameObject gameObject = new GameObject("One Shot Audio", new System.Type[1]
    {
      typeof (AudioSource)
    });
    float t = 0.0f;
    try
    {
      gameObject.transform.position = position;
      gameObject.audio.clip = soundEffect;
      t = gameObject.audio.clip.length;
      gameObject.audio.volume = volume;
      gameObject.audio.rolloffMode = rolloffMode;
      gameObject.audio.minDistance = minDistance;
      gameObject.audio.maxDistance = maxDistance;
      gameObject.audio.Play();
    }
    catch
    {
      UnityEngine.Debug.LogError((object) ("Play3dAudioClip: " + (object) soundEffect + " failed."));
    }
    finally
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) gameObject, t);
    }
  }

  public void PlayFootStepAudioClip(FootStepSoundType footStep, Vector3 position)
  {
    AudioClip[] audioClipArray = (AudioClip[]) null;
    switch (footStep)
    {
      case FootStepSoundType.Dirt:
        audioClipArray = this._footStepDirt;
        break;
      case FootStepSoundType.Grass:
        audioClipArray = this._footStepGrass;
        break;
      case FootStepSoundType.Metal:
        audioClipArray = this._footStepMetal;
        break;
      case FootStepSoundType.Rock:
        audioClipArray = this._footStepRock;
        break;
      case FootStepSoundType.Sand:
        audioClipArray = this._footStepSand;
        break;
      case FootStepSoundType.Water:
        audioClipArray = this._footStepWater;
        break;
      case FootStepSoundType.Wood:
        audioClipArray = this._footStepWood;
        break;
      case FootStepSoundType.Swim:
        audioClipArray = this._swimAboveWater;
        break;
      case FootStepSoundType.Dive:
        audioClipArray = this._swimUnderWater;
        break;
      case FootStepSoundType.Snow:
        audioClipArray = this._footStepSnow;
        break;
      case FootStepSoundType.HeavyMetal:
        audioClipArray = this._footStepHeavyMetal;
        break;
      case FootStepSoundType.Glass:
        audioClipArray = this._footStepGlass;
        break;
    }
    if (audioClipArray != null && audioClipArray.Length > 0)
    {
      AudioClip audioClip = (AudioClip) null;
      if (audioClipArray.Length > 1)
      {
        do
        {
          audioClip = audioClipArray[UnityEngine.Random.Range(0, audioClipArray.Length)];
        }
        while ((UnityEngine.Object) audioClip == (UnityEngine.Object) this._lastFootStep);
      }
      else if (audioClipArray.Length > 0)
        audioClip = audioClipArray[0];
      if (!((UnityEngine.Object) audioClip != (UnityEngine.Object) null))
        return;
      this._lastFootStep = audioClip;
      SfxManager.Play3dAudioClip(audioClip, position);
    }
    else
      UnityEngine.Debug.LogWarning((object) ("FootStep type not supported: " + (object) footStep));
  }

  public void PlayImpactSound(string surfaceType, Vector3 position)
  {
    AudioClip[] audioClipArray = (AudioClip[]) null;
    if (!this._surfaceImpactSoundMap.TryGetValue(surfaceType, out audioClipArray))
      return;
    SfxManager.Play3dAudioClip(audioClipArray[UnityEngine.Random.Range(0, audioClipArray.Length)], position);
  }

  public void EnableAudio(bool enabled) => AudioListener.volume = !enabled ? 0.0f : ApplicationDataManager.ApplicationOptions.AudioMasterVolume;

  public void UpdateMasterVolume()
  {
    if (!ApplicationDataManager.ApplicationOptions.AudioEnabled)
      return;
    AudioListener.volume = ApplicationDataManager.ApplicationOptions.AudioMasterVolume;
  }

  public void UpdateMusicVolume()
  {
    if (!ApplicationDataManager.ApplicationOptions.AudioEnabled)
      return;
    AutoMonoBehaviour<SfxManager>.Instance.musicAudioSource.volume = ApplicationDataManager.ApplicationOptions.AudioMusicVolume;
    AutoMonoBehaviour<BackgroundMusicPlayer>.Instance.Volume = ApplicationDataManager.ApplicationOptions.AudioMusicVolume;
  }

  public void UpdateEffectsVolume()
  {
    if (!ApplicationDataManager.ApplicationOptions.AudioEnabled)
      return;
    AutoMonoBehaviour<SfxManager>.Instance.uiAudioSource.volume = ApplicationDataManager.ApplicationOptions.AudioEffectsVolume;
  }

  public float GetSoundLength(AudioClip AudioClip)
  {
    try
    {
      return AudioClip.length;
    }
    catch
    {
      UnityEngine.Debug.LogError((object) "GetSoundLength: Failed to get AudioClip");
      return 0.0f;
    }
  }

  [Serializable]
  public class SoundValuePair
  {
    [SerializeField]
    private string _name;
    public AudioClip ID;
    public AudioClip Audio;

    public SoundValuePair(AudioClip id, AudioClip clip)
    {
      this._name = id.ToString();
      this.ID = id;
      this.Audio = clip;
    }
  }
}
