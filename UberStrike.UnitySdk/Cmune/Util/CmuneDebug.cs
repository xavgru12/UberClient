// Decompiled with JetBrains decompiler
// Type: Cmune.Util.CmuneDebug
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System;
using System.Collections.Generic;

namespace Cmune.Util
{
  public static class CmuneDebug
  {
    public static int DebugLevel = 0;
    public static bool DebugFileInfo = false;
    private static List<ICmuneDebug> _debugChannels = new List<ICmuneDebug>();

    public static bool IsDebugEnabled => CmuneDebug.DebugLevel == 0;

    public static bool IsWarningEnabled => CmuneDebug.DebugLevel <= 1;

    public static bool IsErrorEnabled => CmuneDebug.DebugLevel <= 2;

    public static void Assert(bool condition, string message)
    {
      if (condition)
        return;
      CmuneDebug.LogError("ASSERT: " + message, new object[0]);
    }

    public static void AddDebugChannel(ICmuneDebug channel)
    {
      if (CmuneDebug._debugChannels.Exists((Predicate<ICmuneDebug>) (p => (object) p.GetType() == (object) channel.GetType())))
        return;
      CmuneDebug._debugChannels.Add(channel);
    }

    public static System.Exception Exception(
      System.Exception innerException,
      string str,
      params object[] args)
    {
      string str1 = string.Format(str + "\n" + innerException.Message, args);
      CmuneDebug.LogError(str1, new object[0]);
      return new System.Exception(str1);
    }

    public static System.Exception Exception(string str, params object[] args)
    {
      string.Format(str, args);
      CmuneDebug.LogError(str, args);
      return new System.Exception(string.Format(str, args));
    }

    public static void Log(string str, params object[] args) => CmuneDebug.log(string.Format(str, args), 0);

    public static void LogWarning(string str, params object[] args) => CmuneDebug.log("[WARNING] " + string.Format(str, args), 1);

    public static void LogError(string str, params object[] args) => CmuneDebug.log("[ERROR] " + string.Format(str, args), 2);

    public static void LogInfo(string str, params object[] args) => CmuneDebug.log(string.Format(str, args), 3);

    public static void Log(object t) => CmuneDebug.log(t.ToString(), 0);

    public static void LogWarning(object t) => CmuneDebug.log("[WARNING] " + t.ToString(), 1);

    public static void LogError(object t) => CmuneDebug.log("[ERROR] " + t.ToString(), 2);

    public static void LogInfo(object t) => CmuneDebug.log(t.ToString(), 3);

    private static void log(string s, int i)
    {
      if (i < CmuneDebug.DebugLevel)
        return;
      foreach (ICmuneDebug debugChannel in CmuneDebug._debugChannels)
        debugChannel.Log(i, s);
    }
  }
}
