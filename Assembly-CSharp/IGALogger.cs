// Decompiled with JetBrains decompiler
// Type: IGALogger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

internal interface IGALogger
{
  void log(string data);

  void logStartRequest(string url);

  void logSuccessfulRequest(string url);

  void logFailedRequest(string url, string error);
}
