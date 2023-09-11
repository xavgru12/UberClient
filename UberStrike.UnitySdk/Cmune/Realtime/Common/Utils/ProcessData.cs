// Decompiled with JetBrains decompiler
// Type: Cmune.Realtime.Common.Utils.ProcessData
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

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
