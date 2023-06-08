using UnityEngine;

namespace UberStrike.Core.Models
{
	public struct ShortVector3
	{
		private Vector3 value;

		public float x => value.x;

		public float y => value.y;

		public float z => value.z;

		public ShortVector3(Vector3 value)
		{
			this.value = value;
		}

		public static implicit operator Vector3(ShortVector3 value)
		{
			return value.value;
		}

		public static implicit operator ShortVector3(Vector3 value)
		{
			return new ShortVector3(value);
		}

		public static ShortVector3 operator *(ShortVector3 vector, float value)
		{
			vector.value.x *= value;
			vector.value.y *= value;
			vector.value.z *= value;
			return vector;
		}

		public static ShortVector3 operator +(ShortVector3 vector, ShortVector3 value)
		{
			vector.value.x += value.value.x;
			vector.value.y += value.value.y;
			vector.value.z += value.value.z;
			return vector;
		}

		public static ShortVector3 operator -(ShortVector3 vector, ShortVector3 value)
		{
			vector.value.x -= value.value.x;
			vector.value.y -= value.value.y;
			vector.value.z -= value.value.z;
			return vector;
		}
	}
}
