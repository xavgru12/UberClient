// Decompiled with JetBrains decompiler
// Type: ParticleEmissionSystem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public static class ParticleEmissionSystem
{
  public static void TrailParticles(
    Vector3 emitPoint,
    Vector3 direction,
    TrailParticleConfiguration particleConfiguration,
    Vector3 muzzlePosition,
    float distance)
  {
    if (!((Object) particleConfiguration.ParticleEmitter != (Object) null))
      return;
    float num = 200f;
    Vector3 velocity = direction * num;
    float energy = (float) ((double) distance / (double) num * 0.89999997615814209);
    if ((double) distance <= 3.0)
      return;
    particleConfiguration.ParticleEmitter.Emit(muzzlePosition + direction * 3f, velocity, Random.Range(particleConfiguration.ParticleMinSize, particleConfiguration.ParticleMaxSize), energy, particleConfiguration.ParticleColor);
  }

  public static void FireParticles(
    Vector3 hitPoint,
    Vector3 hitNormal,
    FireParticleConfiguration particleConfiguration)
  {
    if (!((Object) particleConfiguration.ParticleEmitter != (Object) null))
      return;
    Vector3 velocity = Vector3.zero;
    Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hitNormal);
    Vector3 zero = Vector3.zero;
    for (int index = 0; index < particleConfiguration.ParticleCount; ++index)
    {
      velocity.x = 0.0f + Random.Range(0.0f, 1f / 1000f);
      velocity.y = 2f + Random.Range(0.0f, 0.4f);
      velocity.z = 0.0f + Random.Range(0.0f, 1f / 1000f);
      velocity = rotation * velocity;
      Vector3 pos = hitPoint;
      pos.x += Random.Range(0.0f, 0.2f);
      pos.z += Random.Range(0.0f, 0.4f) * -1f;
      particleConfiguration.ParticleEmitter.Emit(pos, velocity, Random.Range(particleConfiguration.ParticleMinSize, particleConfiguration.ParticleMaxSize), Random.Range(particleConfiguration.ParticleMinLiveTime, particleConfiguration.ParticleMaxLiveTime), particleConfiguration.ParticleColor);
    }
  }

  public static void WaterCircleParticles(
    Vector3 hitPoint,
    Vector3 hitNormal,
    FireParticleConfiguration particleConfiguration)
  {
    if (!((Object) particleConfiguration.ParticleEmitter != (Object) null))
      return;
    Vector3 zero = Vector3.zero;
    for (int index = 0; index < particleConfiguration.ParticleCount; ++index)
    {
      zero.x = Random.Range(0.0f, 0.3f);
      zero.z = Random.Range(0.0f, 0.3f);
      particleConfiguration.ParticleEmitter.Emit(hitPoint, zero, Random.Range(particleConfiguration.ParticleMinSize, particleConfiguration.ParticleMaxSize), Random.Range(particleConfiguration.ParticleMinLiveTime, particleConfiguration.ParticleMaxLiveTime), particleConfiguration.ParticleColor);
    }
  }

  public static void WaterSplashParticles(
    Vector3 hitPoint,
    Vector3 hitNormal,
    FireParticleConfiguration particleConfiguration)
  {
    if (!((Object) particleConfiguration.ParticleEmitter != (Object) null))
      return;
    Vector3 zero = Vector3.zero;
    for (int index = 0; index < particleConfiguration.ParticleCount; ++index)
    {
      zero.x = Random.Range(0.0f, 0.3f);
      zero.y = 2f + Random.Range(0.0f, 0.3f);
      zero.z = Random.Range(0.0f, 0.3f);
      particleConfiguration.ParticleEmitter.Emit(hitPoint, zero, Random.Range(particleConfiguration.ParticleMinSize, particleConfiguration.ParticleMaxSize), Random.Range(particleConfiguration.ParticleMinLiveTime, particleConfiguration.ParticleMaxLiveTime), particleConfiguration.ParticleColor);
    }
  }

  public static void HitMaterialParticles(
    Vector3 hitPoint,
    Vector3 hitNormal,
    ParticleConfiguration particleConfiguration)
  {
    if (!((Object) particleConfiguration.ParticleEmitter != (Object) null))
      return;
    Vector3 velocity = Vector3.zero;
    Quaternion quaternion = new Quaternion();
    Quaternion rotation = Quaternion.FromToRotation(Vector3.back, hitNormal);
    for (int index = 0; index < particleConfiguration.ParticleCount; ++index)
    {
      Vector2 vector2 = Random.insideUnitCircle * Random.Range(particleConfiguration.ParticleMinSpeed, particleConfiguration.ParticleMaxSpeed);
      velocity.x = vector2.x;
      velocity.y = vector2.y;
      velocity.z = Random.Range(particleConfiguration.ParticleMinZVelocity, particleConfiguration.ParticleMaxZVelocity) * -1f;
      velocity = rotation * velocity;
      particleConfiguration.ParticleEmitter.Emit(hitPoint, velocity, Random.Range(particleConfiguration.ParticleMinSize, particleConfiguration.ParticleMaxSize), Random.Range(particleConfiguration.ParticleMinLiveTime, particleConfiguration.ParticleMaxLiveTime), particleConfiguration.ParticleColor);
    }
  }

  public static void HitMaterialRotatingParticles(
    Vector3 hitPoint,
    Vector3 hitNormal,
    ParticleConfiguration particleConfiguration)
  {
    if (!((Object) particleConfiguration.ParticleEmitter != (Object) null))
      return;
    Vector3 velocity = Vector3.zero;
    Quaternion quaternion = new Quaternion();
    Quaternion rotation = Quaternion.FromToRotation(Vector3.back, hitNormal);
    for (int index = 0; index < particleConfiguration.ParticleCount; ++index)
    {
      Vector2 vector2 = Random.insideUnitCircle * Random.Range(particleConfiguration.ParticleMinSpeed, particleConfiguration.ParticleMaxSpeed);
      velocity.x = vector2.x;
      velocity.y = vector2.y;
      velocity.z = Random.Range(particleConfiguration.ParticleMinZVelocity, particleConfiguration.ParticleMaxZVelocity) * -1f;
      velocity = rotation * velocity;
      particleConfiguration.ParticleEmitter.Emit(hitPoint, velocity, Random.Range(particleConfiguration.ParticleMinSize, particleConfiguration.ParticleMaxSize), Random.Range(particleConfiguration.ParticleMinLiveTime, particleConfiguration.ParticleMaxLiveTime), particleConfiguration.ParticleColor, Random.Range(0.0f, 360f), 0.0f);
    }
  }

  public static void HitMateriaHalfSphericParticles(
    Vector3 hitPoint,
    Vector3 hitNormal,
    ParticleConfiguration particleConfiguration)
  {
    if (!((Object) particleConfiguration.ParticleEmitter != (Object) null))
      return;
    Vector3 zero = Vector3.zero;
    Quaternion quaternion = new Quaternion();
    Quaternion rotation = Quaternion.FromToRotation(Vector3.back, hitNormal);
    for (int index = 0; index < particleConfiguration.ParticleCount; ++index)
    {
      Vector3 velocity = Random.insideUnitSphere * Random.Range(particleConfiguration.ParticleMinSpeed, particleConfiguration.ParticleMaxSpeed);
      if ((double) velocity.z > 0.0)
        velocity.z *= -1f;
      velocity = rotation * velocity;
      particleConfiguration.ParticleEmitter.Emit(hitPoint, velocity, Random.Range(particleConfiguration.ParticleMinSize, particleConfiguration.ParticleMaxSize), Random.Range(particleConfiguration.ParticleMinLiveTime, particleConfiguration.ParticleMaxLiveTime), particleConfiguration.ParticleColor);
    }
  }

  public static void HitMateriaFullSphericParticles(
    Vector3 hitPoint,
    Vector3 hitNormal,
    ParticleConfiguration particleConfiguration)
  {
    if (!((Object) particleConfiguration.ParticleEmitter != (Object) null))
      return;
    Vector3 zero = Vector3.zero;
    for (int index = 0; index < particleConfiguration.ParticleCount; ++index)
    {
      Vector3 velocity = Random.insideUnitSphere * Random.Range(particleConfiguration.ParticleMinSpeed, particleConfiguration.ParticleMaxSpeed);
      particleConfiguration.ParticleEmitter.Emit(hitPoint, velocity, Random.Range(particleConfiguration.ParticleMinSize, particleConfiguration.ParticleMaxSize), Random.Range(particleConfiguration.ParticleMinLiveTime, particleConfiguration.ParticleMaxLiveTime), particleConfiguration.ParticleColor);
    }
  }
}
