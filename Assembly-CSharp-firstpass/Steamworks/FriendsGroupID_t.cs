using System;

namespace Steamworks
{
	public struct FriendsGroupID_t : IEquatable<FriendsGroupID_t>, IComparable<FriendsGroupID_t>
	{
		public static readonly FriendsGroupID_t Invalid = new FriendsGroupID_t(-1);

		public short m_FriendsGroupID_t;

		public FriendsGroupID_t(short value)
		{
			m_FriendsGroupID_t = value;
		}

		public override string ToString()
		{
			return m_FriendsGroupID_t.ToString();
		}

		public override bool Equals(object other)
		{
			if (other is FriendsGroupID_t)
			{
				return this == (FriendsGroupID_t)other;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return m_FriendsGroupID_t.GetHashCode();
		}

		public bool Equals(FriendsGroupID_t other)
		{
			return m_FriendsGroupID_t == other.m_FriendsGroupID_t;
		}

		public int CompareTo(FriendsGroupID_t other)
		{
			return m_FriendsGroupID_t.CompareTo(other.m_FriendsGroupID_t);
		}

		public static bool operator ==(FriendsGroupID_t x, FriendsGroupID_t y)
		{
			return x.m_FriendsGroupID_t == y.m_FriendsGroupID_t;
		}

		public static bool operator !=(FriendsGroupID_t x, FriendsGroupID_t y)
		{
			return !(x == y);
		}

		public static explicit operator FriendsGroupID_t(short value)
		{
			return new FriendsGroupID_t(value);
		}

		public static explicit operator short(FriendsGroupID_t that)
		{
			return that.m_FriendsGroupID_t;
		}
	}
}
