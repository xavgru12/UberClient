// Decompiled with JetBrains decompiler
// Type: Cmune.Realtime.Photon.Client.RemoteFunctionCall
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System;
using System.Collections.Generic;

namespace Cmune.Realtime.Photon.Client
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
