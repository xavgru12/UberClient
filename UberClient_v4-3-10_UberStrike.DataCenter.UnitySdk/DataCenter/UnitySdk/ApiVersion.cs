// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.UnitySdk.ApiVersion
// Assembly: UberStrike.DataCenter.UnitySdk, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: 1925C691-C9DE-44B0-95F4-3171E7957DDD
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.DataCenter.UnitySdk.dll

using UnityEngine;

namespace UberStrike.DataCenter.UnitySdk
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
