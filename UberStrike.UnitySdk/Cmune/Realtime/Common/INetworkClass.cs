
using System;

namespace Cmune.Realtime.Common
{
  public interface INetworkClass : IDisposable
  {
    short NetworkID { get; }

    int InstanceID { get; }

    bool IsGlobal { get; }

    bool IsInitialized { get; }

    void Initialize(short id);

    void Uninitialize();

    void CallMethod(byte idx, params object[] parameter);

    bool HasMethod(byte address);

    string GetMethodName(byte address);
  }
}
