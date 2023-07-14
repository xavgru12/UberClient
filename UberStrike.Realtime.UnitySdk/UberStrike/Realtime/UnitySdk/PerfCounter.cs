// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.PerfCounter
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

namespace UberStrike.Realtime.UnitySdk
{
  public enum PerfCounter
  {
    MemoryAvailable = 0,
    ProcessorTime = 1,
    TotalPhysicalMemory = 2,
    ProcessPrivateBytes = 10, // 0x0000000A
    ProcessRuntime = 11, // 0x0000000B
    VirtualBytes = 12, // 0x0000000C
    CountPlayers = 20, // 0x00000014
    CountGames = 21, // 0x00000015
    PhotonPeers = 30, // 0x0000001E
    DotNetExceptions = 40, // 0x00000028
    DotNetCurrentThreads = 41, // 0x00000029
    DotNetBytesInAllHeaps = 42, // 0x0000002A
    DotNetLOH = 43, // 0x0000002B
  }
}
