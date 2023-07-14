// Decompiled with JetBrains decompiler
// Type: ParticleEffectController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectController : MonoBehaviour
{
  [SerializeField]
  private ParticleEffectController.ParticleConfiguration[] _allWeaponData;
  [SerializeField]
  private ParticleEmitter _pickupParticleEmitter;
  [SerializeField]
  private HeatWave _heatWavePrefab;
  [SerializeField]
  private ParticleEmitter _heatWave;
  private Dictionary<ParticleConfigurationType, ParticleConfigurationPerWeapon> _allConfigurations;
  private static Dictionary<Vector3, float> _effects = new Dictionary<Vector3, float>();
  private static float _nextCleanup;
  private ExplosionController _explosionParticleSystem;

  public static ParticleEffectController Instance { get; private set; }

  public static bool Exists => (UnityEngine.Object) ParticleEffectController.Instance != (UnityEngine.Object) null;

  private void Awake()
  {
    ParticleEffectController.Instance = this;
    this._explosionParticleSystem = new ExplosionController();
    this._allConfigurations = new Dictionary<ParticleConfigurationType, ParticleConfigurationPerWeapon>();
    foreach (ParticleEffectController.ParticleConfiguration particleConfiguration in this._allWeaponData)
      this._allConfigurations[particleConfiguration.Type] = particleConfiguration.Configuration;
    Singleton<ExplosionManager>.Instance.HeatWavePrefab = this._heatWavePrefab;
  }

  public static void ShowPickUpEffect(Vector3 pos, int count)
  {
    if (!ParticleEffectController.Exists)
      return;
    ParticleEffectController.Instance._pickupParticleEmitter.transform.position = pos;
    ParticleEffectController.Instance._pickupParticleEmitter.Emit(count);
  }

  public static void ShowHeatwaveEffect(Vector3 pos)
  {
    if (!ParticleEffectController.Exists || !(bool) (UnityEngine.Object) ParticleEffectController.Instance._heatWave || ApplicationDataManager.IsMobile)
      return;
    ParticleEffectController.Instance._heatWave.Emit(pos, Vector3.zero, 1f, 1f, Color.white);
  }

  public static void ShowHitEffect(
    ParticleConfigurationType effectType,
    SurfaceEffectType surface,
    Vector3 direction,
    Vector3 hitPoint,
    Vector3 hitNormal,
    Vector3 muzzlePosition,
    float distance,
    ref MoveTrailrendererObject trailRenderer,
    Transform parent)
  {
    ParticleEffectController.ShowHitEffect(effectType, surface, direction, hitPoint, hitNormal, muzzlePosition, distance, ref trailRenderer, parent, 0);
  }

  public static void ShowHitEffect(
    ParticleConfigurationType effectType,
    SurfaceEffectType surface,
    Vector3 direction,
    Vector3 hitPoint,
    Vector3 hitNormal,
    Vector3 muzzlePosition,
    float distance,
    ref MoveTrailrendererObject trailRenderer,
    Transform parent,
    int damage)
  {
    if (ParticleEffectController.Exists)
    {
      ParticleConfigurationPerWeapon allConfiguration = ParticleEffectController.Instance._allConfigurations[effectType];
      if (!((UnityEngine.Object) allConfiguration != (UnityEngine.Object) null))
        return;
      ParticleEffectController.ShowTrailEffect(allConfiguration, trailRenderer, parent, hitPoint, muzzlePosition, distance, direction);
      switch (surface)
      {
        case SurfaceEffectType.Default:
          if (!ParticleEffectController.CheckVisibility(hitPoint))
            break;
          ParticleEmissionSystem.FireParticles(hitPoint, hitNormal, allConfiguration.WeaponImpactEffectConfiguration.FireParticleConfigurationForInstantHit);
          break;
        case SurfaceEffectType.WoodEffect:
          if (!ParticleEffectController.CheckVisibility(hitPoint))
            break;
          ParticleEmissionSystem.HitMaterialParticles(hitPoint, hitNormal, allConfiguration.WeaponImpactEffectConfiguration.SurfaceParameterSet.WoodEffect);
          ParticleEmissionSystem.FireParticles(hitPoint, hitNormal, allConfiguration.WeaponImpactEffectConfiguration.FireParticleConfigurationForInstantHit);
          break;
        case SurfaceEffectType.WaterEffect:
          if (!ParticleEffectController.CheckVisibility(hitPoint))
            break;
          ParticleEmissionSystem.WaterCircleParticles(hitPoint, hitNormal, allConfiguration.WeaponImpactEffectConfiguration.SurfaceParameterSet.WaterCircleEffect);
          break;
        case SurfaceEffectType.StoneEffect:
          if (!ParticleEffectController.CheckVisibility(hitPoint))
            break;
          ParticleEmissionSystem.HitMaterialParticles(hitPoint, hitNormal, allConfiguration.WeaponImpactEffectConfiguration.SurfaceParameterSet.StoneEffect);
          ParticleEmissionSystem.FireParticles(hitPoint, hitNormal, allConfiguration.WeaponImpactEffectConfiguration.FireParticleConfigurationForInstantHit);
          break;
        case SurfaceEffectType.MetalEffect:
          if (!ParticleEffectController.CheckVisibility(hitPoint))
            break;
          ParticleEmissionSystem.HitMaterialParticles(hitPoint, hitNormal, allConfiguration.WeaponImpactEffectConfiguration.SurfaceParameterSet.MetalEffect);
          ParticleEmissionSystem.FireParticles(hitPoint, hitNormal, allConfiguration.WeaponImpactEffectConfiguration.FireParticleConfigurationForInstantHit);
          break;
        case SurfaceEffectType.GrassEffect:
          if (!ParticleEffectController.CheckVisibility(hitPoint))
            break;
          ParticleEmissionSystem.HitMaterialParticles(hitPoint, hitNormal, allConfiguration.WeaponImpactEffectConfiguration.SurfaceParameterSet.GrassEffect);
          ParticleEmissionSystem.FireParticles(hitPoint, hitNormal, allConfiguration.WeaponImpactEffectConfiguration.FireParticleConfigurationForInstantHit);
          break;
        case SurfaceEffectType.SandEffect:
          if (!ParticleEffectController.CheckVisibility(hitPoint))
            break;
          ParticleEmissionSystem.HitMaterialParticles(hitPoint, hitNormal, allConfiguration.WeaponImpactEffectConfiguration.SurfaceParameterSet.SandEffect);
          ParticleEmissionSystem.FireParticles(hitPoint, hitNormal, allConfiguration.WeaponImpactEffectConfiguration.FireParticleConfigurationForInstantHit);
          break;
        case SurfaceEffectType.Splat:
          if (!ParticleEffectController.CheckVisibility(hitPoint))
            break;
          ParticleEmissionSystem.HitMaterialRotatingParticles(hitPoint, hitNormal, allConfiguration.WeaponImpactEffectConfiguration.SurfaceParameterSet.Splat);
          break;
      }
    }
    else
      Debug.LogError((object) "ParticleEffectController is not attached to a gameobject in scene!");
  }

  private static void ShowTrailEffect(
    ParticleConfigurationPerWeapon effect,
    MoveTrailrendererObject trailRenderer,
    Transform parent,
    Vector3 hitPoint,
    Vector3 muzzlePosition,
    float distance,
    Vector3 direction)
  {
    if (effect.WeaponImpactEffectConfiguration.UseTrailrendererForTrail)
    {
      if (!((UnityEngine.Object) effect.WeaponImpactEffectConfiguration.TrailrendererTrailPrefab != (UnityEngine.Object) null))
        return;
      if ((UnityEngine.Object) trailRenderer == (UnityEngine.Object) null)
      {
        trailRenderer = UnityEngine.Object.Instantiate((UnityEngine.Object) effect.WeaponImpactEffectConfiguration.TrailrendererTrailPrefab, muzzlePosition, Quaternion.identity) as MoveTrailrendererObject;
        trailRenderer.gameObject.transform.parent = parent;
      }
      trailRenderer.MoveTrail(hitPoint, muzzlePosition, distance);
    }
    else
      ParticleEmissionSystem.TrailParticles(hitPoint, direction, effect.WeaponImpactEffectConfiguration.TrailParticleConfigurationForInstantHit, muzzlePosition, distance);
  }

  public static void ShowExplosionEffect(
    ParticleConfigurationType effectType,
    SurfaceEffectType surface,
    Vector3 hitPoint,
    Vector3 hitNormal)
  {
    if (!ParticleEffectController.Exists || !ParticleEffectController.CheckVisibility(hitPoint))
      return;
    ParticleConfigurationPerWeapon allConfiguration = ParticleEffectController.Instance._allConfigurations[effectType];
    bool flag1 = false;
    if (!((UnityEngine.Object) allConfiguration != (UnityEngine.Object) null))
      return;
    switch (surface)
    {
      case SurfaceEffectType.WoodEffect:
        ParticleEmissionSystem.HitMateriaHalfSphericParticles(hitPoint, hitNormal, allConfiguration.WeaponImpactEffectConfiguration.SurfaceParameterSet.WoodEffect);
        break;
      case SurfaceEffectType.WaterEffect:
        ParticleEmissionSystem.WaterCircleParticles(hitPoint, hitNormal, allConfiguration.WeaponImpactEffectConfiguration.SurfaceParameterSet.WaterCircleEffect);
        ParticleEmissionSystem.WaterSplashParticles(hitPoint, hitNormal, allConfiguration.WeaponImpactEffectConfiguration.SurfaceParameterSet.WaterExtraSplashEffect);
        break;
      case SurfaceEffectType.StoneEffect:
        ParticleEmissionSystem.HitMateriaHalfSphericParticles(hitPoint, hitNormal, allConfiguration.WeaponImpactEffectConfiguration.SurfaceParameterSet.StoneEffect);
        break;
      case SurfaceEffectType.MetalEffect:
        ParticleEmissionSystem.HitMateriaHalfSphericParticles(hitPoint, hitNormal, allConfiguration.WeaponImpactEffectConfiguration.SurfaceParameterSet.MetalEffect);
        break;
      case SurfaceEffectType.GrassEffect:
        ParticleEmissionSystem.HitMateriaHalfSphericParticles(hitPoint, hitNormal, allConfiguration.WeaponImpactEffectConfiguration.SurfaceParameterSet.GrassEffect);
        break;
      case SurfaceEffectType.SandEffect:
        ParticleEmissionSystem.HitMateriaHalfSphericParticles(hitPoint, hitNormal, allConfiguration.WeaponImpactEffectConfiguration.SurfaceParameterSet.SandEffect);
        break;
      case SurfaceEffectType.Splat:
        ParticleEmissionSystem.HitMateriaFullSphericParticles(hitPoint, hitNormal, allConfiguration.WeaponImpactEffectConfiguration.SurfaceParameterSet.Splat);
        break;
    }
    bool flag2 = QualitySettings.GetQualityLevel() > 0;
    if (flag2)
    {
      ParticleEffectController.Instance._explosionParticleSystem.EmitDust(hitPoint, hitNormal, allConfiguration.WeaponImpactEffectConfiguration.ExplosionParameterSet.DustParameters);
      ParticleEffectController.Instance._explosionParticleSystem.EmitSmoke(hitPoint, hitNormal, allConfiguration.WeaponImpactEffectConfiguration.ExplosionParameterSet.SmokeParameters);
    }
    if (flag2 || flag1)
      ParticleEffectController.Instance._explosionParticleSystem.EmitTrail(hitPoint, hitNormal, allConfiguration.WeaponImpactEffectConfiguration.ExplosionParameterSet.TrailParameters);
    ParticleEffectController.Instance._explosionParticleSystem.EmitBlast(hitPoint, hitNormal, allConfiguration.WeaponImpactEffectConfiguration.ExplosionParameterSet.BlastParameters);
    ParticleEffectController.Instance._explosionParticleSystem.EmitRing(hitPoint, hitNormal, allConfiguration.WeaponImpactEffectConfiguration.ExplosionParameterSet.RingParameters);
    ParticleEffectController.Instance._explosionParticleSystem.EmitSpark(hitPoint, hitNormal, allConfiguration.WeaponImpactEffectConfiguration.ExplosionParameterSet.SparkParameters);
  }

  private static void WaterRipplesEffect(
    ParticleConfigurationPerWeapon effect,
    Vector3 hitPoint,
    Vector3 direction,
    Vector3 muzzlePosition,
    float distance)
  {
    float num = (float) ((double) Math.Abs(muzzlePosition.y) * (double) distance / ((double) Math.Abs(hitPoint.y) + (double) Math.Abs(muzzlePosition.y)));
    Vector3 vector3 = direction * num + muzzlePosition;
    if (!ParticleEffectController.CanPlayEffectAt(vector3) || !ParticleEffectController.CheckVisibility(vector3))
      return;
    ParticleEmissionSystem.WaterSplashParticles(vector3, Vector3.up, effect.WeaponImpactEffectConfiguration.SurfaceParameterSet.WaterExtraSplashEffect);
    ParticleEmissionSystem.WaterCircleParticles(vector3, Vector3.up, effect.WeaponImpactEffectConfiguration.SurfaceParameterSet.WaterCircleEffect);
  }

  private static Vector3 PositionRaster(Vector3 v) => new Vector3((float) Mathf.RoundToInt(v[0]), (float) Mathf.RoundToInt(v[1]), (float) Mathf.RoundToInt(v[2]));

  private static bool CanPlayEffectAt(Vector3 v)
  {
    if ((double) ParticleEffectController._nextCleanup < (double) Time.time)
    {
      ParticleEffectController._nextCleanup = Time.time + 30f;
      ParticleEffectController._effects.Clear();
    }
    Vector3 key = ParticleEffectController.PositionRaster(v);
    float num;
    if (ParticleEffectController._effects.TryGetValue(key, out num) && (double) num >= (double) Time.time)
      return false;
    ParticleEffectController._effects[key] = Time.time + 1f;
    return true;
  }

  public static void ProjectileWaterRipplesEffect(
    ParticleConfigurationType effectType,
    Vector3 hitPosition)
  {
    if (!ParticleEffectController.Exists || !GameState.HasCurrentSpace)
      return;
    ParticleConfigurationPerWeapon allConfiguration = ParticleEffectController.Instance._allConfigurations[effectType];
    if (!((UnityEngine.Object) allConfiguration != (UnityEngine.Object) null))
      return;
    Vector3 hitPoint = hitPosition;
    ParticleEmissionSystem.WaterSplashParticles(hitPoint, Vector3.up, allConfiguration.WeaponImpactEffectConfiguration.SurfaceParameterSet.WaterExtraSplashEffect);
    ParticleEmissionSystem.WaterCircleParticles(hitPoint, Vector3.up, allConfiguration.WeaponImpactEffectConfiguration.SurfaceParameterSet.WaterCircleEffect);
  }

  private static bool CheckVisibility(Vector3 hitPoint) => true;

  [Serializable]
  private class ParticleConfiguration
  {
    [HideInInspector]
    public string Name = "Effect";
    public ParticleConfigurationType Type;
    public ParticleConfigurationPerWeapon Configuration;

    public ParticleConfiguration(
      string name,
      ParticleConfigurationType type,
      ParticleConfigurationPerWeapon configuration)
    {
      this.Name = name;
      this.Type = type;
      this.Configuration = configuration;
    }
  }
}
