// Decompiled with JetBrains decompiler
// Type: NewRealtime.IOperationDispatcher
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

using System;
using System.Collections.Generic;

namespace NewRealtime
{
  public interface IOperationDispatcher
  {
    void SetSender(
      Func<byte, Dictionary<byte, object>, bool, bool> sender);
  }
}
