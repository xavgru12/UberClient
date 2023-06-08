using System;
using UnityEngine;

[Serializable]
public class EnviromentSettings
{
	public enum TYPE
	{
		NONE,
		WATER,
		SURFACE,
		LATTER
	}

	public TYPE Type;

	public Bounds EnviromentBounds;

	public float GroundAcceleration = 15f;

	public float GroundFriction = 8f;

	public float AirAcceleration = 3f;

	public float WaterAcceleration = 6f;

	public float WaterFriction = 2f;

	public float FlyAcceleration = 8f;

	public float FlyFriction = 3f;

	public float SpectatorFriction = 5f;

	public float StopSpeed = 8f;

	public float Gravity = 50f;

	public void CheckPlayerEnclosure(Vector3 position, float height, out float enclosure)
	{
		if (EnviromentBounds.Contains(position))
		{
			Vector3 origin = position + Vector3.up * height;
			if (EnviromentBounds.IntersectRay(new Ray(origin, Vector3.down), out float distance))
			{
				enclosure = (height - distance) / height;
			}
			else
			{
				enclosure = 0f;
			}
		}
		else
		{
			enclosure = 0f;
		}
	}

	public override string ToString()
	{
		return string.Format("Type: ", Type);
	}
}
