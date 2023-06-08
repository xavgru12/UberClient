using System;

namespace Steamworks
{
	public struct SteamItemDef_t : IEquatable<SteamItemDef_t>, IComparable<SteamItemDef_t>
	{
		public int m_SteamItemDef_t;

		public SteamItemDef_t(int value)
		{
			m_SteamItemDef_t = value;
		}

		public override string ToString()
		{
			return m_SteamItemDef_t.ToString();
		}

		public override bool Equals(object other)
		{
			if (other is SteamItemDef_t)
			{
				return this == (SteamItemDef_t)other;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return m_SteamItemDef_t.GetHashCode();
		}

		public bool Equals(SteamItemDef_t other)
		{
			return m_SteamItemDef_t == other.m_SteamItemDef_t;
		}

		public int CompareTo(SteamItemDef_t other)
		{
			return m_SteamItemDef_t.CompareTo(other.m_SteamItemDef_t);
		}

		public static bool operator ==(SteamItemDef_t x, SteamItemDef_t y)
		{
			return x.m_SteamItemDef_t == y.m_SteamItemDef_t;
		}

		public static bool operator !=(SteamItemDef_t x, SteamItemDef_t y)
		{
			return !(x == y);
		}

		public static explicit operator SteamItemDef_t(int value)
		{
			return new SteamItemDef_t(value);
		}

		public static explicit operator int(SteamItemDef_t that)
		{
			return that.m_SteamItemDef_t;
		}
	}
}
