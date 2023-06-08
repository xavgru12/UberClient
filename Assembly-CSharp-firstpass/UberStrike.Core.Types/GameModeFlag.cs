using System;

namespace UberStrike.Core.Types
{
	[Flags]
	public enum GameModeFlag
	{
		None = 0x0,
		All = -1,
		DeathMatch = 0x1,
		TeamDeathMatch = 0x2,
		EliminationMode = 0x4
	}
}
