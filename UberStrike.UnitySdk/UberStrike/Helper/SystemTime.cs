
using System;

namespace UberStrike.Helper
{
  public static class SystemTime
  {
    public static int Running => Environment.TickCount & int.MaxValue;
  }
}
