// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.Client.RemoteProcedureCall
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;

namespace UberStrike.Realtime.Client
{
  public delegate bool RemoteProcedureCall(
    byte customOpCode,
    Dictionary<byte, object> customOpParameters,
    bool sendReliable = true,
    byte channelId = 0,
    bool encryption = false);
}
