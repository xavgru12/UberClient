// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.ProcessData
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

using System;
using System.Diagnostics;

namespace UberStrike.Realtime.UnitySdk
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
