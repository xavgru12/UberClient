
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
