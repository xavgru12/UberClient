using System;

namespace UberStrike.Realtime.UnitySdk
{
	[Flags]
	public enum PlayerTagFlag
	{
		None = 0x0,
		Speedhacker = 0x1,
		Ammohacker = 0x2,
		ReportedCheater = 0x4,
		ReportedAbuser = 0x8
	}
}
