using System;

namespace UberStrike.Core.Models
{
	[Flags]
	public enum MoveStates : byte
	{
		None = 0x0,
		Grounded = 0x1,
		Jumping = 0x2,
		Flying = 0x4,
		Ducked = 0x8,
		Wading = 0x10,
		Swimming = 0x20,
		Diving = 0x40,
		Landed = 0x80
	}
}
