using System;

namespace UberStrike.Core.Models
{
	[Flags]
	public enum PlayerStates : byte
	{
		None = 0x0,
		Spectator = 0x1,
		Dead = 0x2,
		Paused = 0x4,
		Sniping = 0x8,
		Shooting = 0x10,
		Ready = 0x20,
		Offline = 0x40
	}
}
