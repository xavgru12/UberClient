
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
