// Decompiled with JetBrains decompiler
// Type: CoroutineMonitor
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.Util;
using System.Collections.Generic;

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
