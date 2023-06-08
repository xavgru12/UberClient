using System;

namespace UberStrike.Core.Models
{
	[Flags]
	public enum KeyState : byte
	{
		Still = 0x0,
		Forward = 0x1,
		Backward = 0x2,
		Left = 0x4,
		Right = 0x8,
		Jump = 0x10,
		Crouch = 0x20,
		Vertical = 0x3,
		Horizontal = 0xC,
		Walking = 0xF
	}
}
