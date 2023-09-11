// Decompiled with JetBrains decompiler
// Type: Cmune.Realtime.Common.Security.SecureMemoryMonitor
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System;

namespace Cmune.Realtime.Common.Security
{
  public class SecureMemoryMonitor
  {
    public static readonly SecureMemoryMonitor Instance = new SecureMemoryMonitor();

    private SecureMemoryMonitor()
    {
    }

    public void PerformCheck()
    {
      if (this._sender == null)
        return;
      this._sender();
    }

    private event Action _sender;

    internal event Action AddToMonitor
    {
      add => this._sender += value;
      remove => this._sender -= value;
    }
  }
}
