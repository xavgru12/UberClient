using System;

namespace Steamworks
{
	public struct SteamItemInstanceID_t : IEquatable<SteamItemInstanceID_t>, IComparable<SteamItemInstanceID_t>
	{
		public static readonly SteamItemInstanceID_t Invalid = new SteamItemInstanceID_t(ulong.MaxValue);

		public ulong m_SteamItemInstanceID_t;

		public SteamItemInstanceID_t(ulong value)
		{
			m_SteamItemInstanceID_t = value;
		}

		public override string ToString()
		{
			return m_SteamItemInstanceID_t.ToString();
		}

		public override bool Equals(object other)
		{
			if (other is SteamItemInstanceID_t)
			{
				return this == (SteamItemInstanceID_t)other;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return m_SteamItemInstanceID_t.GetHashCode();
		}

		public bool Equals(SteamItemInstanceID_t other)
		{
			return m_SteamItemInstanceID_t == other.m_SteamItemInstanceID_t;
		}

		public int CompareTo(SteamItemInstanceID_t other)
		{
			return m_SteamItemInstanceID_t.CompareTo(other.m_SteamItemInstanceID_t);
		}

		public static bool operator ==(SteamItemInstanceID_t x, SteamItemInstanceID_t y)
		{
			return x.m_SteamItemInstanceID_t == y.m_SteamItemInstanceID_t;
		}

		public static bool operator !=(SteamItemInstanceID_t x, SteamItemInstanceID_t y)
		{
			return !(x == y);
		}

		public static explicit operator SteamItemInstanceID_t(ulong value)
		{
			return new SteamItemInstanceID_t(value);
		}

		public static explicit operator ulong(SteamItemInstanceID_t that)
		{
			return that.m_SteamItemInstanceID_t;
		}
	}
}
