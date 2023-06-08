using System;

namespace UberStrike.Realtime.UnitySdk
{
	public static class SystemTime
	{
		public static int Running => Environment.TickCount & int.MaxValue;
	}
}
