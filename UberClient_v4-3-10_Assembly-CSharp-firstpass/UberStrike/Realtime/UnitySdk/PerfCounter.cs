// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.PerfCounter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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
