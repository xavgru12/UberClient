// Decompiled with JetBrains decompiler
// Type: Cmune.Realtime.Common.INetworkClass
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

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
