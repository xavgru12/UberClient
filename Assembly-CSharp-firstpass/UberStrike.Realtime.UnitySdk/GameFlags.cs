using System;

namespace UberStrike.Realtime.UnitySdk
{
	public class GameFlags
	{
		[Flags]
		public enum GAME_FLAGS
		{
			None = 0x0,
			LowGravity = 0x1,
			NoArmor = 0x2,
			QuickSwitch = 0x4,
			MeleeOnly = 0x8,
			KnockBack = 0x10
		}

		private GAME_FLAGS gameFlags;

		public bool LowGravity
		{
			get
			{
				return IsFlagSet(GAME_FLAGS.LowGravity);
			}
			set
			{
				SetFlag(GAME_FLAGS.LowGravity, value);
			}
		}

		public bool KnockBack
		{
			get
			{
				return IsFlagSet(GAME_FLAGS.KnockBack);
			}
			set
			{
				SetFlag(GAME_FLAGS.KnockBack, value);
			}
		}

		public bool NoArmor
		{
			get
			{
				return IsFlagSet(GAME_FLAGS.NoArmor);
			}
			set
			{
				SetFlag(GAME_FLAGS.NoArmor, value);
			}
		}

		public bool QuickSwitch
		{
			get
			{
				return IsFlagSet(GAME_FLAGS.QuickSwitch);
			}
			set
			{
				SetFlag(GAME_FLAGS.QuickSwitch, value);
			}
		}

		public bool MeleeOnly
		{
			get
			{
				return IsFlagSet(GAME_FLAGS.MeleeOnly);
			}
			set
			{
				SetFlag(GAME_FLAGS.MeleeOnly, value);
			}
		}

		public int ToInt()
		{
			return (int)gameFlags;
		}

		public static bool IsFlagSet(GAME_FLAGS flag, int state)
		{
			return (state & (int)flag) != 0;
		}

		private bool IsFlagSet(GAME_FLAGS f)
		{
			return (gameFlags & f) == f;
		}

		private void SetFlag(GAME_FLAGS f, bool b)
		{
			gameFlags = ((!b) ? (gameFlags & ~f) : (gameFlags | f));
		}

		public void SetFlags(int flags)
		{
			gameFlags = (GAME_FLAGS)flags;
		}

		public void ResetFlags()
		{
			gameFlags = GAME_FLAGS.None;
		}
	}
}
