// Decompiled with JetBrains decompiler
// Type: ExplosionController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class ExplosionController
{
  public void EmitBlast(Vector3 hitPoint, Vector3 hitNormal, ExplosionBaseParameters parameters)
  {
    Vector3 zero = Vector3.zero;
    if (!((Object) parameters.ParticleEmitter != (Object) null))
      return;
    for (int index = 0; index < parameters.ParticleCount; ++index)
    {
      float size = Random.Range(parameters.MinSize, parameters.MaxSize);
      float energy = Random.Range(parameters.MinLifeTime, parameters.MaxLifeTime);
      parameters.ParticleEmitter.Emit(hitPoint, zero, size, energy, Color.red);
    }
  }

  public void EmitDust(Vector3 hitPoint, Vector3 hitNormal, ExplosionDustParameters parameters)
  {
    Vector3 zero = Vector3.zero;
    if (!((Object) parameters.ParticleEmitter != (Object) null))
      return;
    for (int index = 0; index < parameters.ParticleCount; ++index)
    {
      Vector3 velocity = Random.insideUnitSphere * 0.2f;
      hitPoint += Random.insideUnitSphere * Random.Range(parameters.MinStartPositionSize, parameters.MinStartPositionSize);
      float size = Random.Range(parameters.MinSize, parameters.MaxSize);
      float energy = Random.Range(parameters.MinLifeTime, parameters.MaxLifeTime);
      parameters.ParticleEmitter.Emit(hitPoint, velocity, size, energy, Color.red);
    }
  }

  public void EmitRing(Vector3 hitPoint, Vector3 hitNormal, ExplosionRingParameters parameters)
  {
    Vector3 zero = Vector3.zero;
    float startSize = parameters.StartSize;
    float energy = Random.Range(parameters.MinLifeTime, parameters.MaxLifeTime);
    if (!((Object) parameters.ParticleEmitter != (Object) null))
      return;
    parameters.ParticleEmitter.Emit(hitPoint, zero, startSize, energy, Color.red);
  }

  public void EmitSmoke(Vector3 hitPoint, Vector3 hitNormal, ExplosionBaseParameters parameters)
  {
    Vector3 zero = Vector3.zero;
    if (!((Object) parameters.ParticleEmitter != (Object) null))
      return;
    for (int index = 0; index < parameters.ParticleCount; ++index)
    {
      float size = Random.Range(parameters.MinSize, parameters.MaxSize);
      float energy = Random.Range(parameters.MinLifeTime, parameters.MaxLifeTime);
      Vector3 velocity = Random.insideUnitSphere * 0.3f;
      parameters.ParticleEmitter.Emit(hitPoint, velocity, size, energy, Color.red);
    }
  }

  public void EmitSpark(Vector3 hitPoint, Vector3 hitNormal, ExplosionSphericParameters parameters)
  {
    Vector3 zero = Vector3.zero;
    if (!((Object) parameters.ParticleEmitter != (Object) null))
      return;
    for (int index = 0; index < parameters.ParticleCount; ++index)
    {
      float size = Random.Range(parameters.MinSize, parameters.MaxSize);
      float energy = Random.Range(parameters.MinLifeTime, parameters.MaxLifeTime);
      Vector3 velocity = Random.insideUnitSphere * parameters.Speed;
      parameters.ParticleEmitter.Emit(hitPoint, velocity, size, energy, Color.red);
    }
  }

  public void EmitTrail(Vector3 hitPoint, Vector3 hitNormal, ExplosionSphericParameters parameters)
  {
    Vector3 zero = Vector3.zero;
    if (!((Object) parameters.ParticleEmitter != (Object) null))
      return;
    for (int index = 0; index < parameters.ParticleCount; ++index)
    {
      float size = Random.Range(parameters.MinSize, parameters.MaxSize);
      float energy = Random.Range(parameters.MinLifeTime, parameters.MaxLifeTime);
      Vector3 velocity = Random.insideUnitSphere * parameters.Speed;
      parameters.ParticleEmitter.Emit(hitPoint, velocity, size, energy, Color.red);
    }
  }
}
