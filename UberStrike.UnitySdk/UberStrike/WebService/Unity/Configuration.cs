// Decompiled with JetBrains decompiler
// Type: UberStrike.WebService.Unity.Configuration
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

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
