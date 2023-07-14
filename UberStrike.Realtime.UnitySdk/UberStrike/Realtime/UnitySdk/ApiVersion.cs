// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.ApiVersion
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

using UnityEngine;

namespace UberStrike.Realtime.UnitySdk
{
  public static class ApiVersion
  {
    static ApiVersion()
    {
      try
      {
        ApiVersion.Current = typeof (ApiVersion).Assembly.GetName().Version.ToString(3);
      }
      catch
      {
        ApiVersion.Current = ApiVersion.ReadVersionFromAssembly();
      }
    }

    private static string ReadVersionFromAssembly()
    {
      try
      {
        string str1 = typeof (ApiVersion).Assembly.FullName.Split(',')[1];
        string str2 = str1.Substring(str1.LastIndexOf('=') + 1);
        return str2.Remove(str2.LastIndexOf('.'));
      }
      catch
      {
        Debug.LogError((object) "ApiVersion:ReadVersionFromAssembly failed!");
        return "1.0.0";
      }
    }

    public static string Current { get; private set; }
  }
}
