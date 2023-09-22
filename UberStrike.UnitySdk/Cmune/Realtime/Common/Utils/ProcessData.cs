
using System;
using System.Diagnostics;

namespace Cmune.Realtime.Common.Utils
{
  public static class ProcessData
  {
    private const float ByteToMb = 1048576f;

    public static float PhysicalMemory => (float) Process.GetCurrentProcess().WorkingSet64 / 1048576f;

    public static float VirtualMemory => (float) Process.GetCurrentProcess().VirtualMemorySize64 / 1048576f;

    public static string Name => Process.GetCurrentProcess().ProcessName;

    public static float PrivateMemory => (float) Process.GetCurrentProcess().PrivateMemorySize64 / 1048576f;

    public static int ID => Process.GetCurrentProcess().Id;

    public static float ProcessorTime => (float) Process.GetCurrentProcess().TotalProcessorTime.TotalSeconds;

    public static DateTime StartTime => Process.GetCurrentProcess().StartTime;
  }
}
