// Decompiled with JetBrains decompiler
// Type: ExplosionManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class ExplosionManager : Singleton<ExplosionManager>
{
  private ExplosionManager()
  {
  }

  public HeatWave HeatWavePrefab { get; set; }

  public void PlayExplosionSound(Vector3 point, AudioClip clip)
  {
    if (GameState.HasCurrentSpace && GameState.CurrentSpace.HasWaterPlane && (double) GameState.CurrentSpace.WaterPlaneHeight > (double) point.y)
      clip = Random.Range(0, 2) != 0 ? GameAudio.UnderwaterExplosion2 : GameAudio.UnderwaterExplosion1;
    if (!((Object) clip != (Object) null))
      return;
    SfxManager.Play3dAudioClip(clip, point);
  }

  public void ShowExplosionEffect(
    Vector3 point,
    Vector3 normal,
    string tag,
    ParticleConfigurationType effectType)
  {
    string key = tag;
    if (key != null)
    {
      // ISSUE: reference to a compiler-generated field
      if (ExplosionManager.\u003C\u003Ef__switch\u0024map0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExplosionManager.\u003C\u003Ef__switch\u0024map0 = new Dictionary<string, int>(7)
        {
          {
            "Wood",
            0
          },
          {
            "Stone",
            1
          },
          {
            "Metal",
            2
          },
          {
            "Sand",
            3
          },
          {
            "Grass",
            4
          },
          {
            "Avatar",
            5
          },
          {
            "Water",
            6
          }
        };
      }
      int num;
      // ISSUE: reference to a compiler-generated field
      if (ExplosionManager.\u003C\u003Ef__switch\u0024map0.TryGetValue(key, out num))
      {
        switch (num)
        {
          case 0:
            ParticleEffectController.ShowExplosionEffect(effectType, SurfaceEffectType.WoodEffect, point, normal);
            return;
          case 1:
            ParticleEffectController.ShowExplosionEffect(effectType, SurfaceEffectType.StoneEffect, point, normal);
            return;
          case 2:
            ParticleEffectController.ShowExplosionEffect(effectType, SurfaceEffectType.MetalEffect, point, normal);
            return;
          case 3:
            ParticleEffectController.ShowExplosionEffect(effectType, SurfaceEffectType.SandEffect, point, normal);
            return;
          case 4:
            ParticleEffectController.ShowExplosionEffect(effectType, SurfaceEffectType.GrassEffect, point, normal);
            return;
          case 5:
            ParticleEffectController.ShowExplosionEffect(effectType, SurfaceEffectType.Splat, point, normal);
            return;
          case 6:
            ParticleEffectController.ShowExplosionEffect(effectType, SurfaceEffectType.WaterEffect, point, normal);
            return;
        }
      }
    }
    ParticleEffectController.ShowExplosionEffect(effectType, SurfaceEffectType.Default, point, normal);
  }
}
