using UnityEngine;

public class ExplosionController
{
	public void EmitBlast(Vector3 hitPoint, Vector3 hitNormal, ExplosionBaseParameters parameters)
	{
		Vector3 zero = Vector3.zero;
		if (parameters.ParticleEmitter != null)
		{
			for (int i = 0; i < parameters.ParticleCount; i++)
			{
				float size = Random.Range(parameters.MinSize, parameters.MaxSize);
				float energy = Random.Range(parameters.MinLifeTime, parameters.MaxLifeTime);
				parameters.ParticleEmitter.Emit(hitPoint, zero, size, energy, Color.red);
			}
		}
	}

	public void EmitDust(Vector3 hitPoint, Vector3 hitNormal, ExplosionDustParameters parameters)
	{
		Vector3 zero = Vector3.zero;
		if (parameters.ParticleEmitter != null)
		{
			for (int i = 0; i < parameters.ParticleCount; i++)
			{
				zero = Random.insideUnitSphere * 0.2f;
				hitPoint += Random.insideUnitSphere * Random.Range(parameters.MinStartPositionSize, parameters.MinStartPositionSize);
				float size = Random.Range(parameters.MinSize, parameters.MaxSize);
				float energy = Random.Range(parameters.MinLifeTime, parameters.MaxLifeTime);
				parameters.ParticleEmitter.Emit(hitPoint, zero, size, energy, Color.red);
			}
		}
	}

	public void EmitRing(Vector3 hitPoint, Vector3 hitNormal, ExplosionRingParameters parameters)
	{
		Vector3 zero = Vector3.zero;
		float startSize = parameters.StartSize;
		float energy = Random.Range(parameters.MinLifeTime, parameters.MaxLifeTime);
		if (parameters.ParticleEmitter != null)
		{
			parameters.ParticleEmitter.Emit(hitPoint, zero, startSize, energy, Color.red);
		}
	}

	public void EmitSmoke(Vector3 hitPoint, Vector3 hitNormal, ExplosionBaseParameters parameters)
	{
		Vector3 zero = Vector3.zero;
		if (parameters.ParticleEmitter != null)
		{
			for (int i = 0; i < parameters.ParticleCount; i++)
			{
				float size = Random.Range(parameters.MinSize, parameters.MaxSize);
				float energy = Random.Range(parameters.MinLifeTime, parameters.MaxLifeTime);
				zero = Random.insideUnitSphere * 0.3f;
				parameters.ParticleEmitter.Emit(hitPoint, zero, size, energy, Color.red);
			}
		}
	}

	public void EmitSpark(Vector3 hitPoint, Vector3 hitNormal, ExplosionSphericParameters parameters)
	{
		Vector3 zero = Vector3.zero;
		if (parameters.ParticleEmitter != null)
		{
			for (int i = 0; i < parameters.ParticleCount; i++)
			{
				float size = Random.Range(parameters.MinSize, parameters.MaxSize);
				float energy = Random.Range(parameters.MinLifeTime, parameters.MaxLifeTime);
				zero = Random.insideUnitSphere * parameters.Speed;
				parameters.ParticleEmitter.Emit(hitPoint, zero, size, energy, Color.red);
			}
		}
	}

	public void EmitTrail(Vector3 hitPoint, Vector3 hitNormal, ExplosionSphericParameters parameters)
	{
		Vector3 zero = Vector3.zero;
		if (parameters.ParticleEmitter != null)
		{
			for (int i = 0; i < parameters.ParticleCount; i++)
			{
				float size = Random.Range(parameters.MinSize, parameters.MaxSize);
				float energy = Random.Range(parameters.MinLifeTime, parameters.MaxLifeTime);
				zero = Random.insideUnitSphere * parameters.Speed;
				parameters.ParticleEmitter.Emit(hitPoint, zero, size, energy, Color.red);
			}
		}
	}
}
