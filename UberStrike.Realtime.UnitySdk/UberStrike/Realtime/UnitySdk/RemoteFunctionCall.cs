// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.RemoteFunctionCall
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

using System;
using System.Collections.Generic;

namespace UberStrike.Realtime.UnitySdk
{
  public class RemoteFunctionCall
  {
    public byte LocalAddress;
    public object[] Arguments;
    public DateTime Time;

    public RemoteFunctionCall(byte functionAddress, params object[] args)
    {
      this.LocalAddress = functionAddress;
      this.Arguments = new List<object>((IEnumerable<object>) args).ToArray();
      this.Time = DateTime.Now;
    }
  }
}
