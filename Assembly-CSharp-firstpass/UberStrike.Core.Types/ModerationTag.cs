using System;

namespace UberStrike.Core.Types
{
	[Flags]
	public enum ModerationTag
	{
		None = 0x0,
		Muted = 0x1,
		Ghosted = 0x2,
		Banned = 0x4,
		Speedhacking = 0x8,
		Spamming = 0x10,
		Language = 0x20,
		Name = 0x40
	}
}
