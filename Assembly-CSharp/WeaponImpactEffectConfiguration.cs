// Decompiled with JetBrains decompiler
// Type: WeaponImpactEffectConfiguration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;

[Serializable]
public class WeaponImpactEffectConfiguration
{
  public ExplosionParameterSet ExplosionParameterSet;
  public FireParticleConfiguration FireParticleConfigurationForInstantHit;
  public TrailParticleConfiguration TrailParticleConfigurationForInstantHit;
  public SurfaceParameters SurfaceParameterSet;
  public MoveTrailrendererObject TrailrendererTrailPrefab;
  public bool UseTrailrendererForTrail;
}
