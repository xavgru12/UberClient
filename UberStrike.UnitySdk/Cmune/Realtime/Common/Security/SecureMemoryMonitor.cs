
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
