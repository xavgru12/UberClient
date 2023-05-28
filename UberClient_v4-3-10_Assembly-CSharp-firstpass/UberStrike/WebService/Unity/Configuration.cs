// Decompiled with JetBrains decompiler
// Type: UberStrike.WebService.Unity.Configuration
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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
