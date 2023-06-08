using System;

namespace UberStrike.Core.Models
{
	[Flags]
	public enum BodyPart
	{
		Body = 0x1,
		Head = 0x2,
		Nuts = 0x4
	}
}
