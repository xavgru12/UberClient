// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.SecureMemoryMonitor
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace UberStrike.Realtime.UnitySdk
{
  public class SecureMemoryMonitor
  {
    public static readonly SecureMemoryMonitor Instance = new SecureMemoryMonitor();

    private event Action _sender;

    internal event Action AddToMonitor
    {
      add => this._sender += value;
      remove => this._sender -= value;
    }

    private SecureMemoryMonitor()
    {
    }

    public void PerformCheck()
    {
      if (this._sender == null)
        return;
      this._sender();
    }
  }
}
