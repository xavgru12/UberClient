// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.CoroutineMonitor
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

using System.Collections.Generic;

namespace UberStrike.Realtime.UnitySdk
{
  public static class CoroutineMonitor
  {
    public static readonly Dictionary<string, RoutineState> Routines = new Dictionary<string, RoutineState>();

    public static void Start(string s)
    {
      if (!CoroutineMonitor.Routines.ContainsKey(s))
        CoroutineMonitor.Routines[s] = new RoutineState();
      ++CoroutineMonitor.Routines[s].Count;
    }

    public static void Comment(string s, string comment)
    {
      if (CoroutineMonitor.Routines.ContainsKey(s))
        CoroutineMonitor.Routines[s].State = comment;
      else
        CmuneDebug.LogError("Comment '" + comment + "' dropped as Routine not started: " + s, new object[0]);
    }

    public static void Stop(string s)
    {
      if (CoroutineMonitor.Routines.ContainsKey(s))
        --CoroutineMonitor.Routines[s].Count;
      else
        CmuneDebug.LogError("Stop dropped as Routine not started: " + s, new object[0]);
    }
  }
}
