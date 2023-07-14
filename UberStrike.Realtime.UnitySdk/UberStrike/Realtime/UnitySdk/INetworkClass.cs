// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.INetworkClass
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

using System;

namespace UberStrike.Realtime.UnitySdk
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
