// Decompiled with JetBrains decompiler
// Type: UberStrike.WebService.Unity.Configuration
// Assembly: UberStrike.DataCenter.UnitySdk, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: 1925C691-C9DE-44B0-95F4-3171E7957DDD
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.DataCenter.UnitySdk.dll

using System;

namespace UberStrike.WebService.Unity
{
  public static class Configuration
  {
    public static string WebserviceBaseUrl = "http://localhost:9000/";
    public static string EncryptionInitVector = string.Empty;
    public static string EncryptionPassPhrase = string.Empty;
    public static Action<string> RequestLogger;
    public static bool SimulateWebservicesFail = false;
  }
}
